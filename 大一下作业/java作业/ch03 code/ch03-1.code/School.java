class School
{     
        String name;   // name of school
        String address;   
        int stafNum;   //staff number
        Staff [] sGroup = new Staff[100000];   
        
        School()  {  }
        School(String s, String a, int n)  
        {   
        	  name = s;
        	  address = a;
        	  stafNum = n;
        }
        
        void addStaff(Staff s)   //add new staff
        {
        	 sGroup[stafNum] = s;
        	 stafNum ++ ;
        }
        
        public static void main(String[] args)
        {
        	School s = new School("HFUT", "Hefei", 10000);   
        	School.Staff st = s.new Staff("Wang", 9000, 2018);  
            s.addStaff(st);                                                          //add new staff
        	System.out.println ("The number of new Staff is " + s.stafNum);   
        }
        
        class Staff   //Inner class Staff
        {
        	  String stafName;   //name
        	  double salary;  
        	  int year;   //the year of joining
        	  
        	  Staff()  {    }
        	  Staff(String n, double s, int y) 
        	  {
        	  	    stafName = n;
        	  	    salary = s;
        	  	    year = y;
        	  } 
        	  
        }
}