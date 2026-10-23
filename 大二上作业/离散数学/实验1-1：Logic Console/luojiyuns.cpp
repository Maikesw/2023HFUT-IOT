/**********************************************************
 Discrete Mathematics experiments platform
 Programed by Wang Xiaohua
 School of Computer Science and Information
 HeFei University of Technology
**********************************************************/

// luojiyuns.cpp
#include <iostream>
#include<math.h>
#include "luoji.h"
#include "arithmetic.h"
using namespace std;
char input_str[50];
luoji::luoji() {}
luoji::~luoji() {}
void luoji::Input()
  {	 
     cout<<"\n请输入真值value1和value2: ";	 
  }
 void luoji::Output()
  {
     cout<<"\n真值范围输入错误! 请重新选择输入"<<"\n\n";
  }
 void luoji::Output_error()	
 {
     cout<<"\n"<<"输入格式错误! 请重新选择输入"<<"\n\n";
 }
 void luoji::Attention()
 {
	 cout<<"\n----------------使用说明----------------";
	 cout<<"\n========================================";
	 cout<<"\n==       ture用'1'     false用'0'     ==";
	 cout<<"\n==       真值与真值之间使用空格键隔开 ==";
	 cout<<"\n==       不要输入多余的空格键         ==";
	 cout<<"\n==       输入最后以#键结束            ==";
	 cout<<"\n========================================";
 }
/****************************************************
//该函数用于检测输入的单逻辑值是否正确
//val_one为要进行检测的逻辑值
*****************************************************/
 int luoji::onevalue_Judge(int val_one)                                  
  {
	  if(val_one==0||val_one==1 )
		  return 1;
	  else
		  return 0;
  }
/****************************************************
//该函数用于检测输入的双逻辑值是否正确
//val_one和val_two为要进行检测的逻辑值
*****************************************************/
 int luoji::twovalue_Judge(int val_one,int val_two)                         
  {
      if((val_one==0||val_one==1)&&(val_two==0||val_two==1))
		  return 1;
	  else
		  return 0;
  }
/****************************************************
//该函数用于进行逻辑与运算
//val_one和val_two分别为要进行计算逻辑值
*****************************************************/
 int luoji::And(int val_one,int val_two)                                       
  {
	 int res,jud;
	 jud=twovalue_Judge(val_one,val_two);
	 

    //










	  cout<<"\n\n";
  }
/****************************************************
//该函数用于进行逻辑或运算
//val_one和val_two分别为要进行计算逻辑值
*****************************************************/
 int luoji::Or(int val_one,int val_two)                                     
  {
	 int jud;
	 

	 //










	  cout<<"\n\n";
  }
/****************************************************
//该函数用于进行逻辑非运算
//val_one和val_two分别为要进行计算逻辑值
*****************************************************/
 int luoji::Not(int val_one)                                           
  {
	 int jud;
	 //











	  cout<<"\n\n";
  }
/****************************************************
//该函数用于进行逻辑异或运算
//val_one和val_two分别为要进行计算逻辑值
*****************************************************/
 int luoji::Xor(int val_one,int val_two)                                       
  {
	 int jud;
	 //











      cout<<"\n\n";
  }
/****************************************************
//该函数用于进行逻辑单条件运算
//val_one和val_two分别为要进行计算逻辑值
*****************************************************/
 int luoji::Single_condition(int val_one,int val_two)                                 
  {
	 int jud;
	 //










      cout<<"\n\n";
  }
/****************************************************
//该函数用于进行逻辑双条件运算
//val_one和val_two分别为要进行计算逻辑值
*****************************************************/
int luoji::Double_condition(int val_one,int val_two)                               
 { 
	 int jud;
	 //












	 cout<<"\n\n";
  }
