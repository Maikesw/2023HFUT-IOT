#!/usr/bin/env python3
# -*- coding: utf-8 -*-


import os
import numpy as np

def create_sample_pretrained():
    
    print("创建示例预训练词向量文件...")
    
    
    sample_words = [
        "中国", "经济", "发展", "技术", "学习", "工作", "生活", "人民", "国家", "社会",
        "春", "夏", "秋", "冬", "花", "月", "风", "雨", "山", "水",
        "人", "家", "爱", "美", "好", "大", "小", "高", "低", "新",
        "年", "月", "日", "时", "分", "秒", "今", "明", "昨", "前",
        "北京", "上海", "广州", "深圳", "杭州", "南京", "武汉", "成都", "西安", "重庆",
        "大学", "中学", "小学", "教育", "文化", "艺术", "音乐", "电影", "书籍", "知识"
    ]
    
    
    np.random.seed(42)
    vectors = np.random.randn(len(sample_words), 100)
    
    
    with open("sample_pretrained.txt", "w", encoding="utf-8") as f:
        f.write(f"{len(sample_words)} 100\n")  
        for word, vector in zip(sample_words, vectors):
            vector_str = " ".join([f"{v:.6f}" for v in vector])
            f.write(f"{word} {vector_str}\n")
    
    print("示例预训练词向量文件已创建: sample_pretrained.txt")
    return "sample_pretrained.txt"

def create_sample_pretrained():
    
    print("创建示例预训练词向量文件...")
    
    
    sample_words = [
        "中国", "经济", "发展", "技术", "学习", "工作", "生活", "人民", "国家", "社会",
        "春", "夏", "秋", "冬", "花", "月", "风", "雨", "山", "水",
        "人", "家", "爱", "美", "好", "大", "小", "高", "低", "新",
        "年", "月", "日", "时", "分", "秒", "今", "明", "昨", "前"
    ]
    
    
    np.random.seed(42)
    vectors = np.random.randn(len(sample_words), 100)
    
    
    with open("sample_pretrained.txt", "w", encoding="utf-8") as f:
        f.write(f"{len(sample_words)} 100\n")  
        for word, vector in zip(sample_words, vectors):
            vector_str = " ".join([f"{v:.6f}" for v in vector])
            f.write(f"{word} {vector_str}\n")
    
    print("示例预训练词向量文件已创建: sample_pretrained.txt")
    return "sample_pretrained.txt"

def main():
    
    print("中文预训练词向量下载工具")
    print("=" * 40)
    
    
    existing_files = []
    for filename in ["Tencent_AILab_ChineseEmbedding.txt", "sgns.weibo.bigram-char", 
                     "sample_pretrained.txt"]:
        if os.path.exists(filename):
            existing_files.append(filename)
            print(f"发现现有文件: {filename}")
    
    if existing_files:
        print(f"\n已有 {len(existing_files)} 个预训练词向量文件")
        return existing_files
    
    print("\n可用的中文预训练词向量资源:")
    print("1. 腾讯AI Lab词向量 (推荐)")
    print("   - 下载地址: https://ai.tencent.com/ailab/nlp/embedding/")
    print("   - 文件: Tencent_AILab_ChineseEmbedding.tar.gz")
    print("   - 解压后: Tencent_AILab_ChineseEmbedding.txt")
    
    print("\n2. 哈工大词向量")
    print("   - 下载地址: https://github.com/Embedding/Chinese-Word-Vectors")
    print("   - 文件: sgns.weibo.bigram-char")
    
    print("\n3. 搜狗词向量")
    print("   - 下载地址: https://github.com/Embedding/Chinese-Word-Vectors")
    print("   - 文件: sgns.sogou.bigram-char")
    
    print("\n由于文件较大，建议手动下载。")
    print("现在创建一个示例文件用于演示...")
    
    
    sample_file = create_sample_pretrained()
    return [sample_file]

if __name__ == "__main__":
    import numpy as np
    main()
