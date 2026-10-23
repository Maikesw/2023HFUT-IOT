//使用static修饰成员变量

class Student 
{ 
    static String name;
    String  sex;
    static String school="合肥工业大学";
  

    static void output()
    {
    	System.out.println(name);
    }    

}

public class Example4_03 
{ 
    public static void main(String args[]) 
    {
        Student me = new Student();
        Student you = new Student();
        
        me.name = "张敏 ";
        me.sex = "女";
        you.name = "李明 ";
        you.sex = "男 ";
        
        //对类成员变量赋值
        Student.school = "安徽大学";  
        System.out.println(me.name+" "+me.sex+" "+me.school);
        System.out.println(you.name+" "+you.sex+" "+you.school);
        
        Student.output();
    }    
}