//这段代码用来说明类的覆盖

class Animal
{
	int height, weight;
	
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

class Fish extends Animal   //鱼类继承了Animal类
{
	void breathe()  //重写了动物类的breathe方法，这种机制称为覆盖
	{
		System.out.println("fish breathe: bubble");
	}
}

class Integration
{
	public static void main(String[] args)
	{
		Animal an = new Animal();
		Fish fh = new Fish();
		an.breathe();
		fh.breathe();
	}
}
