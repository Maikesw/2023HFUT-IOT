public class TestStatic
{
	public static void main(String[] args)
	{
		System.out.println("Creating new Tools() in main()");
		new Tools();
		System.out.println("Creating new Tools() in main()");
		new Tools();
	}
}
class Tools
{
	Tool t1 = new Tool(1);
	static Tool t2 = new Tool(2);
	Tools()
	{
		System.out.println("Tools()");
		t4 = new Tool(44);
	}
	static Tool t3 = new Tool(3);
	Tool t4 = new Tool(4);
}

class Tool
{
	Tool(int i)
	{
		System.out.println( "Tool(" + i + ")" );
	}
}
