//用extends实现类的继承
class Animal
{
	protected float height, weight;
	
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
    
}

class Integration
{
	public static void main(String[] args)
	{
		Animal an = new Animal();
		Fish fh = new Fish();
		an.breathe();
		fh.breathe();
		//System.out.println(an.height);
	}
}
