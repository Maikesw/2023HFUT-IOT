
public class Test2 {
    public static void main(String[] s)
    {
   		E1 e1=new E1();
  		e1.m1();  
  		e1.m2(); 
  	}
}

class V1
{
        int a=1;
 		void m1()
 		{ System.out.println(a); } 
}
class E1 extends V1
{
 		int a=2,b=3,c;
 		void m2(){ System.out.println(a); } 
}

