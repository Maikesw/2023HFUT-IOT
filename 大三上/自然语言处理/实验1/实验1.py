import os
import re
import sys
import jieba
import numpy as np
import matplotlib
matplotlib.use('Agg')  # 无界面环境下保存图片
# 配置中文字体，避免中文字符显示为方块与负号显示问题
matplotlib.rcParams['font.sans-serif'] = ['SimHei', 'Microsoft YaHei', 'SimSun', 'Arial Unicode MS']
matplotlib.rcParams['axes.unicode_minus'] = False
import matplotlib.pyplot as plt
from collections import Counter
from gensim.models import Word2Vec
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.decomposition import PCA
from sklearn.metrics.pairwise import cosine_similarity

DATA_CI = 'Ci.txt'
DATA_NEWS = '1998-01-2003版-带音.txt'


def load_corpus(file_path: str, encodings=None) -> str:
    if encodings is None:
        encodings = ['utf-8', 'gbk', 'gb2312', 'gb18030', 'big5']
    last_error = None
    for enc in encodings:
        try:
            with open(file_path, 'r', encoding=enc) as f:
                return f.read()
        except UnicodeDecodeError as e:
            last_error = e
            continue
    raise ValueError(f'无法识别文件编码: {file_path}') from last_error


def clean_text(text: str) -> str:
    text = re.sub(r'[^\u4e00-\u9fa5a-zA-Z0-9]', ' ', text)
    text = re.sub(r'\s+', ' ', text).strip()
    return text


def segment_text(text: str):
    words = jieba.lcut(text)
    return [w for w in words if w.strip()]


def count_word_frequency(words):
    return Counter(words)


def build_ngram(words, n=2):
    grams = []
    for i in range(len(words) - n + 1):
        grams.append(' '.join(words[i:i + n]))
    return Counter(grams)


def save_dict_to_file(counter_obj: Counter, file_path: str, top_n=None):
    pairs = counter_obj.most_common(top_n)
    with open(file_path, 'w', encoding='utf-8') as f:
        for k, v in pairs:
            f.write(f"{k}\t{v}\n")


def plot_top20(counter_obj: Counter, title: str, out_path: str):
    words, freqs = zip(*counter_obj.most_common(20)) if counter_obj else ([], [])
    plt.figure(figsize=(10, 5))
    if words:
        plt.bar(words, freqs)
        plt.xticks(rotation=45, ha='right')
        plt.ylabel('频次')
        plt.title(title)
        plt.tight_layout()
    else:
        plt.text(0.5, 0.5, '无数据', ha='center', va='center')
    plt.savefig(out_path, dpi=150)
    plt.close()


def prepare_sentences(words, window_size=20):
    sentences = []
    for i in range(0, len(words), window_size):
        sentences.append(words[i:i + window_size])
    return sentences


def train_word2vec(sentences, vector_size=100, window=5, min_count=5):
    model = Word2Vec(
        sentences=sentences,
        vector_size=vector_size,
        window=window,
        min_count=min_count,
        workers=4,
        sg=1
    )
    return model


def visualize_word_vectors(model, top_n: int, out_path: str, title: str):
    words = list(model.wv.index_to_key)[:top_n]
    vectors = [model.wv[w] for w in words]
    if not vectors:
        return
    pca = PCA(n_components=2)
    reduced = pca.fit_transform(vectors)
    plt.figure(figsize=(10, 7))
    plt.scatter(reduced[:, 0], reduced[:, 1], c='b', alpha=0.6)
    for i, w in enumerate(words):
        plt.annotate(w, (reduced[i, 0], reduced[i, 1]))
    plt.title(title)
    plt.grid(True)
    plt.tight_layout()
    plt.savefig(out_path, dpi=150)
    plt.close()


def sentence_vector_average(sentence_tokens, model):
    vecs = [model.wv[w] for w in sentence_tokens if w in model.wv]
    if vecs:
        return np.mean(vecs, axis=0)
    return np.zeros(model.wv.vector_size)


