import random
import string

class StringGenerator:
    def __init__(self, num_strings, string_length):
        """
        初始化字符串生成器
        :param num_strings: 要生成的字符串的数量
        :param string_length: 每个字符串的长度
        """
        self.num_strings = num_strings
        self.string_length = string_length

    def generate_strings(self):
        """
        生成多个字符串
        :return: 生成的字符串列表
        """
        return [''.join(random.choices(string.ascii_letters + string.digits, k=self.string_length)) for _ in range(self.num_strings)]

def write_to_file(strings, filename):
    """
    将字符串列表写入文件
    :param strings: 字符串列表
    :param filename: 写入的文件名
    """
    with open(filename, 'w') as file:
        for s in strings:
            file.write(s + '\n')

def read_from_file(filename):
    """
    从文件中读取字符串列表
    :param filename: 读取的文件名
    :return: 字符串列表
    """
    with open(filename, 'r') as file:
        strings = file.readlines()
    return [s.strip() for s in strings]

def count_strings_in_file(filename):
    """
    统计文件中的字符串个数
    :param filename: 读取的文件名
    :return: 字符串个数
    """
    strings = read_from_file(filename)
    return len(strings)

if __name__ == "__main__":
    # 设置生成字符串的数量和长度
    num_strings = 10    
    string_length = 8

    # 创建字符串生成器
    generator = StringGenerator(num_strings, string_length)

    # 生成字符串
    strings = generator.generate_strings()
    print("生成的字符串列表:")
    print(strings)

    # 写入文件
    filename = "output33.txt"
    write_to_file(strings, filename)
    print(f"字符串已写入文件 {filename}")

    # 读取文件并统计字符串个数
    string_count = count_strings_in_file(filename)
    print(f"文件 {filename} 中的字符串个数: {string_count}")