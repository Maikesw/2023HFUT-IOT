//ch06_01.java  抽象类之例
//定义抽象类
abstract class People 
{
	abstract void SayHI();
}

//定义类Chinese实现SayHI抽象方法
class Chinese extends People 
{
	public void SayHI() 
	{ 
	     System.out.println("中国人习惯问候语：你好，吃饭了吗？ ");
    }
}

//定义类English实现SayHI抽象方法
class English extends People 
{
	public void SayHI() 
	{
		 System.out.println("英国人习惯问候语：你好，天气不错 ");
    }
}

//主类
public class ch06_01 
{
	public static void main(String args[]) 
	{
		 People someguy;

		 //Chinese Jet = new Chinese();
		 //Jet.SayHI(); 		 		 
		 someguy = new Chinese();
		 someguy.SayHI();
		 
		 English Beckham = new English();
		 Beckham.SayHI(); 
		 		 
		 Greeting greeting = new Greeting();		 
         Greeting.greet(new Chinese());
         Greeting.greet(new English());
    }
}

//用来说明如何以对象为参数
class Greeting
{
	public static void greet(People p) 
	{
		// 以 SayHI 接口的实现类 的对象为参数
		 p.SayHI();                   
    }
}