def calculate_s(a, n):
    """
    计算 s = a + aa + aaa + ... + a...a 的值。
    
    参数:
    a (int): 1到9之间的整数
    n (int): 相加的项数
    
    返回:
    int: 计算结果
    """
    if a < 1 or a > 9:
        raise ValueError("a 必须是一个1到9之间的整数")
    if n < 1:
        raise ValueError("n 必须是一个正整数")
    
    # 初始化结果变量
    result = 0
    
    # 生成每一项并累加
    current_term = 0
    for i in range(1, n + 1):
        current_term = current_term * 10 + a
        result += current_term
    
    return result

# 示例用法
a = 2
n = 5
s = calculate_s(a, n)

print(f"计算结果: {s}")