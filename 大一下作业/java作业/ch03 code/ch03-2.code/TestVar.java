class Father
{
	int age = 60;
	public void setAge(int i)
	{
		age = i;
	}
}

class Son extends Father
{
	int age = 10;
	public void show()
	{
		System.out.println ("super.age: " + super.age + " this.age: " + this.age);
	}
}

public class TestVar
{
	public static void main(String[] args)
	{
		Son son = new Son();
		System.out.println (son.age);
		son.setAge(100);
		son.show();
		System.out.println (son.age);
	    Father parent = son;
	    System.out.println (parent.age); 
	}
}