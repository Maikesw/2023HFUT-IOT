<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WebRotator.ascx.cs" Inherits="NewTywz.UserControl.WebRotator" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0" width="<%=Width%>" height="<%=Height%>">
 <param name="movie" value="/img/bcastr.swf">
 <param name="quality" value="high">
 <param name="wmode" value="transparent">
 <param name="FlashVars" value="bcastr_xml_url=/UserControl/<%=sXMLFileName%>">
 <embed src="/img/bcastr.swf" FlashVars="bcastr_xml_url=/UserControl/<%=sXMLFileName%>" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="<%=Width%>" height="<%=Height%>"></embed>
</object>

<%--<img src="\img\bigpic.jpg" width="768" height="334" />--%>

<ext:Hidden ID="WebRotator_Width" runat="server" Text="" />
<ext:Hidden ID="WebRotator_Height" runat="server" Text="" />