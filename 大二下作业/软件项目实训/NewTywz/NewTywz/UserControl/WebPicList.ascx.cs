using NewTywz.baseclass;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace NewTywz.UserControl
{
    /// <summary>
    /// list中显示一条一条的信息
    /// </summary>
    public partial class WebPicList : System.Web.UI.UserControl
    {
        private clsMenuManager objMenu = new clsMenuManager();
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        /// <summary>
        /// 每页显示多少行
        /// </summary>
        public int nMaxRowNum
        {
            get
            {
                return Convert.ToInt16(WebPicList_MaxRowNum.Text);
            }
            set
            {
                WebPicList_MaxRowNum.Text = value.ToString();
            }
        }
        /// <summary>
        /// 每页显示多少列
        /// </summary>
        public int nMaxColNum
        {
            get
            {
                return Convert.ToInt16(WebPicList_MaxColNum.Text);
            }
            set
            {
                WebPicList_MaxColNum.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前是多少页
        /// </summary>
        public int nNowPageNum
        {
            get
            {
                return Convert.ToInt16(WebPicList_NowPageNum.Text);
            }
            set
            {
                WebPicList_NowPageNum.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前对应的ChannelTypeID
        /// </summary>
        public string sChannelTypeID
        {
            get
            {
                return WebPicList_ChannelTypeID.Text;
            }
            set
            {
                WebPicList_ChannelTypeID.Text = value;
            }
        }
        /// <summary>
        /// 当前是否需要翻页
        /// </summary>
        public bool bNeedPageToolbar
        {
            get
            {
                if (WebPicList_NeedPageToolbar.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    WebPicList_NeedPageToolbar.Text = "1";
                else
                    WebPicList_NeedPageToolbar.Text = "0";
            }
        }
        #endregion              

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

        private Table GetOneTab(string sContentId, string sContentName, string sContentText, string sLink)
        {
            Table tab = new Table();
            string sSQL = "select * from tblChannelContentPic where RelationID=" + sContentId + " order by TheOrder";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                TableRow tRow = new TableRow();
                TableCell tCell = new TableCell();
                tCell.Attributes.Add("align", "center");
                tCell.Width = new Unit(160);
                HyperLink hLink = new HyperLink();
                hLink.NavigateUrl = sLink;
                Image img = new Image();
                img.ImageUrl = GetImageUrl(sContentText, "/attached/image/" + dt.Rows[0]["Address"].ToString());
                img.Width = new Unit(140);
                img.Height = new Unit(100);
                hLink.Controls.Add(img);
                tCell.Controls.Add(hLink);
                tRow.Cells.Add(tCell);
                tab.Rows.Add(tRow);

                tRow = new TableRow();
                tCell = new TableCell();
                tCell.Attributes.Add("align", "center");
                tCell.Text = sContentName;
                tRow.Cells.Add(tCell);
                tab.Rows.Add(tRow);
            }
            return tab;
        }


        public void InitWebList()
        {
            string sSQL = "select DISTINCT tblChannelContent.ID,tblChannelContent.ChannelCName,tblChannelContent.ChannelCText,tblChannelContent.ChannelCTemplete,tblChannelContent.ChannelCTime,tblChannelContent.ChannelCTop from tblChannelContentPic INNER JOIN tblChannelContent on tblChannelContentPic.RelationID = tblChannelContent.ID where tblChannelContent.ChannelCChannelTID=" + sChannelTypeID + " and tblChannelContent.DelFlag=0 ";
            if (Session["ID"] == null)
                sSQL = sSQL + " and tblChannelContent.ChannelCAuthor=0";
            sSQL = sSQL + " order by tblChannelContent.ChannelCTop DESC,tblChannelContent.ChannelCTime DESC";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            int nAllNum = dt.Rows.Count;
            int i = (nNowPageNum - 1) * nMaxColNum * nMaxRowNum;

            TableRow tRow = new TableRow();
            TableCell tCell;
            int j = 0;//col计数
            while (i < nNowPageNum * nMaxColNum * nMaxRowNum)
            {
                //第一行row，虚线
                if (j == 0)
                {
                    tRow = new TableRow();
                    tRow.Height = new Unit(1);
                    tCell = new TableCell();
                    tCell.ColumnSpan = nMaxColNum;
                    tCell.Attributes.Add("background", "/img/dot2.jpg");
                    tRow.Cells.Add(tCell);
                    WebPicList_tab.Rows.Add(tRow);
                    //第二行row，内容
                    tRow = new TableRow();
                    tRow.Height = new Unit(120);
                    tRow.Attributes.Add("valign", "center");
                }

                tCell = new TableCell();
                tCell.Width = new Unit(160);
                if (i < dt.Rows.Count)
                    tCell.Controls.Add(GetOneTab(dt.Rows[i]["ID"].ToString(), dt.Rows[i]["ChannelCName"].ToString(), dt.Rows[i]["ChannelCText"].ToString(), "/" + dt.Rows[i]["ChannelCTemplete"].ToString() + "?id=" + sChannelTypeID + "&ContentId=" + dt.Rows[i]["ID"].ToString()));
                tRow.Cells.Add(tCell);
                if (j == (nMaxColNum - 1))
                {
                    WebPicList_tab.Rows.Add(tRow);
                    j = 0;
                }
                else
                    j++;
                i++;
            }
            //最后一行row，虚线
            tRow = new TableRow();
            tRow.Height = new Unit(1);
            tCell = new TableCell();
            tCell.ColumnSpan = nMaxColNum;
            tCell.Attributes.Add("background", "/img/dot2.jpg");
            tRow.Cells.Add(tCell);
            WebPicList_tab.Rows.Add(tRow);

            if (bNeedPageToolbar)
            {
                WebPicList_Page.nAllNum = nAllNum;
                WebPicList_Page.nPageNow = nNowPageNum;
                WebPicList_Page.MAXBODYNUM = nMaxColNum * nMaxRowNum;
                WebPicList_Page.sChannelID = sChannelTypeID;
                WebPicList_Page.visiable = true;
                WebPicList_Page.InitPageToolBar();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}