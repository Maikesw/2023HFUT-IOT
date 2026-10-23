#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
基于词典的中文分词算法实现
包含正向最大匹配（FMM）和逆向最大匹配（BMM）算法
"""

import os
import argparse
from typing import Set, List, Tuple, Dict

try:
    import sklearn_crfsuite
    _crf_available = True
except Exception:
    _crf_available = False


def build_dict_from_file(file_path: str, encoding: str = 'utf-8') -> Set[str]:
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
                word = line.strip().split('\t')[0]  # 假设词典格式为"词\t频率"
                if word:
                    word_dict.add(word)
    except Exception as e:
        print(f"读取词典文件出错: {e}")
        # 创建一个简单的词典作为备用
        word_dict = {
            "中国", "人民", "共和国", "北京", "天安门", 
            "计算机", "科学", "自然语言", "处理", "重要", 
            "分支", "机器", "学习", "人工智能", "核心"
        }
        print("使用备用词典")
    
    return word_dict


def load_word_freq(file_path: str, encoding: str = 'utf-8') -> tuple:
    """
    加载词频字典与最大词长
    返回: (word2freq: dict[str,int], max_word_len: int)
    """
    word2freq = {}
    max_len = 1
    try:
        with open(file_path, 'r', encoding=encoding) as f:
            for line in f:
                line = line.strip()
                if not line:
                    continue
                parts = line.split('\t')
                word = parts[0]
                freq = int(parts[1]) if len(parts) > 1 and parts[1].isdigit() else 1
                word2freq[word] = word2freq.get(word, 0) + freq
                if len(word) > max_len:
                    max_len = len(word)
    except Exception as e:
        print(f"读取词频词典失败，使用默认: {e}")
        # 回退到简单集合
        d = build_dict_from_file(file_path, encoding)
        for w in d:
            word2freq[w] = 1
            if len(w) > max_len:
                max_len = len(w)
    return word2freq, max_len


def fmm_segment(text: str, word_dict: Set[str], max_word_len: int = 5) -> List[str]:
    """
    正向最大匹配算法（FMM）
    
    参数:
        text: 待分词文本
        word_dict: 词典
        max_word_len: 最大词长
    
    返回:
        分词结果列表
    """
    words = []
    i = 0
    while i < len(text):
        matched = False
        # 从最大长度开始尝试匹配
        for j in range(max_word_len, 0, -1):
            if i + j > len(text):
                continue
            word = text[i:i+j]
            if word in word_dict:
                words.append(word)
                i += j
                matched = True
                break
        
        if not matched:  # 未匹配，按字符切分
            words.append(text[i])
            i += 1
    
    return words


def bmm_segment(text: str, word_dict: Set[str], max_word_len: int = 5) -> List[str]:
    """
    逆向最大匹配算法（BMM）
    
    参数:
        text: 待分词文本
        word_dict: 词典
        max_word_len: 最大词长
    
    返回:
        分词结果列表
    """
    words = []
    i = len(text)
    while i > 0:
        matched = False
        # 从最大长度开始尝试匹配
        for j in range(max_word_len, 0, -1):
            if i - j < 0:
                continue
            word = text[i-j:i]
            if word in word_dict:
                words.insert(0, word)  # 插入到列表开头
                i -= j
                matched = True
                break
        
        if not matched:  # 未匹配，按字符切分
            words.insert(0, text[i-1])
            i -= 1
    
    return words


def bimm_segment(text: str, word_dict: Set[str], max_word_len: int = 5) -> List[str]:
    """
    双向最大匹配算法（BIMM）
    
    参数:
        text: 待分词文本
        word_dict: 词典
        max_word_len: 最大词长
    
    返回:
        分词结果列表
    """
    fmm_result = fmm_segment(text, word_dict, max_word_len)
    bmm_result = bmm_segment(text, word_dict, max_word_len)
    
    # 如果正反向分词结果词数不同，则取分词数量较少的那个
    if len(fmm_result) != len(bmm_result):
        return fmm_result if len(fmm_result) < len(bmm_result) else bmm_result
    
    # 如果分词数量相同，则返回单字较少的那个
    fmm_single_word_count = sum(1 for word in fmm_result if len(word) == 1)
    bmm_single_word_count = sum(1 for word in bmm_result if len(word) == 1)
    
    return fmm_result if fmm_single_word_count < bmm_single_word_count else bmm_result


def evaluate_segmentation(pred_words: List[str], gold_words: List[str]) -> tuple:
    """
    评估分词效果
    
    参数:
        pred_words: 预测的分词结果
        gold_words: 标准分词结果
    
    返回:
        精确率、召回率、F1值
    """
    # 计算正确分词数
    correct = 0
    gold_set = set(gold_words)
    for word in pred_words:
        if word in gold_set:
            correct += 1
    
    # 计算精确率、召回率、F1值
    precision = correct / len(pred_words) if pred_words else 0
    recall = correct / len(gold_words) if gold_words else 0
    f1 = 2 * precision * recall / (precision + recall) if precision + recall > 0 else 0
    
    return precision, recall, f1


# ===================== 优化：基于词频的DAG+Viterbi =====================
def build_dag(text: str, word2freq: Dict[str, int], max_word_len: int) -> Dict[int, List[int]]:
    """构建有向无环图：位置i可到达的j（包含j）索引集合"""
    dag: Dict[int, List[int]] = {}
    n = len(text)
    for i in range(n):
        ends: List[int] = []
        max_j = min(n, i + max_word_len)
        for j in range(i + 1, max_j + 1):
            if text[i:j] in word2freq:
                ends.append(j - 1)
        if not ends:
            ends = [i]
        dag[i] = ends
    return dag


def dag_viterbi_segment(text: str, word2freq: Dict[str, int], max_word_len: int) -> List[str]:
    """使用词频（取对数）进行全局最优切分（简化版DAG+Viterbi）"""
    import math
    n = len(text)
    dag = build_dag(text, word2freq, max_word_len)
    log_total = math.log(sum(max(1, f) for f in word2freq.values()))

    # 动态规划：dp[i] = (最大log概率, 下一断点k)
    dp: List[Tuple[float, int]] = [(-1e18, -1) for _ in range(n + 1)]
    dp[n] = (0.0, -1)
    for i in range(n - 1, -1, -1):
        best = (-1e18, -1)
        for j in dag[i]:
            w = text[i:j + 1]
            freq = word2freq.get(w, 1)
            score = math.log(freq) - log_total + dp[j + 1][0]
            if score > best[0]:
                best = (score, j + 1)
        dp[i] = best

    # 回溯
    words: List[str] = []
    i = 0
    while i < n and dp[i][1] != -1:
        j = dp[i][1]
        words.append(text[i:j])
        i = j
    if i < n:  # 兜底
        words.extend(list(text[i:]))
    return words


def merge_oov(tokens: List[str]) -> List[str]:
    """合并连续的数字串、英文字母串作为未登录词，以改善OOV处理"""
    import re
    merged: List[str] = []
    buf = ''
    typ = None
    def flush():
        nonlocal buf, typ
        if buf:
            merged.append(buf)
            buf = ''
            typ = None
    for t in tokens:
        if re.fullmatch(r'[0-9]+', t):
            if typ == 'num':
                buf += t
            else:
                flush()
                buf = t
                typ = 'num'
        elif re.fullmatch(r'[A-Za-z]+', t):
            if typ == 'eng':
                buf += t
            else:
                flush()
                buf = t
                typ = 'eng'
        else:
            flush()
            merged.append(t)
    flush()
    return merged
"""
========================= 3. 统计分词方法 =========================
HMM 训练与 Viterbi 解码、CRF 分词演示
"""

from typing import Dict, Tuple  # already imported generics extended above


def train_hmm_from_corpus(corpus_file: str, encoding: str = 'utf-8') -> Tuple[Dict, Dict, Dict]:
    """从已分词语料训练HMM，返回 (A, B, pi)。语料需空格分词。"""
    states = ['B', 'M', 'E', 'S']
    state_count = {s: 0 for s in states}
    transition_count = {s: {t: 0 for t in states} for s in states}
    emission_count = {s: {} for s in states}
    try:
        with open(corpus_file, 'r', encoding=encoding) as f:
            for line in f:
                line = line.strip()
                if not line:
                    continue
                words = line.split()
                for w in words:
                    if len(w) == 1:
                        state_count['S'] += 1
                        emission_count['S'][w] = emission_count['S'].get(w, 0) + 1
                    else:
                        state_count['B'] += 1
                        emission_count['B'][w[0]] = emission_count['B'].get(w[0], 0) + 1
                        for ch in w[1:-1]:
                            state_count['M'] += 1
                            emission_count['M'][ch] = emission_count['M'].get(ch, 0) + 1
                        state_count['E'] += 1
                        emission_count['E'][w[-1]] = emission_count['E'].get(w[-1], 0) + 1
                prev = None
                for w in words:
                    if len(w) == 1:
                        if prev:
                            transition_count[prev]['S'] += 1
                        prev = 'S'
                    else:
                        if prev:
                            transition_count[prev]['B'] += 1
                        transition_count['B']['M' if len(w) > 2 else 'E'] += 1
                        for i in range(1, len(w) - 1):
                            transition_count['M']['M' if i < len(w) - 2 else 'E'] += 1
                        prev = 'E'
    except Exception as e:
        print(f"训练HMM模型出错: {e}")
        return None
    A = {s: {t: (transition_count[s][t] / state_count[s] if state_count[s] > 0 else 0) for t in states} for s in states}
    B = {s: {ch: (emission_count[s][ch] / state_count[s] if state_count[s] > 0 else 0) for ch in emission_count[s]} for s in states}
    total = sum(state_count.values())
    pi = {s: (state_count[s] / total if total > 0 else 0.25) for s in states}
    return A, B, pi


def viterbi_segment(text: str, A, B, pi) -> List[str]:
    """Viterbi 解码分词（BMES 标签）。"""
    states = ['B', 'M', 'E', 'S']
    V = [{}]
    path = {}
    for s in states:
        V[0][s] = pi.get(s, 1e-10) * (B.get(s, {}).get(text[0], 1e-10))
        path[s] = [s]
    for t in range(1, len(text)):
        V.append({})
        new_path = {}
        for cur in states:
            max_prob = -1.0
            max_state = None
            for prev in states:
                prob = V[t - 1][prev] * A.get(prev, {}).get(cur, 1e-10) * B.get(cur, {}).get(text[t], 1e-10)
                if prob > max_prob:
                    max_prob = prob
                    max_state = prev
            V[t][cur] = max_prob
            new_path[cur] = path[max_state] + [cur]
        path = new_path
    max_prob = -1.0
    max_state = None
    for s in states:
        if V[len(text) - 1][s] > max_prob:
            max_prob = V[len(text) - 1][s]
            max_state = s
    state_seq = path[max_state]
    words: List[str] = []
    w = ""
    for i, st in enumerate(state_seq):
        if st in ('B', 'M'):
            w += text[i]
        elif st == 'E':
            w += text[i]
            words.append(w)
            w = ""
        else:
            if w:
                words.append(w)
                w = ""
            words.append(text[i])
    if w:
        words.append(w)
    return words


def word2features(sent: List[str], i: int) -> Dict:
    word = sent[i]
    feats = {
        'bias': 1.0,
        'word': word,
        'word.isdigit()': word.isdigit(),
        'word.isalpha()': word.isalpha(),
    }
    if i > 0:
        pw = sent[i - 1]
        feats.update({'-1:word': pw, '-1:word.isdigit()': pw.isdigit(), '-1:word.isalpha()': pw.isalpha(), 'bigram[-1:]': pw + word})
    else:
        feats['BOS'] = True
    if i < len(sent) - 1:
        nw = sent[i + 1]
        feats.update({'+1:word': nw, '+1:word.isdigit()': nw.isdigit(), '+1:word.isalpha()': nw.isalpha(), 'bigram[+1]': word + nw})
    else:
        feats['EOS'] = True
    return feats


def prepare_crf_data(sentences: List[List[str]]) -> Tuple[List[List[Dict]], List[List[str]]]:
    X: List[List[Dict]] = []
    y: List[List[str]] = []
    for sentence in sentences:
        chars: List[str] = []
        labels: List[str] = []
        for w in sentence:
            if len(w) == 1:
                chars.append(w)
                labels.append('S')
            else:
                chars.append(w[0])
                labels.append('B')
                for ch in w[1:-1]:
                    chars.append(ch)
                    labels.append('M')
                chars.append(w[-1])
                labels.append('E')
        X.append([word2features(chars, i) for i in range(len(chars))])
        y.append(labels)
    return X, y


def crf_segment(text: str, crf_model) -> List[str]:
    chars = list(text)
    feats = [word2features(chars, i) for i in range(len(chars))]
    labels = crf_model.predict([feats])[0]
    words: List[str] = []
    w = ""
    for i, lab in enumerate(labels):
        if lab == 'B':
            if w:
                words.append(w)
            w = chars[i]
        elif lab == 'M':
            w += chars[i]
        elif lab == 'E':
            w += chars[i]
            words.append(w)
            w = ""
        else:
            if w:
                words.append(w)
            words.append(chars[i])
            w = ""
    if w:
        words.append(w)
    return words


def run_stat_demo() -> None:
    print("=== 统计分词方法实验（HMM + CRF） ===\n")
    test_text = "自然语言处理是计算机科学的一个重要分支"
    # HMM：用示例参数直接解码（可替换为 train_hmm_from_corpus 的训练结果）
    A = {
        'B': {'B': 0.01, 'M': 0.7, 'E': 0.29, 'S': 0.0},
        'M': {'B': 0.01, 'M': 0.6, 'E': 0.39, 'S': 0.0},
        'E': {'B': 0.5, 'M': 0.0, 'E': 0.0, 'S': 0.5},
        'S': {'B': 0.5, 'M': 0.0, 'E': 0.0, 'S': 0.5}
    }
    B = {
        'B': {'自': 0.1, '然': 0.05, '语': 0.1, '言': 0.05, '处': 0.1, '理': 0.05, '计': 0.1, '算': 0.05, '机': 0.1, '科': 0.1, '学': 0.05, '重': 0.1, '要': 0.05, '分': 0.05, '支': 0.05},
        'M': {'自': 0.05, '然': 0.1, '语': 0.05, '言': 0.1, '处': 0.05, '理': 0.1, '计': 0.05, '算': 0.1, '机': 0.05, '科': 0.05, '学': 0.1, '重': 0.05, '要': 0.1, '分': 0.1, '支': 0.05},
        'E': {'自': 0.05, '然': 0.1, '语': 0.05, '言': 0.1, '处': 0.05, '理': 0.1, '计': 0.05, '算': 0.05, '机': 0.1, '科': 0.05, '学': 0.1, '重': 0.05, '要': 0.1, '分': 0.05, '支': 0.1},
        'S': {'自': 0.1, '然': 0.05, '语': 0.05, '言': 0.05, '处': 0.05, '理': 0.05, '计': 0.05, '算': 0.05, '机': 0.05, '科': 0.1, '学': 0.1, '重': 0.1, '要': 0.1, '分': 0.1, '支': 0.1}
    }
    pi = {'B': 0.3, 'M': 0.0, 'E': 0.0, 'S': 0.7}
    hmm_result = viterbi_segment(test_text, A, B, pi)
    print(f"HMM分词结果: {hmm_result}")
    # CRF：小样本演示训练
    try:
        if not _crf_available:
            raise RuntimeError('sklearn-crfsuite 未安装，无法演示 CRF')
        train_sentences = [
            ["自然", "语言", "处理"],
            ["计算机", "科学", "的", "一个", "重要", "分支"],
            ["机器", "学习", "是", "人工智能", "的", "核心"]
        ]
        X_train, y_train = prepare_crf_data(train_sentences)
        crf = sklearn_crfsuite.CRF(algorithm='lbfgs', c1=0.1, c2=0.1, max_iterations=100, all_possible_transitions=True)
        crf.fit(X_train, y_train)
        crf_result = crf_segment(test_text, crf)
        print(f"CRF分词结果: {crf_result}")
    except Exception as e:
        print(f"CRF分词出错: {e}")

def run_dict_demo() -> None:
    """词典法演示（FMM/BMM/BIMM）"""
    print("=== 基于词典的中文分词算法实验 ===\n")
    
    dict_path = os.path.join(os.path.dirname(__file__), 'ci_word_dict.txt')
    word_dict = build_dict_from_file(dict_path)
    print(f"词典大小: {len(word_dict)}")
    print(f"词典示例: {list(word_dict)[:10]}")
    print()
    
    test_texts = [
        "自然语言处理是计算机科学的一个重要分支",
        "机器学习是人工智能的核心技术",
        "中国是一个伟大的国家",
        "北京大学计算机科学技术系",
        "今天天气很好"
    ]
    
    for i, test_text in enumerate(test_texts, 1):
        print(f"测试文本 {i}: {test_text}")
        fmm_result = fmm_segment(test_text, word_dict)
        print(f"FMM分词结果: {fmm_result}")
        bmm_result = bmm_segment(test_text, word_dict)
        print(f"BMM分词结果: {bmm_result}")
        bimm_result = bimm_segment(test_text, word_dict)
        print(f"BIMM分词结果: {bimm_result}")
        # 优化分词（词频+DAG+Viterbi + OOV合并）
        dict_path = os.path.join(os.path.dirname(__file__), 'ci_word_dict.txt')
        word2freq, max_len = load_word_freq(dict_path)
        dag_viterbi = dag_viterbi_segment(test_text, word2freq, max_len)
        dag_viterbi = merge_oov(dag_viterbi)
        print(f"DAG+Viterbi优化分词: {dag_viterbi}")
        print("-" * 50)
    
    print("\n=== 分词效果评估（示例句） ===")
    gold_segmentation = ["自然语言", "处理", "是", "计算机科学", "的", "一个", "重要", "分支"]
    test_text = "自然语言处理是计算机科学的一个重要分支"
    
    fmm_result = fmm_segment(test_text, word_dict)
    bmm_result = bmm_segment(test_text, word_dict)
    bimm_result = bimm_segment(test_text, word_dict)
    
    methods = {"FMM": fmm_result, "BMM": bmm_result, "BIMM": bimm_result}
    print(f"标准分词结果: {gold_segmentation}")
    print(f"{'方法':<10}{'精确率':<10}{'召回率':<10}{'F1值':<10}")
    print("-" * 40)
    for method_name, result in methods.items():
        precision, recall, f1 = evaluate_segmentation(result, gold_segmentation)
        print(f"{method_name:<10}{precision:.4f}{' ' * 6}{recall:.4f}{' ' * 6}{f1:.4f}")


if __name__ == '__main__':
    # 默认运行统计法；可通过 --mode dict 运行词典法
    parser = argparse.ArgumentParser(description='中文分词实验')
    parser.add_argument('--mode', choices=['dict', 'stat'], default='stat', help='运行模式：dict=词典法, stat=统计法')
    args = parser.parse_args()
    if args.mode == 'dict':
        run_dict_demo()
    else:
        run_stat_demo()
