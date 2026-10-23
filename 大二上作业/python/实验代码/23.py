import random
import string
import re

def generate_random_string(n):
    # 生成一个长度为 n 的随机字符串，包含字母和数字
    return ''.join(random.choices(string.ascii_letters + string.digits, k=n))

def find_longest_alphabetic_substring(s):
    # 使用正则表达式查找所有由字母组成的子串
    alphabetic_substrings = re.findall(r'[a-zA-Z]+', s)
    
    # 找到最长的子串
    if alphabetic_substrings:
        longest_substring = max(alphabetic_substrings, key=len)
    else:
        longest_substring = None
    
    return longest_substring

# 获取用户输入
n = int(input("请输入字符串的长度 n: "))

# 生成随机字符串
random_string = generate_random_string(n)

# 查找最长的字母子串
longest_substring = find_longest_alphabetic_substring(random_string)

# 输出结果
print(f"生成的随机字符串: {random_string}")
print(f"最长的字母子串: {longest_substring}")