def compare_bow_vs_w2v(sentences_tokens, model):
    texts = [' '.join(s) for s in sentences_tokens]
    vectorizer = CountVectorizer()
    bow = vectorizer.fit_transform(texts)
    print('BoW 维度:', bow.shape, '词表大小:', len(vectorizer.vocabulary_))
    print('W2V 维度:', model.wv.vector_size, '词表大小:', len(model.wv.index_to_key))
    if len(sentences_tokens) >= 2:
        bow_sim = cosine_similarity(bow[0:1], bow[1:2])[0][0]
        v1 = sentence_vector_average(sentences_tokens[0], model)
        v2 = sentence_vector_average(sentences_tokens[1], model)
        w2v_sim = cosine_similarity([v1], [v2])[0][0]
        print(f'前两句相似度：BoW={bow_sim:.4f}, W2V={w2v_sim:.4f}')


def main():
    # 检查语料
    for p in [DATA_CI, DATA_NEWS]:
        if not os.path.exists(p):
            print(f'缺少语料文件: {p}，请放置到当前目录。')
            return 1

    # 读取与预处理
    ci_raw = load_corpus(DATA_CI)
    news_raw = load_corpus(DATA_NEWS)
    ci_clean = clean_text(ci_raw)
    news_clean = clean_text(news_raw)
    ci_words = segment_text(ci_clean)
    news_words = segment_text(news_clean)
    print('宋词样例分词:', ci_words[:20])
    print('新闻样例分词:', news_words[:20])

    # 词频与可视化
    ci_word_freq = count_word_frequency(ci_words)
    news_word_freq = count_word_frequency(news_words)
    plot_top20(ci_word_freq, '宋词 Top-20 词频', 'ci_freq_top20.png')
    plot_top20(news_word_freq, '新闻 Top-20 词频', 'news_freq_top20.png')
    print('已保存词频图：ci_freq_top20.png, news_freq_top20.png')

    # N-gram 与保存
    ci_bigrams = build_ngram(ci_words, 2)
    news_bigrams = build_ngram(news_words, 2)
    save_dict_to_file(ci_word_freq, 'ci_word_dict.txt')
    save_dict_to_file(news_word_freq, 'news_word_dict.txt')
    save_dict_to_file(ci_bigrams, 'ci_bigram_dict.txt')
    save_dict_to_file(news_bigrams, 'news_bigram_dict.txt')
    print('已保存词典与bigram：ci_word_dict.txt, news_word_dict.txt, ci_bigram_dict.txt, news_bigram_dict.txt')

    # Word2Vec 训练与可视化
    ci_sentences = prepare_sentences(ci_words)
    news_sentences = prepare_sentences(news_words)
    ci_w2v = train_word2vec(ci_sentences)
    news_w2v = train_word2vec(news_sentences)
    ci_w2v.save('ci_word2vec.model')
    news_w2v.save('news_word2vec.model')
    print('已保存模型：ci_word2vec.model, news_word2vec.model')

    visualize_word_vectors(ci_w2v, top_n=50, out_path='ci_w2v_pca.png', title='Ci Word Vectors (PCA)')
    visualize_word_vectors(news_w2v, top_n=50, out_path='news_w2v_pca.png', title='News Word Vectors (PCA)')
    print('已保存词向量可视化：ci_w2v_pca.png, news_w2v_pca.png')

    # 相似度与 BoW 对比
    print('宋词相似词：')
    for w in ['春', '月', '花', '人']:
        if w in ci_w2v.wv:
            sims = ci_w2v.wv.most_similar(w, topn=10)
            print(w, '->', ', '.join([f"{sw}:{sim:.3f}" for sw, sim in sims]))
        else:
            print(w, '不在词表中')

    print('新闻相似词：')
    for w in ['中国', '经济', '发展', '技术']:
        if w in news_w2v.wv:
            sims = news_w2v.wv.most_similar(w, topn=10)
            print(w, '->', ', '.join([f"{sw}:{sim:.3f}" for sw, sim in sims]))
        else:
            print(w, '不在词表中')

    print('宋词 BoW 与 W2V 对比：')
    compare_bow_vs_w2v(ci_sentences[:100], ci_w2v)
    print('新闻 BoW 与 W2V 对比：')
    compare_bow_vs_w2v(news_sentences[:100], news_w2v)

    return 0


if __name__ == '__main__':
    sys.exit(main())
