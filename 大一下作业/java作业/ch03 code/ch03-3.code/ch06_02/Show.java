import javax.swing.JOptionPane;
class Show
{
	public static void main(String[] args)
	{
		Shape arrayOfShapes[] = new Shape[10];
		
		//对数组里面的每个元素随机赋值
		for(int i=0; i<arrayOfShapes.length; i++)
		{
			int ran = (int)(Math.random()*100);
			if( ran%3 == 0 )
			{
				Point point = new Point((int)(Math.random()*100),
				                        (int)(Math.random()*100));
				arrayOfShapes[i] = point;
			}
			else if( ran%3 == 1 )
			{
				Circle circle = new Circle(Math.random()*100,
				                           (int)(Math.random()*100),
				                           (int)(Math.random()*100) );
				arrayOfShapes[i] = circle;
			}
			else 
			{
				Cylinder cylinder = new Cylinder( Math.random()*100,
					                              Math.random()*100,
				                                  (int)(Math.random()*100),
				                                  (int)(Math.random()*100) );
				arrayOfShapes[i] = cylinder;	
  		    }
			
		}
		
		String output = "";
		//显示数组里每个元素的类型
		for(int i=0; i<arrayOfShapes.length; i++)
		{
			output += arrayOfShapes[i].showName()+"\n";
		}
		
		//屏幕上输出结果
         JOptionPane.showMessageDialog(null,output,"Demonstring Polymorphism",
                                       JOptionPane.INFORMATION_MESSAGE);
         System.exit(0);
	}
}