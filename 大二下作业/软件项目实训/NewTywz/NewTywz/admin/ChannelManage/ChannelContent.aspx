<%@ Page Language="C#" MasterPageFile="~/admin/TreeSel.Master" AutoEventWireup="true" CodeBehind="ChannelContent.aspx.cs" Inherits="admin.ChannelManage.ChannelContent" Title="无标题页" %>
<%@ Register TagPrefix="UserControl" TagName="WinChannelContent" Src="/admin/UserControl/WinChannelContent.ascx" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<UserControl:WinChannelContent id="WinChannelContent_winMore" runat="server" onSaveClick="WinChannelContent_SaveClick"  />
</asp:Content>
