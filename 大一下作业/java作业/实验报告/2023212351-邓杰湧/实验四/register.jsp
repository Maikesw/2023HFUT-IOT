<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <title>用户注册</title>
</head>
<body>
<h2>注册表单</h2>
<form action="doRegister.jsp" method="post">
    用户名：<input type="text" name="username" required><br>
    密码：<input type="password" name="password" required><br>
    性别：<select name="gender">
        <option value="male">男</option>
        <option value="female">女</option>
    </select><br>
    <input type="submit" value="注册">
</form>
</body>
</html>