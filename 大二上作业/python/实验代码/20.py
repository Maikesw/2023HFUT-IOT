import random

def generate_filtered_tuple(n, m):
    # 生成一个包含 n 个整数元素的元组，每个元素值不大于 m
    # 过滤掉所有的偶数整数
    return tuple(x for x in (random.randint(1, m) for _ in range(n)) if x % 2 != 0)

# 获取用户输入
n = int(input("请输入元组的元素数量 n: "))
m = int(input("请输入整数的最大值 m: "))

# 生成并过滤元组
filtered_tuple = generate_filtered_tuple(n, m)

# 输出结果
print(f"生成的元组: {filtered_tuple}")