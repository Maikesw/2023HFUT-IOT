<%@ Page Language="C#" AutoEventWireup="True" Inherits="admin.main" Codebehind="main.aspx.cs" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>某网站后台信息管理系统</title>
    <link rel="stylesheet" type="text/css" href="/css/css.css" />
<style>#top{width:100%; height:75px; background-image:url(Image/banner.jpg); padding:0px; margin:0px;}</style>    

    <script language="JavaScript" type="text/javascript">
    function createExampleTab(id, url,headname){
    var win, tab, hostName, exampleName, node;
    url="/"+url;
    win = new Ext.Window({
        id: "w"+id,
        layout: "fit",        
        iconCls: "icon-pagewhitecode",
        width: 925,
        height: 650,
        maximizable: true,
        constrain: true,
        closeAction: "hide",
        listeners: {
            beforeshow: {
                fn: function(el) {
                    var height = Ext.getBody().getViewSize().height;
                    if (el.getSize().height > height) {
                        el.setHeight(height - 20)
                    }
                }
            },
            show:{
                single:true
            }
        }        
    });   
    
    
    hostName = window.location.protocol+"//"+window.location.host;   
    exampleName = url.substr(9);
    
    tab = ExampleTabs.add(new Ext.Panel({
        id: id,        
        title: headname,
        autoLoad: {
            showMask: true,
            maskMsg: '正在加载,请稍候...',
            scripts: true,
            mode: "iframe",
            url: hostName+url
        },
        listeners: {
            deactivate: {
                fn: function(el) {
                    if (this.sWin && this.sWin.isVisible()) {
                        this.sWin.hide();
                    }
                }
            }
        },
        closable: true
    }));
    
    tab.sWin = win;    
    ExampleTabs.setActiveTab(tab);
    
}
    
    
        var loadExample = function (href, id, headname) {  
            Ext.net.DirectMethods.VerifySession();
            var tab = ExampleTabs.getComponent(id);
            if (tab) {
                ExampleTabs.setActiveTab(tab);
            } else {
                createExampleTab(id, href,headname);               
            }
        }
        
        
    </script>
    
    
</head>
<body>

    <form id="form1" runat="server">
    <ext:ResourceManager ID="Main_ResourceManager" runat="server" />
        <ext:Viewport ID="Main_Viewport" runat="server">
           <Content>          
                <ext:BorderLayout runat="server">
                    <north Margins-Bottom="1" >
                        <ext:Panel IDMode="Ignore" runat="server"  Header="false" Border="false">
                        <Content>
                            <table width="100%" border="0"  cellpadding="0" cellspacing="0">
                            <tr>
                                <td height="20" id="header" ><h1>某网站后台信息管理系统</h1></td>
                            </tr> 
                            <tr>
                                <td align="right"><ext:Label ID="labWelcome" runat="server" />&nbsp;&nbsp;<a href="login.aspx">退出</a></td>
                            </tr>
                            </table>
                        </Content>                        
                       </ext:Panel>   
                    </north>
                    <west collapsible="true" split="true">
                        <ext:Panel Border="false" runat="server" Title="系统菜单" Width="200"  Header="true">
                            <Content>
                                <ext:AccordionLayout ID="Main_Menu" runat="server" Animate="true" >        
                                </ext:AccordionLayout>
                            </Content>
                        </ext:Panel>
                    </west>                    
                    <center >
                        <ext:Panel runat="server" Header="false" ID="CenterPanel" Layout="fit" >
                            <Content>
                                <ext:TabPanel ID="ExampleTabs" runat="server" ActiveTabIndex="0" Border="false"  EnableTabScroll="true">                                 
                                <Items>
                                    <ext:Panel runat="server" Closable="true" Title="Tab 1" Visible="false" >
                                        <Content><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /></Content>
                                    </ext:Panel>
                                 </Items>
                                 </ext:TabPanel>
                             </Content>
                        </ext:Panel>
                    </center>
                <South>
                    <ext:Panel ID="Main_Panel1" IDMode="Ignore" runat="server" Header="false" Border="false" Html="<div class='message' align=right>版权所有：傻子石</div>" />                
                </South>
                </ext:BorderLayout>
            </Content>
        </ext:Viewport>
    </form>  
</body>
</html>
