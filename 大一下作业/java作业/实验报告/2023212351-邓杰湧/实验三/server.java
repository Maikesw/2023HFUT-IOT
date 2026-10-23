import java.io.*;
import java.lang.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.net.Socket;
import java.net.UnknownHostException;
import java.net.ServerSocket;
import java.net.Socket;
public class server extends JFrame implements ActionListener {
	JLabel L1,L2,L3;
	JTextField t1,t2;
	JTextArea T1;
	JButton B1,B2;
	ServerSocket ss;
	Socket s;
	BufferedWriter bw;
	
	public server(){
		init();
		setVisible(true);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	void init(){
		setLayout(null);
		L1=new JLabel("服务器设置");
		L2=new JLabel("Port: ");
		L3=new JLabel("Say: ");
		t1=new JTextField();
		t2=new JTextField();
		T1=new JTextArea();
		B1=new JButton("Start");
		B2=new JButton("Say");
		add(L1);
		L1.setBounds(20,10,150,30);
		add(L2);
		L2.setBounds(20,50,30,30);
		add(L3);
		L3.setBounds(20,400,30,30);
		add(t1);
		t1.setBounds(60,50,480,30);
		add(t2);
		t2.setBounds(60,400,480,30);
		add(T1);
		T1.setBounds(30,100,620,280);
		add(B1);
		B1.setBounds(560,50,100,30);
		add(B2);
		B2.setBounds(560,400,100,30);
		B1.addActionListener(this);
		B2.addActionListener(this);
	}
	public void actionPerformed(ActionEvent e){
		if(e.getSource()==B1){
			T1.append("server starting...\n");
			connect();
			}
		else if (e.getSource()==B2){send();}
	}
	public void connect(){
		try{
		int port = Integer.parseInt(t1.getText());
		ss = new ServerSocket(port);
		s = ss.accept();
		T1.append("Client connected...\n");
		BufferedReader br = new BufferedReader(new InputStreamReader(s.getInputStream()));
		c cc=new c(br,T1);
		cc.start();
		}
		catch(IOException e){e.printStackTrace();}
	}
	
	public void send(){
		try{
		String mess = t2.getText();
		T1.append(mess+"\n");
		bw = new BufferedWriter(new OutputStreamWriter(s.getOutputStream()));
        bw.write(mess+"\n");
        bw.flush();
		}
		catch(IOException e){e.printStackTrace();}
	}
	public static void main(String []args){
		server a=new server();
		a.setBounds(100,100,700,500);
		a.setTitle("服务器");
	}
	class c extends Thread{
		BufferedReader br;
		JTextArea T;
		public c(BufferedReader br1,JTextArea T1){br=br1;T=T1;}
		public void run(){
			while(true){
				try{
				String mess = br.readLine();
				T.append(mess+"\n");
				}
				catch(IOException e){e.printStackTrace();}
			}	
		}
	}
	
	
}
