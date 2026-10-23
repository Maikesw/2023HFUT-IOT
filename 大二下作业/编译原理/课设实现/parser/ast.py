#!/usr/bin/env python
# -*- coding: utf-8 -*-

class AST:
    """抽象语法树基类"""
    pass

class BinOp(AST):
    """二元运算符节点"""
    def __init__(self, left, op, right):
        self.left = left
        self.token = self.op = op
        self.right = right
    
    def __str__(self):
        return f"BinOp({self.left}, {self.op}, {self.right})"

class UnaryOp(AST):
    """一元运算符节点"""
    def __init__(self, op, expr):
        self.token = self.op = op
        self.expr = expr
    
    def __str__(self):
        return f"UnaryOp({self.op}, {self.expr})"

class Num(AST):
    """数字节点"""
    def __init__(self, token):
        self.token = token
        self.value = token.value
    
    def __str__(self):
        return f"Num({self.value})" 