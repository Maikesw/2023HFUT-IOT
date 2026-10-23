import random

def generate_integer_list(length, min_val, max_val):
    # 生成一个长度为 length 的整型列表，元素在 min_val 和 max_val 之间
    return [random.randint(min_val, max_val) for _ in range(length)]

def reverse_list(input_list):
    # 生成原列表的逆序列表
    return input_list[::-1]

def get_even_index_elements(input_list):
    # 生成包含原列表中所有偶数位置的元素的新列表
    # 切片从索引 0 开始，偶数位置的索引是 0, 2, 4, ...
    return input_list[::2]

# 获取用户输入
length = int(input("请输入列表的长度: "))
min_val = int(input("请输入整数的最小值: "))
max_val = int(input("请输入整数的最大值: "))

# 生成整型列表
original_list = generate_integer_list(length, min_val, max_val)

# 生成新列表、逆序列表和偶数位置元素列表
new_list = original_list.copy()
reversed_list = reverse_list(original_list)
even_index_elements_list = get_even_index_elements(original_list)

# 输出结果
print(f"原列表: {original_list}")
print(f"新列表: {new_list}")
print(f"逆序列表: {reversed_list}")
print(f"偶数位置元素列表: {even_index_elements_list}")