/****************************************************
//该函数用于检测“用命题常量”组合成的表达式的输入是否正确
//str为要进行检测的逻辑表达式
//返回值为输入错误时的各种提示
*****************************************************/
 int luoji::Known_Detection(char *str)                              
 {
	 int i=0,k=0;
	 while(str[i]!='\0')
	 {
		 i++;
	 }
	 if(str[i-1]!='#')
		 return 2;
	 i=0;
	 while(str[i]!='#')
	 {
		 if(!isalnum(str[i])&&(str[i]))
		 {
			 if(str[i]!='*'&&str[i]!='+'&&str[i]!='!'&&str[i]!='^'&&str[i]!='>'&&str[i]!='='&&str[i]!='('&&str[i]!=')')
			 return 3;
		 }
		 if(str[i]!='*'&&str[i]!='+'&&str[i]!='!'&&str[i]!='^'&&str[i]!='>'&&str[i]!='='&&str[i]!='('&&str[i]!=')')
		 {
			 if(str[i]!='0'&&str[i]!='1')
			 return 4;
		 }
		 if (str[i+1]!='#')
		 { 
			 if((str[i]=='0'||str[i]=='1')&&(str[i+1]=='0'||str[i+1]=='1'))
				 return 5;
			 if((str[i]=='0'||str[i]=='1')&&(str[i+1]=='!'||str[i+1]=='('))
				 return 6;
			 if((str[i]=='*'||str[i]=='+'||str[i]=='^'||str[i]=='>'||str[i]=='='||str[i]=='(')&&(str[i+1]=='*'||str[i+1]=='+' ||str[i+1]=='^'||str[i+1]=='>'||str[i+1]=='='||str[i+1]==')'))
				 return 7;
			 if(str[i]=='('&&str[i+1]=='(')
				 return 7;
			 if(str[i]==')'&&(str[i+1]=='!' ||str[i+1]=='('||str[i+1]==')'))
				 return 7; 
			 if(str[i]=='!'&&(str[i+1]=='*'||str[i+1]=='+'||str[i+1]=='^'||str[i+1]=='>'||str[i+1]=='='||str[i+1]==')'))
				 return 7;
		 }
		 if(str[i]=='(')                     //检测括号是否匹配
			 k++;
		 if(str[i]==')')
			 k--;
		 if(k<0)
			 return 8;
		 i++;
	 }
	 if(k!=0&&(str[0]=='*'||str[0]=='+'||str[0]=='^'||str[0]=='>'||str[0]=='=')&&(str[i-1]=='*'||str[i-1]=='+'||str[i-1]=='!'||str[i-1]=='^'||str[i-1]=='>'||str[i-1]=='='))
		 return 0;
	return 1;
 }
/****************************************************
//该函数用于检测“用命题变元”组合成的表达式的输入是否正确
*****************************************************/
 int luoji::Unknown_Detection(char *str)                     
 {
	 int i=0,length;
	 while(str[i]!='\0')
	 {
		 i++;
	 }
	 length=i;
	 if(str[length-1]!='#')
		 return 0;
	 for(i=0;i<length-1;i++)
	 {
		 if(!isalpha(str[i])&&str[i]!='*'&&str[i]!='+'&&str[i]!='!'&&str[i]!='^'&&str[i]!='>'&&str[i]!='='&&str[i]!='('&&str[i]!=')')
			 return 2;
		 if(isalpha(str[i])&&isalpha(str[i+1]))
		     return 3;
		 if(isalpha(str[i])&&str[i+1]=='(')
		     return 4;
		 if(!isalpha(str[i])&&!isalpha(str[i+1])&&(str[i]!='('&&str[i+1]!='!')&&str[i+1]!='('&&str[i]!=')')
			 return 5;
	 }
	 int left=0,right=0,j=0;
	 while(j<length-1)                           //检测括号是否匹配
	 {
		 if(str[j]=='(')
			 left++;
		 if(str[j]==')')  
			 right++;
		 if(right>left)
			 return 6;
		 j++;
	 }
	 if(left!=right)
	         return 7;
   return 1;
 }
