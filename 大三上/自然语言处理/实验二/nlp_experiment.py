import os
import re
from collections import Counter

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

import jieba
import jieba.posseg as pseg

import torch

try:
    import hanlp  # noqa: F401
    _hanlp_available = True
except Exception:
    _hanlp_available = False

try:
    from transformers import BertTokenizer, BertForTokenClassification
    _transformers_available = True
except Exception:
    _transformers_available = False

try:
    import sklearn_crfsuite
    from sklearn_crfsuite import metrics
    from seqeval.metrics import classification_report
    _crf_available = True
except Exception:
    _crf_available = False


# 1. 环境准备完成于导入


# 2. 基于词典的分词算法实现
def build_dict_from_file(file_path: str, encoding: str = 'utf-8') -> set:
    """
    从文件构建词典

    参数:
        file_path: 词典文件路径
        encoding: 文件编码

    返回:
        词典（集合）
    """
    word_dict = set()
    try:
        with open(file_path, 'r', encoding=encoding) as f:
            for line in f:
                word = line.strip().split('\t')[0]
                if word:
                    word_dict.add(word)
    except Exception as e:
        print(f"读取词典文件出错: {e}")
        # 备用简单词典
        word_dict = {"中国", "人民", "共和国", "北京", "天安门", "计算机", "科学", "自然语言", "处理"}
    return word_dict


def fmm_segment(text: str, word_dict: set, max_word_len: int = 5):
    """正向最大匹配算法"""
    words = []
    i = 0
    while i < len(text):
        matched = False
        for j in range(max_word_len, 0, -1):
            if i + j > len(text):
                continue
            word = text[i:i + j]
            if word in word_dict:
                words.append(word)
                i += j
                matched = True
                break
        if not matched:
            words.append(text[i])
            i += 1
    return words


def bmm_segment(text: str, word_dict: set, max_word_len: int = 5):
    """逆向最大匹配算法"""
    words = []
    i = len(text)
    while i > 0:
        matched = False
        for j in range(max_word_len, 0, -1):
            if i - j < 0:
                continue
            word = text[i - j:i]
            if word in word_dict:
                words.insert(0, word)
                i -= j
                matched = True
                break
        if not matched:
            words.insert(0, text[i - 1])
            i -= 1
    return words


def bimm_segment(text: str, word_dict: set, max_word_len: int = 5):
    """双向最大匹配算法"""
    fmm_result = fmm_segment(text, word_dict, max_word_len)
    bmm_result = bmm_segment(text, word_dict, max_word_len)
    if len(fmm_result) != len(bmm_result):
        return fmm_result if len(fmm_result) < len(bmm_result) else bmm_result
    fmm_single_word_count = sum(1 for w in fmm_result if len(w) == 1)
    bmm_single_word_count = sum(1 for w in bmm_result if len(w) == 1)
    return fmm_result if fmm_single_word_count < bmm_single_word_count else bmm_result


