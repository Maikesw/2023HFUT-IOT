import random

def g_nums():
    ran_list=[random.randint(1,100) for _ in range(50)]
    return ran_list

def del_nums(lst):
    for i in range (len(lst)-1,-1,-1):
        if lst[i]%2==1:
            del lst[i]
    return lst

def main():
    ran_list=g_nums()
    print(f"生成随机列表{ran_list}")

    eve_list=del_nums(ran_list)
    print(f"删除后的列表{eve_list}")

if __name__ =="__main__":
    main()
    
