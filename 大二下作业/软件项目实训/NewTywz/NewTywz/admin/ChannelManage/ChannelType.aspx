<%@ Page Language="C#" MasterPageFile="~/admin/Tree.Master" AutoEventWireup="true" CodeBehind="ChannelType.aspx.cs" Inherits="admin.ChannelManage.ChannelType" Title="无标题页" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ext:Window ID="ChannelType_winMore" runat="server"  Width="100" Height="100" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="详细信息">
    <Items>
        <ext:Checkbox ID="ChannelType_chkAuthor" runat="server" FieldLabel="权限阅读" LabelAlign="Right"  X="2" Y="10" />
        <ext:Button ID="ChannelType_btnSave" runat="server" X="32" Y="30" Text="保存" Icon="disk">
            <DirectEvents>
                <Click OnEvent="ChannelType_SaveClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="ChannelType_btnClose" runat="server" X="92" Y="30" Text="退出" Icon="Cross">
            <DirectEvents>
                <Click OnEvent="ChannelType_CloseClick" />
            </DirectEvents>
        </ext:Button>    
    </Items>
    </ext:Window>

</asp:Content>
