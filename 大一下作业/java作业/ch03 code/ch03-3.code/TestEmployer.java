abstract class Employee
{
	private String name;
	private String id;
	private double pay;
	Employee(String name, String id, double pay)
	{
		this.name = name;
		this.id = id;
		this.pay = pay;
	}
	public  abstract void work();
}

class Programmer extends Employee
{
	Programmer(String name, String id, double pay)
	{
		super(name,id,pay);
	}
	public void work()
	{
		System.out.println("code...");
	}
}

//√Ë ˆæ≠¿Ì
class Manager extends Employee
{
	private int bonus;
	Manager(String name, String id, double pay, int bonus)
	{
		super(name, id, pay);
		this.bonus = bonus;
	}
	public void work()
	{
		System.out.println("manage...");
	}
}

public class TestEmployer
{
	public static void main(String[] args)
	{
		Employee emp[] = new Employee[2];
		emp[0] = new Manager("Jack","1",8000.5, 2000);
		emp[0].work();
		emp[1] = new Programmer("John", "2", 6000.0);
		emp[1].work();
	}
}