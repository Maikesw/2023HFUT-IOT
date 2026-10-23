<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormTest.aspx.cs" Inherits="NewTywz.WebForm1" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="Login_ResourceManager" runat="server" />
       <ext:Hidden ID="temp" runat="server"   />    
                <ext:Button ID="Login_btnSure" runat="server" Text="确定" Width="50">
                    <DirectEvents>
                        <Click OnEvent="btnSure_Click" />
                    </DirectEvents>
                </ext:Button>

                 <ext:Button ID="Login_btnReset" runat="server" Text="重置" Width="50">
                    <DirectEvents>
                        <Click OnEvent="btnReset_Click" />
                    </DirectEvents>
                </ext:Button>

    </form>
</body>
</html>
