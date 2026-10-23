class Car
{
	class Wheel
	{
	}
}

class PlaneWheel extends Car.Wheel
{
	PlaneWheel(Car car)
	{
		car.super();
	}
	
	public static void main(String[] args)
	{
		Car car = new Car();   //要先构造car的实例
		PlaneWheel pw = new PlaneWheel(car);  
	}	
}