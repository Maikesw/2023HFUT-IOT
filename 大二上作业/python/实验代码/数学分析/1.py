import matplotlib.pyplot as plt
from matplotlib.font_manager import FontProperties

# 设置中文支持
plt.rcParams['font.sans-serif'] = ['SimHei']  # 使用黑体
plt.rcParams['axes.unicode_minus'] = False    # 正常显示负号

# 数据
labels = ['非常不了解', '比较不了解', '一般', '比较了解', '非常了解']
counts = [14, 35, 65, 49, 13]
percentages = [7.95, 19.89, 36.93, 27.84, 7.39]

# 创建柱状图
plt.bar(labels, counts, color='skyblue')

# 添加数据标签
for i in range(len(labels)):
    plt.text(i, counts[i] + 1, f'{counts[i]} ({percentages[i]}%)', ha='center', va='bottom')

# 设置标题和标签
plt.title('大别山红色民歌的革命历史故事了解程度统计')
plt.xlabel('了解程度')
plt.ylabel('人数')

# 显示图表
plt.show()