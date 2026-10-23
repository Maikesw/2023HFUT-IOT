//枚举类型与for、switch语句
enum Fruit
{
    苹果,梨子,香蕉,西瓜,芒果
}
class Example4_03
{
	public static void main(String[] args)
	{
		double price=0;
	    boolean show=false;
	    for(Fruit fruit:Fruit.values())
	    {
	    	switch(fruit)
	    	{
	    		case 苹果:
	    			price = 3.5;
	    			show = true;
	    			break;
	    		case 梨子:
	    			price = 2.5;
	    			show = true;
	    			break;
	    		default:
	    			show = false;	
	    	}
	    	if(show)
	    	{
	    		System.out.println(fruit + "1斤的价格" + price);
	    	}
	    		
	    }
	}
    
    
	
}