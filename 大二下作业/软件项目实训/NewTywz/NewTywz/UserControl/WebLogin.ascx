<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WebLogin.ascx.cs" Inherits="NewTywz.UserControl.WebLogin" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<script language="javascript" type="text/javascript">
    function changeCode() {   
        var imgNode = document.getElementById("vimg");   
        imgNode.src = "/baseclass/ValidateCode.ashx?t=" + (new Date()).valueOf();  // 这里加个时间的参数是为了防止浏览器缓存的问题   
    }
    </script>

<table width=100%" height="122" border="0" cellpadding="0" cellspacing="0">
<tr visible="true" id="cell1" runat="server">
    <td background="/img/pasback.jpg" valign="top" >
        <table width="100%" height="122" border="0" cellpadding="0" cellspacing="0">
        <tr height="26"><td colspan="3"></td></tr>
        <tr height="24">
            <td width="65"></td>
            <td colspan="2" align="left"><ext:TextField ID="WebLogin_RegisterName" runat="server" Width="150" Cls="Style=border:1px;" Height="18" /></td>
        </tr>
        <tr height="24">
            <td width="65"></td>
            <td colspan="2" align="left"><ext:TextField ID="WebLogin_RegisterPass" runat="server" Width="150" InputType="Password" Cls="Style=border:1px;"  Height="18" /></td>
        </tr>
        <tr height="24">
            <td width="65"></td>
            <td width="74" align="left"><ext:TextField ID="WebLogin_ValidatePass" runat="server" Width="70" height="18"  /></td>
            <td valign="bottom"  align="left"><img width="75" height="18" src="/baseclass/ValidateCode.ashx" id="vimg" onclick="changeCode()"  title="看不清换一张" style="cursor:hand;" /> </td>
            <td></td>
        </tr>
        <tr height="24">
            <td colspan="3" align="center">
                        <ext:ImageButton ID="WebLogin_Login" runat="server" ImageUrl="img/login.jpg" >
                            <DirectEvents>
                                <Click OnEvent="LoginClick" />
                            </DirectEvents>
                        </ext:ImageButton>
           </td>
        </tr>
        </table>
    </td>
</tr>
<tr visible="false" id="cell2" runat="server">
    <td  valign="top">
        <table width="100%" height="102" border="0" cellpadding="0" cellspacing="0">
            <tr><td></td></tr>
            <tr><td align="center"><ext:Label ID="labWelcome" runat="server" /></td></tr>
            <tr><td></td></tr>
        </table>
    </td>
</tr>
</table>
