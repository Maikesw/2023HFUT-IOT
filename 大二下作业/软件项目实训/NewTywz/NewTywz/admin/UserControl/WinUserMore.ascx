<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinUserMore.ascx.cs" Inherits="NewTywz.UserControl.WinUserMore" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="DataComboBox" Src="/admin/UserControl/DataComboBox.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="ThreeListComboBox" Src="/admin/UserControl/ThreeListComboBox.ascx" %> 


<script  type="text/javascript">


var Selector = {
           WinUserMore_add: function() {
               var source=Ext.getCmp('<%=WinUserMore_gpAllRoles.ClientID%>');
               var destination = Ext.getCmp('<%=WinUserMore_gpSelRoles.ClientID%>');
               if (source.hasSelection()) {
                  var records = source.selModel.getSelections();
                  source.deleteSelected();
                  Ext.each(records, function(record){
                    destination.store.addSorted(record);                    
                  });                   
               }
            },
            WinUserMore_addAll: function() {
                var source = Ext.getCmp('<%=WinUserMore_gpAllRoles.ClientID%>');
                var destination = Ext.getCmp('<%=WinUserMore_gpSelRoles.ClientID%>');
                var records = source.store.getRange();
                source.store.removeAll();
                Ext.each(records, function(record){
                    destination.store.addSorted(record);                    
                });                 
            },
            WinUserMore_remove: function() {
                var destination = Ext.getCmp('<%=WinUserMore_gpAllRoles.ClientID%>');
                var source = Ext.getCmp('<%=WinUserMore_gpSelRoles.ClientID%>');
                if (source.hasSelection()) {
                    var records = source.selModel.getSelections();
                    source.deleteSelected();
                    Ext.each(records, function(record){
                        destination.store.addSorted(record);                    
                    });
               }
            },
            WinUserMore_removeAll: function() {
                var destination = Ext.getCmp('<%=WinUserMore_gpAllRoles.ClientID%>');
                var source = Ext.getCmp('<%=WinUserMore_gpSelRoles.ClientID%>');
                var records = source.store.getRange();
                source.store.removeAll();
                Ext.each(records, function(record){
                    destination.store.addSorted(record);                    
                });                 
            }
        };

        function WinUserMore_submitData() {
            var i = 0;
            var s = "";
            while (i < Ext.getCmp('<%=WinUserMore_gpSelRoles.ClientID%>').store.getCount()) {
                s = s + Ext.getCmp('<%=WinUserMore_gpSelRoles.ClientID%>').store.getAt(i).get('ID') + ",";
                i++;
            }
            s = s.slice(0, s.length - 1);
            Ext.net.DirectMethods.winUser.WinUserMore_SaveClick(s);
        }
</script>


