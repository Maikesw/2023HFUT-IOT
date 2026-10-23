<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="RoleAuthorization.aspx.cs" Inherits="admin.UserManage.RoleAuthorization" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="DataList" Src="/admin/UserControl/DataList.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="DataTree" Src="/admin/UserControl/DataTree.ascx" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="RoleAuthorization_ResourceManager" runat="server" />
        <ext:Viewport ID="RoleAuthorization_Viewport" runat="server">
           <Content>
                <ext:BorderLayout ID="RoleAuthorization_BorderLayout1" runat="server">
                    <West collapsible="true" split="true">
                        <ext:Panel ID="RoleAuthorization_ListPanel" runat="server" Header="false"  Width="222" >
                        <Content>
                            <UserControl:DataList ID="RoleAuthorization_DataList" 
                                runat="server" 
                                onListRowClick="RoleAuthorization_ListRowClick"  
                                onListMore1Click="RoleAuthorization_ListMore1Click" 
                            />
                        </Content>
                        </ext:Panel>
                    </West>
                    <Center>
                        <ext:Panel ID="RoleAuthorization_AllTreePanel" runat="server" Border="true" Header="true" AutoScroll="True">
                        <Items>
                            <ext:BorderLayout ID="RoleAuthorization_BorderLayout2" runat="server">
                            <West split="true">
                                <ext:Panel ID="RoleAuthorization_TreeOperationPanel" runat="server" Border="true" Width="280" Title="所有菜单" AutoScroll="True" >
                                <TopBar>
                                    <ext:Toolbar ID="RoleAuthorization_Toolbar1" runat="server">
                                     <Items>
                                        <ext:Button ID="RoleAuthorization_OperationExpandAll" runat="server" Text="展开" Icon="ArrowOut">
                                          <DirectEvents>
                                              <Click OnEvent="RoleAuthorization_ExpandOperationAllClick" />
                                          </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="RoleAuthorization_OperationCollapseAll" runat="server" Text="折叠" Icon="ArrowIn">
                                          <DirectEvents>
                                              <Click OnEvent="RoleAuthorization_CollapseOperationAllClick" />
                                          </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Content> 
                                    <UserControl:DataTree id="RoleAuthorization_DataTreeOperation" runat="server"  width="280" />
                                </Content>
                                </ext:Panel>
                      	    </West>
                            <Center>
                                <ext:Panel ID="RoleAuthorization_TreeChannelPanel" runat="server" Border="true" Width="280" Header="true" Title="所有栏目" AutoScroll="True" >
                                <TopBar>
                                    <ext:Toolbar ID="RoleAuthorization_Toolbar2" runat="server">
                                     <Items>
                                        <ext:Button ID="RoleAuthorization_ChannelExpandAll" runat="server" Text="展开" Icon="ArrowOut">
                                          <DirectEvents>
                                              <Click OnEvent="RoleAuthorization_ExpandChannelAllClick" />
                                          </DirectEvents>
                                        </ext:Button>
                                        <ext:Button ID="RoleAuthorization_ChannelCollapseAll" runat="server" Text="折叠" Icon="ArrowIn">
                                          <DirectEvents>
                                              <Click OnEvent="RoleAuthorization_CollapseChannelAllClick" />
                                          </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Content> 
                                    <UserControl:DataTree id="RoleAuthorization_DataTreeChannel" runat="server"  width="280" />
                                </Content>
                                </ext:Panel>
                      	    </Center>
                      	    </ext:BorderLayout>
                        </Items>
                        </ext:Panel>
           
                    </Center>
                </ext:BorderLayout>
           </Content>
        </ext:Viewport>

    
        <input type="hidden" ID="RoleID" runat="server" value="0"/>
    
    </form>
</body>
</html>
