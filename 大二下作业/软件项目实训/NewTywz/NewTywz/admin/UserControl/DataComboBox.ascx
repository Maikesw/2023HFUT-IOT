<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="DataComboBox.ascx.cs" Inherits="NewTywz.UserControl.DataComboBox" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

  <ext:Store ID="DataComboBox_storeInfo" runat="server" AutoLoad="true" >
     <Reader>
         <ext:JsonReader IDProperty="Value">
            <Fields>
                <ext:RecordField Name="Text" Type="String" />
                <ext:RecordField Name="Value" Type="String" />
            </Fields>
        </ext:JsonReader>
    </Reader>
  </ext:Store>

<ext:ComboBox EmptyText="无建议值" runat="server" ID="DataComboBox_cmbBox" StoreID="DataComboBox_storeInfo" DisplayField="Text" ValueField="Value">
     <DirectEvents>
         <Select OnEvent="ItemChange" />
     </DirectEvents>
</ext:ComboBox>


<ext:Hidden ID="DataComboBox_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="DataComboBox_NeedOrder" runat="server" Text="1" />     <!--是否需要排序，默认需要-->
<ext:Hidden ID="DataComboBox_Where" runat="server" Text="" />     <!---->
<ext:Hidden ID="DataComboBox_HasDelFlag" runat="server" Text="" />     <!---->
