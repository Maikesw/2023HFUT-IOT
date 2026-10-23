public class TestOutputProperty
{
	public static void main(String[] args)
	{
		//访问Output接口的属性
		System.out.println(Output.MAX_CACHE_LINE);
		//下面语句将引起编译异常
		//Output.MAX_CACHE_LINE = 20;
	}
}