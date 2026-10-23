<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulletinInfo.aspx.cs" Inherits="admin.UsualManage.BulletinInfo" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="DataList" Src="/admin/UserControl/DataList.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WinBulletinMore" Src="/admin/UserControl/WinBulletinMore.ascx" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body scroll="no">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="BulletinInfo_ResourceManager" runat="server" />   
        <ext:Viewport ID="BulletinInfo_Viewport" runat="server" Layout="fit">
           <Content>
                <ext:BorderLayout ID="BulletinInfo_BorderLayout" runat="server">
                    <center>
                        <ext:Panel ID="BulletinInfo_ListPanel" runat="server" Layout="Fit" Header="false"  AutoHeight="false">
                        <Content>
                            <UserControl:DataList 
                                ID="BulletinInfo_DataList" 
                                runat="server" 
                                onListEditClick="BulletinInfo_ListEditClick" 
                                onListAddClick="BulletinInfo_ListAddClick"                                 
                              />
                       </Content>
                        </ext:Panel>
                    </center>
                </ext:BorderLayout>
           </Content>
        </ext:Viewport>

        <UserControl:WinBulletinMore id="BulletinInfo_winMore" runat="server" onSaveClick="BulletinInfo_MoreSave" />
    
   </form>
</body>
</html>