# 3. 统计分词方法（HMM + Viterbi）
def train_hmm_from_corpus(corpus_file: str, encoding: str = 'utf-8'):
    """
    从语料库训练HMM模型，返回 (A, B, pi)
    """
    states = ['B', 'M', 'E', 'S']
    state_count = {state: 0 for state in states}
    transition_count = {state: {next_state: 0 for next_state in states} for state in states}
    emission_count = {state: {} for state in states}
    try:
        with open(corpus_file, 'r', encoding=encoding) as f:
            for line in f:
                line = line.strip()
                if not line:
                    continue
                words = line.split()
                for word in words:
                    if len(word) == 1:
                        state_count['S'] += 1
                        emission_count['S'][word] = emission_count['S'].get(word, 0) + 1
                    else:
                        state_count['B'] += 1
                        emission_count['B'][word[0]] = emission_count['B'].get(word[0], 0) + 1
                        for i in range(1, len(word) - 1):
                            state_count['M'] += 1
                            emission_count['M'][word[i]] = emission_count['M'].get(word[i], 0) + 1
                        state_count['E'] += 1
                        emission_count['E'][word[-1]] = emission_count['E'].get(word[-1], 0) + 1
                prev_state = None
                for word in words:
                    if len(word) == 1:
                        if prev_state:
                            transition_count[prev_state]['S'] += 1
                        prev_state = 'S'
                    else:
                        if prev_state:
                            transition_count[prev_state]['B'] += 1
                        transition_count['B']['M' if len(word) > 2 else 'E'] += 1
                        for i in range(1, len(word) - 1):
                            transition_count['M']['M' if i < len(word) - 2 else 'E'] += 1
                        prev_state = 'E'
    except Exception as e:
        print(f"训练HMM模型出错: {e}")
        return None
    A = {s: {t: (transition_count[s][t] / state_count[s] if state_count[s] > 0 else 0) for t in states} for s in states}
    B = {s: {ch: (emission_count[s][ch] / state_count[s] if state_count[s] > 0 else 0) for ch in emission_count[s]} for s in states}
    total = sum(state_count.values())
    pi = {s: (state_count[s] / total if total > 0 else 0.25) for s in states}
    return A, B, pi


def viterbi_segment(text: str, A, B, pi):
    """使用Viterbi进行分词，返回词列表"""
    states = ['B', 'M', 'E', 'S']
    V = [{}]
    path = {}
    for state in states:
        V[0][state] = pi.get(state, 1e-10) * (B.get(state, {}).get(text[0], 1e-10))
        path[state] = [state]
    for t in range(1, len(text)):
        V.append({})
        new_path = {}
        for curr in states:
            max_prob = -1.0
            max_state = None
            for prev in states:
                prob = V[t - 1][prev] * A.get(prev, {}).get(curr, 1e-10) * B.get(curr, {}).get(text[t], 1e-10)
                if prob > max_prob:
                    max_prob = prob
                    max_state = prev
            V[t][curr] = max_prob
            new_path[curr] = path[max_state] + [curr]
        path = new_path
    max_prob = -1.0
    max_state = None
    for state in states:
        if V[len(text) - 1][state] > max_prob:
            max_prob = V[len(text) - 1][state]
            max_state = state
    state_sequence = path[max_state]
    words = []
    word = ""
    for i, state in enumerate(state_sequence):
        if state in ('B', 'M'):
            word += text[i]
        elif state == 'E':
            word += text[i]
            words.append(word)
            word = ""
        else:  # 'S'
            if word:
                words.append(word)
                word = ""
            words.append(text[i])
    if word:
        words.append(word)
    return words


# 3.2 CRF 分词
def word2features(sent, i):
    word = sent[i]
    features = {
        'bias': 1.0,
        'word': word,
        'word.isdigit()': word.isdigit(),
        'word.isalpha()': word.isalpha(),
    }
    if i > 0:
        prev_word = sent[i - 1]
        features.update({
            '-1:word': prev_word,
            '-1:word.isdigit()': prev_word.isdigit(),
            '-1:word.isalpha()': prev_word.isalpha(),
            'bigram[-1:]': prev_word + word,
        })
    else:
        features['BOS'] = True
    if i < len(sent) - 1:
        next_word = sent[i + 1]
        features.update({
            '+1:word': next_word,
            '+1:word.isdigit()': next_word.isdigit(),
            '+1:word.isalpha()': next_word.isalpha(),
            'bigram[+1]': word + next_word,
        })
    else:
        features['EOS'] = True
    return features


def prepare_crf_data(sentences):
    X = []
    y = []
    for sentence in sentences:
        chars = []
        labels = []
        for word in sentence:
            if len(word) == 1:
                chars.append(word)
                labels.append('S')
            else:
                chars.append(word[0])
                labels.append('B')
                for ch in word[1:-1]:
                    chars.append(ch)
                    labels.append('M')
                chars.append(word[-1])
                labels.append('E')
        X.append([word2features(chars, i) for i in range(len(chars))])
        y.append(labels)
    return X, y


