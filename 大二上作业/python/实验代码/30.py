class Employee:
    def __init__(self, name, id, monthly_salary):
        """
        初始化雇员对象
        :param name: 姓名
        :param id: 编号
        :param monthly_salary: 月薪
        """
        self.name = name
        self.id = id
        self.monthly_salary = monthly_salary

    def pay(self):
        """
        计算雇员的月薪
        :return: 月薪
        """
        return self.monthly_salary

    def show(self):
        """
        显示雇员的信息
        """
        print(f"姓名: {self.name}, 编号: {self.id}, 月薪: {self.monthly_salary}")

class Manager(Employee):
    def __init__(self, name, id, monthly_salary, performance_bonus):
        """
        初始化经理对象
        :param performance_bonus: 绩效奖金
        """
        super().__init__(name, id, monthly_salary)
        self.performance_bonus = performance_bonus

    def pay(self):
        """
        重新计算经理的月薪，包括绩效奖金
        :return: 月薪 + 绩效奖金
        """
        return self.monthly_salary + self.performance_bonus

    def show(self):
        """
        重新显示经理的信息，包括绩效奖金
        """
        total_pay = self.pay()
        print(f"姓名: {self.name}, 编号: {self.id}, 月薪: {self.monthly_salary}, 绩效奖金: {self.performance_bonus}, 总薪酬: {total_pay}")

class Salesman(Employee):
    def __init__(self, name, id, monthly_salary, sales_commission):
        """
        初始化销售人员对象
        :param sales_commission: 销售提成
        """
        super().__init__(name, id, monthly_salary)
        self.sales_commission = sales_commission

    def pay(self):
        """
        重新计算销售人员的月薪，包括销售提成
        :return: 月薪 + 销售提成
        """
        return self.monthly_salary + self.sales_commission

    def show(self):
        """
        重新显示销售人员的信息，包括销售提成
        """
        total_pay = self.pay()
        print(f"姓名: {self.name}, 编号: {self.id}, 月薪: {self.monthly_salary}, 销售提成: {self.sales_commission}, 总薪酬: {total_pay}")

if __name__ == "__main__":
    # 创建 Manager 实例
    manager = Manager(name="张三", id="001", monthly_salary=8000, performance_bonus=2000)
    manager.show()
    print(f"经理 {manager.name} 的总薪酬: {manager.pay()}")

    # 创建 Salesman 实例
    salesman = Salesman(name="李四", id="002", monthly_salary=5000, sales_commission=1500)
    salesman.show()
    print(f"销售人员 {salesman.name} 的总薪酬: {salesman.pay()}")