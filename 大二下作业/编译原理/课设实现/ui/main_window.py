#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os
import tempfile
from PySide6.QtWidgets import (
    QMainWindow, QWidget, QVBoxLayout, QHBoxLayout, QLabel, 
    QLineEdit, QPushButton, QTextEdit, QMessageBox, QFileDialog,
    QSplitter, QTabWidget
)
from PySide6.QtCore import Qt, QProcess
from PySide6.QtGui import QPixmap, QFont

from parser.lexer import Lexer
from parser.parser import Parser
from dag.dag_generator import DAGGenerator

class MainWindow(QMainWindow):
    """主窗口"""
    def __init__(self):
        super().__init__()
        
        self.setWindowTitle("算术表达式转换成DAG")
        self.setMinimumSize(800, 600)
        
        # 创建中央部件
        central_widget = QWidget()
        self.setCentralWidget(central_widget)
        
        # 创建主布局
        main_layout = QVBoxLayout(central_widget)
        
        # 创建输入区域
        input_layout = QHBoxLayout()
        input_label = QLabel("输入表达式:")
        self.input_edit = QLineEdit()
        self.input_edit.setPlaceholderText("请输入算术表达式,例如: 3 + 4 * 2 - (1 + 2) * 3")
        self.convert_btn = QPushButton("转换")
        self.convert_btn.clicked.connect(self.convert_expression)
        
        input_layout.addWidget(input_label)
        input_layout.addWidget(self.input_edit)
        input_layout.addWidget(self.convert_btn)
        
        main_layout.addLayout(input_layout)
        
        # 创建分割器
        splitter = QSplitter(Qt.Vertical)
        main_layout.addWidget(splitter, 1)
        
        # 创建选项卡部件
        self.tab_widget = QTabWidget()
        
        # 创建文本输出区域
        self.text_output = QTextEdit()
        self.text_output.setReadOnly(True)
        self.text_output.setFont(QFont("Courier New", 10))
        
        # 创建图形输出区域
        self.graph_widget = QWidget()
        graph_layout = QVBoxLayout(self.graph_widget)
        self.graph_label = QLabel("DAG图形表示")
        self.graph_label.setAlignment(Qt.AlignCenter)
        graph_layout.addWidget(self.graph_label)
        
        # 添加选项卡
        self.tab_widget.addTab(self.text_output, "文本表示")
        self.tab_widget.addTab(self.graph_widget, "图形表示")
        
        splitter.addWidget(self.tab_widget)
        
        # 创建底部按钮区域
        button_layout = QHBoxLayout()
        self.save_text_btn = QPushButton("保存文本")
        self.save_text_btn.clicked.connect(self.save_text)
        self.save_graph_btn = QPushButton("保存图形")
        self.save_graph_btn.clicked.connect(self.save_graph)
        
        button_layout.addWidget(self.save_text_btn)
        button_layout.addWidget(self.save_graph_btn)
        button_layout.addStretch()
        
        main_layout.addLayout(button_layout)
        
        # 初始化Graphviz进程
        self.dot_process = QProcess()
        self.dot_process.finished.connect(self.display_graph)
        
        # 存储当前DAG生成器
        self.dag_generator = None
        self.dot_file = None
        self.png_file = None
    
    def convert_expression(self):
        """转换表达式为DAG"""
        expression = self.input_edit.text().strip()
        if not expression:
            QMessageBox.warning(self, "警告", "请输入表达式")
            return
        
        try:
            # 创建词法分析器
            lexer = Lexer(expression)
            # 创建语法分析器
            parser = Parser(lexer)
            # 解析表达式
            ast = parser.parse()
            # 创建DAG生成器
            self.dag_generator = DAGGenerator()
            # 生成DAG
            self.dag_generator.generate(ast)
            
            # 显示文本表示
            self.text_output.setText(self.dag_generator.get_text_representation())
            
            # 生成图形表示
            self.generate_graph()
            
        except Exception as e:
            QMessageBox.critical(self, "错误", f"转换失败: {str(e)}")
    
    def generate_graph(self):
        """生成DAG图形"""
        if not self.dag_generator:
            return
        
        # 创建临时文件
        fd, self.dot_file = tempfile.mkstemp(suffix='.dot')
        os.close(fd)
        
        fd, self.png_file = tempfile.mkstemp(suffix='.png')
        os.close(fd)
        
        # 将DOT表示写入文件
        with open(self.dot_file, 'w') as f:
            f.write(self.dag_generator.get_dot_representation())
        
        # 使用Graphviz将DOT文件转换为PNG
        self.dot_process.start('dot', ['-Tpng', self.dot_file, '-o', self.png_file])
    
    def display_graph(self):
        """显示生成的图形"""
        if self.dot_process.exitCode() != 0:
            QMessageBox.warning(self, "警告", "无法生成图形,请确保已安装Graphviz")
            return
        
        if os.path.exists(self.png_file):
            pixmap = QPixmap(self.png_file)
            self.graph_label.setPixmap(pixmap)
            self.graph_label.setScaledContents(True)
            self.tab_widget.setCurrentIndex(1)  # 切换到图形选项卡
    
    def save_text(self):
        """保存文本表示"""
        if not self.dag_generator:
            QMessageBox.warning(self, "警告", "请先转换表达式")
            return
        
        file_path, _ = QFileDialog.getSaveFileName(
            self, "保存文本", "", "文本文件 (*.txt);;所有文件 (*.*)"
        )
        
        if file_path:
            try:
                with open(file_path, 'w') as f:
                    f.write(self.dag_generator.get_text_representation())
                QMessageBox.information(self, "成功", "文本已保存")
            except Exception as e:
                QMessageBox.critical(self, "错误", f"保存失败: {str(e)}")
    
    def save_graph(self):
        """保存图形表示"""
        if not self.png_file or not os.path.exists(self.png_file):
            QMessageBox.warning(self, "警告", "请先转换表达式并生成图形")
            return
        
        file_path, _ = QFileDialog.getSaveFileName(
            self, "保存图形", "", "PNG图片 (*.png);;所有文件 (*.*)"
        )
        
        if file_path:
            try:
                pixmap = QPixmap(self.png_file)
                pixmap.save(file_path)
                QMessageBox.information(self, "成功", "图形已保存")
            except Exception as e:
                QMessageBox.critical(self, "错误", f"保存失败: {str(e)}")
    
    def closeEvent(self, event):
        """关闭窗口时清理临时文件"""
        if self.dot_file and os.path.exists(self.dot_file):
            try:
                os.remove(self.dot_file)
            except:
                pass
        
        if self.png_file and os.path.exists(self.png_file):
            try:
                os.remove(self.png_file)
            except:
                pass
        
        super().closeEvent(event) 