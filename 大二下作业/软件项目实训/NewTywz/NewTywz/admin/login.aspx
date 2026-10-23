<%@ Page Language="C#" AutoEventWireup="True" SmartNavigation="true" EnableEventValidation="false" Inherits="admin.login" Codebehind="login.aspx.cs" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>某网站后台信息管理系统</title>
    <link href="/css/css.css" rel="stylesheet" type="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <style type="text/css">
    body {
	background-color: #BEE2FA;
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
</style>
</head>
<script  type="text/javascript">
   function InputPass() {
        Ext.net.DirectMethods.InputPass();
    }

</script>

<body scroll=no>
<form id="form1" runat="server">
    <ext:Hidden ID="tempA" runat="server" Text="0" /> 
  <ext:ResourceManager ID="Login_ResourceManager" runat="server" />
  <table width="100%"  border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td background="img/loginbak2.jpg">&nbsp;</td>
      <td width="580" height="675" background="img/loginbak1.jpg" valign="top" >
      <table width="100%"  border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width=180 height="400">&nbsp;</td>
          <td width=240 colspan="3" >&nbsp;</td>
        </tr>
        <tr>
          <td>&nbsp;</td>
          <td colspan="3"><ext:TextField ID="Login_txtUserName" runat="server"  Width="210px" Height="20" /></td>
        </tr>
        <tr>
          <td>&nbsp;</td>
          <td colspan="3">
            <div id="divInputPass"><ext:TextField ID="Login_txtPassWord" InputType="Password"  runat="server"  Width="210px" Height="20" /></div>
                <ext:KeyNav ID="Login_KeyNav" runat="server" Target="divInputPass">
                    <Enter Handler="InputPass();"  />
                </ext:KeyNav>            
          </td>
        </tr>
        <tr><td colspan="4" height="5"></td></tr>
        <tr>
          <td>&nbsp;</td>
          <td align="center" >
                <ext:Button ID="Login_btnSure" runat="server" Text="确定" Width="50">
                    <DirectEvents>
                        <Click OnEvent="btnSure_Click" />
                    </DirectEvents>
                </ext:Button>
          </td>
          <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
          <td align="center">
                <ext:Button ID="Login_btnReset" runat="server" Text="重置" Width="50">
                    <DirectEvents>
                        <Click OnEvent="btnReset_Click" />
                    </DirectEvents>
                </ext:Button>
          </td>
          <td width="190"></td>
        </tr>
      </table>
      </td>
      <td background="img/loginbak3.jpg">&nbsp;</td>
    </tr>
  </table>
</form>
</body>
</html>
