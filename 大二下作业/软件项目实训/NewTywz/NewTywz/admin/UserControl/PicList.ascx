<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="PicList.ascx.cs" Inherits="NewTywz.UserControl.PicList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="WinNeedConfirm" Src="WinNeedConfirm.ascx" %> 


<style type="text/css">
        .images-view .x-panel-body{
	        background: white;
	        font: 11px Arial, Helvetica, sans-serif;
        }
        .images-view .thumb{
	        background: #dddddd;
	        padding: 3px;
        }
        .images-view .thumb img{
	        height: 60px;
	        width: 80px;
        }
        .images-view .thumb-wrap{
	        float: left;
	        margin: 4px;
	        margin-right: 0;
	        padding: 5px;
	        text-align:center;
        }
        .images-view .thumb-wrap span{
	        display: block;
	        overflow: hidden;
	        text-align: center;
        }

        .images-view .x-view-over{
            border:1px solid #dddddd;
            background: #efefef url(/img/row-over.gif) repeat-x left top;
	        padding: 4px;
        }

        .images-view .x-view-selected{
	        background: #eff5fb url(/img/selected.gif) no-repeat right bottom;
	        border:1px solid #99bbe8;
	        padding: 4px;
        }
        .images-view .x-view-selected .thumb{
	        background:transparent;
        }

        .images-view .loading-indicator {
	        font-size:11px;
	        background-image:url(/img/loading.gif);
	        background-repeat: no-repeat;
	        background-position: left;
	        padding-left:20px;
	        margin:10px;
        }
    </style>

<UserControl:WinNeedConfirm id="PicList_winSure" runat="server"  onSureClick="PicList_SureClick" />

<ext:BorderLayout ID="PicList_BorderLayout" runat="server">
<Center>
        <ext:Panel runat="server" ID="PicList_ImagePanel" Cls="images-view" Frame="true" Layout="Fit" Header="false" Border="true" BodyStyle="background:#FFFFFF">
            <Items>
                <ext:DataView ID="PicList_dvInfo" runat="server" 
                    StoreID="PicList_storeInfo" SingleSelect="true" OverClass="x-view-over" 
                    ItemSelector="div.thumb-wrap" EmptyText="<div style='padding:0px;'>该类没有图片信息</div>">
                    <Template ID="PicList_Template" runat="server">
                        <Html>
				            <tpl for=".">
					            <div class="thumb-wrap" id="{name}">
						            <div class="thumb"><img src="{url}" title="{name}"></div>						            
					            </div>
				            </tpl>
				            <div class="x-clear"></div>        
			            </Html>
                    </Template>
                    <DirectEvents>
                        <DblClick OnEvent="PicList_DBClick" />
                        <Click OnEvent="PicList_Click" />
                    </DirectEvents>
                </ext:DataView>
            </Items>
            <BottomBar>
                <ext:PagingToolBar runat="server" PageSize="9" HideRefresh="True" StoreID="PicList_storeInfo" />
            </BottomBar>
            <TopBar>
                <ext:Toolbar  runat="server">
                    <Items>  
                          <ext:Button ID="PicList_btnAdd" runat="server" Text="新增" Icon="Add" Visible="false">
                              <DirectEvents>
                                  <Click OnEvent="PicList_AddClick" >
                                    <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                                  </Click>
                              </DirectEvents>
                          </ext:Button>
                          <ext:Button ID="PicList_btnEdit" runat="server" Text="修改" Icon="NoteEdit" Visible="false" >
                              <DirectEvents>
                                  <Click OnEvent="PicList_EditClick">
                                    <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                                  </Click>
                              </DirectEvents>
                          </ext:Button>
                          <ext:Button ID="PicList_btnDelete" runat="server" Text="删除" Icon="Delete" Visible="false">
                              <DirectEvents>
                                  <Click OnEvent="PicList_DeleteClick" />
                              </DirectEvents>
                          </ext:Button>
                          <ext:Button ID="PicList_btnOrderUp" runat="server" Icon="ArrowUp" Text="上移" Visible="false">
                              <DirectEvents>
                                  <Click OnEvent="PicList_OrderUpClick" >
                                    <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                                  </Click>
                              </DirectEvents>
                          </ext:Button>
                          <ext:Button ID="PicList_btnOrderDown" runat="server" Icon="ArrowDown" Text="下移" Visible="false">
                              <DirectEvents>
                                  <Click OnEvent="PicList_OrderDownClick" >
                                    <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                                  </Click>
                              </DirectEvents>
                          </ext:Button>
                          <ext:Button ID="PicList_btnMore1" runat="server" Visible="false">
                              <DirectEvents>
                                  <Click OnEvent="PicList_More1Click" >
                                    <EventMask Msg="正在操作,请稍候..." ShowMask="true" />
                                  </Click>
                              </DirectEvents>
                          </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:Panel>
</Center>
</ext:BorderLayout>

    <ext:Store ID="PicList_storeInfo" runat="server" RemoteSort="false"  AutoLoad="true">
    <AutoLoadParams>
        <ext:Parameter Name="start" Value="0" Mode="Raw" />
        <ext:Parameter Name="limit" Value="10" Mode="Raw" />
    </AutoLoadParams>
    <Reader>
        <ext:JsonReader>    
            <Fields>
                <ext:RecordField Name="id" />
                <ext:RecordField Name="name" />
                <ext:RecordField Name="url" />      
                <ext:RecordField Name="size" Type="Int" />
                <ext:RecordField Name="lastmod" Type="Date" DateFormat="Y-m-dTh:i:s" />
            </Fields>
        </ext:JsonReader>
    </Reader>
    </ext:Store>



<ext:Hidden ID="PicList_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="PicList_FileNameColumn" runat="server" Text="" />     <!--当前数据库视图名称-->
<ext:Hidden ID="PicList_FileAddressColumn" runat="server" Text="" />     <!--当前列名称组合-->
<ext:Hidden ID="PicList_HasDelFlag" runat="server" Text="1" />     <!--是否考虑伪删除，默认考虑-->
<ext:Hidden ID="PicList_OrderBy" runat="server" Text=" Order By TheOrder" />     <!--排序方法-->
<ext:Hidden ID="PicList_InitWhere" runat="server" Text="" />     <!--初始的where-->
<ext:Hidden ID="PicList_NowWhere" runat="server" Text="" />     <!--当前的where-->
<ext:Hidden ID="PicList_InitSQL" runat="server" Text="" />     <!--初始时的SQL-->
<ext:Hidden ID="PicList_NowSQL" runat="server" Text="" />     <!--当前的SQL-->
<ext:Hidden ID="PicList_RowNowID" runat="server" Text="" />     <!--当前选中的ID-->
