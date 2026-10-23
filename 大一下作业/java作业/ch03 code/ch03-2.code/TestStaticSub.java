class Blocks {  
    static Tool t1 = new Tool(1);
    Tool t2 = new Tool(2); 
    Blocks() {  
        System.out.println("父类构造函数");  
    }  
}  
   
class SubBlocks extends Blocks {  
    static Tool t1 = new Tool(3);
    Tool t2 = new Tool(4); 
     public SubBlocks() {  
        System.out.println("子类构造函数");  
    }  
}
class TestStaticSub
{
	   public static void main(String[] args)
	   {  
             new SubBlocks(); 
             System.out.println("One more time:");
             new SubBlocks();   
       }  
}  
class Tool{
	Tool(int i) {
		System.out.println( "Tool(" + i + ")" );
	}
}
