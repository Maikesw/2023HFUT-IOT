//java.io.File类的使用

import java.io.*;
//import java.util.Date;

public class TestFileClass 
{
	
  public static void main(String args[]) throws Exception
  {  
    File dir1=new File("D:\\dir1");
    if(!dir1.exists())
    {
    	dir1.mkdir();
    }
         
    //创建子文件夹
    File dir2=new File(dir1,"dir2");
    if(!dir2.exists())
    {
    	dir2.mkdirs(); 
    }
    
	//创建多级子文件夹
    File dir4=new File(dir1,"dir4");
    if(!dir4.exists())
    {
    	dir4.mkdirs(); 
    }
    
    //创建文件
    File file=new File(dir2,"test.txt");
    if(!file.exists())
    {
    	file.createNewFile();
    }
    
	//自编方法，删除目录或文件
    deleteDir(dir4); 
   // deleteDir(dir1); 
  }

  
  /** 删除目录或文件，如果参数file代表目录，会删除当前目录以及目录下的所有内容*/
  public static void deleteDir(File file)
  {
    //如果file代表文件，就删除该文件
    if(file.isFile())
    {
      file.delete();
      return;
    }
    else
    {
    	//如果file代表目录，先删除目录下的所有子目录和文件
	    File[] lists=file.listFiles();
	    for(int i=0;i<lists.length;i++)
	    {
	    	deleteDir(lists[i]);  //递归删除当前目录下的所有子目录和文件
	    } 
	    
	    //最后删除当前目录    
	    file.delete();
    }

  }	
}
