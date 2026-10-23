def find_duplicates(arr):
    """
    在列表中查找重复的数字及其重复次数。
    
    参数:
    arr (list): 列表，包含整数
    
    返回:
    dict: 重复的数字及其出现次数
    """
    # 使用字典记录每个数字出现的次数
    count_dict = {}
    for num in arr:
        if num in count_dict:
            count_dict[num] += 1
        else:
            count_dict[num] = 1
    
    # 找出重复的数字及其出现次数
    duplicates = {num: count for num, count in count_dict.items() if count > 1}
    
    return duplicates

# 示例用法
numbers = [1, 2, 3, 4, 2, 3, 6, 7, 5, 1, 8, 9, 7, 10, 10]
result = find_duplicates(numbers)

if result:
    for num, count in result.items():
        print(f"数字 {num} 重复了 {count} 次")
else:
    print("列表中没有重复的数字")