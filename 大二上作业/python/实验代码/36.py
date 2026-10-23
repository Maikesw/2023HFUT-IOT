import os

def list_txt_files(directory):
    """
    列出指定目录下所有的 .txt 文件
    :param directory: 目录路径
    :return: .txt 文件的列表
    """
    try:
        # 获取目录下的所有文件和文件夹
        files = os.listdir(directory)
        
        # 过滤出 .txt 文件
        txt_files = [file for file in files if file.endswith('.txt')]
        
        # 返回 .txt 文件列表
        return txt_files
    except FileNotFoundError:
        print(f"目录 {directory} 未找到")
        return []
    except Exception as e:
        print(f"列出文件时发生错误: {e}")
        return []

if __name__ == "__main__":
    # 设置目录路径
    directory = r"C:\Users\邓傑湧\Desktop\大二上作业\python\实验代码"
    
    # 调用列出 .txt 文件的函数
    txt_files = list_txt_files(directory)
    
    # 打印 .txt 文件列表
    if txt_files:
        print("目录下的 .txt 文件列表:")
        for file in txt_files:
            print(file)
    else:
        print("目录下没有 .txt 文件")