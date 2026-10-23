<%@ Page Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true" CodeBehind="Content3.aspx.cs" Inherits="NewTywz.Content3" Title="无标题页" %>
<%@ Register TagPrefix="UserControl" TagName="WebContent" Src="/UserControl/WebContent.ascx" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <table width="759"  border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="1" background="/img/dot.jpg"></td>
                <td>
                    <table width="757"  border="0" cellspacing="0" cellpadding="0">
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>                    
                    <tr ><td><UserControl:WebContent id="tabMainList" runat="server"  /></td></tr>
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    </table>
                </td>
                <td width="1" background="/img/dot.jpg"></td>
            </tr>
            </table>



</asp:Content>
