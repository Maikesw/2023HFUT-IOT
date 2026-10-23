class Circle extends Point
{
	protected double radius;
	final double pi = 3.1415926;
	
	Circle(double radius, int x, int y)
	{
		this.radius = radius;
		super.x = x;
		super.y = y;
		//Лђеп
		//super(x,y);
	}
	
	public double area()
    {
    	return pi*radius*radius;
    }
	public double volume()
	{
		return 0.0;
	}
	
	public String showName()
	{
		return "Circle";
	} 
}