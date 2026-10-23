using NewTywz.baseclass;
using System;
using System.Data;
using System.Web.UI.WebControls;



namespace NewTywz.UserControl
{
    /// <summary>
    /// list上左边的导航条
    /// </summary>
    public partial class WebListNavigator : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();
        private clsMenuManager clsMenu = new clsMenuManager();
        private clsTreeManager clsTree = new clsTreeManager();
        private string sNowChannelName = "";

        #region 设定传递的一些value
        /// <summary>
        /// 如果当前ID的Level为3，保存其父节点的ChannelID，否则保存传过来的当前的ChannelID
        /// </summary>
        public string sChannelID
        {
            get
            {
                return WebListNavigator_sChannelID.Text;
            }
            set
            {
                WebListNavigator_sChannelID.Text = value;
            }
        }
        /// <summary>
        /// 如果当前ID的Level为3，保存传过来的当前的ChannelID，否则保存当前节点的第一个子节点的ID
        /// </summary>
        public string sChannelDisID
        {
            get
            {
                return WebListNavigator_sChannelDisID.Text;
            }
            set
            {
                WebListNavigator_sChannelDisID.Text = value;
            }
        }

        /// <summary>
        /// 写在方框框中的标题
        /// </summary>
        public string sHeadName
        {
            get
            {
                return WebListNavigator_sHeadName.Text;
            }
            set
            {
                WebListNavigator_sHeadName.Text = value;
            }
        }

        #endregion

        private void InitFirstName(string sName)
        {
            sNowChannelName = sName;
            sHeadName = sName;
            WebListNavigator_tabFirstName.Rows.Add(InitNameRow(sName));
        }


        private TableRow InitNameRow(string sName)
        {
            TableRow tRow = new TableRow();
            tRow.Height = new Unit(36);
            for (int i = 0; i < sName.Length; i++)
            {
                TableCell tCell = new TableCell();
                tCell.Width = new Unit(36);
                tCell.Attributes.Add("background", "img/FontBack.jpg");
                tCell.Attributes.Add("align", "center");
                tCell.Text = sName[i].ToString();
                tCell.CssClass = "MainChannelFont";
                tRow.Cells.Add(tCell);
                tCell = new TableCell();
                tCell.Width = new Unit(22);
                tRow.Cells.Add(tCell);
            }
            return tRow;
        }

        /// <summary>
        /// 初始化导航主体
        /// </summary>
        /// <param name="sWholeTypeID">2级菜单ID</param>
        /// <param name="sMoreTypeID">3级菜单ID</param>
        private void InitNavMore(string sWholeTypeID, string sMoreTypeID)
        {
            string sSQL = "select * from tblChannelType where ID=" + sWholeTypeID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                InitFirstName(dt.Rows[0]["Name"].ToString());
                sSQL = "select * from tblChannelType where ParentIndex=" + sWholeTypeID + " and DelFlag=0";
                if (Session["ID"] == null)
                    sSQL = sSQL + " and ChannelTAuthor=0 ";
                sSQL = sSQL + " order by TheOrder";
                dt = cprivDBO.ExcuteSqlTable(sSQL);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TableRow tRow = new TableRow();
                    tRow.Height = new Unit(39);
                    //第一个cell选中标志
                    TableCell tCell = new TableCell();
                    Image img;
                    if ((dt.Rows[i]["ID"].ToString() == sMoreTypeID) || ((sMoreTypeID == sWholeTypeID) && (i == 0)))
                    {
                        img = new Image();
                        img.ImageUrl = "/img/select1.jpg";
                        tCell.Controls.Add(img);
                    }
                    else
                    {
                        img = new Image();
                        img.ImageUrl = "/img/select2.jpg";
                        tCell.Controls.Add(img);
                    }
                    Label lab = new Label();
                    lab.Text = "&nbsp;&nbsp;";
                    tCell.Controls.Add(lab);
                    HyperLink hLink = new HyperLink();
                    hLink.Text = dt.Rows[i]["Name"].ToString();
                    hLink.NavigateUrl = "/" + dt.Rows[i]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[i]["ID"].ToString();
                    hLink.ToolTip = dt.Rows[i]["Name"].ToString();
                    if ((dt.Rows[i]["ID"].ToString() == sMoreTypeID) || ((sMoreTypeID == sWholeTypeID) && (i == 0)))
                        hLink.CssClass = "TextLinkStyle3";
                    else
                        hLink.CssClass = "TextLinkStyle4";
                    tCell.Controls.Add(hLink);
                    tRow.Cells.Add(tCell);

                    WebListNavigator_tabSecondMenu.Rows.Add(tRow);
                }
            }
        }

        private string GetChannelTypeID(string sId)
        {
            string sSQL = "select * from tblChannelType where ID=" + sId;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt16(dt.Rows[0]["TheLevel"]) == 2)
                {
                    sChannelID = dt.Rows[0]["ID"].ToString();
                    clsTree.sTableName = "tblChannelType";
                    sChannelDisID = clsTree.GetFirstLeaf(dt.Rows[0]["ID"].ToString());
                    return dt.Rows[0]["ID"].ToString();
                }
                else if (Convert.ToInt16(dt.Rows[0]["TheLevel"]) == 3)
                {
                    sChannelID = dt.Rows[0]["ParentIndex"].ToString();
                    sChannelDisID = sId;
                    return dt.Rows[0]["ParentIndex"].ToString();
                }
                else
                    return "0";
            }
            return "0";
        }

        private void InitTese()
        {
            string sSQL = "";
            sSQL = "select * from tblChannelType where ParentIndex=11 and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteExtraSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TableRow tRow = new TableRow();
                TableCell tCell = new TableCell();
                tCell.Attributes.Add("background", "/img/navbk7.jpg");
                tCell.Attributes.Add("align", "center");
                tCell.Height = new Unit(32);
                tCell.Width = new Unit(173);
                HyperLink hLink = new HyperLink();
                hLink.Text = dt.Rows[i]["Name"].ToString();
                hLink.NavigateUrl = "/" + dt.Rows[i]["ChannelTTemplete"].ToString() + "?id=" + dt.Rows[i]["ID"].ToString();
                hLink.CssClass = "TextLinkStyle3";
                tCell.Controls.Add(hLink);
                tRow.Cells.Add(tCell);
                WebListNavigator_tabTese.Rows.Add(tRow);
            }
        }

        public void InitListNavigator(string sId)
        {
            if (GetChannelTypeID(sId) == "0")
                Response.Redirect("index.htm");
            InitNavMore(sChannelID, sChannelDisID);
            if ((sChannelID != "11") && (sChannelID != "20"))
                InitTese();
            else
                teseTable.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}