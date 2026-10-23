#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
命名实体识别（NER）独立实现

包含三种方式：
- jieba（词性标注+简单规则）：人名/地名/机构名
- HanLP（MSRA_NER_BERT_BASE_ZH）：深度模型
- BERT（演示版，未微调，仅示例流程）：可能需联网与镜像

运行示例：
- 运行全部：
    python ner_recognition.py
- 指定文本：
    python ner_recognition.py --text "李明在北京大学计算机科学系学习自然语言处理"
- 仅运行某一模型：
    python ner_recognition.py --mode jieba|hanlp|bert --text "..."
"""

import argparse
from typing import List, Tuple


def jieba_ner(text: str) -> Tuple[List[Tuple[str, str]], List[Tuple[str, str]]]:
    import jieba.posseg as pseg
    words_with_pos = [(word, pos) for word, pos in pseg.cut(text)]
    entities: List[Tuple[str, str]] = []
    for word, pos in words_with_pos:
        if pos.startswith('nr'):
            entities.append((word, 'PER'))
        elif pos.startswith('ns'):
            entities.append((word, 'LOC'))
        elif pos.startswith('nt'):
            entities.append((word, 'ORG'))
    return words_with_pos, entities


def hanlp_ner(text: str):
    try:
        import hanlp
    except Exception as e:
        raise RuntimeError(f"HanLP 未安装或不可用: {e}")
    # 优先使用不依赖 TensorFlow 的 Electra NER 模型（PyTorch 版）
    for model_name in [
        'NER_ELECTRA_BASE_CRF',          # 通用中文 NER（Electra-Base-CRF）
        'MSRA_NER_ELECTRA_BASE_ZH',      # MSRA 中文 NER（Electra-Base）
        'NER_ELECTRA_SMALL_ZH'           # 轻量中文 NER（Electra-Small）
    ]:
        try:
            ner_model = hanlp.load(model_name)
            return ner_model(text)
        except Exception:
            continue
    # 最后再尝试原 BERT 模型（可能需 TensorFlow）
    ner_model = hanlp.load('MSRA_NER_BERT_BASE_ZH')
    return ner_model(text)


def prepare_bert_ner_data(text, tokenizer):
    import torch
    tokens = tokenizer.tokenize(text)
    tokens = ["[CLS]"] + tokens + ["[SEP]"]
    input_ids = tokenizer.convert_tokens_to_ids(tokens)
    attention_mask = [1] * len(input_ids)
    return {
        "input_ids": torch.tensor([input_ids]),
        "attention_mask": torch.tensor([attention_mask])
    }


def bert_ner(text: str, model_name: str = "bert-base-chinese"):
    """
    说明：此为演示流程，直接用预训练BERT做TokenClassification（未微调），
    预测标签仅作示例用途，真实任务应加载在MSRA NER上微调过的检查点。
    """
    try:
        from transformers import BertTokenizer, BertForTokenClassification
    except Exception as e:
        raise RuntimeError(f"transformers 未安装或不可用: {e}")

    # 优先尝试镜像与本地缓存
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
        print(f"使用镜像加载BERT失败，尝试官方源: {e}")
        tokenizer = BertTokenizer.from_pretrained(model_name)
        model = BertForTokenClassification.from_pretrained(model_name, num_labels=9)

    import torch
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
        7: "B-MISC", 8: "I-MISC"
    }
    results = []
    for token, prediction in zip(tokens[1:-1], predictions[0][1:-1]):
        results.append((token, id2label[prediction.item()]))
    return results


def main():
    parser = argparse.ArgumentParser(description='命名实体识别（jieba / HanLP / BERT）')
    parser.add_argument('--mode', choices=['all', 'jieba', 'hanlp', 'bert'], default='all', help='选择运行的NER方法')
    parser.add_argument('--text', type=str, default='李明在北京大学计算机科学系学习自然语言处理', help='待分析文本')
    args = parser.parse_args()

    text = args.text

    if args.mode in ('all', 'jieba'):
        try:
            pos, ents = jieba_ner(text)
            print(f"jieba词性标注结果: {pos}")
            print(f"jieba命名实体识别结果: {ents}")
        except Exception as e:
            print(f"jieba NER 出错: {e}")

    if args.mode in ('all', 'hanlp'):
        try:
            h_ents = hanlp_ner(text)
            print(f"HanLP命名实体识别结果: {h_ents}")
        except Exception as e:
            print(f"HanLP NER 出错: {e}")

    if args.mode in ('all', 'bert'):
        try:
            b_ents = bert_ner(text)
            print(f"BERT命名实体识别结果: {b_ents}")
        except Exception as e:
            print(f"BERT NER 出错: {e}")


if __name__ == '__main__':
    main()


