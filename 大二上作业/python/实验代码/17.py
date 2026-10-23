def process_words(input_string):
    # 将输入字符串按空格分隔成单词列表
    words = input_string.split()
    # 使用 set 去重
    unique_words = set(words)
    # 对去重后的单词列表进行排序
    sorted_unique_words = sorted(unique_words)
    # 将排序后的单词列表重新组合成空格分隔的字符串
    result_string = ' '.join(sorted_unique_words)
    return result_string

# 测试示例
input_string = "hello world hello bag world without"
result_string = process_words(input_string)

# 输出结果
print(f"输入序列: {input_string}")
print(f"处理后的序列: {result_string}")