def crf_segment(text, crf_model):
    chars = list(text)
    features = [word2features(chars, i) for i in range(len(chars))]
    labels = crf_model.predict([features])[0]
    words = []
    word = ""
    for i, label in enumerate(labels):
        if label == 'B':
            if word:
                words.append(word)
            word = chars[i]
        elif label == 'M':
            word += chars[i]
        elif label == 'E':
            word += chars[i]
            words.append(word)
            word = ""
        else:  # 'S'
            if word:
                words.append(word)
            words.append(chars[i])
            word = ""
    if word:
        words.append(word)
    return words


# 4. 预训练模型分词
def jieba_segment(text: str):
    return list(jieba.cut(text))


def hanlp_segment(text: str):
    if not _hanlp_available:
        raise RuntimeError('HanLP 不可用，请先安装 hanlp 包')
    tokenizer = hanlp.load('CTB9_TOK_ELECTRA_SMALL')
    return tokenizer(text)


def bert_segment(text: str, model_name: str = "bert-base-chinese"):
    if not _transformers_available:
        raise RuntimeError('transformers 不可用，请先安装 transformers 包')
    
    # 使用国内镜像源
    try:
        tokenizer = BertTokenizer.from_pretrained(
            model_name, 
            mirror='https://hf-mirror.com',
            cache_dir='./cache'
        )
    except Exception as e:
        print(f"使用镜像源下载失败，尝试直接下载: {e}")
        tokenizer = BertTokenizer.from_pretrained(model_name)
    
    tokens = tokenizer.tokenize(text)
    words = []
    word = ""
    for token in tokens:
        if token.startswith("##"):
            word += token[2:]
        else:
            if word:
                words.append(word)
            word = token
    if word:
        words.append(word)
    return words


# 5. 命名实体识别
def jieba_ner(text: str):
    words_with_pos = [(w, pos) for w, pos in pseg.cut(text)]
    entities = []
    for word, pos in words_with_pos:
        if pos.startswith('nr'):
            entities.append((word, 'PER'))
        elif pos.startswith('ns'):
            entities.append((word, 'LOC'))
        elif pos.startswith('nt'):
            entities.append((word, 'ORG'))
    return words_with_pos, entities


def hanlp_ner(text: str):
    if not _hanlp_available:
        raise RuntimeError('HanLP 不可用，请先安装 hanlp 包')
    ner_model = hanlp.load('MSRA_NER_BERT_BASE_ZH')
    return ner_model(text)


def prepare_bert_ner_data(text: str, tokenizer):
    tokens = tokenizer.tokenize(text)
    tokens = ["[CLS]"] + tokens + ["[SEP]"]
    input_ids = tokenizer.convert_tokens_to_ids(tokens)
    attention_mask = [1] * len(input_ids)
    return {
        "input_ids": torch.tensor([input_ids]),
        "attention_mask": torch.tensor([attention_mask]),
    }


def bert_ner(text: str, model_name: str = "bert-base-chinese"):
    if not _transformers_available:
        raise RuntimeError('transformers 不可用，请先安装 transformers 包')
    
    # 使用国内镜像源
    try:
        tokenizer = BertTokenizer.from_pretrained(
            model_name, 
            mirror='https://hf-mirror.com',
            cache_dir='./cache'
        )
        model = BertForTokenClassification.from_pretrained(
            model_name, 
            num_labels=9,
            mirror='https://hf-mirror.com',
            cache_dir='./cache'
        )
    except Exception as e:
        print(f"使用镜像源下载失败，尝试直接下载: {e}")
        tokenizer = BertTokenizer.from_pretrained(model_name)
        model = BertForTokenClassification.from_pretrained(model_name, num_labels=9)
    
    inputs = prepare_bert_ner_data(text, tokenizer)
    with torch.no_grad():
        outputs = model(**inputs)
        predictions = torch.argmax(outputs.logits, dim=2)
    tokens = tokenizer.tokenize(text)
    tokens = ["[CLS]"] + tokens + ["[SEP]"]
    id2label = {
        0: "O",
        1: "B-PER", 2: "I-PER",
        3: "B-LOC", 4: "I-LOC",
        5: "B-ORG", 6: "I-ORG",
        7: "B-MISC", 8: "I-MISC",
    }
    results = []
    for token, prediction in zip(tokens[1:-1], predictions[0][1:-1]):
        results.append((token, id2label[prediction.item()]))
    return results


