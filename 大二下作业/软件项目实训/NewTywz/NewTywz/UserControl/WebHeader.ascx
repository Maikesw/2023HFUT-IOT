<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WebHeader.ascx.cs" Inherits="NewTywz.UserControl.WebHeader" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>


    <table width="1007" border="0"  align="center" cellpadding="0" cellspacing="0">
    <tr>
        <td width="4"></td>
        <td width="998"><img src="/img/head.jpg" /></td>
        <td width="5"></td>
    </tr>
    <tr>
        <td></td>
        <td bgcolor="#2daab3">
            <table cellpadding="0" border="0" width="100%" align="center" >
            <tr>
                <td width="730" valign="bottom"><asp:Label ID="WebHeader_MainHeadMenu" runat="server" /></td>
                <td width="242"></td>
            </tr>
            </table>
        </td>
        <td></td>
    </tr>
    <tr height="4"><td colspan="3"></td></tr>
    </table>

<ext:Hidden ID="WebHeader_Header" runat="server" Text="" />