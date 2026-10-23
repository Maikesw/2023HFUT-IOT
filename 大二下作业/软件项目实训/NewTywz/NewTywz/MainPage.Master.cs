using NewTywz.baseclass;
using NewTywz.basepage;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace NewTywz
{
    public partial class MainPage : System.Web.UI.MasterPage
    {
        private DbObject cprivDBO = new DbObject();
        private clsMenuManager clsMenu = new clsMenuManager();
        private string sChannelContentID = "";
        private string sChannelTypeRealID = "";
        private string sChannelTypeID = "";


        public void InitChannelID()
        {
            webPageHeader.InitWebHeader();
            Header.Title = webPageHeader.sHeader;
            if (Request["ContentId"] != null)
            {
                sChannelContentID = Request["ContentId"].ToString();
                sChannelTypeRealID = cprivDBO.GetValue(sChannelContentID, "ChannelCChannelTID", "tblChannelContent").ToString();
                InitHeadNav();
            }
            else if (Request["id"] != null)
            {
                sChannelTypeRealID = Request["id"].ToString();
                sChannelTypeID = sChannelTypeRealID;
                InitHeadNav();
            }
            else
                Response.Redirect("index.aspx");
        }

        private void InitHeadNav()
        {
            string sSQL = "select * from tblChannelType where ID=" + sChannelTypeRealID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["TheLevel"].ToString() == "2")
                    sChannelTypeID = dt.Rows[0]["ParentIndex"].ToString();
                else
                    sChannelTypeID = sChannelTypeRealID;
            }
            labNav.Text = cprivDBO.GetValue(sChannelTypeID, "Name", "tblChannelType").ToString();
            labNav.Cls = "TextLinkStyle3";
        }

        private void InitMoreNav()
        {
            string sSQL = "select * from tblChannelType where ParentIndex=" + sChannelTypeID + " and DelFlag=0 order by TheOrder ";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TableRow tRow = new TableRow();
                TableCell tCell = new TableCell();
                HyperLink hLink = new HyperLink();
                hLink.Text = dt.Rows[i]["Name"].ToString();
                hLink.NavigateUrl = dt.Rows[i]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[i]["ID"].ToString();
                if (dt.Rows[i]["ID"].ToString() == sChannelTypeRealID)
                    hLink.CssClass = "TextLinkStyle4";
                else
                    hLink.CssClass = "TextLinkStyle3";
                tCell.Attributes.Add("align", "center");
                tCell.Attributes.Add("valign", "middle");
                tCell.Attributes.Add("height", "30");
                tCell.Controls.Add(hLink);
                tRow.Cells.Add(tCell);
                tabMoreNav.Rows.Add(tRow);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitChannelID();
                InitMoreNav();
                WebPage currentPage = Page as WebPage;
                if (currentPage != null)
                    currentPage.InitMainList(sChannelTypeRealID);
            }
        }
    }
}
