//  接口在编程中的应用  例06_04
//  接口定义的成员变量全部使用final static进行修饰
//  接口不允许有程序体
//  接口不是类，所有接口中不能定义构造方法
//  运行时用java ch06_04.Car06_04

package ch06_04;
import java.util.*;//引用使用类

interface Automobile
{
   int i=5;           //static &final
   void accelent();   
   void maintain();   //Automatically public
   String forward(); 
   String reverse();
}

class Little_car implements Automobile
{
   public void accelent()
   {
       System.out.println("Little_car.accelent()");
   }
   public void maintain(){ }
   public String forward() 
   {
   		return "Little_car.forward";
   	}
   public String reverse()
   {
   		return "Little_car.reverse";
   }
   
   /*
   public void output()
   {
   	    System.out.println("This is a little car!");
   }
   */
}

class Big_car implements Automobile  
{
   public void accelent() 
   {
       System.out.println(" Big_car.accelent()");
   }
   public void maintain(){ }
   public String forward() {return "Big_car.forward";}
   public String reverse(){return "Big_car.reverse";}
}  

class  Jeep  implements Automobile  
{
   public void accelent()  
   {
       System.out.println("Jeep.accelent()");
   }
   public void maintain(){ }
   public String forward() {return " Jeep.forward";}
   public String reverse(){return " Jeep.reverse";}
}

class  Microbus  extends Big_car 
{
   public void accelent()   
   {
       System.out.println("Microbus.accelent()");
   }
   public void maintain() 
   {
       System.out.println("Microbus. maintain()");
   }
}

class Bus extends Big_car 
{
   public String forward() {return "  Bus.forward";}
   public String reverse(){return "  Bus.reverse";} 
   
} 

public class Car06_04
{
   public static void main(String[] args)
   {
       Automobile[] cars=new Automobile[5];
       int i=0;
       cars[i++]=new Little_car();
       cars[i++]=new Big_car();
       cars[i++]=new Jeep();
       cars[i++]=new Microbus();
       cars[i++]=new Bus();
       for(i=0;i<cars.length;i++)
       {
       	   cars[i].accelent();
       }
       
       //cars[0].output();
   }
}