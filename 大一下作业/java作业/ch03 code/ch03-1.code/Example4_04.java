//通过final实参对类进行初始化操作

class Calculate 
{
    private double r;  //半径
    static final double PI=3.14159;
    
    final void area(double r) 
    {
       double x;
       x=PI*r*r;
       System.out.println("Area=" + x);
       
    }      
}

class Calculate1 extends Calculate
{
	final void area(double r)
	{
	}
}

public class Example4_04 
{ 
    public static void main(String args[]) 
    {
       Calculate dd = new Calculate();      
       dd.area(12.3);
    }    
}

