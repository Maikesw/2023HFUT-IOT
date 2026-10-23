<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="SimpleDlg.aspx.cs" Inherits="admin.SimpleDlg" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="DataList" Src="/admin/UserControl/DataList.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WinSimpleMore" Src="/admin/UserControl/WinSimpleMore.ascx" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link rel="stylesheet" type="text/css" href="/css/css.css" />
</head>

<body scroll="no">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="SimpleDlg_ResourceManager" runat="server" />   
        <ext:Viewport ID="SimpleDlg_Viewport" runat="server" Layout="fit">
           <Content>
                <ext:BorderLayout ID="SimpleDlg_BorderLayout" runat="server">
                    <center>
                        <ext:Panel ID="SimpleDlg_ListPanel" runat="server" Layout="Fit" Header="false"  AutoHeight="false">
                        <Content>
                            <UserControl:DataList 
                                ID="SimpleDlg_DataList" 
                                runat="server" 
                                onListEditClick="SimpleDlg_ListEditClick" 
                                onListAddClick="SimpleDlg_ListAddClick" 
                                />
                       </Content>
                        </ext:Panel>
                    </center>
                </ext:BorderLayout>
           </Content>
        </ext:Viewport>

        <UserControl:WinSimpleMore id="SimpleDlg_winSimpleMore" runat="server" onSaveClick="SimpleDlg_SimpleMoreSave" />
    
   </form>
</body>
</html>
