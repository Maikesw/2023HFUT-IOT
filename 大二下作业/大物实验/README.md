# 悬挂法与支撑法测量数据处理

本项目包含用于处理悬挂法与支撑法测量数据的Excel表格和MATLAB分析脚本。

## 文件说明

1. **悬挂法测量数据_新版.xlsx**：包含实验数据的Excel文件，包括三个表格：
   - 表1：悬挂法测黄铜棒和不锈钢棒的共振频率数据记录
   - 表2：支撑法测黄铜棒的共振频率数据记录
   - 表3：被测试样的参数与共振基频记录

2. **create_excel.py**：创建表1的Python脚本

3. **create_excel2.py**：向Excel文件添加表2的Python脚本

4. **create_excel3.py**：向Excel文件添加表3的Python脚本

5. **data_fitting.m**：MATLAB脚本，用于从Excel文件读取数据并进行多项式拟合
   - 注意：此脚本需要Excel文件中的频率数据已填写

6. **fitting_demo.m**：MATLAB演示脚本，使用模拟数据进行多项式拟合演示
   - 可以直接运行此脚本，无需填写Excel文件

## 使用方法

### Excel文件准备
1. 运行Python脚本创建Excel文件：
   ```
   python create_excel.py
   python create_excel2.py
   python create_excel3.py
   ```

2. 在生成的Excel文件中填写实验测得的频率数据

### 数据分析
1. 如果您已完成实验并填写了频率数据，运行：
   ```
   matlab -r "run data_fitting.m"
   ```

2. 如果您希望先看到分析效果，可以运行演示脚本：
   ```
   matlab -r "run fitting_demo.m"
   ```

## 脚本功能

### data_fitting.m
- 从Excel文件读取实验数据
- 执行二次多项式拟合分析
- 生成频率平方与位置关系图
- 计算拟合优度和样品参数

### fitting_demo.m
- 使用模拟数据进行多项式拟合演示
- 生成四个可视化图表：
  1. 黄铜棒悬挂法拟合
  2. 不锈钢棒悬挂法拟合
  3. 黄铜棒支撑法拟合
  4. 三种方法对比

## 数据拟合原理
在悬挂法和支撑法实验中，频率的平方与悬挂/支撑点位置存在二次函数关系：

f² = A·(x/L)² + B·(x/L) + C

通过多项式拟合可以确定系数A、B和C，进而可以计算材料的弹性模量等物理参数。 