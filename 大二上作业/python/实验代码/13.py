# 定义一个lambda函数来查找两个字符串首尾交叉的最大子串长度
find_max_overlap = lambda s1, s2: max((i for i in range(min(len(s1), len(s2)) + 1) if s1[-i:] == s2[:i]), default=0)

# 定义一个函数来连接两个字符串，首尾交叉部分只保留一份
def connect_strings(s1, s2):
    max_overlap = find_max_overlap(s1, s2)
    # 连接字符串，首尾交叉部分只保留一份
    return s1 + s2[max_overlap:]

# 测试示例
s1 = "1234"
s2 = "2347"

# 调用函数并输出结果
result = connect_strings(s1, s2)
print(f"连接后的字符串: {result}")