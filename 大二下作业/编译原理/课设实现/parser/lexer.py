#!/usr/bin/env python
# -*- coding: utf-8 -*-

class Token:
    """词法单元类"""
    # 词法单元类型
    NUMBER = 'NUMBER'     # 数字
    PLUS = 'PLUS'         # 加号 +
    MINUS = 'MINUS'       # 减号 -
    MUL = 'MUL'           # 乘号 *
    DIV = 'DIV'           # 除号 /
    LPAREN = 'LPAREN'     # 左括号 (
    RPAREN = 'RPAREN'     # 右括号 )
    EOF = 'EOF'           # 结束标记
    
    def __init__(self, type, value):
        self.type = type
        self.value = value
    
    def __str__(self):
        return f'Token({self.type}, {repr(self.value)})'
    
    def __repr__(self):
        return self.__str__()

class Lexer:
    """词法分析器"""
    def __init__(self, text):
        self.text = text
        self.pos = 0
        self.current_char = self.text[self.pos] if self.text else None
    
    def error(self):
        raise Exception('词法分析错误')
    
    def advance(self):
        """指针前进一个字符"""
        self.pos += 1
        if self.pos >= len(self.text):
            self.current_char = None
        else:
            self.current_char = self.text[self.pos]
    
    def skip_whitespace(self):
        """跳过空白字符"""
        while self.current_char is not None and self.current_char.isspace():
            self.advance()
    
    def number(self):
        """解析数字"""
        result = ''
        while self.current_char is not None and self.current_char.isdigit():
            result += self.current_char
            self.advance()
        
        # 处理小数点
        if self.current_char == '.':
            result += self.current_char
            self.advance()
            
            while self.current_char is not None and self.current_char.isdigit():
                result += self.current_char
                self.advance()
                
        return float(result)
    
    def get_next_token(self):
        """获取下一个词法单元"""
        while self.current_char is not None:
            # 跳过空白字符
            if self.current_char.isspace():
                self.skip_whitespace()
                continue
            
            # 解析数字
            if self.current_char.isdigit():
                return Token(Token.NUMBER, self.number())
            
            # 解析运算符和括号
            if self.current_char == '+':
                self.advance()
                return Token(Token.PLUS, '+')
            
            if self.current_char == '-':
                self.advance()
                return Token(Token.MINUS, '-')
            
            if self.current_char == '*':
                self.advance()
                return Token(Token.MUL, '*')
            
            if self.current_char == '/':
                self.advance()
                return Token(Token.DIV, '/')
            
            if self.current_char == '(':
                self.advance()
                return Token(Token.LPAREN, '(')
            
            if self.current_char == ')':
                self.advance()
                return Token(Token.RPAREN, ')')
            
            self.error()
        
        return Token(Token.EOF, None) 