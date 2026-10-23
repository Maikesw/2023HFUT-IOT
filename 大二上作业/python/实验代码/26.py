def reverse_number_and_count_digits(n):
    """
    输出一个不多于5位的正整数的位数，以及逆序输出各位数字。
    
    参数:
    n (int): 不多于5位的正整数
    
    返回:
    tuple: (位数, 逆序数字的列表)
    """
    if n <= 0 or n >= 100000:
        raise ValueError("输入必须是一个不多于5位的正整数")
    
    # 将整数转换为字符串
    n_str = str(n)
    
    # 计算位数
    num_digits = len(n_str)
    
    # 逆序输出各位数字
    reversed_digits = [int(digit) for digit in reversed(n_str)]
    
    return num_digits, reversed_digits

# 示例用法
number = 12345
num_digits, reversed_digits = reverse_number_and_count_digits(number)

print(f"位数: {num_digits}")
print(f"逆序输出各位数字: {reversed_digits}")