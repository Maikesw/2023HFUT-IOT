# 定义可用的数字
digits = [1, 2, 3, 4]

# 用于存储所有符合条件的三位数
three_digit_numbers = []

# 使用嵌套循环生成所有可能的三位数
for i in digits:
    for j in digits:
        for k in digits:
            # 确保每个三位数中的数字互不相同
            if i != j and j != k and i != k:
                # 将数字组合成一个三位数
                num = 100 * i + 10 * j + k
                # 将生成的三位数添加到列表中
                three_digit_numbers.append(num)

# 输出结果
print(f"能组成的互不相同且无重复数字的三位数数量: {len(three_digit_numbers)}")
print(f"这些三位数分别是: {three_digit_numbers}")