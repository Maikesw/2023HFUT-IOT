<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="DataTree.ascx.cs" Inherits="NewTywz.UserControl.DataTree" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="WinNeedConfirm" Src="WinNeedConfirm.ascx" %> 


<UserControl:WinNeedConfirm id="DataTree_winDelSure" runat="server"  onSureClick="DataTree_DelSureClick" />

    <ext:TreePanel ID="DataTree_TreeInfo" runat="server" header="false" Animate="False" AnimCollapse="False" CollapseFirst="False" Border="false" Layout="fit" >                
        <Root>
        </Root>        
        <SelectionModel>
            <ext:DefaultSelectionModel ID="DataTree_Select" runat="server" />            
        </SelectionModel>
        <DirectEvents>
            <Click OnEvent="TreeNodeClick" />            
        </DirectEvents>
        <LoadMask ShowMask="true" Msg="正在加载,请稍候..." />
    </ext:TreePanel>

<ext:Hidden ID="DataTree_TreeNowID" runat="server"  Text="0" />     <!--当前点击节点ID-->
<ext:Hidden ID="DataTree_TreeNewID" runat="server"  Text="-1" />     <!--当前新增节点ID-->
<ext:Hidden ID="DataTree_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="DataTree_RootName" runat="server" Text="" />      <!--树根名-->
<ext:Hidden ID="DataTree_NeedRoot" runat="server" Text="1" />      <!--是否需要树根-->
<ext:Hidden ID="DataTree_SomeSQL" runat="server"  Text="" />       <!--如果是显示部分时有效-->
<ext:Hidden ID="DataTree_CodeName" runat="server"  Text="" />      <!--第二个文本框对应数据库表的字段名-->
<ext:Hidden ID="DataTree_NewFlag" runat="server" Text="0" />   <!--当前编辑操作是新增还是修改-->
<ext:Hidden ID="DataTree_MaxLevel" runat="server" Text="3" />      <!--树节点最大层数-->
<ext:Hidden ID="DataTree_TreeListName" runat="server" Text="" />
<ext:Hidden ID="DataTree_RelationTableName" runat="server" Text="" />     
<ext:Hidden ID="DataTree_RelationID1" runat="server" Text="" />     
<ext:Hidden ID="DataTree_OldCheckListIDs" runat="server" Text="" />
<ext:Hidden ID="DataTree_NewCheckListIDs" runat="server" Text="" />
<ext:Hidden ID="DataTree_IsCheckTree" runat="server" Text="0" />