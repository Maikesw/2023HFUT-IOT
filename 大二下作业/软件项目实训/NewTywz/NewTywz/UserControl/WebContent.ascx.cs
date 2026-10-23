using Ext.Net;
using NewTywz.baseclass;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace NewTywz.UserControl
{
    /// <summary>
    /// list中显示一条一条的信息
    /// </summary>
    public partial class WebContent : System.Web.UI.UserControl
    {
        private clsMenuManager objMenu = new clsMenuManager();
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        /// <summary>
        /// 当前类型
        /// </summary>
        public int nType
        {
            get
            {
                return Convert.ToInt16(WebContent_Type.Text);
            }
            set
            {
                WebContent_Type.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前对应的ContentID
        /// </summary>
        public string sContentID
        {
            get
            {
                return WebContent_ContentID.Text;
            }
            set
            {
                WebContent_ContentID.Text = value;
            }
        }

        #endregion

        private Table InitPicBody(string sChannelContentID)
        {
            Table tab = new Table();
            tab.Width = new Unit(600);
            tab.CellPadding = 0;
            tab.CellSpacing = 0;


            string sSQL = "select * from tblChannelContentPic where RelationID=" + sChannelContentID + " order by TheOrder";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TableRow tRow = new TableRow();
                TableCell tCell = new TableCell();
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = "/attached/image/" + dt.Rows[i]["Address"].ToString();
                img.Attributes.Add("onload", "if (this.width>735) this.width=735;");
                img.ToolTip = dt.Rows[i]["Name"].ToString();
                tCell.Controls.Add(img);
                tCell.Attributes.Add("align", "center");
                tRow.Cells.Add(tCell);
                tab.Rows.Add(tRow);

                tRow = new TableRow();
                tCell = new TableCell();
                tRow.Height = new Unit(10);
                tRow.Cells.Add(tCell);
                tab.Rows.Add(tRow);
            }
            return tab;
        }

        private Table InitOnePicBody(string sChannelContentID)
        {
            Table tab = new Table();
            tab.Width = new Unit(600);
            tab.CellPadding = 0;
            tab.CellSpacing = 0;


            string sSQL = "select * from tblChannelContentPic where RelationID=" + sChannelContentID + " order by TheOrder";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                TableRow tRow = new TableRow();
                TableCell tCell = new TableCell();
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ImageUrl = "/attached/image/" + dt.Rows[0]["Address"].ToString();
                img.Attributes.Add("onload", "if (this.width>735) this.width=735;");
                img.ToolTip = dt.Rows[0]["Name"].ToString();
                tCell.Controls.Add(img);
                tCell.Attributes.Add("align", "center");
                tRow.Cells.Add(tCell);
                tab.Rows.Add(tRow);

                tRow = new TableRow();
                tCell = new TableCell();
                tRow.Height = new Unit(10);
                tRow.Cells.Add(tCell);
                tab.Rows.Add(tRow);
            }
            return tab;
        }

        private Table InitDocBody(string sChannelContentID)
        {
            Table tab = new Table();
            tab.Width = new Unit(600);
            tab.CellPadding = 0;
            tab.CellSpacing = 0;

            TableRow tRow = new TableRow();
            TableCell tCell = new TableCell();
            System.Web.UI.WebControls.Label lab = new System.Web.UI.WebControls.Label();
            lab.Text = "相关附件：";
            tCell.Controls.Add(lab);
            tRow.Cells.Add(tCell);
            tab.Rows.Add(tRow);

            string sSQL = "select * from tblChannelContentDoc where RelationID=" + sChannelContentID + " order by TheOrder";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tRow = new TableRow();
                tCell = new TableCell();

                System.Web.UI.WebControls.HyperLink hlink = new System.Web.UI.WebControls.HyperLink();
                hlink.CssClass = "TextLinkStyle";
                hlink.NavigateUrl = "/doc/" + dt.Rows[i]["Address"].ToString();
                hlink.Text = i + "．" + dt.Rows[i]["Name"].ToString();

                tCell.Controls.Add(hlink);
                tCell.Attributes.Add("align", "left");
                tRow.Cells.Add(tCell);
                tab.Rows.Add(tRow);
            }
            return tab;
        }



        public void InitMain()
        {
            DataTable dt = cprivDBO.GetValue(sContentID, "tblChannelContent");
            if (dt.Rows.Count > 0)
            {
                if ((Convert.ToBoolean(dt.Rows[0]["ChannelCAuthor"])) && (Session["ID"] == null))
                {
                    X.Msg.Alert("注意", "您无权查看该页！").Show();
                    return;
                }
                TableRow tRow = new TableRow();
                TableCell tCell = new TableCell();
                tCell.Attributes.Add("align", "center");
                System.Web.UI.WebControls.Label lab = new System.Web.UI.WebControls.Label();
                lab.Text = dt.Rows[0]["ChannelCName"].ToString();
                lab.CssClass = "MainChannelFont";
                tCell.Controls.Add(lab);
                tCell.Height = new Unit(40);
                tCell.Attributes.Add("valign", "middle");
                tRow.Cells.Add(tCell);
                WebContent_tab.Rows.Add(tRow);

                int nHitTime = 1;
                if (dt.Rows[0]["HitTime"] != DBNull.Value)
                    nHitTime = Convert.ToInt32(dt.Rows[0]["HitTime"]) + 1;
                cprivDBO.SetValue("HitTime", nHitTime, "tblChannelContent", Convert.ToInt32(sContentID));
                if ((nType == 1) || (nType == 3))
                {
                    tRow = new TableRow();
                    tRow.Height = new Unit(30);
                    tCell = new TableCell();
                    tCell.Attributes.Add("align", "center");
                    lab = new System.Web.UI.WebControls.Label();
                    lab.Text = "<table width=735 cellpadding=0 cellspacing=0 class=DashTable><tr><td height=25 align=center>";
                    lab.Text = lab.Text + "编辑日期：" + Convert.ToDateTime(dt.Rows[0]["ChannelCTime"]).ToString("yyyy-MM-dd") + "&nbsp;&nbsp;" + "作者/编辑：" + dt.Rows[0]["ChannelCAuthorName"].ToString();
                    lab.Text = lab.Text + "&nbsp;&nbsp;" + "点击数：" + nHitTime.ToString() + "次";
                    lab.Text = lab.Text + "</td></tr></table><table width=735 cellpadding=0 cellspacing=0><tr><td height=10></td></tr></table>";
                    tCell.Controls.Add(lab);
                    tRow.Cells.Add(tCell);
                    WebContent_tab.Rows.Add(tRow);
                }

                if (nType == 2)
                {
                    tRow = new TableRow();
                    tCell = new TableCell();
                    tCell.Controls.Add(InitOnePicBody(sContentID));
                    tRow.Cells.Add(tCell);
                    WebContent_tab.Rows.Add(tRow);
                }

                tRow = new TableRow();
                tCell = new TableCell();
                lab = new System.Web.UI.WebControls.Label();
                string sMainText = dt.Rows[0]["ChannelCText"].ToString().Replace("&nbsp;", "");
                sMainText = sMainText.Replace("<p", "<p class=MainText").Replace("<P", "<P class=MainText");
                sMainText = sMainText.Replace("<BR>", "</P><P class=MainText>").Replace("<br>", "</P><P class=MainText>").Replace("<BR />", "</P><P class=MainText>").Replace("<br />", "</P><P class=MainText>");
                sMainText = sMainText.Replace("<img", "<img class=MainPic ");
                if (sMainText.Substring(0, 2) != "<P")
                    sMainText = "<P class=MainText>" + sMainText;
                if (sMainText.Substring(sMainText.Length - 5, 4) != "</P>")
                    sMainText = sMainText + "</P>";
                lab.Text = sMainText;
                //lab.CssClass = "MainText";
                tCell.Controls.Add(lab);
                tRow.Cells.Add(tCell);
                WebContent_tab.Rows.Add(tRow);

                if ((nType == 1) || (nType == 3))
                {
                    if (nType == 1)
                    {
                        if (cprivDBO.GetCount(" RelationID=" + sContentID, "tblChannelContentPic") > 0)
                        {
                            tRow = new TableRow();
                            tCell = new TableCell();
                            tCell.Controls.Add(InitPicBody(sContentID));
                            tRow.Cells.Add(tCell);
                            WebContent_tab.Rows.Add(tRow);
                        }
                    }

                    if (cprivDBO.GetCount(" RelationID=" + sContentID, "tblChannelContentDoc") > 0)
                    {
                        tRow = new TableRow();
                        tCell = new TableCell();
                        tCell.Controls.Add(InitDocBody(sContentID));
                        tRow.Cells.Add(tCell);
                        WebContent_tab.Rows.Add(tRow);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}