def josephus_circle(n, k=3):
    """
    求解约瑟夫环问题
    :param n: 人数
    :param k: 报数的上限（默认为3）
    :return: 最后留下的人的原始编号
    """
    # 初始化一个列表，表示n个人的编号
    people = list(range(1, n + 1))
    
    # 从第一个人开始报数
    index = 0
    
    while len(people) > 1:
        # 找到报数到k的人的索引
        index = (index + k - 1) % len(people)
        
        # 报数到k的人退出圈子
        people.pop(index)
    
    # 返回最后留下的人的编号
    return people[0]

if __name__ == "__main__":
    # 用户输入人数
    n = int(input("请输入人数 n: "))
    
    # 解决约瑟夫环问题
    last_person = josephus_circle(n)
    
    # 输出最后留下的人的原始编号
    print(f"最后留下的人是原来第 {last_person} 号的那位")