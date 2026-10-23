<%@ page import="java.io.*, java.util.Scanner" contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <title>注册结果</title>
</head>
<body>
<%
    String filePath = application.getRealPath("/") + "user.txt"; // 获取真实路径并指向user.txt
    String username = request.getParameter("username");
    String password = request.getParameter("password");
    String gender = request.getParameter("gender");

    File file = new File(filePath);
    boolean exists = false;

    // 检查用户是否已存在
    if(file.exists()){
        Scanner scanner = new Scanner(new FileInputStream(file));
        while(scanner.hasNextLine()){
            String line = scanner.nextLine();
            if(line.split(",")[0].equalsIgnoreCase(username)){
                exists = true;
                break;
            }
        }
        scanner.close();
    }

    // 根据是否存在用户显示信息
    if(exists){
        out.println("<h2>用户已存在！</h2>");
    } else {
        // 如果文件不存在，则尝试创建文件
        if(!file.exists()){
            file.createNewFile();
        }
        // 写入新用户数据
        FileWriter fw = new FileWriter(file, true); // true表示追加模式
        BufferedWriter bw = new BufferedWriter(fw);
        bw.write(username + "," + password + "," + gender);
        bw.newLine();
        bw.close();
        out.println("<h2>注册成功！</h2>");
    }
%>
</body>
</html>