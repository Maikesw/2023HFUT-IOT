//Stack.h 
#include <iostream>  
using namespace std;
class intNode                           //栈节点数据结构
{  
	friend class intStack;  
private:  
    int value;  
    intNode *next;  
    intNode(int value, intNode *p):value(value),next(p){}      //构造函数
};  
 
class intStack                    //栈，用来保存采用后缀表达式进行逻辑运算时的操作数 
{  
private:  
    intNode *top;  
public:  
    intStack():top(NULL)  {}  
    ~intStack();  
    void push(int value);  
    int pop();  
    bool isEmpty()  {return top == NULL;}  
    int getTop(); 
	void makeEmpty();
};  
class CharNode                         //栈节点数据结构  
{  
	friend class CharStack;  
private:  
    char value;  
    CharNode *next;   
    CharNode(char value, CharNode *p):value(value),next(p){}          //构造函数 
};  
 
class CharStack                   //栈，用来保存中缀表达式转换成后缀表达式时的运算符  
{  
private:  
    CharNode *top;  
public:  
    CharStack():top(NULL)  {}  
    ~CharStack();  
    void push(char value);  
    char pop();  
    bool isEmpty()  {return top == NULL;}  
    char getTop();  
    void makeEmpty(); 
	int stacklength();
};  