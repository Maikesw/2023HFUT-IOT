<%@ Page Language="C#" MasterPageFile="~/admin/Tree.Master" AutoEventWireup="true" CodeBehind="DepartmentInfo.aspx.cs" Inherits="admin.UserManage.DepartmentInfo" Title="无标题页" %>
<%@ Register TagPrefix="UserControl" TagName="WinDepartmentMore" Src="/admin/UserControl/WinDepartmentMore.ascx" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<UserControl:WinDepartmentMore id="DepartmentInfo_winMore" runat="server" onSaveClick="DepartmentInfo_MoreSave"  />

</asp:Content>
