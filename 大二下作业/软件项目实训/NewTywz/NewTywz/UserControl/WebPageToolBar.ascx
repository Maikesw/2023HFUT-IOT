<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WebPageToolBar.ascx.cs" Inherits="NewTywz.UserControl.WebPageToolBar" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<table cellpadding="0"  cellspacing="0" border="0" width="100%" visible="false" id="WebPageToolBar_MainTab" runat="server">
<tr>
    <td><asp:table ID="WebPageToolBar_tab" runat="server" cellpadding="0"  cellspacing="0" border="0" width="100%" /></td>
    <td valign="bottom">&nbsp;&nbsp;跳转到第<asp:DropDownList ID="WebPageToolBar_ddlist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="WebPageToolBar_SelectedIndexChanged" />页</td>
</tr>
</table>

<ext:Hidden ID="WebPageToolBar_AllNum" runat="server" Text="" />
<ext:Hidden ID="WebPageToolBar_NowPageNum" runat="server" Text="" />
<ext:Hidden ID="WebPageToolBar_MaxBodyNum" runat="server" Text="" />
<ext:Hidden ID="WebPageToolBar_ChannelID" runat="server" Text="" />
<ext:Hidden ID="WebPageToolBar_PageName" runat="server" Text="" />