using NewTywz.baseclass;
using System;
using System.Data;

namespace NewTywz.UserControl
{
    /// <summary>
    /// main.index右下角那一块区域
    /// </summary>
    public partial class WebRightBottom : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        public void InitWebPage()
        {
            WebRightBottom_Table1.Visible = true;
            WebRightBottom_Table2.Visible = false;
            WebRightBottom_cmbLink1.InitCtrl("tblFriendLink");
        }

        protected void MainPage_CmbChange1(object sender, EventArgs e)
        {
            string sSQL = "select * from tblFriendLink where ID=" + WebRightBottom_cmbLink1.GetValue();
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                Response.Redirect(dt.Rows[0]["Remark"].ToString());
        }

        protected void MainPage_CmbChange2(object sender, EventArgs e)
        {
            string sSQL = "select * from tblFriendLink where ID=" + WebRightBottom_cmbLink2.GetValue();
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                Response.Redirect(dt.Rows[0]["Remark"].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}