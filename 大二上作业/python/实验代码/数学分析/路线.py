import matplotlib.pyplot as plt
from matplotlib.font_manager import FontProperties

# 设置中文支持
plt.rcParams['font.sans-serif'] = ['SimHei']  # 使用黑体
plt.rcParams['axes.unicode_minus'] = False    # 正常显示负号

# 数据
labels = ['非常不感兴趣', '比较不感兴趣', '一般', '比较感兴趣', '非常感兴趣']
counts = [4, 9, 38, 85, 40]
percentages = [2.27, 5.11, 21.59, 48.3, 22.73]

# 创建图形
fig, ax = plt.subplots(figsize=(10, 6))

# 绘制条形图
ax.bar(labels, counts, alpha=0.7)

# 添加百分比标签
for i in range(len(labels)):
    ax.text(i, counts[i] + 2, f'{percentages[i]}%', ha='center', va='bottom', fontsize=10, color='blue')

# 设置标题和标签
ax.set_title('您对于以大别山红色民歌为主题的红色旅游路线感兴趣吗？')
ax.set_xlabel('选项')
ax.set_ylabel('选择人数')

# 添加网格线
ax.yaxis.grid(True, linestyle='--', which='major', color='grey', alpha=0.5)

# 显示图表
plt.show()