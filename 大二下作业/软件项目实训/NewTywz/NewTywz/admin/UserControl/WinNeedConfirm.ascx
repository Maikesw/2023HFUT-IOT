<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinNeedConfirm.ascx.cs" Inherits="NewTywz.UserControl.WinNeedConfirm" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

    <ext:Window ID="WinNeedConfirm_winMore" runat="server"  Width="200" Height="80" Icon="RecordBlue" Modal="true" Hidden="true" Title="注意">
    <Content>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr><td colspan="3" height="10"></td></tr>
        <tr>
            <td colspan="3" align="center"><ext:Label ID="WinNeedConfirm_labTip" runat="server" /></td>
        </tr>
        <tr><td colspan="3" height="10"></td></tr>
        <tr>
            <td width="45%" align="right">
                <ext:Button ID="WinNeedConfirm_btnSure" runat="server" Text="确定" Icon="Tick">
                    <DirectEvents>
                        <Click OnEvent="WinNeedConfirm_SureClick" />
                    </DirectEvents>
                </ext:Button>
            <td width="10%"></td>
            </td width="45%">
            <td align="left">
                <ext:Button ID="WinNeedConfirm_btnClose" runat="server" Text="取消" Icon="Cross">
                    <DirectEvents>
                        <Click OnEvent="WinNeedConfirm_CloseClick" />
                    </DirectEvents>
                </ext:Button>
            </td>
        </tr>
        </table>    
    </Content>
    </ext:Window>

<ext:Hidden ID="WinNeedConfirm_TableName" runat="server" Text="" />     <!--当前数据库表名称-->
<ext:Hidden ID="WinNeedConfirm_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
