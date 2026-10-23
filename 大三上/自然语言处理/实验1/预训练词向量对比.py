#!/usr/bin/env python3
# -*- coding: utf-8 -*-


import os
import numpy as np
import matplotlib.pyplot as plt
from gensim.models import Word2Vec, KeyedVectors
from sklearn.metrics.pairwise import cosine_similarity
import jieba
import re
from collections import Counter


plt.rcParams['font.sans-serif'] = ['SimHei', 'Microsoft YaHei', 'SimSun', 'Arial Unicode MS']
plt.rcParams['axes.unicode_minus'] = False

def load_corpus(file_path):
    
    encodings = ['utf-8', 'gbk', 'gb2312', 'gb18030', 'big5']
    for enc in encodings:
        try:
            with open(file_path, 'r', encoding=enc) as f:
                return f.read()
        except UnicodeDecodeError:
            continue
    raise ValueError(f"无法读取文件: {file_path}")

def clean_and_segment(text):
    
    text = re.sub(r'[^\u4e00-\u9fa5a-zA-Z0-9]', ' ', text)
    text = re.sub(r'\s+', ' ', text).strip()
    words = jieba.lcut(text)
    return [w for w in words if w.strip()]

def load_pretrained_vectors():
    
    
    pretrained_models = {
        'tencent': {
            'url': 'https://ai.tencent.com/ailab/nlp/embedding/Tencent_AILab_ChineseEmbedding.tar.gz',
            'file': 'Tencent_AILab_ChineseEmbedding.txt',
            'description': '腾讯AI Lab中文词向量'
        },
        'hfl': {
            'url': 'https://github.com/Embedding/Chinese-Word-Vectors',
            'file': 'sgns.weibo.bigram-char',
            'description': '哈工大中文词向量'
        }
    }
    
    
    pretrained_files = []
    for name, info in pretrained_models.items():
        if os.path.exists(info['file']):
            pretrained_files.append((name, info['file']))
        else:
            print(f"未找到预训练文件: {info['file']}")
            print(f"请从 {info['url']} 下载 {info['description']}")
    
    return pretrained_files

def load_model(model_path, is_pretrained=False):
    
    try:
        if is_pretrained:
            
            model = KeyedVectors.load_word2vec_format(model_path, binary=False)
        else:
            
            model = Word2Vec.load(model_path)
        return model
    except Exception as e:
        print(f"加载模型失败: {e}")
        return None

def compare_word_similarity(model1, model2, test_words, model1_name="模型1", model2_name="模型2"):
    
    print(f"\n=== {model1_name} vs {model2_name} 相似词对比 ===")
    
    for word in test_words:
        print(f"\n查询词: '{word}'")
        
        
        if hasattr(model1, 'wv') and word in model1.wv:
            sim1 = model1.wv.most_similar(word, topn=5)
            print(f"{model1_name}: {', '.join([f'{w}:{s:.3f}' for w, s in sim1])}")
        elif not hasattr(model1, 'wv') and word in model1:
            sim1 = model1.most_similar(word, topn=5)
            print(f"{model1_name}: {', '.join([f'{w}:{s:.3f}' for w, s in sim1])}")
        else:
            print(f"{model1_name}: 词不在词汇表中")
        
        
        if hasattr(model2, 'wv') and word in model2.wv:
            sim2 = model2.wv.most_similar(word, topn=5)
            print(f"{model2_name}: {', '.join([f'{w}:{s:.3f}' for w, s in sim2])}")
        elif not hasattr(model2, 'wv') and word in model2:
            sim2 = model2.most_similar(word, topn=5)
            print(f"{model2_name}: {', '.join([f'{w}:{s:.3f}' for w, s in sim2])}")
        else:
            print(f"{model2_name}: 词不在词汇表中")

def compare_vocabulary_coverage(model1, model2, test_words, model1_name="模型1", model2_name="模型2"):
    
    print(f"\n=== 词汇表覆盖率对比 ===")
    
    
    vocab1 = set(model1.wv.index_to_key) if hasattr(model1, 'wv') else set(model1.index_to_key)
    vocab2 = set(model2.wv.index_to_key) if hasattr(model2, 'wv') else set(model2.index_to_key)
    
    print(f"{model1_name} 词汇表大小: {len(vocab1)}")
    print(f"{model2_name} 词汇表大小: {len(vocab2)}")
    
    
    covered1 = sum(1 for w in test_words if w in vocab1)
    covered2 = sum(1 for w in test_words if w in vocab2)
    
    print(f"测试词覆盖率 - {model1_name}: {covered1}/{len(test_words)} ({covered1/len(test_words):.2%})")
    print(f"测试词覆盖率 - {model2_name}: {covered2}/{len(test_words)} ({covered2/len(test_words):.2%})")

