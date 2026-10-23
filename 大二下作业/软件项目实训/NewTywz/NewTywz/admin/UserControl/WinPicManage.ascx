<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinPicManage.ascx.cs" Inherits="NewTywz.UserControl.WinPicManage" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="PicList" Src="PicList.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WinUpload" Src="WinUpload.ascx" %> 

<UserControl:WinUpload id="WinPicManage_winUpload" runat="server"  onSaveClick="WinPicManage_UploadSaveClick" />

    <ext:Window ID="WinPicManage_winMore" runat="server"  Width="530" Height="270" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="图片详细信息">
    <Items>
        <ext:Panel ID="WinPicManage_ListPanel" runat="server" Header="false"  Width="520" Height="240" X="0" Y="0" >
        <Content>
            <UserControl:PicList 
                ID="WinPicManage_PicList" 
                runat="server" 
                onListEditClick="WinPicManage_ListEditClick" 
                onListAddClick="WinPicManage_ListAddClick" 
                onListDeleteClick="WinPicManage_ListDeleteClick" 
                onListMore1Click="WinPicManage_ListMore1Click" 
                />
        </Content>
        </ext:Panel>
    </Items>
    </ext:Window>

<ext:Hidden ID="WinPicManage_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="WinPicManage_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
<ext:Hidden ID="WinPicManage_RelationID" runat="server" Text="" /> 
