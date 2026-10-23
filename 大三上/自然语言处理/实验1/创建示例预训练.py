#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
创建示例预训练词向量文件
"""

import os
import numpy as np

def create_sample_pretrained():
    """创建一个示例预训练词向量文件（用于演示）"""
    print("创建示例预训练词向量文件...")
    
    # 一些常见的中文词及其简单向量表示
    sample_words = [
        "中国", "经济", "发展", "技术", "学习", "工作", "生活", "人民", "国家", "社会",
        "春", "夏", "秋", "冬", "花", "月", "风", "雨", "山", "水",
        "人", "家", "爱", "美", "好", "大", "小", "高", "低", "新",
        "年", "月", "日", "时", "分", "秒", "今", "明", "昨", "前",
        "北京", "上海", "广州", "深圳", "杭州", "南京", "武汉", "成都", "西安", "重庆",
        "大学", "中学", "小学", "教育", "文化", "艺术", "音乐", "电影", "书籍", "知识"
    ]
    
    # 生成简单的随机向量
    np.random.seed(42)
    vectors = np.random.randn(len(sample_words), 100)
    
    # 写入文件
    with open("sample_pretrained.txt", "w", encoding="utf-8") as f:
        f.write(f"{len(sample_words)} 100\n")  # 词汇表大小和向量维度
        for word, vector in zip(sample_words, vectors):
            vector_str = " ".join([f"{v:.6f}" for v in vector])
            f.write(f"{word} {vector_str}\n")
    
    print("示例预训练词向量文件已创建: sample_pretrained.txt")
    return "sample_pretrained.txt"

if __name__ == "__main__":
    create_sample_pretrained()
