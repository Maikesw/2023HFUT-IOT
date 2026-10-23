class Outer
{
	private int index = 100;
	
	class Inner
	{
		private int index = 50;
		void print()
		{
			int index = 30;
			System.out.println(index);
			System.out.println(this.index);
			System.out.println(Outer.this.index);
		}
	}
	
	void print()
	{
		Inner inner = new Inner();
		inner.print();
	}
	
	Inner getInner()
	{
		return new Inner();
	}
}

class Test
{
	public static void main(String[] args)
	{
		Outer outer = new Outer();
		//outer.print();
		//Outer.Inner inner = outer.getInner();
		
		Outer.Inner inner = outer.new Inner();
		inner.print();
	}
}