//如果把父类中无参的构造函数注释掉，则会出错

class Animal
{
	float height, weight;
	
	//Animal的无参构造函数
	
	
	Animal()
	{
		System.out.println("Animal Constructed without Parameters");
	}
	
	//Animal的有参构造函数
	Animal(int height, int weight)
	{
		this.height = height;
		this.weight = weight;
		
		System.out.println("Animal Constructed with Parameters");
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
	float height;  
	
	//Fish的构造函数
    Fish()
	{
		//super();  //子类中隐式地调用了父类的构造函数
		//super(10,20);  //显式地调用父类中带参数的构造方法
		System.out.println("Fish Constructed");
	}
	
}

class Integration
{
	public static void main(String[] args)
	{
		//Animal an = new Animal();
		Fish fh = new Fish();
		//an.breathe();
		//fh.breathe();
	}
}
