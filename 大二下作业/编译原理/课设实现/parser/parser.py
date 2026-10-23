#!/usr/bin/env python
# -*- coding: utf-8 -*-

from .lexer import Token
from .ast import BinOp, UnaryOp, Num

class Parser:
    """语法分析器
    
    文法规则:
    expr   : term ((PLUS | MINUS) term)*
    term   : factor ((MUL | DIV) factor)*
    factor : (PLUS | MINUS) factor | NUMBER | LPAREN expr RPAREN
    """
    def __init__(self, lexer):
        self.lexer = lexer
        self.current_token = self.lexer.get_next_token()
    
    def error(self):
        raise Exception('语法分析错误')
    
    def eat(self, token_type):
        """验证当前词法单元类型是否与传入的类型匹配，
        如果匹配则获取下一个词法单元，否则报错
        """
        if self.current_token.type == token_type:
            self.current_token = self.lexer.get_next_token()
        else:
            self.error()
    
    def factor(self):
        """解析factor
        factor : (PLUS | MINUS) factor | NUMBER | LPAREN expr RPAREN
        """
        token = self.current_token
        
        if token.type == Token.PLUS:
            self.eat(Token.PLUS)
            return UnaryOp(token, self.factor())
        
        elif token.type == Token.MINUS:
            self.eat(Token.MINUS)
            return UnaryOp(token, self.factor())
        
        elif token.type == Token.NUMBER:
            self.eat(Token.NUMBER)
            return Num(token)
        
        elif token.type == Token.LPAREN:
            self.eat(Token.LPAREN)
            node = self.expr()
            self.eat(Token.RPAREN)
            return node
        
        self.error()
    
    def term(self):
        """解析term
        term : factor ((MUL | DIV) factor)*
        """
        node = self.factor()
        
        while self.current_token.type in (Token.MUL, Token.DIV):
            token = self.current_token
            
            if token.type == Token.MUL:
                self.eat(Token.MUL)
            elif token.type == Token.DIV:
                self.eat(Token.DIV)
            
            node = BinOp(left=node, op=token, right=self.factor())
        
        return node
    
    def expr(self):
        """解析expr
        expr : term ((PLUS | MINUS) term)*
        """
        node = self.term()
        
        while self.current_token.type in (Token.PLUS, Token.MINUS):
            token = self.current_token
            
            if token.type == Token.PLUS:
                self.eat(Token.PLUS)
            elif token.type == Token.MINUS:
                self.eat(Token.MINUS)
            
            node = BinOp(left=node, op=token, right=self.term())
        
        return node
    
    def parse(self):
        """解析输入并返回AST"""
        return self.expr() 