import matplotlib.pyplot as plt
from matplotlib.font_manager import FontProperties

# 设置中文支持
plt.rcParams['font.sans-serif'] = ['SimHei']  # 使用黑体
plt.rcParams['axes.unicode_minus'] = False    # 正常显示负号

# 数据
labels = ['从来没有', '很少有', '一般', '偶尔有', '经常有']
counts = [29, 37, 17, 32, 61]
percentages = [16.48, 21.02, 9.66, 18.18, 34.66]

# 创建柱状图
plt.bar(labels, counts, color='lightgreen')

# 添加数据标签
for i in range(len(labels)):
    plt.text(i, counts[i] + 1, f'{counts[i]} ({percentages[i]}%)', ha='center', va='bottom')

# 设置标题和标签
plt.title('您所在的地区大别山红色民歌相关活动的频率统计')
plt.xlabel('活动频率')
plt.ylabel('人数')

# 显示图表
plt.show()