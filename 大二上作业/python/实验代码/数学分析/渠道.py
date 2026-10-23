import matplotlib.pyplot as plt
from matplotlib.font_manager import FontProperties

# 设置中文支持
plt.rcParams['font.sans-serif'] = ['SimHei']  # 使用黑体
plt.rcParams['axes.unicode_minus'] = False    # 正常显示负号

# 数据
labels = ['家庭熏陶', '学校教育', '微信公众号、抖音快手APP等新媒体平台', '电视戏曲频道', '剧院、剧场演出', '当地风俗习惯影响', '其他(请具体说明)', '不了解']
counts = [45, 113, 102, 58, 45, 84, 3, 14]
percentages = [25.57, 64.2, 57.95, 32.95, 25.57, 47.73, 1.7, 7.95]

# 创建堆积柱状图
fig, ax = plt.subplots()

# 计算累积百分比
cumulative_percentage = [0] * len(labels)

# 绘制每个选项的柱状图
for i in range(len(labels)):
    ax.bar('接触渠道', counts[i], bottom=cumulative_percentage[i], label=f'{labels[i]} ({percentages[i]}%)', alpha=0.7)
    cumulative_percentage[i] += percentages[i]

# 设置标题和标签
ax.set_title('主要通过哪些渠道接触、了解大别山红色民歌作品')
ax.set_ylabel('人数')

# 添加图例
ax.legend(loc='upper left', bbox_to_anchor=(1, 1))

# 显示图表
plt.show()