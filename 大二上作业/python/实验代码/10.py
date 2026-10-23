def contains_apple(s):
    # 将字符串转换为小写
    s_lower = s.lower()
    
    # 检查字符串中是否包含 "apple"
    if "apple" in s_lower:
        return "yes"
    else:
        return "no"

# 获取用户输入
input_str = input("请输入一个字符串: ")

# 调用函数并输出结果
result = contains_apple(input_str)
print(result)