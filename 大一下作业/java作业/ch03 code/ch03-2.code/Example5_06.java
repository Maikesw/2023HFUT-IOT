//∂‡Ã¨

class Animal
{
	int height, weight;
	
	Animal()
	{
		System.out.println("Animal Constructed without Parameters");
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
		System.out.println("Fish Constructed");
	}
	void breathe()
	{
		System.out.println("Fish breathe: bubble");
	}
	
}

class Integration 
{
     static void fn(Animal an) 
     {
         an.breathe();  
     }
	 public static void main(String[] args)
	 {
		 Fish fh = new Fish();
         Animal an;
         an = fh;
	     Integration.fn(an);
	 }
}

