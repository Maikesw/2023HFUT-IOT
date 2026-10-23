import matplotlib.pyplot as plt
import matplotlib.patches as patches
from matplotlib.font_manager import FontProperties
import matplotlib

# 确保使用支持中文的字体
matplotlib.rcParams['font.family'] = ['SimHei', 'Microsoft YaHei', 'SimSun', 'sans-serif']
plt.rcParams['axes.unicode_minus'] = False

# 创建一个大尺寸的图形窗口
fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(16, 9))

# 设置背景色为白色
fig.patch.set_facecolor('white')

# 基础算法流程图 - 左侧
y_positions = [0.85, 0.65, 0.45, 0.25]
box_width = 0.6
box_height = 0.15

# 绘制基础算法的处理流程框和文字
texts = ['原始数据加载', '时间窗口构建 (lags=20)', '数据归一化 (mapminmax)', '数据集划分 (训练:测试=9:1)']

for i, y in enumerate(y_positions):
    rect = patches.Rectangle((0.2, y-0.07), box_width, box_height, 
                           facecolor='#E6E6FA', edgecolor='black', alpha=0.7)
    ax1.add_patch(rect)
    ax1.text(0.5, y, texts[i], ha='center', va='center', fontsize=12)
    
    # 添加箭头（除了最后一个框）
    if i < len(y_positions) - 1:
        ax1.annotate('', xy=(0.5, y_positions[i+1]+0.08), xytext=(0.5, y-0.07),
                    arrowprops=dict(facecolor='black', shrink=0.05, width=1.5, headwidth=8))

ax1.set_title('基础LSTM算法数据处理流程', fontsize=14, fontweight='bold')
ax1.axis('off')

# 改进算法流程图 - 右侧
y_positions_right = [0.85, 0.65, 0.45, 0.25, 0.05]
texts_right = ['原始数据加载与初步分析', '多尺度时间窗口 (lags=30)', '', '特征组合与归一化', '三部分数据集划分 (7:2:1)']

for i, y in enumerate(y_positions_right):
    rect = patches.Rectangle((0.2, y-0.07), box_width, box_height, 
                           facecolor='#FFE6E6', edgecolor='black', alpha=0.7)
    ax2.add_patch(rect)
    if texts_right[i]:  # 跳过空文本
        ax2.text(0.5, y, texts_right[i], ha='center', va='center', fontsize=12)
    
    # 添加箭头（除了最后一个框）
    if i < len(y_positions_right) - 1:
        ax2.annotate('', xy=(0.5, y_positions_right[i+1]+0.08), xytext=(0.5, y-0.07),
                    arrowprops=dict(facecolor='black', shrink=0.05, width=1.5, headwidth=8))

# 特征工程小模块
feat_texts = ['一阶差分', '移动平均', '异常值检测']
y_feat = y_positions_right[2]
feat_positions = [0.25, 0.5, 0.75]
box_small_width = 0.15

for i, x in enumerate(feat_positions):
    rect = patches.Rectangle((x-box_small_width/2, y_feat-0.05), box_small_width, 0.1, 
                           facecolor='#FFEFEF', edgecolor='black', alpha=0.7)
    ax2.add_patch(rect)
    ax2.text(x, y_feat, feat_texts[i], ha='center', va='center', fontsize=10)
    
    # 从特征工程小模块到下一个大框的箭头
    ax2.annotate('', xy=(0.5, y_positions_right[3]+0.08), xytext=(x, y_feat-0.05),
                arrowprops=dict(facecolor='black', shrink=0.05, width=1.5, headwidth=8))

ax2.set_title('改进算法数据处理流程', fontsize=14, fontweight='bold')
ax2.axis('off')

# 设置总标题
fig.suptitle('两种算法数据处理流程对比', fontsize=16, fontweight='bold')

# 调整布局
plt.tight_layout()
plt.subplots_adjust(top=0.9)

# 保存图像
plt.savefig('两种算法数据处理流程对比图.png', dpi=300, bbox_inches='tight')
print('图像已保存为：两种算法数据处理流程对比图.png')

# 显示图像
plt.show()