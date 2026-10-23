//Stack.cpp
#include "Stack.h"  
intStack::~intStack()  
{  
    intNode *p;  
    while (NULL != top)  
    {  
        p = top;  
        top = top->next;  
        delete p;  
    }  
}  
void intStack::push(int value)              //逻辑值入栈
{  
    top = new intNode(value,top);  
}  
int intStack::pop()                         //逻辑值出栈
{  
    if (!isEmpty())  
    {  
        intNode *p;  
        p = top;  
        top = top->next;  
        int nodeValue = p->value;  
        delete p;  
        return nodeValue;  
    }  
}  
void intStack::makeEmpty()                  //初始化逻辑值栈
{  
    intNode *p;  
    while (NULL != top)  
    {  
        p = top;  
        top = top->next;  
        delete p;  
    }  
}  
int intStack::getTop()                      //得到栈顶元素

{  
    return top->value;  
}  
CharStack::~CharStack()  
{  
    CharNode *p;  
    while (NULL != top)  
    {  
        p = top;  
        top = top->next;  
        delete p;  
    }  
}  
void CharStack::push(char value)             //运算符入栈
{  
    top = new CharNode(value,top);  
}  
char CharStack::pop()                        //运算符出栈
{  
    if (!isEmpty())  
    {  
        CharNode *p;  
        p = top;  
        top = top->next;  
        char nodeValue = p->value;  
        delete p;  
        return nodeValue;  
    }  
}  
void CharStack::makeEmpty()                  //初始化运算符栈
{  
    CharNode *p;  
    while (NULL != top)  
    {  
        p = top;  
        top = top->next;  
        delete p;  
    }  
}  
char CharStack::getTop()                     //得到栈顶元素
{  
    return top->value;  
}



int CharStack::stacklength()                 //得到栈的长度
{
	CharNode *p;
	int n=0;
	p=top;
	while (p!=NULL)
	{
		n++;
		p=p->next;
	}
	return (n);	 
}