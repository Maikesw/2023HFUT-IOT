<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ThreeListComboBox.ascx.cs" Inherits="NewTywz.UserControl.ThreeListComboBox" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<ext:Store ID="ThreeListComboBox_storeCombo1" runat="server" AutoLoad="true">
 <Reader>
     <ext:JsonReader IDProperty="ID">
        <Fields>
            <ext:RecordField Name="Text" Type="String" />
            <ext:RecordField Name="Value" Type="String" />
        </Fields>
    </ext:JsonReader>
</Reader>
</ext:Store>

<ext:Store ID="ThreeListComboBox_storeCombo2" runat="server" AutoLoad="true">
 <Reader>
     <ext:JsonReader IDProperty="ID">
        <Fields>
            <ext:RecordField Name="Text" Type="String" />
            <ext:RecordField Name="Value" Type="String" />
        </Fields>
    </ext:JsonReader>
</Reader>
</ext:Store>

<ext:Store ID="ThreeListComboBox_storeCombo3" runat="server" AutoLoad="true">
 <Reader>
     <ext:JsonReader IDProperty="ID">
        <Fields>
            <ext:RecordField Name="Text" Type="String" />
            <ext:RecordField Name="Value" Type="String" />
        </Fields>
    </ext:JsonReader>
</Reader>
</ext:Store>              


<table  border="0" cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td>
        <ext:ComboBox EmptyText="无建议值" runat="server" ID="ThreeListComboBox_cmbBox1" Editable="False" StoreID="ThreeListComboBox_storeCombo1" DisplayField="Text" ValueField="Value" Width="80" >
            <DirectEvents>
                <Select OnEvent="ItemChange1" />
            </DirectEvents>
        </ext:ComboBox>
    </td>
</tr>
<tr>
    <td>
        <ext:ComboBox EmptyText="无建议值" runat="server" ID="ThreeListComboBox_cmbBox2" Editable="False" StoreID="ThreeListComboBox_storeCombo2" DisplayField="Text" ValueField="Value"  Width="80">
            <DirectEvents>
                <Select OnEvent="ItemChange2" />
            </DirectEvents>
        </ext:ComboBox>
    </td>
</tr>
<tr>
    <td>
        <ext:ComboBox EmptyText="无建议值" runat="server" ID="ThreeListComboBox_cmbBox3" Editable="False" StoreID="ThreeListComboBox_storeCombo3" DisplayField="Text" ValueField="Value"  Width="80" >
            <DirectEvents>
                <Select OnEvent="ItemChange3" />
            </DirectEvents>
        </ext:ComboBox>
    </td>
</tr>
</table>


<ext:Hidden ID="ThreeListComboBox_TableName" runat="server" Text="" />   <!--当前数据库表名称-->
              