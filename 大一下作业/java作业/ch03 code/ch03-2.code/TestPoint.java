class Point2D
{
	int X,Y;
	public Point2D()
	{  //无参的Point2D构造方法
		X=0;  		Y=0;
		System.out.println("Point2D Constructed without Parameter!");
	}	
	public Point2D(int x, int y)
	{ //有参的Point2D构造方法
		X=x;     	Y=y;
		System.out.println("Point2D Constructed With Parameters!");
	}
	void offset(int a, int b)
	{//位移Point2D
		X+=a;       Y+=b;
		System.out.println("Point2D Offset to " + this.X +"  " + this.Y);
	}
    void show_2d( )
    {
         System.out.println("Point2D position  " + this.X +"  " + this.Y); 
     }
     
      //这段注释后，会出错；如果不注释，则是运行时的多态现象
     /*void show_3d()
     {
     	System.out.println("nothing!");
     	
     }	*/
}
class Point3D extends Point2D 
{
	int X, Y, Z;	//定义3D自己的X,Y,Z
	public Point3D(int x, int y, int z)
	{ //3D的有参构造函数
		X=x;    Y=y;   Z=z;
		System.out.println("Point3D Constructed with Parameters!"
			     +this.X  + "  " + this.Y + "  " + this.Z); 
    }		
	public Point3D(Point2D p2d,int z)
    { //由2D扩展的构造函数
		X=p2d.X;    Y=p2d.Y;
		Z=z;
		System.out.println("Point3D Constructed from Point2D and Z! " 
	                        + this.X + "  " + this.Y + "  " + this.Z); 
	}					
	void offset(int a, int b,int c)
	{  //3D点的位移
		this.X+=a;  this.Y+=b;  this.Z+=c;
		System.out.println("Point3D Offset!/n");  
    }
	void show_3d( ) 
	{
         System.out.println("Point3D position  " + this.X +"  " + this.Y  +"  " + this.Z ); 
    }	
}
public class TestPoint{
	public static void main(String[] args)
	{
		Point2D p2d1, p2d2;           
		Point3D p3d1, p3d2;	
		p2d1 = new Point2D(2,2);    //生成父类对象		
		p3d1 = new Point3D(3,4,5); //生成子类对象
        p3d1.show_2d();
		p3d1.show_3d();

		p2d2 = p3d1;                        //父类引用指向子类对象
		p2d2.show_2d();                  //引用父类的成员变量
		p2d2.show_3d();                  //父类不能引用子类的方法
		                    
		//p3d2 = (Point3D)p2d1; 
                                ////强制将父类对象转换为子类对象，系统运行时出错 	       
       }	
}
