import random

def guess():
    target = random.randint(1, 100)
    attempts = 0
    max_attempts = 7
    print("猜数字游戏开始!范围在1到100之间,你有7次机会。")

    while attempts < max_attempts:
        attempts += 1
        guess = int(input(f"第{attempts}次猜测，请输入一个数字: "))
        if guess == target:
            print("恭喜你，猜对了！")
            return
        elif guess < target:
            print("太小了！")
        else:
            print("太大了！")
    
    print(f"游戏结束，正确的数字是 {target}")

guess()