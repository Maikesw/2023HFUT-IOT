<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinGBookManage.ascx.cs" Inherits="NewTywz.UserControl.WinGBookManage" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<%--<script language="JavaScript">
    
    document.onkeydown = check;
    function check(e) {
        var code;
        if (!e) var e = window.event;
        if (e.keyCode) code = e.keyCode;
        else if (e.which && e) code = e.which;
        //alert(code);
        if (code == 8) {
            code = 0;
            // window.alert('test');
            return false;
        }
        return true;
    //    }
   
</script>--%>

<ext:Window ID="WinGBookManage_winMore" runat="server"  Width="615" Height="420" Icon="RecordBlue" Modal="true" Hidden="true" Layout="Absolute" Title="留言详细信息">
    <Items>
        <ext:Label ID="WinGBookManage_labHead" runat="server" X="5" Y="10" Text="留言标题:" />
        <ext:TextField ID="WinGBookManage_txtHead" X="58" Y="8" runat="server"  width="260"   ReadOnly="true" />
        <ext:Label ID="WinGBookManage_labTime" runat="server" X="320" Y="10" Text="留言时间:" />
        <ext:datefield ID="WinGBookManage_dateTime" X="373" Y="8" runat="server" width="85"   ReadOnly="true" />
        <ext:Label ID="WinGBookManage_labName" runat="server" X="460" Y="10" Text="留言者:" />
        <ext:TextField ID="WinGBookManage_txtName" X="500" Y="8" runat="server" width="90" ReadOnly="true"/>        
        <ext:Label ID="WinGBookManage_labText" runat="server" X="5" Y="100" Text="留言内容:"  />
        <ext:TextArea ID="WinGBookManage_txtText" X="58" Y="28" runat="server" width="532"  Height="150" ReadOnly="true"/>
        <ext:Label ID="WinGBookManage_labAnswer" X="5" Y="250" runat="server" Text="回复内容:"/>
        <ext:TextArea ID="WinGBookManage_txtAnswer" X="58" Y="178" runat="server" width="532" Height="150" />
        <ext:Label ID="WinGBookManage_labAnswerTime" X="5" Y="330" runat="server" Text="回复时间:"/>
        <ext:datefield ID="WinGBookManage_dateAnswerTime" X="58" Y="328" runat="server" width="532" />
        <ext:Button ID="WinGBookManage_btnSave" runat="server" X="160" Y="360" Text="保存" Icon="disk">
            <DirectEvents>
                <Click OnEvent="WinGBookManage_SaveClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="WinGBookManage_btnClose" runat="server" X="360" Y="360" Text="退出" Icon="Cross">
            <DirectEvents>
                <Click OnEvent="WinGBookManage_CloseClick" />
            </DirectEvents>
        </ext:Button>
    
    </Items>
</ext:Window>

<ext:Hidden ID="WinGBookManage_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="WinGBookManage_TableID" runat="server" Text="" /> 
