#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
统计分词方法实现（HMM + CRF）

功能：
- HMM：从空格分词语料训练 (A, B, pi)，或使用示例参数；Viterbi 解码分词
- CRF：小样本演示训练并进行分词（需安装 sklearn-crfsuite）

用法：
- 仅运行HMM（示例参数）：
    python statistical_segmentation.py --mode hmm
- 从语料训练HMM并分词：
    python statistical_segmentation.py --mode hmm --corpus your_corpus.txt --text "待分词文本"
- 运行CRF演示训练：
    python statistical_segmentation.py --mode crf
"""

import argparse
from typing import Dict, Tuple, List


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
                # 发射与状态计数
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
                # 转移计数（按词粒度）
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
    # 归一化
    A = {s: {t: (transition_count[s][t] / state_count[s] if state_count[s] > 0 else 0) for t in states} for s in states}
    B = {s: {ch: (emission_count[s][ch] / state_count[s] if state_count[s] > 0 else 0) for ch in emission_count[s]} for s in states}
    total = sum(state_count.values())
    pi = {s: (state_count[s] / total if total > 0 else 0.25) for s in states}
    return A, B, pi


def viterbi_segment(text: str, A: Dict, B: Dict, pi: Dict) -> List[str]:
    """Viterbi 解码分词（BMES 标签）。"""
    states = ['B', 'M', 'E', 'S']
    V = [{}]
    path = {}
    # 初始化
    for s in states:
        V[0][s] = pi.get(s, 1e-10) * (B.get(s, {}).get(text[0], 1e-10))
        path[s] = [s]
    # 递推
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
    # 终止
    max_prob = -1.0
    max_state = None
    for s in states:
        if V[len(text) - 1][s] > max_prob:
            max_prob = V[len(text) - 1][s]
            max_state = s
    state_seq = path[max_state]
    # 还原词
    words: List[str] = []
    w = ""
    for i, st in enumerate(state_seq):
        if st in ('B', 'M'):
            w += text[i]
        elif st == 'E':
            w += text[i]
            words.append(w)
            w = ""
        else:  # 'S'
            if w:
                words.append(w)
                w = ""
            words.append(text[i])
    if w:
        words.append(w)
    return words


# ======================= CRF 分词（演示） =======================
try:
    import sklearn_crfsuite  # type: ignore
    _crf_available = True
except Exception:
    _crf_available = False


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
        else:  # 'S'
            if w:
                words.append(w)
            words.append(chars[i])
            w = ""
    if w:
        words.append(w)
    return words


def run_hmm_demo(args) -> None:
    text = args.text or "自然语言处理是计算机科学的一个重要分支"
    if args.corpus:
        trained = train_hmm_from_corpus(args.corpus)
        if trained is None:
            print("HMM训练失败，使用示例参数。")
        else:
            A, B, pi = trained
            seg = viterbi_segment(text, A, B, pi)
            print(f"HMM分词结果: {seg}")
            return
    # 示例参数
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
    seg = viterbi_segment(text, A, B, pi)
    print(f"HMM分词结果: {seg}")


def run_crf_demo(args) -> None:
    if not _crf_available:
        print("未安装 sklearn-crfsuite，无法演示CRF分词。")
        return
    # 小样本演示
    train_sentences = [
        ["自然", "语言", "处理"],
        ["计算机", "科学", "的", "一个", "重要", "分支"],
        ["机器", "学习", "是", "人工智能", "的", "核心"]
    ]
    X_train, y_train = prepare_crf_data(train_sentences)
    crf = sklearn_crfsuite.CRF(
        algorithm='lbfgs', c1=0.1, c2=0.1, max_iterations=100, all_possible_transitions=True
    )
    crf.fit(X_train, y_train)
    text = args.text or "自然语言处理是计算机科学的一个重要分支"
    seg = crf_segment(text, crf)
    print(f"CRF分词结果: {seg}")


def main():
    parser = argparse.ArgumentParser(description='统计分词方法（HMM + CRF）')
    parser.add_argument('--mode', choices=['hmm', 'crf'], default='hmm', help='选择运行 HMM 或 CRF')
    parser.add_argument('--corpus', type=str, default='', help='HMM训练用的空格分词语料路径')
    parser.add_argument('--text', type=str, default='', help='待分词文本，不提供则使用内置示例')
    args = parser.parse_args()
    if args.mode == 'hmm':
        run_hmm_demo(args)
    else:
        run_crf_demo(args)


if __name__ == '__main__':
    main()


