<%@ Page Language="C#"  MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="guestbook.aspx.cs" Inherits="NewTywz.guestbook" %>
<%@ Register TagPrefix="UserControl" TagName="WinLeaveMessage" Src="/admin/UserControl/WinLeaveMessage.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WebPageToolBar" Src="/UserControl/WebPageToolBar.ascx" %> 
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="672"  border="0" cellspacing="0" cellpadding="0"  align="center">
<tr>
    <td height="30" align="center" valign="middle" colspan="3"><asp:Button ID="GuestBook_ButtonMessage" 
            runat="server" Text="我要留言"  onclick="LeaveMessage_btnClick"/></td>
</tr>
<tr>
    <td width="30"></td>
    <td><asp:Table ID="tabBigBook" runat="server" border="0"  cellspacing="2" cellpadding="2" /></td>    
    <td width="20"></td>
</tr>
<tr>
    <td colspan="3"><UserControl:WebPageToolBar id="LeaveMessage_Page" runat="server"  /></td>
</tr>
</table>
<UserControl:WinLeaveMessage id="guestbook_winMsg" runat="server" />
<ext:Hidden ID="GBook_PageNow" runat="server" Text="1" />

</asp:Content>
