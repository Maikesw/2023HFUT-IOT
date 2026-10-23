class Point extends Shape
{
	protected int x, y;
	
    Point()
    {
    }
    Point(int x, int y)
    {
    	this.x = x; this.y = y;
    }
    
    public double area()
    {
    	return 0.0;
    }
	public double volume()
	{
		return 0.0;
	}
	public String showName()
	{
		return "Point";
	}
}