def is_palindrome(s):
    # 去除字符串中的空格和转换为小写，以忽略大小写和空格的影响
    s = ''.join(c for c in s if c.isalnum()).lower()
    
    # 检查字符串是否等于其逆序字符串
    return s == s[::-1]

# 获取用户输入
input_str = input("请输入一个字符串: ")

# 调用函数并输出结果
result = is_palindrome(input_str)
print(f"字符串 '{input_str}' 是回文字符串: {result}")