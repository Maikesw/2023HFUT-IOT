import random
import string

def generate_nested_list(n, m):
    # 定义生成随机字符串的函数
    def generate_random_string(max_length):
        # 可用的字符集
        characters = string.ascii_letters + string.digits
        # 生成长度不超过 max_length 的随机字符串
        length = random.randint(1, max_length)
        random_string = ''.join(random.choice(characters) for _ in range(length))
        return random_string

    # 生成 n 个嵌套列表
    nested_list = [[generate_random_string(m) for _ in range(n)] for _ in range(n)]
    return nested_list

def sort_strings_by_length(strings):
    # 按字符串长度降序排序
    sorted_strings = sorted(strings, key=len, reverse=True)
    return sorted_strings

# 获取用户输入
n = int(input("请输入嵌套列表的元素数量 n: "))
m = int(input("请输入字符串的最大长度 m: "))

# 生成嵌套列表
nested_list = generate_nested_list(n, m)

# 打印嵌套列表
print("生成的嵌套列表:")
for sublist in nested_list:
    print(sublist)

# 将嵌套列表中的所有字符串提取到一个列表中
all_strings = [item for sublist in nested_list for item in sublist]

# 按字符串长度降序排序
sorted_strings = sort_strings_by_length(all_strings)

# 打印排序后的字符串
print("按字符串长度降序排序后的结果:")
print(sorted_strings)