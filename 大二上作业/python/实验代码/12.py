import math

def find_effort(target_final_multiply, work_days, rest_days):
    # 定义方程的变量
    x = 0.01  # 初始猜测值
    epsilon = 1e-6  # 精度阈值
    
    # 使用二分法找到x
    low = 0
    high = 1
    
    while high - low > epsilon:
        mid = (low + high) / 2
        current_final_multiply = (1 + mid) ** work_days * (1 - 0.01) ** rest_days
        
        if current_final_multiply < target_final_multiply:
            low = mid
        else:
            high = mid
    
    return (low + high) / 2

# 一年有365天，每周工作5天，休息2天
work_days = 52 * 5
rest_days = 52 * 2
target_final_multiply = 37.78

# 调用函数找到工作日的努力程度
effort = find_effort(target_final_multiply, work_days, rest_days)
print(f"工作日需要努力的程度: {effort:.4%}")