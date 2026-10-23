using NewTywz.baseclass;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace NewTywz.UserControl
{
    /// <summary>
    /// list中显示一条一条的信息
    /// </summary>
    public partial class WebPageToolBar : System.Web.UI.UserControl
    {
        private clsMenuManager objMenu = new clsMenuManager();
        private DbObject cprivDBO = new DbObject();

        #region 定义PageToolBar中的DropDownList控件


        private string GetChannelID()
        {
            return Request["id"].ToString();
        }

        public void WebPageToolBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = "/" + sPageName + "?id=" + GetChannelID() + "&PageID=" + WebPageToolBar_ddlist.SelectedItem.Value.ToString();
            Response.Redirect(s);
        }
        #endregion

        #region 设定传递的一些value

        /// <summary>
        /// 总共有多少条
        /// </summary>
        public int nAllNum
        {
            get
            {
                return Convert.ToInt16(WebPageToolBar_AllNum.Text);
            }
            set
            {
                WebPageToolBar_AllNum.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前是多少页
        /// </summary>
        public int nPageNow
        {
            get
            {
                return Convert.ToInt16(WebPageToolBar_NowPageNum.Text);
            }
            set
            {
                WebPageToolBar_NowPageNum.Text = value.ToString();
            }
        }
        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public int MAXBODYNUM
        {
            get
            {
                return Convert.ToInt16(WebPageToolBar_MaxBodyNum.Text);
            }
            set
            {
                WebPageToolBar_MaxBodyNum.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前对应的ChannelTypeID
        /// </summary>
        public string sChannelID
        {
            get
            {
                return WebPageToolBar_ChannelID.Text;
            }
            set
            {
                WebPageToolBar_ChannelID.Text = value;
            }
        }

        public bool visiable
        {
            get
            {
                return WebPageToolBar_MainTab.Visible;
            }
            set
            {
                WebPageToolBar_MainTab.Visible = value;
            }
        }

        public string sPageName
        {
            get
            {
                return WebPageToolBar_PageName.Text;
            }
            set
            {
                WebPageToolBar_PageName.Text = value;
            }
        }
        #endregion

        public void InitPageToolBar()
        {
            TableRow tRow = new TableRow();
            tRow.Height = new Unit(25);
            TableCell tCell = new TableCell();
            tCell.Attributes.Add("align", "right");
            tCell.Attributes.Add("valign", "bottom");

            string sSQL = "select * from tblChannelType where ID=" + sChannelID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);

            sPageName = dt.Rows[0]["ChannelTTemplete"].ToString();
            //首页
            System.Web.UI.WebControls.HyperLink hLink = new System.Web.UI.WebControls.HyperLink();
            hLink.Text = "首页";
            hLink.CssClass = "TextLinkStyle";
            hLink.NavigateUrl = "/" + sPageName + "?id=" + sChannelID + "&PageID=1";
            tCell.Controls.Add(hLink);
            System.Web.UI.WebControls.Label lab = new System.Web.UI.WebControls.Label();
            lab.Text = "&nbsp;&nbsp;";
            tCell.Controls.Add(lab);

            //上一页
            hLink = new System.Web.UI.WebControls.HyperLink();
            hLink.Text = "上一页";
            if (nPageNow > 1)
            {
                hLink.CssClass = "TextLinkStyle";
                hLink.NavigateUrl = "/" + sPageName + "?id=" + sChannelID + "&PageID=" + Convert.ToString(nPageNow - 1);
            }
            tCell.Controls.Add(hLink);
            lab = new System.Web.UI.WebControls.Label();
            lab.Text = "&nbsp;&nbsp;";
            tCell.Controls.Add(lab);
            //下一页
            hLink = new System.Web.UI.WebControls.HyperLink();
            hLink.Text = "下一页";
            if (nPageNow * MAXBODYNUM < nAllNum)
            {
                hLink.CssClass = "TextLinkStyle";
                hLink.NavigateUrl = "/" + sPageName + "?id=" + sChannelID + "&PageID=" + Convert.ToString(nPageNow + 1);
            }
            tCell.Controls.Add(hLink);
            lab = new System.Web.UI.WebControls.Label();
            lab.Text = "&nbsp;&nbsp;";
            tCell.Controls.Add(lab);
            //尾页
            hLink = new System.Web.UI.WebControls.HyperLink();
            hLink.CssClass = "TextLinkStyle";
            hLink.Text = "尾页";
            int nEndPage;
            if (nAllNum % MAXBODYNUM != 0)
                nEndPage = nAllNum / MAXBODYNUM + 1;
            else nEndPage = nAllNum / MAXBODYNUM;
            hLink.NavigateUrl = "/" + sPageName + "?id=" + sChannelID + "&PageID=" + nEndPage.ToString();
            tCell.Controls.Add(hLink);
            lab = new System.Web.UI.WebControls.Label();
            lab.Text = "&nbsp;&nbsp;";
            tCell.Controls.Add(lab);
            //当前页
            System.Web.UI.WebControls.Label lab1 = new System.Web.UI.WebControls.Label();
            lab1.Text = "当前为";
            tCell.Controls.Add(lab1);
            System.Web.UI.WebControls.Label labPageNow = new System.Web.UI.WebControls.Label();
            labPageNow.Text = nPageNow.ToString();
            tCell.Controls.Add(labPageNow);
            System.Web.UI.WebControls.Label lab2 = new System.Web.UI.WebControls.Label();
            lab2.Text = "/" + nEndPage + "页";
            tCell.Controls.Add(lab2);
            tRow.Cells.Add(tCell);
            WebPageToolBar_tab.Rows.Add(tRow);

            //跳转页
            WebPageToolBar_ddlist.Items.Clear();
            for (int i = 1; i <= nEndPage; i++)
                WebPageToolBar_ddlist.Items.Add(i.ToString());
            WebPageToolBar_ddlist.SelectedIndex = nPageNow - 1;
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}