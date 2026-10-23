<%@ Page Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="True" CodeBehind="list1.aspx.cs" Inherits="NewTywz.list1" Title="无标题页" %>
 <%@ Register TagPrefix="UserControl" TagName="WebList" Src="/UserControl/WebTextList.ascx" %> 
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="759"  border="0" cellspacing="0" cellpadding="0">
<tr>
    <td width="1" background="/img/dot.jpg"></td>
    <td>
        <table width="757"  border="0" cellspacing="0" cellpadding="0">
        <tr height="1"><td background="/img/dot.jpg"></td></tr>
        <tr ><td><UserControl:WebList id="tabMain" runat="server" Width="757" /></td></tr>
        <tr height="1"><td background="/img/dot.jpg"></td></tr>
        </table>
    </td>
    <td width="1" background="/img/dot.jpg"></td>
</tr>
</table>
<ext:Hidden ID="list1_PageNow" runat="server" Text="1" />
</asp:Content>
