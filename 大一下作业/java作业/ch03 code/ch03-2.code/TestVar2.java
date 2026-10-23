class Animal
{
	static int cons = 1;
	int val = 1;
	final int fin = 1;
	
	public void eat()
	{
		System.out.println ("Animal eat");
	}
	static void eat2()
	{
		System.out.println ("Static Animal eat");
	}
}

class Cat extends Animal
{
	static int cons = 2;
	int val = 2;
	final int fin = 2;
	
	public void eat()
	{
		System.out.println ("Cat eat");
	}
	static void eat2()
	{
		System.out.println ("Static Cat eat");
	}
}

public class TestVar2
{
	public static void main(String[] args)
	{
		Animal a = new Cat();
		System.out.println (a.cons);
		System.out.println (a.val);
		System.out.println (a.fin);
		a.eat();
		a.eat2();
		
		Cat b = new Cat();
		System.out.println (b.cons);
		System.out.println (b.val);
		System.out.println (b.fin);
		b.eat();
		b.eat2();
	}
}