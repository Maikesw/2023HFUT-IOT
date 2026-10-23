import java.io.*;
import javax.swing.*;
import java.awt.event.*;
import java.net.Socket;
public class client extends JFrame implements ActionListener{
	JLabel L1,L2,L3,L4;
	JTextField t1,t2,t3;
	JTextArea T1;
	JButton B1,B2;
	Socket s;
	BufferedReader br;
	BufferedWriter bw;
	Threadcli cc;
	public client(){
		init();
		setVisible(true);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	void init(){
		setLayout(null);
		L1=new JLabel("客户端设置:");
		L2=new JLabel("Server IP: ");
		L3=new JLabel("Server port: ");
		L4=new JLabel("Say: ");
		t1=new JTextField();
		t2=new JTextField();
		t3=new JTextField();
		T1=new JTextArea();
		B1=new JButton("Connect");
		B2=new JButton("Say");
		add(L1);
		L1.setBounds(20,10,150,30);
		add(L2);
		L2.setBounds(20,40,150,30);
		add(L3);
		L3.setBounds(280,40,150,30);
		add(L4);
		L4.setBounds(20,390,30,30);
		add(t1);
		t1.setBounds(100,50,150,20);
		add(t2);
		t2.setBounds(380,50,150,20);
		add(t3);
		t3.setBounds(60,400,480,20);
		add(T1);
		T1.setBounds(30,100,620,280);
		add(B1);
		B1.setBounds(560,50,100,20);
		add(B2);
		B2.setBounds(560,400,100,20);
		B1.addActionListener(this);
		B2.addActionListener(this);
	}
	public void actionPerformed(ActionEvent e){
		if(e.getSource()==B1){connect();}
		else if (e.getSource()==B2){send();}
	}
	public void connect(){
		try{
		String address = t1.getText();
		int port = Integer.parseInt(t2.getText());
		s = new Socket(address,port);
		T1.append("Connect to server...\n");
		BufferedReader br = new BufferedReader(new InputStreamReader(s.getInputStream()));
		Threadcli cc=new Threadcli(br,T1);
		cc.start();
		}
		catch(IOException e){e.printStackTrace();}
	}
	
	public void send(){
		try{
		String mess = t3.getText();
		T1.append(mess+"\n");
		bw = new BufferedWriter(new OutputStreamWriter(s.getOutputStream()));
		bw.write(mess+"\n");
		bw.flush();
     
		}
		catch(IOException e){e.printStackTrace();}
	}
	class Threadcli extends Thread{
		BufferedReader br;
		JTextArea T;
		public Threadcli(BufferedReader br1,JTextArea T1){br=br1;T=T1;}
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
	public static void main(String []args){
		client a=new client();
		a.setBounds(100,100,700,500);
		a.setTitle("客户端");
	}
}
