import numpy as np  # 导入numpy库，用于生成数据和进行数值计算
import matplotlib.pyplot as plt  # 导入matplotlib库的pyplot模块，用于绘图
def plot_sine_cosine():
    """
    绘制y = sin(x)和y = cos(x)的图形，x坐标取值范围为(0, 2π)
    """
    # 使用numpy的linspace函数生成从0到2π的1000个等间距的点，作为x轴的值
    # np.linspace(start, stop, num)生成从start到stop（包括stop）的num个等间距的点
    x = np.linspace(0, 2 * np.pi, 1000)
    
    # 计算y = sin(x)的值
    # np.sin(x)计算x的正弦值
    y1 = np.sin(x)
    
    # 计算y = cos(x)的值
    # np.cos(x)计算x的余弦值
    y2 = np.cos(x)
    
    # 创建一个新的图形，并设置图形的大小为10英寸宽，5英寸高
    # plt.figure(figsize=(width, height))创建一个新的图形， figsize设置图形的大小
    plt.figure(figsize=(10, 5))
    
    # 绘制y = sin(x)的图形
    # plt.plot(x, y, label, color)用于绘制x和y的关系图
    # label用于设置图例的标签，color用于设置图形的颜色
    plt.plot(x, y1, label='y = sin(x)', color='blue')
    
    # 绘制y = cos(x)的图形
    plt.plot(x, y2, label='y = cos(x)', color='red')
    
    # 设置图形的标题
    # plt.title(string)用于设置图形的标题
    plt.title('y = sin(x) 和 y = cos(x) 在 (0, 2π) 的图形')
    
    # 设置x轴的标签
    # plt.xlabel(string)用于设置x轴的标签
    plt.xlabel('x')
    
    # 设置y轴的标签
    # plt.ylabel(string)用于设置y轴的标签
    plt.ylabel('y')
    
    # 添加图例，显示每个图形的标签
    # plt.legend()用于显示图例
    plt.legend()
    
    # 添加网格，提高图形的可读性
    # plt.grid(boolean)用于设置是否显示网格
    plt.grid(True)
    
    # 显示图形
    # plt.show()用于显示图形
    plt.show()
if __name__ == "__main__":
    # 调用绘图函数
    # 执行plot_sine_cosine()函数，绘制y = sin(x)和y = cos(x)的图形
    plot_sine_cosine()