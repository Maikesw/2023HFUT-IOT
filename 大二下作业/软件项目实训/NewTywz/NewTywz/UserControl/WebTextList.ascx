<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WebTextList.ascx.cs" Inherits="NewTywz.UserControl.WebTextList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
 <%@ Register TagPrefix="UserControl" TagName="WebPageToolBar" Src="/UserControl/WebPageToolBar.ascx" %> 

<asp:Table ID="WebTextList_tab" runat="server" border="0" cellspacing="0" cellpadding="0" />
<UserControl:WebPageToolBar id="WebTextList_Page" runat="server"  />

<ext:Hidden ID="WebTextList_ListNum" runat="server" Text="" />
<ext:Hidden ID="WebTextList_NowPageNum" runat="server" Text="" />
<ext:Hidden ID="WebTextList_NameMaxLength" runat="server" Text="" />
<ext:Hidden ID="WebTextList_ChannelTypeID" runat="server" Text="" />
<ext:Hidden ID="WebTextList_NeedPageToolbar" runat="server" Text="" />
