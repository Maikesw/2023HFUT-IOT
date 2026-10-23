def sieve_of_eratosthenes(n):
    if n <= 2:
        return []
    
    # 创建一个布尔列表，初始时假设所有数都是素数
    is_prime = [True] * n
    is_prime[0] = is_prime[1] = False  # 0 和 1 不是素数
    
    # 从2开始，遍历到sqrt(n)，标记非素数
    for i in range(2, int(n**0.5) + 1):
        if is_prime[i]:
            for j in range(i*i, n, i):
                is_prime[j] = False
    
    # 收集所有素数
    primes = [i for i in range(2, n) if is_prime[i]]
    
    return primes

# 获取用户输入
n = int(input("请输入一个大于2的自然数: "))

# 调用函数并输出结果
primes = sieve_of_eratosthenes(n)
print(f"小于 {n} 的所有素数: {primes}")