/****************************************************
//该函数用于计算“用常量”组合的表达式的逻辑值
*****************************************************/
 void luoji::Known_Logic()                  
 {
	 int det;
	 int i=0;
	 cout<<"\n\n请输入要计算的逻辑表达式(以#键结束):";
	 getchar();
	 gets(input_str);
	 det=Known_Detection(input_str);
	 if(det==0)
	 {
		 cout<<"\n逻辑表达式输入错误! 请按enter键重新选择输入"<<"\n";
		 getchar();
	 }
	 else if(det==2)
	 {
		 cout<<"\n未以#结尾,请重新输入! 请按enter键重新选择输入"<<"\n";
		 getchar();
	 }
	 else if(det==3)
	 {
		 cout<<"\n逻辑符号输入错误,请按enter键重新选择输入"<<"\n";
      getchar();
	 }
	 else if(det==4)
	 {
		 cout<<"\n逻辑值输入错误,请按enter键重新选择输入"<<"\n";
		 getchar();
	 }
	 else if(det==5)
	 {
		 cout<<"\n逻辑值之间无连接词,请按enter键重新选择输入"<<"\n";
		 getchar();
	 }
	 else if(det==6)
	 {
		 cout<<"\n逻辑值与逻辑符号连接不正确,请按enter键重新选择输入"<<"\n";
		 getchar();
	 }
	 else if(det==7)
	 {
		 cout<<"\n逻辑符号之间连接不正确,请按enter键重新选择输入"<<"\n";
		 getchar();
	 }
	 else if(det==8)
	 {
		 cout<<"\n括号匹配不正确,请按enter键重新选择输入"<<"\n";
		 getchar();
	 }
	 else
	 {
          Arithmetic luoji=Arithmetic();
		  luoji.midTopost(input_str);	 
		  cout<<"\n逻辑运算结果为:"<<luoji.calculate()<<"\n\n";	
		  cout<<"请按enter键重新选择输入";
		  getchar();
	 }
 }
