def toggle_case(char):
    """
    切换字符的大小写
    :param char: 输入的字符
    :return: 切换大小写后的字符
    """
    if char.isupper():
        return char.lower()
    elif char.islower():
        return char.upper()
    else:
        return char

def process_file(input_filename, output_filename):
    """
    读取输入文件内容，并将大写字母变为小写字母，小写字母变为大写字母，然后写入输出文件
    :param input_filename: 输入文件名
    :param output_filename: 输出文件名
    """
    try:
        # 读取输入文件内容
        with open(input_filename, 'r') as infile:
            content = infile.read()

        # 处理内容，切换大小写
        toggled_content = ''.join(toggle_case(char) for char in content)

        # 写入输出文件
        with open(output_filename, 'w') as outfile:
            outfile.write(toggled_content)

        print(f"文件 {input_filename} 已成功处理并保存到 {output_filename}")
    except FileNotFoundError:
        print(f"文件 {input_filename} 未找到")
    except Exception as e:
        print(f"处理文件时发生错误: {e}")

if __name__ == "__main__":
    # 设置输入和输出文件名
    input_filename = "input34.txt"
    output_filename = "output34.txt"

    # 调用处理文件的函数
    process_file(input_filename, output_filename)