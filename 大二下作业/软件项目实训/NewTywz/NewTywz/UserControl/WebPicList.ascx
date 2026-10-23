<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WebPicList.ascx.cs" Inherits="NewTywz.UserControl.WebPicList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="WebPageToolBar" Src="/UserControl/WebPageToolBar.ascx" %> 

<asp:Table ID="WebPicList_tab" runat="server" border="0" cellspacing="0" cellpadding="0" />
<UserControl:WebPageToolBar id="WebPicList_Page" runat="server"  />

<ext:Hidden ID="WebPicList_NowPageNum" runat="server" Text="" />
<ext:Hidden ID="WebPicList_MaxRowNum" runat="server" Text="" />
<ext:Hidden ID="WebPicList_MaxColNum" runat="server" Text="" />
<ext:Hidden ID="WebPicList_ChannelTypeID" runat="server" Text="" />
<ext:Hidden ID="WebPicList_NeedPageToolbar" runat="server" Text="" />
