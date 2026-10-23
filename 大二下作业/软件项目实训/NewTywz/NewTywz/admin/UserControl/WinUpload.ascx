<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinUpload.ascx.cs" Inherits="NewTywz.UserControl.WinUpload" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

        <ext:Window ID="WinUpload_winMore" runat="server" Width="280" Height="150" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="附件管理">
        <Items>
            <ext:Label ID="WinUpload_labName" runat="server" X="5" Y="10" Text="附件名称:" />
            <ext:TextField ID="WinUpload_txtName" X="58" Y="8" runat="server" AnchorHorizontal="98%" />
            <ext:Label ID="WinUpload_labRemark" runat="server" X="5" Y="35" Text="附件说明:" />
            <ext:TextField ID="WinUpload_txtRemark" X="58" Y="33" runat="server" AnchorHorizontal="98%" />
            <ext:Label ID="winUpload_txtupFile" runat="server" X="5" Y="60" Text="附件选择:" />
            <ext:FileUploadField  ID="WinUpload_upFile" runat="server" X="58" Y="58" Icon="ImageAdd" EmptyText="选择文件" ButtonText="" AnchorHorizontal="98%"  />
            <ext:Button ID="WinUpload_btnDownLoad" runat="server" X="40" Y="83" Text="下载" Icon="DiskUpload">
                <DirectEvents>
                    <Click OnEvent="WinUpload_DownLoadClick" />
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="WinUpload_btnSave" runat="server" X="110" Y="83" Text="保存" Icon="disk">
                <DirectEvents>
                    <Click OnEvent="WinUpload_SaveClick" />
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="WinUpload_btnClose" runat="server" X="180" Y="83" Text="退出" Icon="Cross">
                <DirectEvents>
                    <Click OnEvent="WinUpload_CloseClick" />
                </DirectEvents>
            </ext:Button>
        </Items>
        </ext:Window>      

<ext:Hidden ID="WinUpload_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="WinUpload_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
<ext:Hidden ID="WinUpload_RelationID" runat="server" Text="" />     <!--当前关联ID-->
<ext:Hidden ID="WinUpload_FilePath" runat="server" Text="" />     <!--全文件path-->
<ext:Hidden ID="WinUpload_Filter" runat="server" Text="" />
<ext:Hidden ID="WinUpload_RemarkVisit" runat="server" Text="0" />
<ext:Hidden ID="WinUpload_FileType" runat="server" Text="" />     <!--文件类型-->
<ext:Hidden ID="WinUpload_FileAddress" runat="server" Text="" />     <!--文件地址-->
