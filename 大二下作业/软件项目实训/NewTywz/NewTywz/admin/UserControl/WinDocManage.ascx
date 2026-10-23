<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinDocManage.ascx.cs" Inherits="NewTywz.UserControl.WinDocManage" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="DataList" Src="DataList.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WinUpload" Src="WinUpload.ascx" %> 

<UserControl:WinUpload id="WinDocManage_winUpload" runat="server"  onSaveClick="WinDocManage_UploadSaveClick" />

    <ext:Window ID="WinDocManage_winMore" runat="server"  Width="500" Height="300" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="文档详细信息">
    <Items>
        <ext:Panel ID="WinDocManage_ListPanel" runat="server" Header="false"  Width="490" Height="270" X="0" Y="0" >
        <Content>
            <UserControl:DataList 
                ID="WinDocManage_DataList" 
                runat="server" 
                onListEditClick="WinDocManage_ListEditClick" 
                onListAddClick="WinDocManage_ListAddClick" 
                onListDeleteClick="WinDocManage_ListDeleteClick" 
                onListMore1Click="WinDocManage_ListMore1Click" 
                />
        </Content>
        </ext:Panel>
    </Items>
    </ext:Window>

<ext:Hidden ID="WinDocManage_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="WinDocManage_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
<ext:Hidden ID="WinDocManage_RelationID" runat="server" Text="" /> 
