<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="BaseInfo.aspx.cs" Inherits="admin.UsualManage.BaseInfo" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>网站基本信息</title>
    <link rel="stylesheet" type="text/css" href="/css/css.css" />
</head>

<body scroll="no">
    <form id="form2" runat="server" >
      <ext:ResourceManager ID="BaseInfo_ResourceManager" runat="server" />   
        <ext:Viewport ID="BulletinInfo_Viewport" runat="server" Layout="fit">
           <Content>
                <ext:BorderLayout ID="BaseInfo_BorderLayout" runat="server">
                    <center>
                        <ext:Panel ID="BaseInfo_ListPanel" runat="server"  Header="false"  AutoHeight="false" Layout="Absolute" bodyStyle="background:#C6D4E6">
                        <Content>
                            <ext:Label ID="BaseInfo_labName" runat="server" X="25" Y="10" Text="本站名称：" />   
		                    <ext:TextField ID="BaseInfo_txtName" runat="server" X="95" Y="8" AnchorHorizontal="95%" />
                            <ext:Label ID="BaseInfo_labURL" runat="server" X="25" Y="37" Text="本站网址：" />   
		                    <ext:TextField ID="BaseInfo_txtURL" runat="server" X="95" Y="35" AnchorHorizontal="95%" />
                            <ext:Label ID="BaseInfo_labEmail" runat="server" X="25" Y="64" Text="站长信箱：" />   
		                    <ext:TextField ID="BaseInfo_txtEmail" runat="server" X="95" Y="62" AnchorHorizontal="95%" />
		                    <ext:Label ID="BaseInfo_labInformation" runat="server" X="25" Y="91" Text="本站简介：" />   
		                    <ext:HtmlEditor ID="BaseInfo_txtInformation" runat="server" X="95" Y="89" AnchorHorizontal="95%" Height="120" />
		                    <ext:Label ID="BaseInfo_labAboutUS" runat="server" X="25" Y="215" Text="关于我们：" />   
		                    <ext:HtmlEditor ID="BaseInfo_txtAboutUS" runat="server" X="95" Y="213" AnchorHorizontal="95%" Height="120" />
		                    <ext:Label ID="BaseInfo_labFootMark" runat="server" X="25" Y="340" Text="脚　　注：" />   
		                    <ext:HtmlEditor ID="BaseInfo_txtFootMark" runat="server" X="95" Y="338" AnchorHorizontal="95%" Height="120" />
		                    <ext:Button ID="BaseInfo_btnSave" runat="server" Text="保存" X="470" Y="470">
                                    <DirectEvents>
                                        <Click OnEvent="BaseInfo_SaveClick" />
                                    </DirectEvents>
		    			    </ext:Button>
		                                     
                       </Content>
                        </ext:Panel>
                    </center>
                </ext:BorderLayout>
           </Content>
        </ext:Viewport>
    
   </form>
</body>
</html>

