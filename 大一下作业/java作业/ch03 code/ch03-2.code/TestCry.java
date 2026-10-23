//多态示例
class  动物 
{
     void cry( ) 
     {
          // do nothing
     }  
}

class 狗 extends 动物 
{
      void cry() 
      {
          System.out.println("汪汪.....");  
      }  
}

class 猫 extends 动物 
{
      void cry() 
      {
          System.out.println("喵喵.....");  
      }  
}

public class TestCry 
{  
      public static void main(String args[]) 
      {
          动物 dongwu;
          
          dongwu=new 狗();
          dongwu.cry(); 
          
          dongwu=new 猫();
          dongwu.cry();
      }
}
