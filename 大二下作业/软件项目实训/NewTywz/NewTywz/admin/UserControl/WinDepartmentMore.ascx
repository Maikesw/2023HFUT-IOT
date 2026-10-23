<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinDepartmentMore.ascx.cs" Inherits="NewTywz.UserControl.WinDepartmentMore" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

    <ext:Window ID="WinDepartmentMore_winMore" runat="server"  Width="350" Height="330" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="详细信息">
    <Items>
        <ext:Label ID="WinDepartmentMore_labName" runat="server" X="5" Y="10" Text="部门名称:" />
        <ext:TextField ID="WinDepartmentMore_txtName" X="58" Y="8" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinDepartmentMore_labCode" runat="server" X="5" Y="35" Text="部门编码:" />
        <ext:TextField ID="WinDepartmentMore_txtCode" X="58" Y="33" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinDepartmentMore_labPhone" runat="server" X="5" Y="60" Text="部门电话:" />
        <ext:TextField ID="WinDepartmentMore_txtPhone" X="58" Y="58" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinDepartmentMore_labEmail" runat="server" X="5" Y="85" Text="电子信箱:" />
        <ext:TextField ID="WinDepartmentMore_txtEmail" X="58" Y="83" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinDepartmentMore_labFax" runat="server" X="5" Y="110" Text="部门传真:" />
        <ext:TextField ID="WinDepartmentMore_txtFax" X="58" Y="108" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinDepartmentMore_labAddress" runat="server" X="5" Y="135" Text="部门地址:" />
        <ext:TextField ID="WinDepartmentMore_txtAddress" X="58" Y="133" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinDepartmentMore_labPostalCode" runat="server" X="5" Y="160" Text="邮政编码:" />
        <ext:TextField ID="WinDepartmentMore_txtPostalCode" X="58" Y="158" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinDepartmentMore_labRemark" runat="server" X="5" Y="185" Text="说　　明:" />
        <ext:TextArea ID="WinDepartmentMore_txtRemark" X="58" Y="183" runat="server" AnchorHorizontal="99%"  Height="80" />
        <ext:Button ID="WinDepartmentMore_btnSave" runat="server" X="90" Y="270" Text="保存" Icon="disk">
            <DirectEvents>
                <Click OnEvent="WinDepartmentMore_SaveClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="WinDepartmentMore_btnClose" runat="server" X="210" Y="270" Text="退出" Icon="Cross">
            <DirectEvents>
                <Click OnEvent="WinDepartmentMore_CloseClick" />
            </DirectEvents>
        </ext:Button>
    
    </Items>
    </ext:Window>

<ext:Hidden ID="WinSimpleMore_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
