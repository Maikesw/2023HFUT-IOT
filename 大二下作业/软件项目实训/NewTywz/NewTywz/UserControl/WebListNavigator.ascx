<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebListNavigator.ascx.cs" Inherits="NewTywz.UserControl.WebListNavigator" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<table width="278"  border="0" cellspacing="0" cellpadding="0">
<tr>
    <td>
    <table width="278"  border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td width="28"></td>
        <td width="250" align="left"><asp:Table ID="WebListNavigator_tabFirstName" runat="server" border="0"  cellspacing="0" cellpadding="0" /></td>
    </tr>
    <tr><td colspan="2" height="10"></td></tr>
    <tr>
        <td width="28"></td>
        <td>
            <table width="250"  border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td background="/img/navbk4.jpg" width="219" align="center"><asp:Table ID="WebListNavigator_tabSecondMenu" runat="server" border="0"  cellspacing="0" cellpadding="0" /></td>
                <td width="31"></td>
            </tr>
            </table>
        </td>
    </tr>
    </table>
    </td>
</tr>
<tr>
    <td>
    <table width="278"  border="0" cellspacing="0" cellpadding="0" id="teseTable" runat="server">
    <tr>
        <td width="28"></td>
        <td width="220"><img src="/img/navbk5.jpg" /></td>
        <td width="30"></td>
    </tr>
    <tr>
        <td width="28"></td>
        <td width="220" background="/img/navbk6.jpg">
        <table width="220"  border="0" cellspacing="0" cellpadding="0">
        <tr><td height="5"></td></tr>
        <tr>
            <td width="23"></td>
            <td width="173"><asp:Table ID="WebListNavigator_tabTese" runat="server" border="0"  cellspacing="0" cellpadding="0" /></td>
            <td width="24"></td>
        </tr>
        </table>        
        </td>
        <td width="30"></td>
    </tr>
    <tr>
        <td width="28"></td>
        <td width="220"><img src="/img/navbk8.jpg" /></td>
        <td width="30"></td>
    </tr>
    </table>
    </td>
</tr>
<tr><td align="center"><a href="#"><img src="/img/kefa.jpg" /></a></td></tr>
<tr><td align="center"><a href="#"><img src="/img/chuangyou.jpg" /></a></td></tr>

</table>

<ext:Hidden ID="WebListNavigator_sChannelDisID" runat="server" Text="" />
<ext:Hidden ID="WebListNavigator_sChannelID" runat="server" Text="" />
<ext:Hidden ID="WebListNavigator_sHeadName" runat="server" Text="" />