<ext:Window ID="WinUserMore_winMore" runat="server"  Width="620" Height="375" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="详细信息">
  <Items>
     
     <ext:TabPanel ID="WinUserMore_tabPanelInfo" Title="Panel详细信息" runat="server" Height="360"  Width="625"  Plain="true"  EnableTabScroll="true">
    <Items>
        <ext:Panel ID="WinUserMore_panelUserInfo" runat="server" Title="用户信息" Padding="6"  Height="360" Width="625"   bodyStyle="background:#C6D4E6">  
           <Content>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td width="52">登陆帐号:</td>
                    <td width="240"><ext:TextField ID="WinUserMore_txtCode" runat="server" width="240" /></td>
                    <td width="52">登陆密码:</td>
                    <td width="240"><ext:TextField ID="WinUserMore_txtPsd"  runat="server" width="235" /></td>
                </tr>
                <tr>
                    <td>用户姓名:</td>
                    <td><ext:TextField ID="WinUserMore_txtName" runat="server" width="240" /></td>
                    <td>用户性别:</td>
                    <td>
                        <ext:ComboBox ID="WinUserMore_cmbSex" runat="server" Width="235">
                            <Items>
                                <ext:ListItem Text="男" Value="男" />
                                <ext:ListItem Text="女" Value="女" />
                            </Items>
                        </ext:ComboBox>
                    </td>
                </tr>
                <tr>
                    <td>身份证号:</td>
                    <td><ext:TextField ID="WinUserMore_txtIDCard" runat="server" width="240" /></td>
                    <td>手&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;机:</td>
                    <td><ext:TextField ID="WinUserMore_txtMobilePhone" runat="server" width="235" /></td>
                </tr>
                <tr>
                    <td>出生日期:</td>
                    <td><ext:datefield ID="WinUserMore_dateBirth" runat="server" width="240" /></td>
                    <td>联系电话:</td>
                    <td><ext:TextField ID="WinUserMore_txtPhone" runat="server" Width="235" /></td>
                </tr>
                <tr>
                    <td>地&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;址:</td>
                    <td><ext:TextField ID="WinUserMore_txtAddress" runat="server" width="240" /></td>
                    <td >所在部门:</td>
                    <td rowspan="3"><UserControl:ThreeListComboBox ID="WinUserMore_cmbDepartment" runat="server" Width="235"  /></td>
                </tr>
                <tr>
                    <td>邮政编码:</td>
                    <td><ext:TextField ID="WinUserMore_txtPostalCode" runat="server" width="240" /></td>
                </tr>
                <tr>
                    <td>电子邮件:</td>
                    <td><ext:TextField ID="WinUserMore_txtEmail" runat="server" width="240" /></td>
                </tr>
                <tr>
                    <td>注册时间:</td>
                    <td><ext:datefield ID="WinUserMore_dateRegister" runat="server"  Width="240" /></td>
                    <td>职&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;务:</td>
                    <td><UserControl:DataComboBox ID="WinUserMore_cmbDuty" runat="server" Width="235" EmptyText="我就是来测试一下" /></td>
                </tr>
                <tr  height="2"><td colspan="4"></td></tr>
                <tr>
                    <td valign="top">备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;注:</td>
                    <td width="532" colspan="3"><ext:TextArea ID="WinUserMore_txtRemark" runat="server" width="537"  Height="80" /></td>
                </tr>
                </table>
           </Content>
      </ext:Panel>

      <ext:Panel ID="WinUserMore_panelRoleInfo" runat="server" Title="角色信息" Padding="6" AutoScroll="true" Height="360" Width="625" Layout="Absolute" bodyStyle="background:#C6D4E6" >  
       <Content>

        <ext:GridPanel ID="WinUserMore_gpAllRoles"  runat="server"  Title=""  Header="false" TrackMouseOver="true" width="230"  height="270" X="32" Y="10"  AutoExpandColumn="Name">
             <Store>
                <ext:Store ID="WinUserMore_storeAllRoles" runat="server">
                    <Reader>
                        <ext:ArrayReader> 
                            <Fields>
                                <ext:RecordField Name="ID" />
                                <ext:RecordField Name="Name" />
                            </Fields>
                        </ext:ArrayReader>
                    </Reader>
                </ext:Store>
            </Store>
            <ColumnModel ID="WinUserMore_ColumnModelChoose" runat="server">
                <Columns>
                    <ext:Column ColumnID="WinUserMore_Name" Header="可选项" DataIndex="Name" />
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel ID="WinUserMore_RowSelectionChoose" runat="server" SingleSelect="false" />
            </SelectionModel>
        </ext:GridPanel>        
           
            <ext:Button ID="WinUserMore_Add" runat="server" Icon="ResultsetNext" ToolTip="加入" X="291" Y="70">
            <Listeners>
                <Click Handler="Selector.WinUserMore_add();" />
            </Listeners>
            </ext:Button>
 
            <ext:Button ID="WinUserMore_AddAll" runat="server" Icon="ResultsetLast" ToolTip="全部加入" X="291" Y="110">
            <Listeners>
                <Click Handler="Selector.WinUserMore_addAll();" />
            </Listeners>
            </ext:Button>

            <ext:Button ID="WinUserMore_Remove" runat="server" Icon="ResultsetPrevious" ToolTip="移出" X="291" Y="150">
            <Listeners>
                <Click Handler="Selector.WinUserMore_remove();" />
            </Listeners>
            </ext:Button>
      
            <ext:Button ID="WinUserMore_RemoveAll" runat="server" Icon="ResultsetFirst" ToolTip="全部移出" X="291" Y="190">
            <Listeners>
                <Click Handler="Selector.WinUserMore_removeAll();" />
            </Listeners>
            </ext:Button>
           
        <ext:GridPanel  ID="WinUserMore_gpSelRoles" runat="server" Title="" Header="false" TrackMouseOver="true" width="230" height="270" X="340" Y="10" AutoExpandColumn="Name">
               <Store>
                    <ext:Store ID="WinUserMore_storeSelRoles" runat="server">
                        <Reader>
                            <ext:ArrayReader>
                                <Fields>
                                    <ext:RecordField Name="ID" />
                                    <ext:RecordField Name="Name" />
                                </Fields>
                            </ext:ArrayReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel ID="WinUserMore_ColumnModelChose" runat="server">
                    <Columns>
                        <ext:Column ColumnID="WinUserMore_NameChose" Header="已选项" DataIndex="Name" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="WinUserMore_RowSelectionChose" runat="server" SingleSelect="false" />
                </SelectionModel>
           </ext:GridPanel>         
        
       </Content>
      </ext:Panel>
  
    </Items>
   </ext:TabPanel>
     
     <ext:Button ID="WinUserMore_btnSave" runat="server" X="140" Y="315" Text="保存" Icon="disk">
         <Listeners>
            <Click Handler="WinUserMore_submitData();" />
         </Listeners>  
     </ext:Button>
     <ext:Button ID="WinUserMore_btnClose" runat="server" X="415" Y="315" Text="退出" Icon="Cross">
         <DirectEvents>
             <Click OnEvent="WinUserMore_CloseClick" />
         </DirectEvents>
     </ext:Button>  
            
  </Items>
</ext:Window>

<%-- <input type="hidden" ID="WinUserMore_RowID" runat="server" value="0"/><!--当前选中的记录ID-->   --%>


<ext:Hidden ID="WinUserMore_RowID" runat="server" Text="" />           <!--当前选中的记录ID-->
<ext:Hidden ID="WinUserMore_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="WinUserMore_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
