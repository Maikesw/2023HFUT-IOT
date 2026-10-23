import matplotlib.pyplot as plt
from matplotlib.font_manager import FontProperties

# 设置中文支持
plt.rcParams['font.sans-serif'] = ['SimHei']  # 使用黑体
plt.rcParams['axes.unicode_minus'] = False    # 正常显示负号

# 数据
questions = [
    '开展普查，整理相关资料',
    '宣传活动，提升保护意识',
    '吸纳人才，完善传承制度',
    '政策支持，加大经费投入',
    '推出路线，发掘旅游价值',
    '走进校园，加强人才培养'
]

options = ['1', '2', '3', '4', '5']
data = {
    '1': [3, 3, 3, 5, 3, 3],
    '2': [4, 5, 1, 3, 4, 3],
    '3': [12, 12, 20, 17, 15, 18],
    '4': [37, 34, 31, 28, 30, 33],
    '5': [120, 122, 121, 123, 124, 119]
}

# 创建图形
fig, ax = plt.subplots(figsize=(10, 8))

# 绘制每个选项的条形图
bottom = [0] * len(questions)
for option in options:
    values = data[option]
    ax.bar(questions, values, bottom=bottom, label=f'{option} ({sum(values)/176:.2f}%)', alpha=0.7)
    bottom = [b + v for b, v in zip(bottom, values)]

# 设置标题和标签
ax.set_title('您认为政府应该给予大别山红色民歌怎样的政策保护')
ax.set_xlabel('政策保护措施')
ax.set_ylabel('选择人数')

# 添加图例
ax.legend(loc='upper left', bbox_to_anchor=(1.05, 1), borderaxespad=0.)

# 添加平均分标签
for i, question in enumerate(questions):
    ax.text(i, bottom[i] + 5, f'平均分: {4.51 if i == 6 else 4.52 if i < 4 else 4.49 if i == 5 else 4.48}', ha='center', va='bottom', fontsize=10, color='blue')

# 显示图表
plt.show()