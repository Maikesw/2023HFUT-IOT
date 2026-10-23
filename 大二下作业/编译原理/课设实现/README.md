# 算术表达式转换成DAG的程序实现

## 项目简介

本项目实现了一个将算术表达式转换成有向无环图(DAG)的语法制导翻译器,包含以下功能:

1. 词法分析器 - 将输入的算术表达式分解为词法单元(token)
2. 语法分析器 - 根据预定义的文法规则解析表达式,生成抽象语法树(AST)
3. DAG生成器 - 将AST转换为DAG表示
4. 图形界面 - 使用PySide6实现的用户界面,支持输入表达式、显示DAG的文本和图形表示

## 文法定义

本项目采用以下文法规则来定义算术表达式:

```
expr   : term ((PLUS | MINUS) term)*
term   : factor ((MUL | DIV) factor)*
factor : (PLUS | MINUS) factor | NUMBER | LPAREN expr RPAREN
```

## 运行环境要求

- Python 3.6+
- PySide6
- Graphviz (用于生成DAG图形)

## 使用方法

1. 运行主程序:
   ```
   python main.py
   ```

2. 在输入框中输入算术表达式,如 `3 + 4 * 2 - (1 + 2) * 3`

3. 点击"转换"按钮,程序将解析表达式并生成DAG

4. 在"文本表示"选项卡中查看DAG的文本表示

5. 在"图形表示"选项卡中查看DAG的图形表示(需要安装Graphviz)

6. 可以使用"保存文本"和"保存图形"按钮保存结果

## 项目结构

- `main.py`: 主程序入口
- `parser/`: 包含词法分析器、语法分析器和AST相关代码
  - `lexer.py`: 词法分析器
  - `parser.py`: 语法分析器
  - `ast.py`: 抽象语法树节点定义
- `dag/`: 包含DAG生成器相关代码
  - `dag_generator.py`: DAG生成器
- `ui/`: 包含用户界面相关代码
  - `main_window.py`: 主窗口实现

## 注意事项

1. 使用图形表示功能需要安装Graphviz,并确保其可执行文件在系统PATH中
2. 支持的算术运算包括加(+)、减(-)、乘(*)、除(/)以及括号()
3. 支持正负数和小数 