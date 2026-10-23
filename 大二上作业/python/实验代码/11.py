import random
def generate_random(n):
    return [random.randint(1,100) for _ in range(n)]

def pro_list(lst):
    aver=sum(lst)/len(lst)
    
    greater_list=[i for i in lst if i>aver]
    return (aver,*greater_list)
n = int(input("请输入列表的长度: "))

# 生成随机列表
random_list = generate_random(n)

# 处理列表并输出结果
result = pro_list(random_list)
print(f"生成的随机列表: {random_list}")
print(f"处理结果: {result}")