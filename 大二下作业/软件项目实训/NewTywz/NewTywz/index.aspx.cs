using NewTywz.baseclass;
using NewTywz.basepage;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace NewTywz
{
    public partial class index : WebPage
    {
        private void InitChannel(string sName, Ext.Net.HyperLink ChannelLink, System.Web.UI.WebControls.Image ChannelImage, UserControl.WebTextList tab, int nListNum, int MaxLength)
        {
            clsTreeManager clsTree = new clsTreeManager();
            clsTree.sTableName = "tblChannelType";
            string sSQL = "select * from tblChannelType where Name='" + sName + "' and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                ChannelLink.Text = sName;
                ChannelLink.Cls = "TextLinkStyle3";
                ChannelImage.ImageUrl = "/img/more.jpg";
                ChannelImage.CssClass = "sl-image";
                ChannelImage.Attributes.Add("onClick", "window.location.href='" + dt.Rows[0]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[0]["ID"].ToString() + "';");
                if (dt.Rows.Count > 0)
                {
                    ChannelImage.Attributes.Add("onClick", "window.location.href='" + dt.Rows[0]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[0]["ID"].ToString() + "';");
                    ChannelLink.NavigateUrl = dt.Rows[0]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[0]["ID"].ToString();
                    tab.nListNum = nListNum;
                    tab.nNameMaxLength = MaxLength;
                    tab.bNeedPageToolbar = false;
                    tab.sChannelTypeID = dt.Rows[0]["ID"].ToString();
                    tab.nNowPageNum = 1;
                    tab.InitWebList();
                }
            }

        }
        private string GetImageUrl(string sText, string sUrl)
        {
            if (sText.IndexOf("<img") != -1)
            {
                sText = sText.Substring(sText.IndexOf("<img"));
                sText = sText.Substring(sText.IndexOf("src=") + 4, sText.IndexOf(".jpg") - sText.IndexOf("src="));
                return sText;
            }
            else
                return sUrl;
        }
        private void InitPicChannel(string sName, Ext.Net.HyperLink ChannelLink, System.Web.UI.WebControls.Image ChannelImage)
        {
            clsTreeManager clsTree = new clsTreeManager();
            clsTree.sTableName = "tblChannelType";
            string sSQL = "select * from tblChannelType where Name='" + sName + "' and TheLevel=2 and DelFlag=0";
            //string sSQL = "select * from tblChannelType where Name='" + sName + "' and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            ChannelLink.Text = sName;
            ChannelLink.Cls = "TextLinkStyle3";
            ChannelLink.NavigateUrl = dt.Rows[0]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[0]["ID"].ToString();
            ChannelImage.ImageUrl = "/img/more.jpg";
            ChannelImage.CssClass = "sl-image";
            ChannelImage.Attributes.Add("onClick", "window.location.href='" + dt.Rows[0]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[0]["ID"].ToString() + "';");

            sSQL = "select * from tblChannelContent where ChannelCChannelTID=" + dt.Rows[0]["ID"] + " and DelFlag=0 order by ChannelCTop DESC,ChannelCTime DESC,ID DESC";
            dt = cprivDBO.ExcuteSqlTable(sSQL);
            int i = 0, j = 0;
            TableRow tRow = new TableRow();
            Table hTab;
            TableRow tRow1;
            TableCell tCell, tCell1;
            System.Web.UI.WebControls.HyperLink hLink;
            System.Web.UI.WebControls.Image img;
            while ((i < 10) && (j < dt.Rows.Count))
            {
                if (cprivDBO.GetCount(" RelationID=" + dt.Rows[j]["ID"].ToString(), "tblChannelContentPic") > 0)
                {
                    tCell = new TableCell();
                    hTab = new Table();

                    tRow1 = new TableRow();
                    tCell1 = new TableCell();
                    string sSQL1 = "select * from tblChannelContentPic where RelationID=" + dt.Rows[j]["ID"].ToString() + " order by TheOrder";
                    DataTable dt1 = cprivDBO.ExcuteSqlTable(sSQL1);
                    img = new System.Web.UI.WebControls.Image();
                    img.ImageUrl = GetImageUrl(dt.Rows[j]["ChannelCText"].ToString(), "/attached/image/" + dt1.Rows[0]["Address"].ToString());
                    img.Width = new Unit(190);
                    img.Height = new Unit(133);
                    img.ToolTip = dt.Rows[j]["ChannelCName"].ToString();
                    hLink = new System.Web.UI.WebControls.HyperLink();
                    hLink.Controls.Add(img);
                    hLink.NavigateUrl = "/" + dt.Rows[j]["ChannelCTemplete"].ToString() + "?ContentId=" + dt.Rows[j]["ID"].ToString();
                    tCell1.Controls.Add(hLink);
                    tRow1.Cells.Add(tCell1);
                    hTab.Rows.Add(tRow1);

                    tRow1 = new TableRow();
                    tCell1 = new TableCell();
                    hLink = new System.Web.UI.WebControls.HyperLink();
                    hLink.Text = dt.Rows[j]["ChannelCName"].ToString();
                    if (hLink.Text.Length > 6)
                        hLink.Text = hLink.Text.Remove(6) + "...";
                    hLink.NavigateUrl = "/" + dt.Rows[j]["ChannelCTemplete"].ToString() + "?ContentId=" + dt.Rows[j]["ID"].ToString();
                    hLink.CssClass = "TextLinkStyle";
                    tCell1.Controls.Add(hLink);
                    tCell1.Attributes.Add("align", "center");
                    tRow1.Cells.Add(tCell1);
                    hTab.Rows.Add(tRow1);

                    tCell.Controls.Add(hTab);
                    tRow.Cells.Add(tCell);
                    i++;
                    tabPic.Rows.Add(tRow);
                }
                j++;
            }

        }

        private void InitNav(Ext.Net.Label labNav, System.Web.UI.WebControls.Image labNavImage, string sName)
        {
            labNav.Text = sName;
            labNav.Cls = "NavStyle";

            labNavImage.ImageUrl = "/img/nav3.jpg";
            labNavImage.CssClass = "sl-image";

            string sSQL = "select * from tblChannelType where Name='" + sName + "' and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                labNavImage.Attributes.Add("onClick", "window.location.href='" + dt.Rows[0]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[0]["ID"].ToString() + "';");
        }

        private void InitNav2(Ext.Net.HyperLink labNavLink, string sName)
        {
            labNavLink.Text = sName;
            labNavLink.NavigateUrl = "#";
            labNavLink.Cls = "TextLinkStyle3";
            string sSQL = "select * from tblChannelType where Name='" + sName + "' and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                labNavLink.NavigateUrl = dt.Rows[0]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[0]["ID"].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                webPageRotator.sXMLFileName = "bcastr.xml";
                webPageHeader.InitWebHeader();
                Header.Title = webPageHeader.sHeader;
                //InitNav(labNav1, labNavImage1, "政策引导服务");
                //InitNav(labNav2, labNavImage2, "创业辅导服务");
                //InitNav(labNav3, labNavImage3, "管理咨询服务");
                //InitNav(labNav4, labNavImage4, "知识产权服务");
                //InitNav(labNav5, labNavImage5, "项目编报服务");
                InitNav2(labNavLink1, "政策引导服务");
                InitNav2(labNavLink2, "创业辅导服务");
                InitNav2(labNavLink3, "管理咨询服务");
                InitNav2(labNavLink4, "知识产权服务");
                InitNav2(labNavLink5, "项目编报服务");
                InitNav2(labNavLink6, "信用评价服务");
                InitNav2(labNavLink7, "投融资服务");
                InitNav2(labNavLink8, "人力资源服务");
                InitNav2(labNavLink9, "电子商务服务");
                InitNav2(labNavLink10, "关于我们");
                InitChannel("公告通知", ChannelLink1, ChannelImage1, tabChannelList1, 6, 15);
                InitChannel("新闻动态", ChannelLink2, ChannelImage2, tabChannelList2, 6, 15);
                InitChannel("科技成果", ChannelLink3, ChannelImage3, tabChannelList3, 6, 25);
                InitChannel("服务案例", ChannelLink4, ChannelImage4, tabChannelList4, 6, 25);
                InitPicChannel("企业展示", ChannelLink5, ChannelImage5);
            }
        }
    }
}
