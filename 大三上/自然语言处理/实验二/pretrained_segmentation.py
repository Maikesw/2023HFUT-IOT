#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
预训练模型分词实现：jieba、HanLP、BERT

运行示例：
- 运行全部并使用示例文本：
    python pretrained_segmentation.py
- 指定文本：
    python pretrained_segmentation.py --text "自然语言处理是计算机科学的一个重要分支"
- 仅运行某一模型：
    python pretrained_segmentation.py --mode jieba|hanlp|bert --text "..."
"""

import argparse


def jieba_segment(text: str):
    import jieba
    return list(jieba.cut(text))


def hanlp_segment(text: str):
    try:
        import hanlp
    except Exception as e:
        raise RuntimeError(f"HanLP 未安装或不可用: {e}")
    tokenizer = hanlp.load('CTB9_TOK_ELECTRA_SMALL')
    return tokenizer(text)


def bert_segment(text: str, model_name: str = "bert-base-chinese"):
    try:
        from transformers import BertTokenizer
    except Exception as e:
        raise RuntimeError(f"transformers 未安装或不可用: {e}")

    # 优先尝试国内镜像缓存
    try:
        tokenizer = BertTokenizer.from_pretrained(
            model_name,
            mirror='https://hf-mirror.com',
            cache_dir='./cache'
        )
    except Exception as e:
        print(f"使用镜像加载BERT失败，尝试官方源: {e}")
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


def main():
    parser = argparse.ArgumentParser(description='预训练模型分词（jieba / HanLP / BERT）')
    parser.add_argument('--mode', choices=['all', 'jieba', 'hanlp', 'bert'], default='all', help='选择运行的分词器')
    parser.add_argument('--text', type=str, default='自然语言处理是计算机科学的一个重要分支', help='待分词文本')
    args = parser.parse_args()

    text = args.text

    if args.mode in ('all', 'jieba'):
        try:
            j_res = jieba_segment(text)
            print(f"jieba分词结果: {j_res}")
        except Exception as e:
            print(f"jieba分词出错: {e}")

    if args.mode in ('all', 'hanlp'):
        try:
            h_res = hanlp_segment(text)
            print(f"HanLP分词结果: {h_res}")
        except Exception as e:
            print(f"HanLP分词出错: {e}")

    if args.mode in ('all', 'bert'):
        try:
            b_res = bert_segment(text)
            print(f"BERT分词结果: {b_res}")
        except Exception as e:
            print(f"BERT分词出错: {e}")


if __name__ == '__main__':
    main()


