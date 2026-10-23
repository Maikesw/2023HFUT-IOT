//isinstanceof
class Animal
{
	int height, weight;
	
	Animal()
	{
		//System.out.println("Animal Constructed without Parameters");
	}
	
	void eat()
	{
		System.out.println("animal eat");
	}
	void sleep()
	{
		System.out.println("animal sleep");
	}
	void breathe()
	{
		System.out.println("animal breathe");
	}
	
}

class Fish extends Animal   
{
	int height;  
	
    Fish()
	{
		//System.out.println("Fish Constructed");
	}
	void breathe()
	{
		System.out.println("Fish breathe: bubble");
	}
	
}

class Integration 
{
 	 public static void main(String[] args)
	 {
	 	 Animal an = new Animal();
		 Fish fh = new Fish();
         /*
         Animal an = new Animal();
         Fish fh = new fish();
         an = fh;
          */
         if(an instanceof Animal)
         {
         	System.out.println("an is Animal' instance");
         }
         
         if(fh instanceof Fish)
         {
         	System.out.println("fh is Fish's instance");
         }
         
         if(an instanceof Fish)
         {
         	System.out.println("an is Fish's instance");
         }
         else
         {
         	System.out.println("an is not Fish's instance");
         }
         
         if(fh instanceof Animal)
         {
         	System.out.println("fh is Animal's instance");
         }
         else
         {
         	System.out.println("fh is not Animal's instance");
         }
	 }
}

