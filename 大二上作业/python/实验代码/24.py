def binary_search(arr, target):
    """
    在已排序的列表中查找目标值的位置。
    
    参数:
    arr (list): 已排序的列表
    target: 需要查找的目标值
    
    返回:
    int: 目标值的索引，如果目标值不存在则返回 -1
    """
    left, right = 0, len(arr) - 1
    
    while left <= right:
        mid = (left + right) // 2
        if arr[mid] == target:
            return mid
        elif arr[mid] < target:
            left = mid + 1
        else:
            right = mid - 1
    
    return -1

# 示例用法
sorted_list = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
target = 7
result = binary_search(sorted_list, target)

if result != -1:
    print(f"目标值 {target} 在列表中的索引为 {result}")
else:
    print(f"目标值 {target} 不在列表中")