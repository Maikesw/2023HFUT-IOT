class Compute
{
	public double getAverage(int num, double ... x)
	{
		double sum = 0.0;
		for(double i:x)
		{
			sum += i;
		}
		sum = sum/num;
		return sum;
	}
}
public class Test_02
{
	public static void main(String[] args)
	{
		Compute result = new Compute();
		double average = result.getAverage(5, 11.2, 11.0, 11.5, 22.0, 51.2);
		System.out.println("平均数为:" + average);
	}
}