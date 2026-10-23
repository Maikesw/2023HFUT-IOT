<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinChannelContent.ascx.cs" Inherits="NewTywz.UserControl.WinChannelContent" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register TagPrefix="UserControl" TagName="WinPicManage" Src="WinPicManage.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WinDocManage" Src="WinDocManage.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="DataComboBox" Src="DataComboBox.ascx" %> 

        <link rel="stylesheet" href="/themes/default/default.css" />
	    <link rel="stylesheet" href="/plugins/code/prettify.css" />
	    <script charset="utf-8" src="/kindeditor.js"></script>		
		<script charset="utf-8" src="/zh_CN.js"></script>
    	<script charset="utf-8" src="/plugins/code/prettify.js"></script>
	    <script>
	    	var editor1;
	    
		    KindEditor.ready(function(K) {
    			    editor1 = K.create('#content', {
	    			cssPath : '/plugins/code/prettify.css',
    				uploadJson : '/upload_json.ashx',
	    			fileManagerJson : '/file_manager_json.ashx',
		    		allowFileManager : true,
			    	afterCreate : function() {
				    	var self = this;
					    K.ctrl(document, 13, function() {
    						self.sync();
	    					K('form[name=example]')[0].submit();
		    			});
    					K.ctrl(self.edit.doc, 13, function() {
	    					self.sync();
		    				K('form[name=example]')[0].submit();
    					});
    				}
	    		});
    	    	prettyPrint();
	    	});
	    	
		    function SetText(s)
		    {
		        editor1.html(s);
		    }
		    
		    function SaveClick()
		    {
		        Ext.net.DirectMethods.WinChannelContent.WinChannelContent_SaveClick(editor1.html());
		        
		    }
		    
	    </script>


<UserControl:WinPicManage id="WinChannelContent_winPicMore" runat="server" />
<UserControl:WinDocManage id="WinChannelContent_winDocMore" runat="server" />

    <ext:Window ID="WinChannelContent_winMore" runat="server"  Width="750" Height="450" Icon="RecordBlue" Modal="true" Maximizable="true" Hidden="true" Layout="Absolute" Title="详细信息">
    <Items>
        <ext:Label ID="WinChannelContent_labName" runat="server" X="5" Y="5" Text="标　　题:" />
        <ext:TextField ID="WinChannelContent_txtName" X="58" Y="3" runat="server" AnchorHorizontal="99%" />
        <ext:Label ID="WinChannelContent_labAuthorName" runat="server" X="5" Y="33" Text="发 表 人:" />
        <ext:TextField ID="WinChannelContent_txtAuthorName" X="58" Y="30" runat="server" width="80"  />
        <ext:ComboBox ID="WinChannelContent_cmbTemplete" runat="server" width="180" FieldLabel="使用模版" LabelAlign="Right" X="100" Y="30" >
            <Items>
                <ext:ListItem Text="模版一" Value="Content3.aspx" />                
            </Items>
        </ext:ComboBox>
        <ext:DateField ID="WinChannelContent_FoundDate" runat="server" FieldLabel="发布时间" LabelAlign="Right" X="240" Y="30" Width="200" />
        <ext:Checkbox ID="WinChannelContent_chkTop" runat="server" FieldLabel="置顶" LabelAlign="Right" X="380" Y="30" />
        <ext:Checkbox ID="WinChannelContent_chkPop" runat="server" FieldLabel="首页显示" LabelAlign="Right" X="460" Y="30" />
        <ext:Checkbox ID="WinChannelContent_chkAuthor" runat="server" FieldLabel="权限阅读" LabelAlign="Right" X="540" Y="30" />
        <ext:panel  runat="server" AnchorHorizontal="99%" Height="330" X="5" Y="55" Header="false" AutoScroll="true" >
        <Content>
			<textarea id="content" style="width:700px;height:400px;"></textarea>        
        </Content>
        </ext:panel>

        <ext:Checkbox ID="WinChannelContent_chkSendNews" runat="server" X="5" Y="390" Hidden="true" />
        <ext:Label runat="server" Text="同时发送到:" X="26" Y="395" Hidden="true" />
        <ext:panel runat="server" Width="100" X="90" Y="390" Header="false"  Hidden="true">
        <Content>
        <UserControl:DataComboBox ID="WinChannelContent_cmbSendNews" runat="server" Width="100" />
        </Content>
        </ext:panel>
        <ext:Button ID="WinChannelContent_btnPic" runat="server" X="370" Y="390" Text="图片" Icon="ImageAdd" Visible="false">
            <DirectEvents>
                <Click OnEvent="WinChannelContent_PicClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="WinChannelContent_btnDoc" runat="server" X="430" Y="390" Text="文档" Icon="Page" >
            <DirectEvents>
                <Click OnEvent="WinChannelContent_DocClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="WinChannelContent_btnSave" runat="server" X="490" Y="390" Text="保存" Icon="disk">
<%--            <DirectEvents>
                <Click OnEvent="WinChannelContent_SaveClick" />
            </DirectEvents>--%>
            <Listeners>
                <Click  Handler="SaveClick();" />
            </Listeners>
        </ext:Button>
        <ext:Button ID="WinChannelContent_btnPreview" runat="server" X="550" Y="390" Text="预览" Icon="eye">
            <DirectEvents>
                <Click OnEvent="WinChannelContent_PreviewClick" />
            </DirectEvents>
        </ext:Button>
        <ext:Button ID="WinChannelContent_btnClose" runat="server" X="610" Y="390" Text="退出" Icon="Cross">
            <DirectEvents>
                <Click OnEvent="WinChannelContent_CloseClick" />
            </DirectEvents>
        </ext:Button>    
    </Items>
    </ext:Window>

<ext:Hidden ID="WinChannelContent_TableID" runat="server" Text="" />     <!--当前数据库表主ID-->
<ext:Hidden ID="WinChannelContent_ChannelID" runat="server" Text="" />     <!--当前所属channnel的ID-->
<ext:Hidden ID="WinChannelContent_OldPop" runat="server" Text="0" />     <!--原来的状态-->
