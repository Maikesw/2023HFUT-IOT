#pragma execution_character_set("utf-8")
#include "parser.h"
#include <iostream>
#include <stdexcept>

DAG Parser::parse(const std::string& expression) {

    lexer = Lexer(expression);

    dag = DAG();

    nextToken();

    int rootId = parseE();

    dag.setRoot(rootId);

    if (currentToken.type != END) {
        error("Extra tokens");
    }
    
    return dag;
}

void Parser::nextToken() {
    currentToken = lexer.getNextToken();
}

int Parser::parseE() {
    // E -> T E'
    int t = parseT();
    return parseEPrime(t);
}

int Parser::parseEPrime(int leftValue) {
    // E' -> + T E' | - T E' | ε
    if (currentToken.type == PLUS) {
        nextToken();
        int t = parseT();
        int addNode = dag.addOperator("+", leftValue, t);
        return parseEPrime(addNode);
    } else if (currentToken.type == MINUS) {
        nextToken();
        int t = parseT();
        int subNode = dag.addOperator("-", leftValue, t);
        return parseEPrime(subNode);
    } else {
        // ε production
        return leftValue;
    }
}

int Parser::parseT() {
    // T -> F T'
    int f = parseF();
    return parseTPrime(f);
}

int Parser::parseTPrime(int leftValue) {
    // T' -> * F T' | / F T' | ε
    if (currentToken.type == MULTIPLY) {
        nextToken();
        int f = parseF();
        int mulNode = dag.addOperator("*", leftValue, f);
        return parseTPrime(mulNode);
    } else if (currentToken.type == DIVIDE) {
        nextToken();
        int f = parseF();
        int divNode = dag.addOperator("/", leftValue, f);
        return parseTPrime(divNode);
    } else {
        // ε production
        return leftValue;
    }
}

int Parser::parseF() {
    // F -> (E) | number
    if (currentToken.type == LPAREN) {
        nextToken();
        int e = parseE();
        
        if (currentToken.type != RPAREN) {
            error("Missing right parenthesis");
        }
        
        nextToken();
        return e;
    } else if (currentToken.type == NUMBER) {
        int leafNode = dag.addLeaf(currentToken.value);
        nextToken();
        return leafNode;
    } else {
        error("Expected a number or left parenthesis");
        return -1;
    }
}

void Parser::error(const std::string& message) {
    throw std::runtime_error("Syntax error: " + message);
} 