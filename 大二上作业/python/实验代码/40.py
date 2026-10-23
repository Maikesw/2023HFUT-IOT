def is_divisible_by_5(binary_str):
    """
    检查二进制字符串表示的数字是否可以被5整除
    :param binary_str: 4位二进制字符串
    :return: 布尔值，表示该数字是否可以被5整除
    """
    # 将二进制字符串转换为十进制整数
    decimal_number = int(binary_str, 2)
    
    # 检查该数字是否可以被5整除
    return decimal_number % 5 == 0

def check_binary_numbers(input_string):
    """
    检查输入字符串中的所有4位二进制数，返回可以被5整除的数字列表
    :param input_string: 逗号分隔的4位二进制字符串
    :return: 可以被5整除的4位二进制数字列表
    """
    divisible_by_5 = []
    binary_numbers = input_string.split(',')
    
    for binary_number in binary_numbers:
        # 去除前后空白字符并检查是否为4位二进制数
        binary_number = binary_number.strip()
        if len(binary_number) == 4 and binary_number.isdigit() and all(c in '01' for c in binary_number):
            if is_divisible_by_5(binary_number):
                divisible_by_5.append(binary_number)
        else:
            print(f"警告：无效的二进制数 - {binary_number}")
    
    return divisible_by_5

if __name__ == "__main__":
    # 用户输入一系列逗号分隔的4位二进制数
    input_string = input("请输入一系列逗号分隔的4位二进制数: ")

    # 检查并获取可以被5整除的二进制数
    divisible_by_5 = check_binary_numbers(input_string)

    # 输出结果
    if divisible_by_5:
        print("能被5整除的数字: " + ', '.join(divisible_by_5))
    else:
        print("没有能被5整除的数字")