interface Demo
{
	//默认的
	//public static final int NUM = 4;
	//public abstract void show1();
	//public abstract void show2();
	
	//可以简写成
	int NUM = 4;
	void show1();
	void show2();
}

class InterfaceDemo implements Demo
{
	public void show1()
	{  	}
	public void show2()
	{ 	}
}

class TestExample
{
	public static void main(String[] args)
	{
		InterfaceDemo d = new InterfaceDemo();
		//d.NUM = 3;  //不对，NUM为final类型的
		System.out.println(d.NUM);
		System.out.println(Demo.NUM);
		System.out.println(InterfaceDemo.NUM);
	}
}