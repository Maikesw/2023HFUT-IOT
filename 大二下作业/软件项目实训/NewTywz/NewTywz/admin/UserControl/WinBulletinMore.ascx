<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinBulletinMore.ascx.cs" Inherits="NewTywz.UserControl.WinBulletinMore"%> 
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<ext:Window ID="WinBulletinMore_winMore" runat="server"  Width="350" Height="200" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="详细信息">
    <Items>
        <ext:Label ID="WinBulletinMore_labName" runat="server" X="5" Y="10" Text="标　　题:" />
        <ext:TextField ID="WinBulletinMore_txtName" X="58" Y="8" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinBulletinMore_labRemark" runat="server" X="5" Y="30" Text="内　　容:" />
        <ext:TextArea ID="WinBulletinMore_txtRemark" X="58" Y="28" runat="server" AnchorHorizontal="99%"  Height="80" />
        <ext:Label ID="WinBulletinMore_labTime" X="5" Y="108" runat="server" Text="发布时间:"/>
        <ext:datefield ID="WinBulletinMore_dateBulletinITime" X="58" Y="106" runat="server" AnchorHorizontal="99%" />
        <ext:Button ID="WinBulletinMore_btnSave" runat="server" X="80" Y="142" Text="保存" Icon="disk">
            <DirectEvents>
                <Click OnEvent="WinBulletinMore_SaveClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="WinBulletinMore_btnClose" runat="server" X="200" Y="142" Text="退出" Icon="Cross">
            <DirectEvents>
                <Click OnEvent="WinBulletinMore_CloseClick" />
            </DirectEvents>
        </ext:Button>
    
    </Items>
    </ext:Window>

<ext:Hidden ID="WinBulletinMore_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="WinBulletinMore_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
