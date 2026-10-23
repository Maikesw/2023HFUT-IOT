#ifndef PARSER_H
#define PARSER_H

#include "lexer.h"
#include "dag.h"
#include <string>

/*
Grammar definition:
E -> T E'
E' -> + T E' | - T E' | ε
T -> F T'
T' -> * F T' | / F T' | ε
F -> (E) | number
*/

class Parser {
public:
    // Default constructor
    Parser() : lexer(""), currentToken(TokenType::END), dag() {}
    
    // Parse expression and generate DAG
    DAG parse(const std::string& expression);
    
private:
    Lexer lexer;
    Token currentToken;
    DAG dag;
    
    // Get next Token
    void nextToken();
    
    // Syntax analysis functions
    int parseE();      // Parse expression E
    int parseEPrime(int leftValue);  // Parse expression E'
    int parseT();      // Parse term T
    int parseTPrime(int leftValue);  // Parse term T'
    int parseF();      // Parse factor F
    
    // Error handling
    void error(const std::string& message);
};

#endif // PARSER_H 