<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="DataList.ascx.cs" Inherits="NewTywz.UserControl.DataList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="WinNeedConfirm" Src="WinNeedConfirm.ascx" %> 


<UserControl:WinNeedConfirm id="DataList_winSure" runat="server"  onSureClick="DataList_SureClick" />

<ext:BorderLayout ID="DataList_BorderLayout" runat="server">
<Center>
    <ext:GridPanel ID="DataList_gpInfo" runat="server" StoreID="DataList_storeInfo" AutoScroll="True" StripeRows="true" remoteSort="true" AutoHeight="false" Layout="fit" AnchorVertical="100%" >
        <ColumnModel runat="server" >
        <Columns>
            <ext:Column ColumnID="aa" Header="111" />        
        </Columns>
        </ColumnModel> 
    <SelectionModel>
        <ext:RowSelectionModel runat="server"  SingleSelect="false"   />
    </SelectionModel>    
    <BottomBar>
        <ext:PagingToolBar runat="server" PageSize="25" HideRefresh="True" >
        <DirectEvents>        
            <Change OnEvent="DataList_PageClick" />
        </DirectEvents>        
        </ext:PagingToolBar>
    </BottomBar>
    <LoadMask Msg="正在操作,请稍候..." ShowMask="true" />
    <TopBar>
        <ext:Toolbar  runat="server">
            <Items>  
                  <ext:Button ID="DataList_btnAdd" runat="server" Text="新增" Icon="Add" Visible="false">
                      <DirectEvents>
                          <Click OnEvent="DataList_AddClick" >
                            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
                  <ext:Button ID="DataList_btnEdit" runat="server" Text="修改" Icon="NoteEdit" Visible="false" >
                      <DirectEvents>
                          <Click OnEvent="DataList_EditClick">
                            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
                  <ext:Button ID="DataList_btnDelete" runat="server" Text="删除" Icon="Delete" Visible="false">
                      <DirectEvents>
                          <Click OnEvent="DataList_DeleteClick" />
                      </DirectEvents>
                  </ext:Button>
	              <ext:Button ID="DataList_btnReset" runat="server" Text="重置" Icon="Database" Visible="false">
                      <DirectEvents>
                          <Click OnEvent="DataList_ResetClick" >
                            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
	              <ext:Button ID="DataList_btnSearch" runat="server" Text="查询" Icon="Zoom" Visible="false">
                      <DirectEvents>
                          <Click OnEvent="DataList_SearchClick" >
                            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
	              <ext:Button ID="DataList_btnAll" runat="server" Text="全部" Icon="DiskMultiple" Visible="false">
                      <DirectEvents>
                          <Click OnEvent="DataList_AllClick" >
                            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
                  <ext:Button ID="DataList_btnOrderUp" runat="server" Icon="ArrowUp" Text="上移" Visible="false">
                      <DirectEvents>
                          <Click OnEvent="DataList_OrderUpClick" >
                            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
                  <ext:Button ID="DataList_btnOrderDown" runat="server" Icon="ArrowDown" Text="下移" Visible="false">
                      <DirectEvents>
                          <Click OnEvent="DataList_OrderDownClick" >
                            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
                  <ext:Button ID="DataList_btnMore1" runat="server" Visible="false">
                      <DirectEvents>
                          <Click OnEvent="DataList_More1Click" >
                            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
            </Items>
        </ext:Toolbar>
    </TopBar>
    <DirectEvents>
        <DblClick OnEvent="DataList_DBClick">
            <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
        </DblClick>
        <Click OnEvent="DataList_Click" />
    </DirectEvents>
    </ext:GridPanel>
</Center>
</ext:BorderLayout>

    <ext:Store ID="DataList_storeInfo" runat="server" RemoteSort="false"  AutoLoad="true">
    <AutoLoadParams>
        <ext:Parameter Name="start" Value="0" Mode="Raw" />
        <ext:Parameter Name="limit" Value="25" Mode="Raw" />
    </AutoLoadParams>
    <Reader>
        <ext:ArrayReader IDProperty="ID">
        <Fields>                     
                                         
        </Fields>
        </ext:ArrayReader>
    </Reader>
    </ext:Store>



<ext:Hidden ID="DataList_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="DataList_ViewName" runat="server" Text="" />     <!--当前数据库视图名称-->
<ext:Hidden ID="DataList_HeadName" runat="server" Text="" />     <!--当前列名称组合-->
<ext:Hidden ID="DataList_DataIndex" runat="server" Text="" />     <!--当前列值组合-->
<ext:Hidden ID="DataList_DataType" runat="server" Text="" />     <!--当前列值类型组合-->
<ext:Hidden ID="DataList_ColWidth" runat="server" Text="" />     <!--当前列宽组合-->
<ext:Hidden ID="DataList_FirstVisit" runat="server" Text="1" />     <!--刚开始是否可见-->
<ext:Hidden ID="DataList_HasDelFlag" runat="server" Text="1" />     <!--是否考虑伪删除，默认考虑-->
<ext:Hidden ID="DataList_DelFlagValue" runat="server" Text="0" />     <!--如果考虑伪删除，如果考虑，默认DelFlag=0-->
<ext:Hidden ID="DataList_OrderBy" runat="server" Text=" Order By ID DESC" />     <!--排序方法-->
<ext:Hidden ID="DataList_InitWhere" runat="server" Text="" />     <!--初始的where-->
<ext:Hidden ID="DataList_NowWhere" runat="server" Text="" />     <!--当前的where-->
<ext:Hidden ID="DataList_InitSQL" runat="server" Text="" />     <!--初始时的SQL-->
<ext:Hidden ID="DataList_NowSQL" runat="server" Text="" />     <!--当前的SQL-->
<ext:Hidden ID="DataList_RowNowID" runat="server" Text="" />     <!--当前选中的ID-->
<ext:Hidden ID="DataList_RowAllID" runat="server" Text="" />     <!--当前选中的所有ID-->