/****************************************************
//该函数用于计算用变元组合成的表达式的逻辑值
//input_str为输入的逻辑表达式
//在逻辑表达式正确的情况下，得出运算结果
//根据真值表计算结果，得出主析取范式和主合取范式
*****************************************************/
  void luoji::Unknown_Logic()                      
  {  
	  int det;
	  int i=0;
	  CharStack xiqu_one,xiqu_two;
	  CharStack hequ_one,hequ_two;
      xiqu_one.makeEmpty(); 
	  hequ_one.makeEmpty();
	  xiqu_two.makeEmpty(); 
	  hequ_two.makeEmpty(); 
	  xiqu_one.push('(');
      hequ_one.push('(');
	  cout<<"\n\n请输入要计算的逻辑表达式(以#键结束):";
	  getchar();
	  gets(input_str);
	  det=Unknown_Detection(input_str);
	  if(det==0)
	  {
		  cout<<"\n未以#结尾,请重新输入, 请按enter键重新选择输入"<<"\n";
		  getchar();
	  }
	  else if(det==2)
	  {
		  cout<<"\n错误的字符, 请按enter键重新选择输入"<<"\n";
		  getchar();
	  }
	  else if(det==3)
	  {
		  cout<<"\n命题变元之间无联结词,请按enter键重新选择输入"<<"\n";
		  getchar();
	  }
	  else if(det==4)
	  {
		  cout<<"\n命题变元后不可接'('，请按enter键重新选择输入"<<"\n";
		  getchar();
	  }
	  else if(det==5)
	  {
		  cout<<"\n联结词间无命题变元,请按enter键重新选择输入"<<"\n";
		  getchar();
	  }
	  else if(det==6)
	  {
		  cout<<"\n括号次序不匹配,请按enter键重新选择输入"<<"\n";
		  getchar();
	  }
	  else if(det==7)
	  {
		  cout<<"\n括号个数不匹配,请按enter键重新选择输入"<<"\n";
		  getchar();
	  }
	  else
	  {
		  cout<<"\n"<<" 真值表如下:"<<"\n";
		  int i=0;
		  int count=0;                                           //记录命题变元个数
		  int turn;                                              //十进制转换为二进制时的余数
		  int length;
		  char bianyuan[8];                                      //命题变元数组
		  char shift[50];                                        
		  while(input_str[i]!='#')                               
		  {
	     	  if(isalpha(input_str[i]))                          //isalpha函数判断输入字符是否为26个字母之一
			  {
				  bianyuan[count++]=input_str[i];                //bianyuan[]数组用于范式输出
				  cout<<"  "<<input_str[i];
			  }
			      i++;
		  } 
		  cout<<"  "<<"Value";	
		  
        /* 以下for循环把变元表达式转换为2的count次幂个常元表达式*/
		
		  for(int number=0;number<pow(2.0,count);number++)        
		  {
		 	  cout<<"\n";
			  turn=number;                                        //10进制number值转换二进制
			  length=count;
			  int binary[8]={0};                                 
			  int k=1;                                               
			  int j=0;  
			  strcpy(shift, input_str);                           //将输入的字符串存入shift数组

 
			  while(turn)                                        
			  {
				  binary[8-k]=turn%2;
				  turn/=2;
				  k++;
			  } 
		      
                                     
			 //以下循环将变元逻辑式转化为常元逻辑式，也就是改变shift数组中的值,同时把该number对应的行输出
			 //以便参考luoji::Known_Logic()进行计算  
			  while(shift[j]!='#')                                                   
			  {
				  if(isalpha(shift[j]))                            //只改变变元，操作符不变
				  { 
					shift[j]=binary[8-length]+'0';                //字符型变量转换为数值(0或1),操作符不变                                         
					cout<< "  "<<shift[j];
					length--;
				  }
				  j++;
			  }
			 
			  
			  //通过以上操作，变元表达式已转换为常元表达式，请参考luoji::Known_Logic()函数计算表达式值的方法
			  Arithmetic luoji=Arithmetic();
			  luoji.midTopost(shift);                               
		      cout<<"  "<<luoji.calculate();
			  
			  //
			 
			  
			  
			  
			  if(luoji.calculate()==1)                            //逻辑值为真时入析取栈 
			  {
				  for(int t=0;t<count;t++)
				  {
					  if(binary[8-count+t]==1)
					  {
						  xiqu_one.push(bianyuan[t]);
						  xiqu_one.push('*');
					  }
					  else
					  {
						  xiqu_one.push('!');
						  xiqu_one.push(bianyuan[t]);
						  xiqu_one.push('*');
					  }
				  }
				  xiqu_one.pop();
				  xiqu_one.push(')');
				  xiqu_one.push('+');
                  xiqu_one.push('(');
			  }
		  
		      if(luoji.calculate()==0)                          //逻辑值为假时入合取栈               
			  {
			     for(int t=0;t<count;t++)
				 {
				     if(binary[8-count+t]==0)
					 {
					   hequ_one.push(bianyuan[t]);
					   hequ_one.push('+');
					 }
				     else
					 {
					   hequ_one.push('!');
					   hequ_one.push(bianyuan[t]);
					   hequ_one.push('+');
					 }
				 }
			     hequ_one.pop();
			     hequ_one.push(')');
			     hequ_one.push('*');
			     hequ_one.push('(');
			  }
		  }
		  cout<<"\n析取范式为:";                      //下面为析取范式输出
		  if(xiqu_one.stacklength()==1)
			  cout<<"无析取范式,此表达式为矛盾式";
		  else
		  {
			  xiqu_one.pop();                         //输出'('
			  xiqu_one.pop();                         //输出'+'
			  while(xiqu_one.stacklength()!=0)
			  {
				  xiqu_two.push(xiqu_one.getTop());
				  xiqu_one.pop();
			  }
			  while(xiqu_two.stacklength()!=0)
			  {
				  cout<<xiqu_two.getTop();
				  xiqu_two.pop();
			  }
		  }
   		  cout<<"\r\n";
		  cout<<"合取范式为:";                         //下面为合取范式输出
		  if(hequ_one.stacklength()==1)
			  cout<<"无合取范式,此表达式为重言式";
		  else
		  {
			  hequ_one.pop();                          //输出'('
			  hequ_one.pop();                          //输出'*'
			  while(hequ_one.stacklength()!=0)
			  {
				  hequ_two.push(hequ_one.getTop());
				  hequ_one.pop();
			  }
			  while(hequ_two.stacklength()!=0)
			  {
				  cout<<hequ_two.getTop();
				  hequ_two.pop();
			  }
		  }
		  cout<<"\n\n请按enter键重新选择输入";
		  getchar();
	  }
  }
 void luoji::quit()                                       //程序结束
 {
     cout<<"程序结束!谢谢使用!"<<"\n";
	 exit(0);
 }