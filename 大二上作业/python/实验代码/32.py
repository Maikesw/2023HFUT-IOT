class MyQueue:
    def __init__(self, size):
        """
        初始化队列
        :param size: 队列的最大长度
        """
        self.size = size
        self.data = [None] * size  # 初始化一个固定长度的列表来存储队列数据
        self.current = 0  # 当前队列中的元素个数
        self.front = 0  # 队头指针
        self.rear = 0  # 队尾指针

    def is_empty(self):
        """
        判断队列是否为空
        :return: 若为空，则返回True，否则返回False
        """
        return self.current == 0

    def is_full(self):
        """
        判断队列是否为满
        :return: 若为满，则返回True，否则返回False
        """
        return self.current == self.size

    def get_front(self):
        """
        取队头元素
        :return: 队头元素，若队列为空，则返回None
        """
        if self.is_empty():
            return None
        return self.data[self.front]

    def enqueue(self, element):
        """
        入队：将元素入队，即放到队列的尾部
        :param element: 要入队的元素
        :return: 若队列已满，则返回False，否则返回True
        """
        if self.is_full():
            return False
        self.data[self.rear] = element
        self.rear = (self.rear + 1) % self.size  # 队尾指针移动
        self.current += 1
        return True

    def dequeue(self):
        """
        出队：删除当前队头的元素
        :return: 队头元素，若队列为空，则返回None
        """
        if self.is_empty():
            return None
        element = self.data[self.front]
        self.data[self.front] = None
        self.front = (self.front + 1) % self.size  # 队头指针移动
        self.current -= 1
        return element

    def show_queue(self):
        """
        显示队列中的所有元素
        """
        if self.is_empty():
            print("队列为空")
        else:
            print("队列中的元素: ", end="")
            for i in range(self.current):
                index = (self.front + i) % self.size
                print(self.data[index], end=" ")
            print()
if __name__ == "__main__":
    # 创建一个长度为N的队列
    N = 5
    queue = MyQueue(N)

    # 调用成员方法
    print("初始化队列")
    queue.show_queue()

    # 判断队列是否为空
    print("队列是否为空:", queue.is_empty())

    # 入队
    elements_to_enqueue = [10, 20, 30, 40, 50]
    for element in elements_to_enqueue:
        if queue.enqueue(element):
            print(f"元素 {element} 入队成功")
        else:
            print(f"队列已满，元素 {element} 无法入队")
        queue.show_queue()

    # 取队头元素
    front_element = queue.get_front()
    print(f"队头元素: {front_element}")

    # 出队
    for _ in range(6):
        element = queue.dequeue()
        if element is not None:
            print(f"元素 {element} 出队成功")
        else:
            print("队列为空，无法出队")
        queue.show_queue()

    # 判断队列是否为满
    print("队列是否为满:", queue.is_full())