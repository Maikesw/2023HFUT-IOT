public class Example4_05 
{ 
    public static void main(String args[]) 
    {
    	Player huangA = new Player();
    	huangA.setInfo("Ronaldo", 1.86);
       
       Player pMostFamous = new Player();
       pMostFamous = huangA;
          
       pMostFamous.setInfo("Messi", 1.89);
       System.out.println(huangA.name);
       System.out.println(huangA.height);
    }    
}
class Player
{
	final int PI;
	String name; //姓名
	double height; //身高
	String teamName;  //属于哪个球队
	String gameNames[];  //参加过的比赛名称
	int scores[];    //每次比赛中的得分
	
	Player()
	{
	}
	void setInfo(String name, double height)
	{ 
		this.name = name;
		this.height = height;
	}
}
