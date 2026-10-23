abstract class Outer
{
	public abstract void show();
}
class TestAno
{
	public static void main(String[] args)
	{
		Outer out = new Outer()
		{
			public void show()
			{
				System.out.println ("run in Anonymous");
			}
		};
		out.show();
	}
}