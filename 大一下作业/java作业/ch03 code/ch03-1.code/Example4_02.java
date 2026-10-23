class Point
{
	int px, py;
	static final double PI= 3.1415;
	
	Point(int px, int py)
	{
		this.px = px;  //this区分成员变量与局部变量
		this.py = py;
		
	}
	
	Point()
	{
		this(1,1);  //this简化构造函数的调用
	}
	
	static void output()
	{
	    System.out.println("Output() called");
	    System.out.println(PI);	
	}
	
	void move(int x, int y)
	{
		px = px + x;
		py = py + y;
	}
}

public class Example4_02
{
	public static void main(String[] args)
	{
		Point pt1 = new Point();
		pt1.px = 3;
		pt1.py = 6;
		
		Point pt2 = new Point();
		pt2.px = 5;
		pt2.py = 5;
		
		pt1.move(1,1);
		pt2.move(1,1);
		
		Point.output();  //静态方法可以通过类名直接调用
	}
}