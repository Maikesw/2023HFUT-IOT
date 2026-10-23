import random

def generate_random_list(length=20, min_val=1, max_val=100):
    # 生成一个包含 20 个随机整数的列表
    random_list = [random.randint(min_val, max_val) for _ in range(length)]
    return random_list

def sort_even_indices(lst):
    # 提取偶数下标的元素
    even_indices = [i for i in range(len(lst)) if i % 2 == 0]
    even_elements = [lst[i] for i in even_indices]
    
    # 对偶数下标的元素进行降序排列
    even_elements.sort(reverse=True)
    
    # 将排序后的偶数下标元素重新放回原列表
    for i, value in zip(even_indices, even_elements):
        lst[i] = value
    
    return lst

def main():
    # 生成随机列表
    random_list = generate_random_list()
    print(f"生成的随机列表: {random_list}")

    # 对偶数下标的元素进行降序排列
    sorted_list = sort_even_indices(random_list)
    print(f"偶数下标元素降序排列后的列表: {sorted_list}")

if __name__ == "__main__":
    main()