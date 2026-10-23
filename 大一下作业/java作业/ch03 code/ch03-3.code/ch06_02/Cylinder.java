class Cylinder extends Circle
{
	protected double height;
	Cylinder(double height, double radius, int x, int y)
	{
		super(radius, x, y);
		this.height = height;
	}
	public double area()
    {
    	return pi*radius*radius;
    }
	public double volume()
	{
		return this.area()*height;
	}
	
	public String showName()
	{
		return "Cylinder";
	} 
}