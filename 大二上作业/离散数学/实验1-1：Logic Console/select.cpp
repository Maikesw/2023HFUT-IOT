// select.cpp
#include <iostream>
#include <string>
#include "luoji.h"
using namespace std;
int value1,value2;
void main()
{
	luoji Lj;
	char number;
	int res;
	for(int i=0;i<1000;i++)                                //选择要进行的逻辑运算
	{
		cout<<"************** 逻辑运算 *************"<<"\n";
		cout<<"          1---逻辑与运算             "<<"\n";
		cout<<"          2---逻辑或运算             "<<"\n";
		cout<<"          3---逻辑非运算             "<<"\n";
		cout<<"          4---逻辑异或运算           "<<"\n";
		cout<<"          5---逻辑单条件运算         "<<"\n";
		cout<<"          6---逻辑双条件运算         "<<"\n";
		cout<<"          7---命题常元表达式运算   "<<"\n";
		cout<<"          8---命题变元表达式运算   "<<"\n";
		cout<<"          0---程序退出               "<<"\n";
		cout<<"*************************************"<<"\n";
		cout<<"\n请选择您要进行逻辑运算的序号: ";
		cin>>number;
		if(number=='1')                                           //逻辑与运算
		{
            Lj.Attention();
			Lj.Input();		 
			int count=0;
			char value[5];
			getchar();
			gets(value);
		    while(value[count]!='#')
			    count++;
			if(value[0]==' '||value[1]!=' '||value[2]==' '||count!=3 )        //检测输入的格式是否正确
			{
				Lj.Output_error();
				continue;
			}
			res=Lj.And(value[0]-'0',value[2]-'0');
			if(res==2)
				continue;
			else
			    cout<<"\n逻辑与结果为:"<<res<<"\n\n";
			cout<<"请按enter键重新选择输入";
		    getchar();
		}
		else if(number=='2')                                      //逻辑或运算
		{
			Lj.Attention();
			Lj.Input();
			int count=0;
			char value[5];
			getchar();
			gets(value);
			while(value[count]!='#')
				count++;
			if(value[0]==' '||value[1]!=' '||value[2]==' '||count!=3 )        //检测输入的格式是否正确
			{
				Lj.Output_error();
				continue;
			}
			res=Lj.Or(value[0]-'0',value[2]-'0');
		    if(res==0)
			    cout<<"\n逻辑或结果为:0"<<"\n\n";
			else if(res==1)
			    cout<<"\n逻辑或结果为:1"<<"\n\n";
			else
				continue;
			cout<<"请按enter键重新选择输入";
		    getchar();
		}
		else if(number=='3')                                      //逻辑非运算
		{
            Lj.Attention();
			cout<<"请输入真值value1: ";
			int count=0;
			char value[3];
			getchar();
			gets(value);
			while(value[count]!='#')
				count++;
			if(value[0]==' '||count!=1 )        //检测输入的格式是否正确
			{
				Lj.Output_error();
				continue;
			}
			res=Lj.Not(value[0]-'0');
			if(res==1)
			  	cout<<"\n逻辑非结果为:1"<<"\n\n";
		    else if(res==0)
				cout<<"\n逻辑非结果为:0"<<"\n\n";
			else 
				continue;
			cout<<"请按enter键重新选择输入";
		    getchar();
		}
		else if(number=='4')                                      //逻辑异或运算
		{
			Lj.Attention();
			Lj.Input();
			int count=0;
			char value[5];
			getchar();
			gets(value);
			while(value[count]!='#')
				count++;
			if(value[0]==' '||value[1]!=' '||value[2]==' '||count!=3 )        //检测输入的格式是否正确
			{
				Lj.Output_error();
				continue;
			}
			res=Lj.Xor(value[0]-'0',value[2]-'0');
			if(res==1)
				cout<<"\n逻辑异或结果为:1"<<"\n\n";
			else if(res==0)
				cout<<"\n逻辑异或结果为:0"<<"\n\n";
			else 
				continue;
			cout<<"请按enter键重新选择输入";
		    getchar();

		}
		else if(number=='5')                                      //逻辑蕴含运算
		{
			Lj.Attention();
			Lj.Input();
			int count=0;
			char value[5];
			getchar();
			gets(value);
			while(value[count]!='#')
				count++;
			if(value[0]==' '||value[1]!=' '||value[2]==' '||count!=3 )        //检测输入的格式是否正确
			{
				Lj.Output_error();
				continue;
			}
			res=Lj.Single_condition(value[0]-'0',value[2]-'0'); 
			if(res==0)
				cout<<"\n逻辑单条件运算结果为:0"<<"\n\n";
			else if(res==1)
				cout<<"\n逻单条件运算蕴含结果为:1"<<"\n\n";
			else
				continue;
			cout<<"请按enter键重新选择输入";
		    getchar();
		}
		else if(number=='6')                                      //逻辑等价运算
		{
			Lj.Attention();
			Lj.Input();
			int count=0;
			char value[5];
			getchar();
			gets(value);
			while(value[count]!='#')
				count++;
			if(value[0]==' '||value[1]!=' '||value[2]==' '||count!=3 )        //检测输入的格式是否正确
			{
				Lj.Output_error();
				continue;
			}
			res=Lj.Double_condition(value[0]-'0',value[2]-'0');
			if(res==1)
				cout<<"\n逻辑双条件运算结果为:1"<<"\n\n";
			else if(res==0)
				cout<<"\n逻辑双条件运算结果为:0"<<"\n\n";
			else
				continue;
			cout<<"请按enter键重新选择输入";
		    getchar();
		}
		else if(number=='7')                                       //已知逻辑值运算式运算
		{
			cout<<"\n----------------使用说明----------------";
			cout<<"\n========================================";
            cout<<"\n==       ture用'1'     false用'0'     ==";
			cout<<"\n==      与逻辑用'*'    或逻辑用'+'    ==";
			cout<<"\n==      非逻辑用'!'    异或逻辑用'^'  ==";
			cout<<"\n==      单条件用'>'    双条件用'='    ==";
			cout<<"\n========================================";
			Lj.Known_Logic();
		}
		else if(number=='8')                                       //未知逻辑值运算式运算
		{
			cout<<"\n----------------使用说明----------------";
			cout<<"\n========================================";
			cout<<"\n==      命题变元所用字母不能重复      ==";
			cout<<"\n==	     且变元个数不超过8个           ==";
            cout<<"\n==      每个逻辑变元用单个字母表示    ==";
			cout<<"\n==      与逻辑用'*'    或逻辑用'+'    ==";
			cout<<"\n==      非逻辑用'!'    异或逻辑用'^'  ==";
			cout<<"\n==      单条件用'>'    双条件用'='    ==";
			cout<<"\n========================================";   
			Lj.Unknown_Logic();
		}
		else if(number=='0')                                        //程序结束
			Lj.quit();
		else
		{
	        cout<<"\n输入错误,请重新选择输入"<<"\n\n";
		}
  }
}