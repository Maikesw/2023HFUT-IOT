from graphviz import Digraph

# 创建流程图对象
flowchart = Digraph(format='png', comment="Logic Calculator Flowchart")

# 设置字体
flowchart.attr('node', fontname='SimSun')
flowchart.attr('edge', fontname='SimSun')

# 添加节点
flowchart.node('Start', '程序开始')
flowchart.node('GUI', '显示图形用户界面\n输入命题常元\n输入逻辑表达式\n提供按钮')
flowchart.node('Wait', '等待用户操作')

# 计算真值分支
flowchart.node('CalcTruth', '计算真值\n(calculate_truth_value)')
flowchart.node('InputSymbols', '获取输入命题常元和逻辑表达式\n将命题常元转换为符号')
flowchart.node('UserInput', '弹出对话框\n获取每个命题的真值')
flowchart.node('EvaluateTruth', '计算逻辑表达式的真值\n替换命题符号\n计算结果')
flowchart.node('ShowResult', '显示表达式真值\n输出到文本框')
flowchart.node('Error1', '异常处理\n输入有误')

# 生成真值表分支
flowchart.node('GenerateTable', '生成真值表\n(generate_truth_table)')
flowchart.node('TruthCombinations', '生成所有真值组合\n(itertools.product)')
flowchart.node('EvaluateTable', '计算每个组合的真值\n替换命题符号')
flowchart.node('OutputTable', '生成真值表\n输出表头和内容')
flowchart.node('CNFDNF', '计算主合取范式(CNF)\n主析取范式(DNF)\n输出结果')
flowchart.node('Error2', '异常处理\n输入有误')

# 清空输入分支
flowchart.node('ClearInputs', '清空输入框和输出框\n(clear_inputs)')

# 结束节点
flowchart.node('End', '程序结束')

# 添加边
flowchart.edges([('Start', 'GUI'), ('GUI', 'Wait')])

# 分支逻辑
flowchart.edge('Wait', 'CalcTruth', label='点击“计算真值”')
flowchart.edge('CalcTruth', 'InputSymbols')
flowchart.edge('InputSymbols', 'UserInput')
flowchart.edge('UserInput', 'EvaluateTruth')
flowchart.edge('EvaluateTruth', 'ShowResult')
flowchart.edge('EvaluateTruth', 'Error1', label='错误')

flowchart.edge('Wait', 'GenerateTable', label='点击“生成真值表”')
flowchart.edge('GenerateTable', 'InputSymbols')
flowchart.edge('InputSymbols', 'TruthCombinations')
flowchart.edge('TruthCombinations', 'EvaluateTable')
flowchart.edge('EvaluateTable', 'OutputTable')
flowchart.edge('OutputTable', 'CNFDNF')
flowchart.edge('OutputTable', 'Error2', label='错误')
flowchart.edge('CNFDNF', 'ShowResult')

flowchart.edge('Wait', 'ClearInputs', label='点击“清空输入”')
flowchart.edge('CalcTruth', 'Error1', label='异常')
flowchart.edge('GenerateTable', 'Error2', label='异常')

flowchart.edge('ClearInputs', 'Wait')

# 最后结束
flowchart.edge('ShowResult', 'Wait')
flowchart.edge('Error1', 'Wait')
flowchart.edge('Error2', 'Wait')
flowchart.edge('Wait', 'End', label='关闭窗口')

# 渲染流程图
flowchart_path = flowchart.render('/mnt/data/logic_calculator_flowchart', view=True)
flowchart_path