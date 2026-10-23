import random
from collections import Counter

# 定义生成随机字符串的函数
def generate_random_string(length):
    # 可用的字符集
    characters = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'
    # 生成指定长度的随机字符串
    random_string = ''.join(random.choice(characters) for _ in range(length))
    return random_string

# 定义统计字符出现次数的函数
def count_characters(s):
    # 使用 Counter 统计每个字符的出现次数
    char_count = Counter(s)
    return char_count

# 生成包含1000个随机字符的字符串
random_string = generate_random_string(1000)

# 统计每个字符出现的次数
char_count = count_characters(random_string)

# 输出结果
print(f"生成的随机字符串: {random_string}")
print("每个字符出现的次数:")
for char, count in char_count.items():
    print(f"{char}: {count}",end=" ")