<%@ Page Language="C#" MasterPageFile="~/admin/TreeSel.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="admin.UserManage.UserInfo" Title="无标题页" %>
<%@ Register TagPrefix="UserControl" TagName="WinUserMore" Src="/admin/UserControl/WinUserMore.ascx" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <UserControl:WinUserMore id="UserInfo_winMore" runat="server" onSaveClick="UserInfo_MoreSave"  />
</asp:Content>
