from collections import defaultdict
import re

def count_words(text):
    # 使用正则表达式分词，将文本中的单词提取出来
    words = re.findall(r'\b\w+\b', text.lower())
    
    # 使用 defaultdict 统计每个单词出现的次数
    word_count = defaultdict(int)
    for word in words:
        word_count[word] += 1
    
    return word_count

# 获取用户输入
text = input("请输入一段英文: ")

# 调用函数统计单词出现次数
word_count = count_words(text)

# 输出结果
print("单词出现次数统计:")
for word, count in word_count.items():
    print(f"{word}: {count}",end=" ")