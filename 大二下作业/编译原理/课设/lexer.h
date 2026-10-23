#ifndef LEXER_H
#define LEXER_H

#include <string>
#include <vector>

enum TokenType {
    NUMBER,     // Number
    PLUS,       // +
    MINUS,      // -
    MULTIPLY,   // *
    DIVIDE,     // /
    LPAREN,     // (
    RPAREN,     // )
    END         // End marker
};

struct Token {
    TokenType type;
    std::string value;
    
    Token(TokenType t, const std::string& v = "") : type(t), value(v) {}
};

class Lexer {
public:
    Lexer(const std::string& input);
    
    Token getNextToken();
    Token peekToken();
    bool hasMoreTokens() const;
    
private:
    std::string input;
    size_t position;
    Token currentToken;
    
    void skipWhitespace();
    char peek() const;
    char advance();
    Token readNumber();
};

#endif // LEXER_H 