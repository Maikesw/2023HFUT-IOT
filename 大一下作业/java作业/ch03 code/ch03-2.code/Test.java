class Base
{
	int i;
	Base()
	{
		System.out.println ("调用父类构造方法");
		add(1);
		
	}
	void add(int v)
	{
		System.out.println ("调用父类的add");
		i += v;		
	}
	void print()
	{
		System.out.println(i);
	}
}
class Extension extends Base
{
	int i;
	Extension()
	{
		System.out.println ("调用子类的构造方法");
		add(2);	
	}
	void add(int v)
	{
		System.out.println ("调用子类的add");
		i += v*2;	
		System.out.println (i);
	}
}
public class Test
{
	public static void main(String[] args)
	{
		bogo(new Extension());  //初始化
		System.out.println ("初始化结束");
	}
	static void bogo(Base b)
	{
		b.add(8);
		b.print();
	}
}