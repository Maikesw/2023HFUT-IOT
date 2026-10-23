//arithmetic.cpp
 
#include "arithmetic.h"  
#include "luoji.h"
luoji Lj;
 
/****************************************************
//该函数对输入的逻辑表达式进行操作
//将输入的逻辑表达式(中缀表达式)转化为后缀表达式 
//ch存储的是要进行转化的中缀表达式，转化后的后缀表达式存储在栈中
*****************************************************/
void Arithmetic::midTopost(char *ch)               
{  
    int i=0;  
    int k=0;   
    char tmp; 
	//下面开始把字符串中的操作符(*+~^>=)放入后缀表达式
    cStack.makeEmpty();                   //初始化运算符栈
    cStack.push('&');                     //将&作为栈底元素
    strcat(ch, "&");                       //字符串最后都加一个&   
	while (ch[k] != '\0')  
    { 	
        if (ch[k] == '*' || ch[k] == '+' || ch[k] == '!' || ch[k] == '^' || ch[k] == '>'|| ch[k] == '='|| ch[k] == '(')  
        {
            while (isp(cStack.getTop()) > icp(ch[k]))         //如果优先级低，一直退栈，否则入栈 
            {  
                post[i++].op = cStack.pop(); 
            }  
            cStack.push(ch[k]);  
        }  
        else if (ch[k] == ')')  
        {  
            while (isp(cStack.getTop()) >= icp(')'))          //一直退栈到'('
            {  
                tmp = cStack.pop();  
                if (tmp != '(')  
                {  
                    post[i++].op = tmp;  
                }  
            }       
        }  
        else if (ch[k] == '&')  
        {  
            while (isp(cStack.getTop()) > icp('&'))  
            {  
                tmp = cStack.pop();  
                post[i++].op = tmp;  
            }  
        }  
        else if (ch[k] == '0' || ch[k] == '1')               //数字
        { 
		
            post[i].num = ch[k]-48;
            post[i].op = '&';  
            i++;   
        }  
        k++;  
    }  
    curLen = i;  	
} 
/**************************************************** 
//栈内优先级，栈内优先级大于栈外优先级  
//'('的栈内优先级应该比所有的操作符都小（除了#）  
//')'的栈内优先级应该最大  
//优先级由大到小顺序为：! * + ^ = >  
*****************************************************/
int Arithmetic::isp(char op)  
{  
    int pri;  
    switch (op)  
    {  
    case '&' : pri = 0;  
        break;  
    case '(' : pri = 1;  
        break;  
    case '>' : pri = 3;  
        break;  
    case '=' : pri = 5;  
        break;  
    case '^' : pri = 7;  
        break;  
    case '+' : pri = 9;  
        break; 
	case '*' : pri = 11;  
        break; 
	case '!' : pri = 13;  
        break; 
    case ')' : pri = 15;  
        break;  
    }  
    return pri;  
}  
  
int Arithmetic::icp(char op)                    //栈外优先级
{  
    int pri;  
    switch (op)  
    {  
    case '&' : pri = 0;  
        break;  
    case '(' : pri = 15;  
        break;  
	case '>' : pri = 2;  
        break;  
    case '=' : pri = 4;  
        break;  
    case '^' : pri = 6;  
        break;  
    case '+' : pri = 8;  
        break; 
	case '*' : pri = 10;  
        break; 
	case '!' : pri = 12;  
        break; 
    case ')' : pri = 1;  
        break;  
    }  
    return pri;  
}  
/****************************************************
//该函数对存储在栈中的后缀表达式进行计算
//栈中最后的值即为逻辑计算的结果
*****************************************************/
int Arithmetic::calculate()                  //根据后缀表达式进行逻辑式运算 
{  
    int i;  
    int left_value, right_value; 
	dStack.makeEmpty();
    for (i = 0; i < curLen; i++)  
    {  
        if (post[i].op == '&')               //如果是逻辑值就入栈
        {  
            dStack.push(post[i].num); 	 
        }  
		else if(post[i].op == '!')
		{
			 
			right_value = dStack.pop(); 
			dStack.push(Lj.Not(right_value)); 
		}
        else                                  //如果是操作符，就取出两个逻辑值进行运算  
        {  
            right_value = dStack.pop();  
            left_value = dStack.pop();  
            switch (post[i].op)  
            {  
            case '*' : dStack.push(Lj.And(right_value,left_value));  
                break;  
            case '+' : dStack.push(Lj.Or(right_value,left_value));  
                break;   
            case '^' : dStack.push(Lj.Xor(right_value,left_value));  
                break;  
            case '>' : dStack.push(Lj.Single_condition(left_value,right_value));  
                break; 
			case '=' : dStack.push(Lj.Double_condition(right_value,left_value));  
                break; 
            }  
		 
        }  
    }  
    return dStack.pop();  
}  