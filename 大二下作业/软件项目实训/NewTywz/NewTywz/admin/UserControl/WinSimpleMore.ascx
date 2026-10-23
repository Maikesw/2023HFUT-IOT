<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinSimpleMore.ascx.cs" Inherits="NewTywz.UserControl.WinSimpleMore" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

    <ext:Window ID="WinSimpleMore_winMore" runat="server"  Width="350" Height="170" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="详细信息">
    <Items>
        <ext:Label ID="WinSimpleMore_labName" runat="server" X="5" Y="10" Text="名　　称:" />
        <ext:TextField ID="WinSimpleMore_txtName" X="58" Y="8" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinSimpleMore_labRemark" runat="server" X="5" Y="30" Text="说　　明:" />
        <ext:TextArea ID="WinSimpleMore_txtRemark" X="58" Y="28" runat="server" AnchorHorizontal="99%"  Height="80" />
        <ext:Button ID="WinSimpleMore_btnSave" runat="server" X="90" Y="112" Text="保存" Icon="disk">
            <DirectEvents>
                <Click OnEvent="WinSimpleMore_SaveClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="WinSimpleMore_btnClose" runat="server" X="210" Y="112" Text="退出" Icon="Cross">
            <DirectEvents>
                <Click OnEvent="WinSimpleMore_CloseClick" />
            </DirectEvents>
        </ext:Button>
    </Items>
    </ext:Window>

<ext:Hidden ID="WinSimpleMore_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="WinSimpleMore_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
