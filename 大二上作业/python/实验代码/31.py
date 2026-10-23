class Vehicle:
    def __init__(self, max_speed, weight):
        """
        初始化汽车类对象
        :param max_speed: 最大速度
        :param weight: 重量
        """
        self.__max_speed = max_speed
        self.__weight = weight

    def get_max_speed(self):
        """
        获取最大速度
        :return: 最大速度
        """
        return self.__max_speed

    def set_max_speed(self, max_speed):
        """
        设置最大速度
        :param max_speed: 新的最大速度
        """
        self.__max_speed = max_speed

    def get_weight(self):
        """
        获取重量
        :return: 重量
        """
        return self.__weight

    def set_weight(self, weight):
        """
        设置重量
        :param weight: 新的重量
        """
        self.__weight = weight

    def show_info(self):
        """
        显示汽车信息
        """
        print(f"最大速度: {self.__max_speed} km/h, 重量: {self.__weight} kg")


class Bicycle(Vehicle):
    def __init__(self, max_speed, weight, height):
        """
        初始化自行车类对象
        :param max_speed: 最大速度
        :param weight: 重量
        :param height: 高度
        """
        super().__init__(max_speed, weight)
        self.__height = height

    @property#@property 装饰器将 height 方法变成一个属性访问器，允许你通过 bicycle.height 的方式访问 __height 属性，而不需要调用 height() 方法。
    def height(self):
        """
        获取高度属性
        :return: 高度
        """
        return self.__height

    @height.setter#@height.setter 装饰器将 height 方法变成一个属性设置器，允许你通过 bicycle.height = new_height 的方式设置 __height 属性，而不需要调用 height(new_height) 方法。
    def height(self, height):
        """
        设置高度属性
        :param height: 新的高度
        """
        self.__height = height

    def set_max_speed(self, max_speed):
        """
        实现给父类的实例属性MaxSpeed的赋值
        :param max_speed: 新的最大速度
        """
        super().set_max_speed(max_speed)

    def show_info(self):
        """
        显示自行车信息
        """
        super().show_info()
        print(f"高度: {self.__height} m")

if __name__ == "__main__":
    # 创建 Bicycle 实例
    bike = Bicycle(max_speed=25, weight=15, height=1.2)
    
    # 设置和获取 height 属性
    bike.height = 1.3  # 设置高度
    print(f"自行车的高度: {bike.height} m")  # 获取高度
    
    # 设置和获取 max_speed 属性
    bike.set_max_speed(30)  # 设置最大速度
    print(f"自行车的最大速度: {bike.get_max_speed()} km/h")  # 获取最大速度
    
    # 显示自行车信息
    bike.show_info()