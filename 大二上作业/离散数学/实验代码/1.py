import tkinter as tk
from tkinter import messagebox
from itertools import product
from sympy import symbols, And, Or, Not, Xor, Implies, Equivalent, to_cnf, to_dnf

class LogicCalculator:
    def __init__(self, root):
        self.root = root
        self.root.title("命题逻辑计算器")
        
        self.propositions = []
        self.expression = tk.StringVar()
        
        self.create_widgets()
    
    def create_widgets(self):
        # 输入命题常元
        tk.Label(self.root, text="输入命题变元（例如：P, Q, R）:").grid(row=0, column=0, padx=10, pady=10)#grid方法,row,column为第0行第0列,padx表示跟其他组件x轴10格,pady表示与其他组件10格
        self.prop_input = tk.Entry(self.root)
        self.prop_input.grid(row=0, column=1, padx=10, pady=10)
        
        # 输入命题表达式
        tk.Label(self.root, text="输入命题表达式（例如：P & Q | ~R）:").grid(row=1, column=0, padx=10, pady=10)
        self.expr_input = tk.Entry(self.root)
        self.expr_input.grid(row=1, column=1, padx=10, pady=10)
        
        # 默认输入框值
        self.prop_input.insert(0, "P, Q, R")
        self.expr_input.insert(0, "P & Q | ~R")
        
        # 计算按钮
        tk.Button(self.root, text="计算真值", command=self.calculate_truth_value).grid(row=2, column=0, padx=10, pady=10)
        tk.Button(self.root, text="生成真值表", command=self.generate_truth_table).grid(row=2, column=1, padx=10, pady=10)
        
        # 清空按钮
        tk.Button(self.root, text="清空输入", command=self.clear_inputs).grid(row=3, column=0, columnspan=2, padx=10, pady=10)
        
        # 输出区域
        self.output = tk.Text(self.root, height=20, width=50)
        self.output.grid(row=4, column=0, columnspan=2, padx=10, pady=10)
    
    def clear_inputs(self):
        self.prop_input.delete(0, tk.END)
        self.expr_input.delete(0, tk.END)#tk.END文本框的末尾
        self.output.delete(1.0, tk.END)
    
    def calculate_truth_value(self):
        propositions = self.prop_input.get().split(',')
        propositions = [p.strip().upper() for p in propositions]
        expression = self.expr_input.get().strip()
        
        try:
            # 将命题常元转换为符号
            symbols_dict = {prop: symbols(prop) for prop in propositions}
            expr = eval(expression, symbols_dict)
            
            # 输入命题常元的真值
            truth_values = {}
            for prop in propositions:
                truth_value = messagebox.askyesno("输入真值", f"请输入 {prop} 的真值（True 或 False）")
                truth_values[prop] = truth_value
            
            # 计算表达式的真值
            result = expr.subs(truth_values)
            self.output.insert(tk.END, f"命题表达式的真值：{result}\n")
        except Exception as e:
            messagebox.showerror("错误", f"输入有误：{e}")
    
    def generate_truth_table(self):
        propositions = self.prop_input.get().split(',')
        propositions = [p.strip().upper() for p in propositions]
        expression = self.expr_input.get().strip()
        
        try:
            # 将命题常元转换为符号
            symbols_dict = {prop: symbols(prop) for prop in propositions}
            expr = eval(expression, symbols_dict)
            
            # 生成所有可能的真值组合
            truth_combinations = list(product(["+", "-"], repeat=len(propositions)))
            
            # 计算每种组合下的表达式真值
            table = []
            for combination in truth_combinations:
                truth_values = dict(zip(propositions, combination))
                result = expr.subs(truth_values)
                result_str = "+" if result else "-"
                table.append([str(val) for val in combination] + [str(result_str)])
            
            # 输出真值表
            header = propositions + ["结果"]
            self.output.insert(tk.END, "真值表：\n")
            self.output.insert(tk.END, " | ".join(header) + "\n")
            self.output.insert(tk.END, "-" * (len(header) * 4 - 1) + "\n")
            for row in table:
                self.output.insert(tk.END, " | ".join(row) + "\n")
            
            # 计算主范式
            cnf_expr = to_cnf(expr)
            dnf_expr = to_dnf(expr)
            self.output.insert(tk.END, f"\n主合取范式（CNF）：{cnf_expr}\n")
            self.output.insert(tk.END, f"主析取范式（DNF）：{dnf_expr}\n")
        except Exception as e:
            messagebox.showerror("错误", f"输入有误：{e}")

if __name__ == "__main__":
    root = tk.Tk()
    app = LogicCalculator(root)
    root.mainloop()