# 6. 分词效果评估
def evaluate_segmentation(pred_words, gold_words):
    correct = 0
    gold_set = set(gold_words)
    for word in pred_words:
        if word in gold_set:
            correct += 1
    precision = correct / len(pred_words) if pred_words else 0.0
    recall = correct / len(gold_words) if gold_words else 0.0
    f1 = 2 * precision * recall / (precision + recall) if (precision + recall) > 0 else 0.0
    return precision, recall, f1


def main():
    print("== 实验脚本开始 ==")
    dict_path = os.path.join(os.path.dirname(__file__), 'ci_word_dict.txt')
    word_dict = build_dict_from_file(dict_path)
    print(f"词典大小: {len(word_dict)}")
    print(f"词典示例: {list(word_dict)[:10]}")

    test_text = "自然语言处理是计算机科学的一个重要分支"

    # 2. 词典分词
    fmm_result = fmm_segment(test_text, word_dict)
    bmm_result = bmm_segment(test_text, word_dict)
    bimm_result = bimm_segment(test_text, word_dict)
    print(f"FMM分词结果: {fmm_result}")
    print(f"BMM分词结果: {bmm_result}")
    print(f"BIMM分词结果: {bimm_result}")

    # 3. HMM 分词（使用预定义参数）
    A = {
        'B': {'B': 0.01, 'M': 0.7, 'E': 0.29, 'S': 0.0},
        'M': {'B': 0.01, 'M': 0.6, 'E': 0.39, 'S': 0.0},
        'E': {'B': 0.5, 'M': 0.0, 'E': 0.0, 'S': 0.5},
        'S': {'B': 0.5, 'M': 0.0, 'E': 0.0, 'S': 0.5},
    }
    B = {
        'B': {'自': 0.1, '然': 0.05, '语': 0.1, '言': 0.05, '处': 0.1, '理': 0.05, '计': 0.1, '算': 0.05, '机': 0.1, '科': 0.1, '学': 0.05, '重': 0.1, '要': 0.05, '分': 0.05, '支': 0.05},
        'M': {'自': 0.05, '然': 0.1, '语': 0.05, '言': 0.1, '处': 0.05, '理': 0.1, '计': 0.05, '算': 0.1, '机': 0.05, '科': 0.05, '学': 0.1, '重': 0.05, '要': 0.1, '分': 0.1, '支': 0.05},
        'E': {'自': 0.05, '然': 0.1, '语': 0.05, '言': 0.1, '处': 0.05, '理': 0.1, '计': 0.05, '算': 0.05, '机': 0.1, '科': 0.05, '学': 0.1, '重': 0.05, '要': 0.1, '分': 0.05, '支': 0.1},
        'S': {'自': 0.1, '然': 0.05, '语': 0.05, '言': 0.05, '处': 0.05, '理': 0.05, '计': 0.05, '算': 0.05, '机': 0.05, '科': 0.1, '学': 0.1, '重': 0.1, '要': 0.1, '分': 0.1, '支': 0.1},
    }
    pi = {'B': 0.3, 'M': 0.0, 'E': 0.0, 'S': 0.7}
    hmm_result = viterbi_segment(test_text, A, B, pi)
    print(f"HMM分词结果: {hmm_result}")

    # 3.2 CRF 分词
    try:
        if not _crf_available:
            raise RuntimeError('sklearn-crfsuite/seqeval 不可用')
        train_sentences = [
            ["自然", "语言", "处理"],
            ["计算机", "科学", "的", "一个", "重要", "分支"],
            ["机器", "学习", "是", "人工智能", "的", "核心"],
        ]
        X_train, y_train = prepare_crf_data(train_sentences)
        crf = sklearn_crfsuite.CRF(
            algorithm='lbfgs', c1=0.1, c2=0.1, max_iterations=100, all_possible_transitions=True
        )
        crf.fit(X_train, y_train)
        crf_result = crf_segment(test_text, crf)
        print(f"CRF分词结果: {crf_result}")
    except Exception as e:
        crf_result = []
        print(f"CRF分词出错: {e}")

    # 4. 预训练分词
    jieba_result = jieba_segment(test_text)
    print(f"jieba分词结果: {jieba_result}")

    try:
        hanlp_result = hanlp_segment(test_text) if _hanlp_available else []
        print(f"HanLP分词结果: {hanlp_result}")
    except Exception as e:
        hanlp_result = []
        print(f"HanLP分词出错: {e}")

    try:
        bert_result = bert_segment(test_text) if _transformers_available else []
        print(f"BERT分词结果: {bert_result}")
    except Exception as e:
        bert_result = []
        print(f"BERT分词出错: {e}")

    # 5. 命名实体识别
    test_ner_text = "李明在北京大学计算机科学系学习自然语言处理"
    jieba_pos, jieba_entities = jieba_ner(test_ner_text)
    print(f"jieba词性标注结果: {jieba_pos}")
    print(f"jieba命名实体识别结果: {jieba_entities}")

    try:
        hanlp_ner_result = hanlp_ner(test_ner_text) if _hanlp_available else []
        print(f"HanLP命名实体识别结果: {hanlp_ner_result}")
    except Exception as e:
        print(f"HanLP命名实体识别出错: {e}")

    try:
        bert_ner_result = bert_ner(test_ner_text) if _transformers_available else []
        print(f"BERT命名实体识别结果: {bert_ner_result}")
    except Exception as e:
        print(f"BERT命名实体识别出错: {e}")

    # 6. 评估
    gold_segmentation = ["自然语言", "处理", "是", "计算机科学", "的", "一个", "重要", "分支"]
    methods = {
        "FMM": fmm_result,
        "BMM": bmm_result,
        "BIMM": bimm_result,
        "HMM": hmm_result,
        "CRF": crf_result,
        "jieba": jieba_result,
    }
    print("\n分词效果评估:")
    print(f"{'方法':<10}{'精确率':<10}{'召回率':<10}{'F1值':<10}")
    print("-" * 40)
    methods_names = []
    f1_scores = []
    for method_name, result in methods.items():
        precision, recall, f1 = evaluate_segmentation(result, gold_segmentation)
        print(f"{method_name:<10}{precision:.4f}{' ' * 6}{recall:.4f}{' ' * 6}{f1:.4f}")
        methods_names.append(method_name)
        f1_scores.append(f1)

    # 可视化
    try:
        plt.figure(figsize=(10, 6))
        plt.bar(methods_names, f1_scores)
        plt.title('不同分词方法的F1值比较')
        plt.xlabel('分词方法')
        plt.ylabel('F1值')
        plt.ylim(0, 1)
        plt.grid(axis='y', linestyle='--', alpha=0.7)
        plt.tight_layout()
        plt.savefig('segmentation_f1.png', dpi=150)
        print("已保存图表: segmentation_f1.png")
    except Exception as e:
        print(f"绘图出错: {e}")

    print("== 实验脚本结束 ==")


if __name__ == '__main__':
    main()


