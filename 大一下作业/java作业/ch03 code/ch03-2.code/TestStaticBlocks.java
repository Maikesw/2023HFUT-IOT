class Blocks 
{  
    static    
    {  
        System.out.println("父类静态初始化块");  
    }  
    {        
    	System.out.println("父类初始化块");       
    }  
    Blocks() 
    {  
        System.out.println("父类构造函数");  
    }  
} 
class SubBlocks extends Blocks 
{  
    static 
    {   
          System.out.println("子类静态初始化块");     
    }    
    {    
    	System.out.println("子类初始化块");      
    }  
    public SubBlocks() 
    {  
        System.out.println("子类构造函数");  
    }  
}
class TestStaticBlocks
{
	public static void main(String[] args)
	{  
                    new SubBlocks(); 
                    System.out.println("One more time:");
                    new SubBlocks();   
       }  
} 
