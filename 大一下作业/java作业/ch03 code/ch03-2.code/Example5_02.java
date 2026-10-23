//测试构造函数的调用顺序
//如果子类没有构造方法，则它继承父类无参数的构造方法作为自己的构造方法；
//如果子类有构造方法，那么在创建子类对象时，则将先执行继承下来的父类的无参构造方法，
//然后再执行自己的构造方法（可以用super来指定调用父类的有参构造方法）
class Integration
{
	public static void main(String[] args)
	{
		Fish fh = new Fish();
		//Fish fh = new Fish(3);
	}
}

class Animal
{
	float height, weight;
	
	//Animal的构造函数
	Animal()
	{
		System.out.println("Animal is constructed without parameters");
	}
	
	Animal(float height, float weight)
	{
		System.out.println("Animal is constructed with parameters");
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

class Fish extends Animal   //鱼类继承了Animal类
{
	float length;
	
	//Fish的构造函数
    //先把这段注释掉，查看输出结果；然后不注释，查看输出结果
	Fish()
	{
		//super(1,1);
		System.out.println("Fish is constructed without parameters");
	}
	
	Fish(float length)
	{
		System.out.println("Fish is constructed with parameters");
	}
}



/*
final class PoisonAnimal
{
	
}

class PoisonFish extends PoisonAnimal
{
	
}
*/