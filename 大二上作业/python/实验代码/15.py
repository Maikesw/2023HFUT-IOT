def count_characters(s):
    # 初始化计数器
    counts = {
        'digits': 0,
        'letters': 0,
        'spaces': 0,
        'others': 0
    }

    # 遍历字符串中的每个字符
    for char in s:
        if char.isdigit():
            counts['digits'] += 1
        elif char.isalpha():
            counts['letters'] += 1
        elif char.isspace():
            counts['spaces'] += 1
        else:
            counts['others'] += 1

    return counts

# 测试示例
input_string = "Hello 123, this is a test 456!"
result = count_characters(input_string)

# 输出结果
print(f"输入字符串: {input_string}")
print(f"数字数量: {result['digits']}")
print(f"字母数量: {result['letters']}")
print(f"空格数量: {result['spaces']}")
print(f"其他字符数量: {result['others']}")