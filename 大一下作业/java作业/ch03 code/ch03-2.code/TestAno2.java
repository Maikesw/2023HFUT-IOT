interface Cubic
{
	double getCubic(double x); 
}
class A
{
	void f(Cubic cubic)
	{
		double result = cubic.getCubic(3);
		System.out.println("result=" + result);
	}
}
public class TestAno2 {
 
    public static void main(String[] args)     
    {
        Cubic cu = new Cubic()          //匿名类里实现了接口中的抽象方法getCubic
        {
        	public double getCubic(double x)         	
        	{
        		return x*x*x;
        	}         
        };
        
        double m = cu.getCubic(5);
        System.out.println ("m=" + m);
        
        A a = new A();
        a.f(new Cubic()         
        	{
	        	public double getCubic(double x)         	
	        	{
	        		System.out.println ("2");
	        		return x*x*x;
	        	}     		
           });
    }
}
