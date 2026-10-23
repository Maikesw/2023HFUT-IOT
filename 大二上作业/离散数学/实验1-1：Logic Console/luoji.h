// luoji.h
class luoji
{
   public:
	   luoji();                           //构造函数
	   ~luoji();                          //析构函数
	   void Input();                      //输入真值
	   void Output();                     //输出错误
	   void Output_error();               //输出格式错误
	   void luoji::Attention();           //输出使用说明
	   int onevalue_Judge(int val_one);         //判断真值
	   int twovalue_Judge(int val_one,int val_two);   //判断真值
	   int And(int val_one,int val_two);              //逻辑与运算
	   int Or(int val_one,int val_two);               //逻辑或运算
	   int Not(int val_one);                          //逻辑非运算
	   int Xor(int val_one,int val_two);              //逻辑异或运算
	   int Single_condition(int val_one,int val_two);          //逻辑蕴含运算
	   int Double_condition(int val_one,int val_two);          //逻辑等值运算   
	   void Known_Logic();                 //计算已知逻辑值表达式
       void Unknown_Logic();               //计算未知逻辑值表达式
       int Known_Detection(char *str);     //检测逻辑运算式是否正确
	   int Unknown_Detection(char *str);   //检测逻辑运算式是否正确
       void quit();                       //程序结束
};