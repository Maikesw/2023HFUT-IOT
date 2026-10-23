using NewTywz.baseclass;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace NewTywz.UserControl
{
    /// <summary>
    /// list中显示一条一条的信息
    /// </summary>
    public partial class WebTextList : System.Web.UI.UserControl
    {
        private clsMenuManager objMenu = new clsMenuManager();
        private DbObject cprivDBO = new DbObject();
        private clsTreeManager clsTree = new clsTreeManager();

        #region 设定传递的一些value
        public int Width
        {
            get
            {
                return Convert.ToInt16(WebTextList_tab.Width.Value);
            }
            set
            {
                WebTextList_tab.Width = new Unit(value);
            }
        }


        /// <summary>
        /// 每页显示多少条
        /// </summary>
        public int nListNum
        {
            get
            {
                return Convert.ToInt16(WebTextList_ListNum.Text);
            }
            set
            {
                WebTextList_ListNum.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前是多少页
        /// </summary>
        public int nNowPageNum
        {
            get
            {
                return Convert.ToInt16(WebTextList_NowPageNum.Text);
            }
            set
            {
                WebTextList_NowPageNum.Text = value.ToString();
            }
        }
        /// <summary>
        /// 名称字符最大长度
        /// </summary>
        public int nNameMaxLength
        {
            get
            {
                return Convert.ToInt16(WebTextList_NameMaxLength.Text);
            }
            set
            {
                WebTextList_NameMaxLength.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前对应的ChannelTypeID
        /// </summary>
        public string sChannelTypeID
        {
            get
            {
                return WebTextList_ChannelTypeID.Text;
            }
            set
            {
                WebTextList_ChannelTypeID.Text = value;
            }
        }
        /// <summary>
        /// 当前是否需要翻页
        /// </summary>
        public bool bNeedPageToolbar
        {
            get
            {
                if (WebTextList_NeedPageToolbar.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    WebTextList_NeedPageToolbar.Text = "1";
                else
                    WebTextList_NeedPageToolbar.Text = "0";
            }
        }
        #endregion              

        private string GetSQL(int nAllNum)
        {
            string sSQL = "";
            if (nAllNum > 0)
            {
                string sWhere = GetWhere();
                if (nAllNum >= nNowPageNum * nListNum)
                {
                    sSQL = "select top " + (nNowPageNum * nListNum) + " * " + "from tblChannelContent where " + sWhere;
                    sSQL = "select top " + nListNum + " * from (" + sSQL + " order by ChannelCTop DESC,ChannelCTime DESC,ID DESC) A order by ChannelCTop ASC,ChannelCTime ASC,ID ASC";
                    sSQL = "select  * from (" + sSQL + ") B order by ChannelCTop DESC,ChannelCTime DESC,ID DESC";
                }
                else
                {
                    sSQL = "select top " + nAllNum + " * " + "from tblChannelContent where " + sWhere;
                    sSQL = "select top " + (nAllNum - (nNowPageNum - 1) * nListNum) + " * from (" + sSQL + " order by ChannelCTop DESC,ChannelCTime DESC,ID DESC) A order by ChannelCTop ASC,ChannelCTime ASC,ID ASC";
                    sSQL = "select  * from (" + sSQL + ") B order by ChannelCTop DESC,ChannelCTime DESC,ID DESC";
                }
            }
            return sSQL;
        }

        private string GetWhere()
        {
            clsTree.sTableName = "tblChannelType";
            string sAllID = clsTree.GetAllLeaf(sChannelTypeID);
            string sReturn = " ChannelCChannelTID IN (" + sAllID + ") ";
            if (Session["ID"] == null)
                sReturn = sReturn + " and ChannelCAuthor=0 ";
            sReturn = sReturn + " and DelFlag=0";
            return sReturn;
        }

        public void InitWebList()
        {
            int nAllNum = cprivDBO.GetCount(GetWhere(), "tblChannelContent");
            int i = 0;
            DataTable dt = cprivDBO.ExcuteSqlTable(GetSQL(nAllNum));
            TableRow tRow;
            TableCell tCell;
            while (i < nListNum)
            {
                //第二行row，内容
                tRow = new TableRow();
                tRow.Height = new Unit(30);
                tRow.Attributes.Add("valign", "center");
                if (i < dt.Rows.Count)
                {
                    //第一个cell，cycle
                    tCell = new TableCell();
                    tCell.Width = new Unit(25);
                    Image img = new Image();
                    img.ImageUrl = "/img/cycle.jpg";
                    img.Width = new Unit(15);
                    img.Height = new Unit(15);
                    tCell.Controls.Add(img);
                    tRow.Cells.Add(tCell);
                    //第2个cell，频道名称和链接
                    int nFirstCellWidth = 20;
                    int nNameMaxLength = 0;
                    tCell = new TableCell();
                    HyperLink hLink1 = new HyperLink();
                    DataTable dt1 = cprivDBO.GetValue(dt.Rows[i]["ChannelCChannelTID"].ToString(), "tblChannelType");
                    if (dt1.Rows.Count > 0)
                    {
                        hLink1 = new HyperLink();
                        hLink1.Text = "[" + dt1.Rows[0]["Name"].ToString() + "]";
                        nFirstCellWidth = nFirstCellWidth * dt1.Rows[0]["Name"].ToString().Length;
                        nNameMaxLength = Convert.ToInt16(Math.Ceiling((Width - 120) / 13.8)) - dt1.Rows[0]["Name"].ToString().Length - 1;
                        string sLink = "/" + dt1.Rows[0]["ChannelTTemplete"].ToString();
                        if (sLink[sLink.Length - 1] == '/')
                            sLink = sLink.Remove(sLink.Length - 1, 1);
                        else
                            sLink = sLink + "?id=" + dt1.Rows[0]["ID"].ToString();
                        hLink1.NavigateUrl = sLink;
                        hLink1.CssClass = "TableChannelName";
                        tCell.Controls.Add(hLink1);
                    }
                    tCell.Width = new Unit(nFirstCellWidth);
                    tCell.CssClass = "line2";
                    tRow.Cells.Add(tCell);

                    //第二个cell，标题
                    tCell = new TableCell();
                    tCell.Width = new Unit(Width - 120);
                    HyperLink hLink = new HyperLink();
                    string sText = dt.Rows[i]["ChannelCName"].ToString();
                    hLink.ToolTip = sText;
                    if (sText.Length > nNameMaxLength)
                        sText = sText.Remove(nNameMaxLength) + "...";
                    hLink.Text = sText;
                    hLink.NavigateUrl = "/" + dt.Rows[i]["ChannelCTemplete"].ToString() + "?id=" + sChannelTypeID + "&ContentId=" + dt.Rows[i]["ID"].ToString();
                    hLink.CssClass = "TextLinkStyle";
                    tCell.Attributes.Add("align", "left");
                    tCell.Controls.Add(hLink);
                    //是否置顶
                    if (Convert.ToBoolean(dt.Rows[i]["ChannelCTop"]))
                    {
                        img = new Image();
                        img.ImageUrl = "/img/ding.gif";
                        tCell.Controls.Add(img);
                    }
                    //是否有图片
                    if (cprivDBO.GetCount("RelationID=" + dt.Rows[i]["ID"].ToString(), "tblChannelContentPic") > 0)
                    {
                        img = new Image();
                        img.ImageUrl = "/img/picicon.jpg";
                        tCell.Controls.Add(img);
                    }
                    tRow.Cells.Add(tCell);
                    //第三个cell，日期
                    tCell = new TableCell();
                    tCell.Width = new Unit(100);
                    Label lab = new Label();
                    lab.Text = Convert.ToDateTime(dt.Rows[i]["ChannelCTime"]).ToString("yyyy-MM-dd") + "&nbsp;&nbsp;&nbsp;&nbsp;";
                    tCell.Attributes.Add("align", "right");
                    tCell.Controls.Add(lab);
                    tRow.Cells.Add(tCell);
                }
                else
                {
                    tCell = new TableCell();
                    tCell.ColumnSpan = 3;
                    tRow.Cells.Add(tCell);
                }
                WebTextList_tab.Rows.Add(tRow);

                //最后一行row，虚线
                tRow = new TableRow();
                tRow.Height = new Unit(1);
                tCell = new TableCell();
                tRow.Cells.Add(tCell);
                tCell = new TableCell();
                tCell.ColumnSpan = 2;
                tCell.Attributes.Add("background", "/img/dot3.jpg");
                tRow.Cells.Add(tCell);
                WebTextList_tab.Rows.Add(tRow);

                i++;
            }

            if (bNeedPageToolbar)
            {
                WebTextList_Page.nAllNum = nAllNum;
                WebTextList_Page.nPageNow = nNowPageNum;
                WebTextList_Page.MAXBODYNUM = nListNum;
                WebTextList_Page.sChannelID = sChannelTypeID;
                WebTextList_Page.visiable = true;
                WebTextList_Page.InitPageToolBar();
            }
        }




        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}