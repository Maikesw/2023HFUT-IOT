def convert_encoding(input_filename, output_filename, input_encoding='gbk', output_encoding='utf-8'):
    """
    读取输入文件内容，将其从输入编码转换为输出编码，然后写入输出文件
    :param input_filename: 输入文件名
    :param output_filename: 输出文件名
    :param input_encoding: 输入文件的编码格式，默认为 'gbk'
    :param output_encoding: 输出文件的编码格式，默认为 'utf-8'
    """
    try:
        # 读取输入文件内容
        with open(input_filename, 'r', encoding=input_encoding) as infile:
            content = infile.read()

        # 写入输出文件，使用指定的输出编码
        with open(output_filename, 'w', encoding=output_encoding) as outfile:
            outfile.write(content)

        print(f"文件 {input_filename} 已成功转换编码并保存到 {output_filename}")
    except FileNotFoundError:
        print(f"文件 {input_filename} 未找到")
    except Exception as e:
        print(f"转换编码时发生错误: {e}")

if __name__ == "__main__":
    # 设置输入和输出文件名
    input_filename = "input_gbk.txt"
    output_filename = "output_utf8.txt"

    # 调用转换编码的函数
    convert_encoding(input_filename, output_filename, input_encoding='gbk', output_encoding='utf-8')