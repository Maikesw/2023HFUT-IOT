//狗的抽象类
abstract class Dog
{
	void cry()
	{
		System.out.println("汪汪");
	}
}
//猪的抽象类
abstract class Pig
{
	void cry()
	{
		System.out.println("哼哼");
	}
}
//导盲功能
interface Guidable
{
	void guide();
}
//搜爆功能
interface SearchExplosive
{
	void search();
}
class GuideDog extends Dog implements Guidable
{
	public void guide()
	{
		System.out.println("I can guide blind!");
	}
}

class SearchDog extends Dog implements SearchExplosive
{
	public void search()
	{
		System.out.println("I can search explosive!");
	}
}
class TestDog
{
	public static void main(String[] args)
	{
		SearchDog dog1 = new SearchDog();
	    dog1.cry();
	    dog1.search();
	    
	    GuideDog dog2 = new GuideDog();
	    dog2.cry();
	    dog2.guide();
	}
}