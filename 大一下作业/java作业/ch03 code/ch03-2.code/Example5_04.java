//利用super调用父类中被隐藏的方法和变量

class Animal
{
	int height, weight;
	
	//Animal的无参构造函数
	Animal()
	{
		System.out.println("Animal Constructed without Parameters");
	}
	
	void eat()
	{
		System.out.println("animal eat");
	}
	void sleep()
	{
		System.out.println("animal sleep");
	}
	void breathe()
	{
		System.out.println("animal breathe");
	}
	
}

class Fish extends Animal   
{
	int height;  
	
	//Fish的构造函数
    Fish()
	{
		System.out.println("Fish Constructed");
	}
	
	void breathe()  
	{
		super.breathe();    //利用super调用父类中被隐藏的方法
		super.height = 50;  //利用super调用父类中被隐藏的变量
		System.out.println(super.height);
		super.breathe();
		System.out.println("fish breathe: bubble");
	}
}

class Integration
{
	public static void main(String[] args)
	{
		//Animal an = new Animal();
		Fish fh = new Fish();
		//an.breathe();
		fh.breathe();
	}
}
