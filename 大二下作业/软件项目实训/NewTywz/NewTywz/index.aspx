<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="NewTywz.index" Title="无标题页" %>
<%@ Register TagPrefix="UserControl" TagName="WebHeader" Src="/UserControl/WebHeader.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WebRotator" Src="/UserControl/WebRotator.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WebFoot" Src="/UserControl/WebFoot.ascx" %> 
<%@ Register TagPrefix="UserControl" TagName="WebList" Src="/UserControl/WebTextList.ascx" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server"  id="Header">
    <title>无标题页</title>
    <link href="css/css.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="MainPage_ResourceManager" runat="server" />
    <UserControl:WebHeader id="webPageHeader" runat="server"  />
    <table width="1007" border="0"  align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
    <tr>
         <td width="4"></td>
         <td width="998">
            <table border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="768" valign="top">
                    <table width="100%"  border="0" cellspacing="0" cellpadding="0">
                    <tr><td><UserControl:WebRotator id="webPageRotator" runat="server" width="768" height="449" /></td></tr>
                    </table>
                </td>                
                <td width="230" valign="top">
                    <table width="100%"  border="0" cellspacing="0" cellpadding="0">
                    <tr height="159"><td colspan="3" align="center"><img src="/img/login.jpg" /></td></tr>
                    <tr><td colspan="3" height="2"></td></tr>
                    <tr height="33"><td  colspan="3"  background="/img/nav4.jpg" align="center"><a href="list1.aspx?id=8"  class="TextLinkStyle3">服务指南</a></td></tr>
                    <tr colspan="3" >
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink1" runat="server" /></td></tr>
                        <tr height="1"><td></td></tr>
                        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink2" runat="server" /></td></tr>
                        <tr height="1"><td></td></tr>
                        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink3" runat="server" /></td></tr>
                        <tr height="1"><td></td></tr>
                        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink4" runat="server" /></td></tr>
                        <tr height="1"><td></td></tr>
                        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink5" runat="server" /></td></tr>
                        </table>
                    </td>
                    </tr>                   
                   </table>
                </td>
             </tr>
            </table>
         <td width="5"></td>
      </tr>
      <tr><td colspan="3" height="3"></td></tr>
    </table>
    <table width="1007" border="0"  align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
    <tr>
        <td width="4"></td>
        <td width="383" valign="top" >
            <table width="383" height="252"  border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="1" background="/img/dot.jpg"></td>
                <td>
                    <table width="381" height="252"  border="0" cellspacing="0" cellpadding="0">
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    <tr height="50"  bgcolor="#f1f7f7">
                        <td valign="middle">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="280">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<ext:HyperLink ID="ChannelLink1" runat="server" /></td>
                                <td width="101" align="center"><asp:Image ID="ChannelImage1" runat="server" /></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr  height="200"><td><UserControl:WebList id="tabChannelList1" runat="server" Width="381" /></td></tr>
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    </table>
                </td>
                <td width="1" background="/img/dot.jpg"></td>
            </tr>
            </table>
        </td>
        <td width="2"></td>
        <td width="383" valign="top" >
            <table width="383" height="252"  border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="1" background="/img/dot.jpg"></td>
                <td>
                    <table width="381" height="252"  border="0" cellspacing="0" cellpadding="0">
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    <tr height="50"  bgcolor="#f1f7f7">
                        <td valign="middle">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="280">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<ext:HyperLink ID="ChannelLink2" runat="server" /></td>
                                <td width="101" align="center"><asp:Image ID="ChannelImage2" runat="server" /></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr  height="200"><td><UserControl:WebList id="tabChannelList2" runat="server" Width="381" /></td></tr>
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    </table>
                </td>
                <td width="1" background="/img/dot.jpg"></td>
            </tr>
            </table>
        </td>
        <td width="230" valign="top">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink6" runat="server" /></td></tr>
        <tr height="1"><td></td></tr>
        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink7" runat="server" /></td></tr>
        <tr height="1"><td></td></tr>
        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink8" runat="server" /></td></tr>
        <tr height="1"><td></td></tr>
        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink9" runat="server" /></td></tr>
        <tr height="1"><td></td></tr>
        <tr height="50"><td background="/img/nav4.jpg" align="center"><ext:HyperLink ID="labNavLink10" runat="server" /></td></tr>
        </table>
        </td>
        <td width="5"></td>        
   </tr>
   </table>
    <table width="1007" border="0"  align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
    <tr height="5"><td colspan="3"></td></tr>
    <tr>
         <td width="4"></td>
         <td><img src="/img/banner.jpg" /></td>
         <td width="5"></td>
    </tr>
    <tr height="5"><td colspan="3"></td></tr>
