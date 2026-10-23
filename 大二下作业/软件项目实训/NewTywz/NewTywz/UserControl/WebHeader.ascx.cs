using NewTywz.baseclass;
using System;
using System.Data;

namespace NewTywz.UserControl
{
    /// <summary>
    /// 头部的banner以及头部的一二级菜单
    /// </summary>
    public partial class WebHeader : System.Web.UI.UserControl
    {
        private clsMenuManager objMenu = new clsMenuManager();
        private DbObject cprivDBO = new DbObject();
        public string sSWFName;

        public string sHeader
        {
            set
            {
                WebHeader_Header.Text = value;
            }
            get
            {
                return WebHeader_Header.Text;
            }
        }


        private string GetLink(DataTable dt, int i)
        {
            string sLink = dt.Rows[i]["ChannelTTemplete"].ToString();
            if (sLink[sLink.Length - 1] == '/')
                sLink = sLink.Substring(0, sLink.Length - 1);
            else
                sLink = sLink + "?id=" + dt.Rows[i]["ID"].ToString();
            return sLink;
        }


        private void InitHeadMenu()
        {
            string sSQL = "select * from tblChannelType where DelFlag=0 and ParentIndex=-1 order by TheOrder";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                WebHeader_MainHeadMenu.Text = "<table width=100% cellpadding=0 cellspacing=0><tr>";
                int nWidth = 730 / dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                    WebHeader_MainHeadMenu.Text = WebHeader_MainHeadMenu.Text + "<td width=" + nWidth + " align=center><a href=" + GetLink(dt, i) + " class=HeadMenuStyle1>" + dt.Rows[i]["Name"].ToString() + "</a></td><td width=1 class=HeadMenuStyle1>|</td>";
                WebHeader_MainHeadMenu.Text = WebHeader_MainHeadMenu.Text + "</tr></table>";
            }
        }

        public void InitWebHeader()
        {
            string sSQL = "select * from tblMainInfo";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                sHeader = dt.Rows[0]["MainIName"].ToString();
                sSWFName = "banner1.swf";
            }
            InitHeadMenu();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}