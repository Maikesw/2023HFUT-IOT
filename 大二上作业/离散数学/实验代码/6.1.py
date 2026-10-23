from graphviz import Digraph

# 创建流程图对象
dot = Digraph(comment='图可视化器流程图', format='png')
dot.attr('node', fontname='SimSun')
dot.attr('edge', fontname='SimSun')
# 添加主要流程节点
dot.node('A', '开始: 初始化 GUI')
dot.node('B', '定义 GraphVisualizer 类')
dot.node('C', '初始化画布和按钮')
dot.node('D', '设置交互模式')
dot.node('E', '处理画布点击和拖动')
dot.node('F', '添加顶点/边')
dot.node('G', '移动/删除顶点')
dot.node('H', '运行算法 (Dijkstra/Floyd)')
dot.node('I', '输出结果')
dot.node('J', '结束: 运行 Tkinter 主循环')

# 定义流程连接
dot.edges([
    ('A', 'B'), 
    ('B', 'C'), 
    ('C', 'D'), 
    ('D', 'E'), 
    ('E', 'F'), 
    ('F', 'G'), 
    ('G', 'H'), 
    ('H', 'I'), 
    ('I', 'J')
])

# 添加算法分支
dot.node('K', 'Dijkstra 算法')
dot.node('L', 'Floyd 算法')
dot.edge('H', 'K', label='如果选择 Dijkstra')
dot.edge('H', 'L', label='如果选择 Floyd')
dot.edge('K', 'I')
dot.edge('L', 'I')

# 输出流程图到文件
dot.render('图可视化器流程图', view=True)
