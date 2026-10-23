class People
{
	//成员变量
	String name;
	String address;
	/*
	//构造函数
	People()
	{
		this("John","England");
	}	
	
	People(String strName, String strAdd)
	{
		name = strName;
		address = strAdd;
	}
	*/
	/* this的使用
	People(String name, String address)
	{
		this.name = name;
		this.address = address;
	}
	
	 */
	//成员方法
	void setInfo(String strName, String strAdd)
	{
		name = strName;
		address = strAdd;
	}
	
	String showName()
	{
		return name;
	}
}

public class Example4_01
{
	public static void main(String[] args)
	{
		People someguy = new People();
		someguy.setInfo("Jame", "America");
	//	System.out.println(someguy.showName());
		System.out.println(someguy.name);
	}
}