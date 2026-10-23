<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<%@ page import="java.io.*" %>
<%
    String username = request.getParameter("username");
    String password = request.getParameter("password");
    boolean isAuthenticated = false;

    // 读取user.txt文件进行验证
    try {
        BufferedReader reader = new BufferedReader(new FileReader(getServletContext().getRealPath("/") + "user.txt"));
        String line;
        while ((line = reader.readLine()) != null) {
            String[] userDetails = line.split(",");
            if (userDetails.length == 3) {
                String fileUsername = userDetails[0];
                String filePassword = userDetails[1];
                if(fileUsername.equals(username) && filePassword.equals(password)) {
                    isAuthenticated = true;
                    break;
                }
            }
        }
        reader.close();
    } catch (Exception e) {
        out.println("<p>Error reading user data: " + e.getMessage() + "</p>");
    }

    if(isAuthenticated) {
        out.println("<h2>登录成功</h2>");
        out.println("<p>Welcome, " + username + "!</p>");
    } else {
        out.println("<h2>登录失败</h2>");
        out.println("<p>无效密码或用户名</p>");
        out.println("<a href='login.jsp'>Try again</a>");
    }
%>