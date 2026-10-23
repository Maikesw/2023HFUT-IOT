def fibonacci_below_n(n):
    # 初始化斐波那契数列的前两个值
    fib_sequence = []
    a, b = 0, 1

    # 生成斐波那契数列
    while a < n:
        fib_sequence.append(a)
        a, b = b, a + b

    return fib_sequence

def main():
    # 用户输入
    n = int(input("请输入一个正整数 n: "))
    
    # 计算斐波那契数列中小于 n 的所有值
    result = fibonacci_below_n(n)
    
    # 打印结果
    print(f"斐波那契数列中小于 {n} 的所有值是: {result}")

if __name__ == "__main__":
    main()