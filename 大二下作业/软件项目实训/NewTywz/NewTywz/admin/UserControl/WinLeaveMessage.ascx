<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinLeaveMessage.ascx.cs" Inherits="NewTywz.UserControl.WinLeaveMessage" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>


<ext:Window ID="WinLeaveMessage_winMsg" runat="server"  Width="450" Height="210" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="留言">
    <Items>
        <ext:Label ID="WinLeaveMessage_labGuestBName" runat="server" X="5" Y="10" Text="您的称呼:" />
        <ext:TextField ID="WinLeaveMessage_txtGuestBName" X="58" Y="8" runat="server" AnchorHorizontal="99%"/>
        <ext:Label ID="WinLeaveMessage_labHead" runat="server" X="5" Y="35" Text="留言标题:" />
        <ext:TextField ID="WinLeaveMessage_txtHead" X="58" Y="33" runat="server" AnchorHorizontal="99%"/>
        <ext:Label ID="WinLeaveMessage_labMsg" runat="server" X="5" Y="60" Text="留言内容:"   />
        <ext:TextArea ID="WinLeaveMessage_txtMsg" X="58" Y="58" runat="server" AnchorHorizontal="99%"  Height="80" />
        <%--<ext:Label ID="WinLeaveMessage_labTime" runat="server" X="5" Y="119" Text="留言时间:" />
        <ext:datefield ID="WinLeaveMessage_dateTime" X="58" Y="117" runat="server" AnchorHorizontal="99%"/>--%>
        <ext:Button ID="WinLeaveMessage_btnSave" runat="server" X="100" Y="150" Text="保存" Icon="disk">
            <DirectEvents>
                <Click OnEvent="WinLeaveMessage_SaveClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="WinLeaveMessage_btnClose" runat="server" X="280" Y="150" Text="退出" Icon="Cross">
            <DirectEvents>
                <Click OnEvent="WinLeaveMessage_CloseClick" />
            </DirectEvents>
        </ext:Button>
    
    </Items>
</ext:Window>

<ext:Hidden ID="WinLeaveMessage_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
