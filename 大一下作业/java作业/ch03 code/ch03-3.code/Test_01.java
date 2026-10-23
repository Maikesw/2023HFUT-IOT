class Test_01
{
	public static void change(int x, int y)
	{
		x = x+y;
		y = x-y;
		x = x-y;
	}
	public static void change(int[] num)
	{
		num[0] = num[0]+num[1];
		num[1] = num[0]-num[1];
		num[0] = num[0]-num[1];
	}
	
	public static void change(Point pt)
	{
		pt.x = pt.x+pt.y;
		pt.y = pt.x-pt.y;
		pt.x = pt.x-pt.y;
	}
	
	public static void main(String[] args)
	{
		int x = 3;
		int y = 4;
		change(x,y);
		System.out.println ("x="+x+", "+"y="+y+";");
		
		int[] num = new int[]{3,4};
		change(num);
		System.out.println ("num[0]="+num[0]+", "+"num[1]="+num[1]+";");
		
		Point pt = new Point();
		pt.x = 3;
		pt.y = 4;
		change(pt);
		System.out.println ("pt.x="+pt.x+", "+"pt.y="+pt.y+";");
	}
}

class Point
{
	int x, y;
}