def visualize_comparison(model1, model2, test_words, model1_name="模型1", model2_name="模型2"):
    
    from sklearn.decomposition import PCA
    
    
    common_words = []
    for word in test_words:
        in_model1 = word in model1.wv if hasattr(model1, 'wv') else word in model1
        in_model2 = word in model2.wv if hasattr(model2, 'wv') else word in model2
        if in_model1 and in_model2:
            common_words.append(word)
    
    if len(common_words) < 2:
        print("共同词汇太少，无法进行可视化对比")
        return
    
    
    vectors1 = [model1.wv[word] if hasattr(model1, 'wv') else model1[word] for word in common_words]
    vectors2 = [model2.wv[word] if hasattr(model2, 'wv') else model2[word] for word in common_words]
    
    
    pca = PCA(n_components=2)
    coords1 = pca.fit_transform(vectors1)
    coords2 = pca.fit_transform(vectors2)
    
    
    fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(15, 6))
    
    
    ax1.scatter(coords1[:, 0], coords1[:, 1], c='blue', alpha=0.7)
    for i, word in enumerate(common_words):
        ax1.annotate(word, (coords1[i, 0], coords1[i, 1]))
    ax1.set_title(f'{model1_name} 词向量分布')
    ax1.grid(True)
    
    
    ax2.scatter(coords2[:, 0], coords2[:, 1], c='red', alpha=0.7)
    for i, word in enumerate(common_words):
        ax2.annotate(word, (coords2[i, 0], coords2[i, 1]))
    ax2.set_title(f'{model2_name} 词向量分布')
    ax2.grid(True)
    
    plt.tight_layout()
    plt.savefig('预训练vs自训练词向量对比.png', dpi=150, bbox_inches='tight')
    plt.show()

def main():
    
    print("预训练词向量与自训练词向量对比实验")
    print("=" * 50)
    
    
    print("加载语料...")
    ci_text = load_corpus('Ci.txt')
    news_text = load_corpus('1998-01-2003版-带音.txt')
    
    
    ci_words = clean_and_segment(ci_text)
    news_words = clean_and_segment(news_text)
    
    
    test_words = ['春', '月', '花', '人', '中国', '经济', '发展', '技术', '学习', '工作']
    
    
    pretrained_files = load_pretrained_vectors()
    
    
    if os.path.exists("sample_pretrained.txt"):
        pretrained_files.append(("sample", "sample_pretrained.txt"))
    
    
    print("加载自训练模型...")
    ci_model = load_model('ci_word2vec.model')
    news_model = load_model('news_word2vec.model')
    
    if ci_model is None or news_model is None:
        print("请先运行 实验1.py 生成自训练模型")
        return
    
    
    if pretrained_files:
        for name, file_path in pretrained_files:
            print(f"\n加载预训练模型: {name}")
            pretrained_model = load_model(file_path, is_pretrained=True)
            
            if pretrained_model is not None:
                
                compare_word_similarity(ci_model, pretrained_model, test_words, 
                                      "自训练(宋词)", f"预训练({name})")
                compare_vocabulary_coverage(ci_model, pretrained_model, test_words,
                                          "自训练(宋词)", f"预训练({name})")
                
                
                visualize_comparison(ci_model, pretrained_model, test_words,
                                   "自训练(宋词)", f"预训练({name})")
    else:
        print("\n未找到预训练词向量文件，仅展示自训练模型结果")
        
        
        print("\n=== 自训练模型结果 ===")
        print("宋词模型相似词:")
        for word in test_words[:5]:  
            if word in ci_model.wv:
                sim = ci_model.wv.most_similar(word, topn=5)
                print(f"{word}: {', '.join([f'{w}:{s:.3f}' for w, s in sim])}")
        
        print("\n新闻模型相似词:")
        for word in test_words[5:]:  
            if word in news_model.wv:
                sim = news_model.wv.most_similar(word, topn=5)
                print(f"{word}: {', '.join([f'{w}:{s:.3f}' for w, s in sim])}")

if __name__ == "__main__":
    main()
