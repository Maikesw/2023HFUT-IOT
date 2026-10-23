<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleUserInfo.aspx.cs" Inherits="NewTywz.admin.UserManage.SingleUserInfo" %>
<%@ Register TagPrefix="UserControl" TagName="WinUserMore" Src="/admin/UserControl/WinUserMore.ascx" %> 
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="SingleUserInfo_ResourceManager" runat="server" />
    <UserControl:WinUserMore id="SingleUserInfo_winMore" runat="server" onSaveClick="SingleUserInfo_MoreSave"  onCloseClick="SingleUserInfo_MoreClose" />
    </form>
</body>
</html>
