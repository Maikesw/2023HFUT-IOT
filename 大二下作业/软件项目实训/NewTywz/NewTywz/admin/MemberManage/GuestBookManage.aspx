<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuestBookManage.aspx.cs" Inherits="admin.MemberManage.GuestBookManage" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="DataList" Src="/admin/UserControl/DataList.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WinGBookManage" Src="/admin/UserControl/WinGBookManage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body scroll="no">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="GBookManage_ResourceManager" runat="server" />   
        <ext:Viewport ID="GBookManage_Viewport" runat="server" Layout="fit">
           <Content>
                <ext:BorderLayout ID="GBookManage_BorderLayout" runat="server">
                    <center>
                        <ext:Panel ID="GBookManage_ListPanel" runat="server" Layout="Fit" Header="false"  AutoHeight="false">
                        <Content>
                            <UserControl:DataList 
                                ID="GBookManage_DataList" 
                                runat="server"                                 
                                onListEditClick="GBookManage_ListEditClick"
                                onListMore1Click="GBookManage_ListMore1Click" 
                                onListAllClick="GBookManage_ListAllClick"                               
                             />
                        </Content>
                        </ext:Panel>
                    </center>
                </ext:BorderLayout>
           </Content>
        </ext:Viewport>

        <UserControl:WinGBookManage id="GBookManage_winMore" runat="server" onSaveClick="GBookManage_MoreSave" />
    
   </form>
</body>
</html>
