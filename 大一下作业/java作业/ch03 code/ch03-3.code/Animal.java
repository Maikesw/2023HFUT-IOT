public abstract class Animal
{
	public abstract void run(); //有抽象方法的类一定是抽象类
	public void breath()
	{
		System.out.println("呼吸");
		run();
	}
	public static void main(String[] args)
	{
		Cat cat1 = new Cat();
		cat1.breath();
	}
}

class Cat extends Animal
{
	public void run()
	{
		System.out.println("猫跑");
	}
}
