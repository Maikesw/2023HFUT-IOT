<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="NewTywz.WebForm11" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>    
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
    <div>
        <ext:Hidden ID="hiddenValue" runat="server" Text="" />
                <ext:Button ID="Button1" runat="server" Text="确定1" Width="50">
                    <DirectEvents>
                        <Click OnEvent="btnSure1_Click" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="Button2" runat="server" Text="确定2" Width="50">
                    <DirectEvents>
                        <Click OnEvent="btnSure2_Click" />
                    </DirectEvents>
                </ext:Button>

    
    </div>
    </form>
</body>
</html>
