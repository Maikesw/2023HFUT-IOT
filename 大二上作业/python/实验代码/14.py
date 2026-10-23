#14、写一个删除列表中重复元素的函数，要求去重后元素相对位置保持不变。
def remove_duplicates(lst):
    seen = set()  # 用于记录已经出现过的元素
    result = []  # 用于存储去重后的元素

    for item in lst:
        if item not in seen:
            seen.add(item)  # 将元素添加到集合中
            result.append(item)  # 将元素添加到结果列表中

    return result

# 测试示例
original_list = [1, 2, 3, 2, 1, 4, 5, 3, 6, 7, 5]
unique_list = remove_duplicates(original_list)

# 输出结果
print(f"原始列表: {original_list}")
print(f"去重后的列表: {unique_list}")