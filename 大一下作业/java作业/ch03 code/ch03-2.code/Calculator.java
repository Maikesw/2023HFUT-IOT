public class Calculator
{
	final double pi = 3.14;
	
	//计算矩形的面积
	double calcArea(double width, double height, boolean isTri)
	{
	    double area = 0.0;
	    
	    if(!isTri)
	    {
	    	area = width*height;
	    }
		else
		{
			area = 0.5*width*height;
		}
		
		return area;
	}
	//计算圆的面积
	double calcArea(double radius)
	{
		//double area = 0.0;
		area = pi*radius*radius;
		return area;
	}
	
	
	public static void main(String[] args)
	{
		Calculator cal = new Calculator();
		
		//分别计算边长为5，半径为5及长、高分别为5、10的三角形的面积。
		double area1 = cal.calcArea(5.0,5.0,false);
		double area2 = cal.calcArea(5.0);
		double area3 = cal.calcArea(5.0,5.0, true);
		
		System.out.println("矩形的面积是："+area1);
		System.out.println("圆的面积是："+area2);
		System.out.println("三角形的面积是："+area3);
	}
}