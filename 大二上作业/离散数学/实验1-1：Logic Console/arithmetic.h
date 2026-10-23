//arithmetic.h 
#include <iostream>
#include <malloc.h>  
#include <string.h>  
#include <stdlib.h>  
#include "Stack.h" 
using namespace std;
class Data  
{  
public:  
    int num;  
    char op;  
};  
class Arithmetic  
{  
private:  
    intStack dStack;                  //保存逻辑运算时，后缀表达式中的操作数  
    CharStack cStack;                //保存中缀表达式转换成后缀表达式过程中的操作符  
    int isp(char op);                 //栈内优先级  
    int icp(char op);                 //栈外优先级  
public:  
    Data post[100];                   //保存后缀表达式  
    int curLen;                       //后缀表达式数组的实际长度  
    void midTopost(char *c);          //把中缀表达式转换成后缀表达式  
    int calculate();                  //进行逻辑表达式运算  
};  