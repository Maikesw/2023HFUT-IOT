#include "lexer.h"
#include <cctype>

Lexer::Lexer(const std::string& input) : input(input), position(0), currentToken(END) {
    // 初始化词法分析器
}

void Lexer::skipWhitespace() {
    while (position < input.size() && std::isspace(input[position])) {
        position++;
    }
}

char Lexer::peek() const {
    if (position >= input.size()) {
        return '\0';
    }
    return input[position];
}

char Lexer::advance() {
    if (position >= input.size()) {
        return '\0';
    }
    return input[position++];
}

Token Lexer::readNumber() {
    std::string result;
    while (position < input.size() && (std::isdigit(peek()) || peek() == '.')) {
        result += advance();
    }
    return Token(NUMBER, result);
}

Token Lexer::getNextToken() {
    skipWhitespace();
    
    if (position >= input.size()) {
        return Token(END);
    }
    
    char current = peek();
    
    if (std::isdigit(current)) {
        return readNumber();
    }
    
    switch (current) {
        case '+':
            advance();
            return Token(PLUS, "+");
        case '-':
            advance();
            return Token(MINUS, "-");
        case '*':
            advance();
            return Token(MULTIPLY, "*");
        case '/':
            advance();
            return Token(DIVIDE, "/");
        case '(':
            advance();
            return Token(LPAREN, "(");
        case ')':
            advance();
            return Token(RPAREN, ")");
        default:
            // 未知字符，跳过
            advance();
            return getNextToken();
    }
}

Token Lexer::peekToken() {
    size_t savedPosition = position;
    Token token = getNextToken();
    position = savedPosition;
    return token;
}

bool Lexer::hasMoreTokens() const {
    return position < input.size();
} 