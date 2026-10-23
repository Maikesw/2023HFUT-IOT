#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
分词效果评估（独立脚本）

功能：
- 计算各分词方法（FMM/BMM/BIMM/HMM/CRF/jieba）的精确率、召回率、F1
- 打印对比表，并保存F1柱状图

说明：
- CRF依赖 sklearn-crfsuite，若未安装则自动跳过CRF
"""

import os
import matplotlib
matplotlib.use('Agg')  # 无界面环境下保存图片
import matplotlib.pyplot as plt

# 支持中文显示与负号
plt.rcParams['font.sans-serif'] = ['SimHei', 'Microsoft YaHei', 'Arial Unicode MS', 'Noto Sans CJK SC']
plt.rcParams['axes.unicode_minus'] = False

from typing import List, Dict

# 从已有实现复用方法
from dict_segmentation import (
    build_dict_from_file,
    fmm_segment,
    bmm_segment,
    bimm_segment,
    evaluate_segmentation,
)

from statistical_segmentation import (
    viterbi_segment,
    prepare_crf_data,
    crf_segment,
)


def try_crf_segment(text: str):
    try:
        import sklearn_crfsuite  # noqa: F401
    except Exception:
        return None
    # 小样本演示训练
    import sklearn_crfsuite
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
    return crf_segment(text, crf)


def hmm_segment_demo(text: str) -> List[str]:
    # 使用示例HMM参数（与实验指导一致）
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
    return viterbi_segment(text, A, B, pi)


def jieba_segment(text: str):
    import jieba
    return list(jieba.cut(text))


def main():
    print("== 分词效果评估（独立脚本）==")
    # 文本与标准结果
    test_text = "自然语言处理是计算机科学的一个重要分支"
    gold_segmentation = ["自然语言", "处理", "是", "计算机科学", "的", "一个", "重要", "分支"]

    # 词典法
    dict_path = os.path.join(os.path.dirname(__file__), 'ci_word_dict.txt')
    word_dict = build_dict_from_file(dict_path)
    fmm_result = fmm_segment(test_text, word_dict)
    bmm_result = bmm_segment(test_text, word_dict)
    bimm_result = bimm_segment(test_text, word_dict)

    # 统计法
    hmm_result = hmm_segment_demo(test_text)
    crf_result = try_crf_segment(test_text)

    # 预训练（仅jieba，避免网络依赖）
    jieba_result = jieba_segment(test_text)

    methods: Dict[str, List[str]] = {
        "FMM": fmm_result,
        "BMM": bmm_result,
        "BIMM": bimm_result,
        "HMM": hmm_result,
        "jieba": jieba_result,
    }
    if crf_result is not None:
        methods["CRF"] = crf_result

    print("\n分词效果评估:")
    print(f"{'方法':<10}{'精确率':<10}{'召回率':<10}{'F1值':<10}")
    print("-" * 40)

    names, f1_scores = [], []
    for name, result in methods.items():
        precision, recall, f1 = evaluate_segmentation(result, gold_segmentation)
        print(f"{name:<10}{precision:.4f}{' ' * 6}{recall:.4f}{' ' * 6}{f1:.4f}")
        names.append(name)
        f1_scores.append(f1)

    # 保存图
    plt.figure(figsize=(10, 6))
    plt.bar(names, f1_scores)
    plt.title('不同分词方法的F1值比较')
    plt.xlabel('分词方法')
    plt.ylabel('F1值')
    plt.ylim(0, 1)
    plt.grid(axis='y', linestyle='--', alpha=0.7)
    out_file = 'segmentation_f1.png'
    plt.tight_layout()
    plt.savefig(out_file, dpi=150)
    print(f"已保存图表: {out_file}")


if __name__ == '__main__':
    main()