<%--    <tr>
         <td width="4"></td>
         <td><img src="img/banner2.jpg" width=996 height=123 border="0" usemap="#Map" /></td>
         <td width="5"></td>
    </tr>
    <tr height="5"><td colspan="3"></td></tr>--%>
    </table>           
    <table width="1007" border="0"  align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
    <tr>
         <td width="4"></td>
         <td width="497" valign="top" >
            <table width="497" height="252"  border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="1" background="/img/dot.jpg"></td>
                <td>
                    <table width="495" height="252"  border="0" cellspacing="0" cellpadding="0">
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    <tr height="50" bgcolor="#f1f7f7">
                        <td valign="middle">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="400">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<ext:HyperLink ID="ChannelLink3" runat="server" /></td>
                                <td width="95" align="center"><asp:Image ID="ChannelImage3" runat="server" /></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr  height="200"><td><UserControl:WebList id="tabChannelList3" runat="server" Width="495" /></td></tr>
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    </table>
                </td>
                <td width="1" background="/img/dot.jpg"></td>
            </tr>
            </table>
         </td>
         <td width="4"></td>    
         <td width="497" valign="top" >
            <table width="497" height="252"  border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="1" background="/img/dot.jpg"></td>
                <td>
                    <table width="495" height="252"  border="0" cellspacing="0" cellpadding="0">
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    <tr height="50"  bgcolor="#f1f7f7">
                        <td valign="middle">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="400">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<ext:HyperLink ID="ChannelLink4" runat="server" /></td>
                                <td width="95" align="center"><asp:Image ID="ChannelImage4" runat="server" /></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr  height="200"><td><UserControl:WebList id="tabChannelList4" runat="server" Width="495" /></td></tr>
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    </table>
                </td>
                <td width="1" background="/img/dot.jpg"></td>
            </tr>
            </table>
         </td>
         <td width="5"></td>
    </tr>         
    </table>  
    <table width="1007" border="0"  align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
    <tr height="5"><td colspan="3"></td></tr>
    <tr>
         <td width="4"></td>
         <td valign="top" width="998">
            <table width="998" height="152"  border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="1" background="/img/dot.jpg"></td>
                <td >
                    <table width="996" height="202"  border="0" cellspacing="0" cellpadding="0">
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    <tr height="50"  bgcolor="#f1f7f7">
                        <td valign="middle">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="900">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<ext:HyperLink ID="ChannelLink5" runat="server" /></td>
                                <td width="96" align="center"><asp:Image ID="ChannelImage5" runat="server" /></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="150">
                        <td>
                            <DIV id="demo" style="OVERFLOW: hidden; WIDTH: 972px; COLOR: #ffffff; HEIGHT: 150px">
                                <TABLE cellSpacing="0" cellPadding="0" width="996" align="center" border="0">
                                <TR>
                                   <TD id="demo1" vAlign="bottom">
                                   <TABLE height="80" cellSpacing="0" cellPadding="0" width="100%" border="0">
                                       <TR><TD><asp:Table ID="tabPic" runat="server" align="center" border="0" cellPadding="0" cellSpacing="0" width="100%" /></TD></TR>
                                   </TABLE>
                                   </TD>
                                   <TD id="demo2" vAlign="top">&nbsp;</TD>
                                </TR>
                                </TABLE>
                            </DIV>
                        </td>
                    </tr>
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    </table>
                </td>
                <td width="1" background="/img/dot.jpg"></td>
            </tr>
            </table>
         </td>
         <td width="5"></td>
    </tr>
    <tr height="5"><td colspan="3"></td></tr>
    </table>
    <table width="1007" border="0"  align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
    <tr height="3"><td colspan="3"></td></tr>
    <tr>
         <td width="4"></td>
         <td valign="top" width="998">
            <table width="998" height="50"  border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="1" background="/img/dot.jpg"></td>
                <td>
                    <table width="996" height="50"  border="0" cellspacing="0" cellpadding="0">
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    <tr height="48"  bgcolor="#ffffff">
                        <td valign="middle">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" height="48">
                            <tr>
                                <td  bgcolor="#f1f7f7" width="100"  align="center" class="TextLinkStyle2">服务网站链接</td>
                                <td width="886" align="center">
                                    <a href="#" class="TextLinkStyle3">安徽省科技创新平台某网站</a>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <a href="#" class="TextLinkStyle3">安徽省某公共服务平台</a>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <a href="#" class="TextLinkStyle3">某某县科技基础查询平台</a>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <a href="#" class="TextLinkStyle3">某某市招聘平台</a>
                                </td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="1"><td background="/img/dot.jpg"></td></tr>
                    </table>
                </td>
                <td width="1" background="/img/dot.jpg"></td>
            </tr>
            </table>
         </td>
         <td width="5"></td>
    </tr>
    <tr height="5"><td colspan="3"></td></tr>
    </table>


    <UserControl:WebFoot id="webPageFoot" runat="server" />
    
    
<SCRIPT language="javascript">
var speed=30;
demo2.innerHTML=demo1.innerHTML;
demo.scrollLeft=demo.scrollWidth;

function Marquee()
{
    if (demo.scrollLeft<=0)
        demo.scrollLeft+=demo2.offsetWidth;
    else
        demo.scrollLeft--;
}

var MyMar=setInterval(Marquee,speed)
demo.onmouseover=function() {clearInterval(MyMar)}
demo.onmouseout=function() {MyMar=setInterval(Marquee,speed)}
</SCRIPT>    
    
    </form>

<map name="Map" id="Map">
  <area shape="rect" coords="3,68,242,116" href="http://220.178.27.148/front/train_index.action" />
  <area shape="rect" coords="253,67,493,116" href="http://220.178.27.148/front/tech_index.action" />
  <area shape="rect" coords="502,66,744,118" href="http://220.178.27.148/front/financ_index.action" />
  <area shape="rect" coords="753,66,992,118" href="http://www.smeah.gov.cn/helpe/info/" />
</map>
</body>
</html>
