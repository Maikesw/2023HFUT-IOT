def sort_words(input_string):
    # 将输入字符串按逗号分隔成单词列表
    words = input_string.split(',')
    # 对单词列表进行排序
    sorted_words = sorted(words)
    # 将排序后的单词列表重新组合成逗号分隔的字符串
    sorted_string = ','.join(sorted_words)
    return sorted_string

# 测试示例
input_string = "without,hello,bag,world"
sorted_string = sort_words(input_string)

# 输出结果
print(f"输入序列: {input_string}")
print(f"排序后的序列: {sorted_string}")