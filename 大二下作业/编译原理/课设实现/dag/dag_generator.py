#!/usr/bin/env python
# -*- coding: utf-8 -*-

from parser.lexer import Token

class DAGNode:
    """DAG节点"""
    def __init__(self, op=None, left=None, right=None, value=None):
        self.id = None  # 节点ID
        self.op = op    # 操作符
        self.value = value  # 值(如果是叶子节点)
        self.left = left    # 左子节点
        self.right = right  # 右子节点
        self.labels = set() # 节点标签(变量名)
    
    def __str__(self):
        if self.op:
            return f"Node{self.id}[{self.op}]"
        else:
            return f"Node{self.id}[{self.value}]"

class DAGGenerator:
    """DAG生成器"""
    def __init__(self):
        self.nodes = []  # DAG节点列表
        self.node_map = {}  # 用于查找已存在的节点
    
    def _get_node_id(self, node):
        """获取节点ID"""
        for i, n in enumerate(self.nodes):
            if (n.op == node.op and n.value == node.value and 
                n.left == node.left and n.right == node.right):
                return i
        
        # 如果节点不存在,则添加到节点列表
        node.id = len(self.nodes)
        self.nodes.append(node)
        return node.id
    
    def _find_or_create_node(self, op=None, left=None, right=None, value=None):
        """查找或创建节点"""
        # 创建一个新节点
        new_node = DAGNode(op, left, right, value)
        
        # 查找是否已存在相同的节点
        for node in self.nodes:
            if (node.op == op and node.value == value and 
                node.left == left and node.right == right):
                return node
        
        # 如果不存在,则添加到节点列表
        new_node.id = len(self.nodes)
        self.nodes.append(new_node)
        return new_node
    
    def generate(self, ast):
        """从AST生成DAG"""
        return self._visit(ast)
    
    def _visit(self, node):
        """访问AST节点"""
        method_name = f'_visit_{type(node).__name__}'
        visitor = getattr(self, method_name, self._generic_visit)
        return visitor(node)
    
    def _generic_visit(self, node):
        """默认的访问方法"""
        raise Exception(f'没有针对{type(node).__name__}的访问方法')
    
    def _visit_BinOp(self, node):
        """访问二元运算符节点"""
        left_node = self._visit(node.left)
        right_node = self._visit(node.right)
        
        # 根据操作符类型创建DAG节点
        if node.op.type == Token.PLUS:
            return self._find_or_create_node(op='+', left=left_node, right=right_node)
        elif node.op.type == Token.MINUS:
            return self._find_or_create_node(op='-', left=left_node, right=right_node)
        elif node.op.type == Token.MUL:
            return self._find_or_create_node(op='*', left=left_node, right=right_node)
        elif node.op.type == Token.DIV:
            return self._find_or_create_node(op='/', left=left_node, right=right_node)
    
    def _visit_UnaryOp(self, node):
        """访问一元运算符节点"""
        expr_node = self._visit(node.expr)
        
        if node.op.type == Token.PLUS:
            # 一元加法,直接返回表达式节点
            return expr_node
        elif node.op.type == Token.MINUS:
            # 一元减法,创建一个零减去表达式的节点
            zero_node = self._find_or_create_node(value=0)
            return self._find_or_create_node(op='-', left=zero_node, right=expr_node)
    
    def _visit_Num(self, node):
        """访问数字节点"""
        return self._find_or_create_node(value=node.value)
    
    def get_dot_representation(self):
        """获取DAG的DOT表示(用于Graphviz可视化)"""
        dot = ['digraph DAG {', '  node [shape=circle];']
        
        # 添加节点
        for node in self.nodes:
            if node.op:
                dot.append(f'  node{node.id} [label="{node.op}"];')
            else:
                dot.append(f'  node{node.id} [label="{node.value}"];')
        
        # 添加边
        for node in self.nodes:
            if node.left:
                dot.append(f'  node{node.id} -> node{node.left.id};')
            if node.right:
                dot.append(f'  node{node.id} -> node{node.right.id};')
        
        dot.append('}')
        return '\n'.join(dot)
    
    def get_text_representation(self):
        """获取DAG的文本表示"""
        result = []
        for i, node in enumerate(self.nodes):
            if node.op:
                left_id = node.left.id if node.left else None
                right_id = node.right.id if node.right else None
                result.append(f"Node{i}: op={node.op}, left=Node{left_id}, right=Node{right_id}")
            else:
                result.append(f"Node{i}: value={node.value}")
        
        return '\n'.join(result) 