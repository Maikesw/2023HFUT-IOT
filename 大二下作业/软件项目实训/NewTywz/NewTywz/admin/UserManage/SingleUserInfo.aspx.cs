using NewTywz.baseclass;
using System;
using System.Data;

namespace NewTywz.admin.UserManage
{
    public partial class SingleUserInfo : System.Web.UI.Page
    {
        private DbObject cprivDBO = new DbObject();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ID"] != null)
                {
                    ((UserControl.DataComboBox)SingleUserInfo_winMore.FindControl("WinUserMore_cmbDuty")).InitCtrl("tblDutyInfo");
                    ((UserControl.ThreeListComboBox)SingleUserInfo_winMore.FindControl("WinUserMore_cmbDepartment")).InitCtrl("tblDepartmentInfo");
                    ((Ext.Net.Panel)SingleUserInfo_winMore.FindControl("WinUserMore_panelRoleInfo")).Visible = false;
                    SingleUserInfo_winMore.Show(Session["ID"].ToString());
                    ((Ext.Net.Window)SingleUserInfo_winMore.FindControl("WinUserMore_winMore")).Closable = false;
                }
            }
        }



        protected void SingleUserInfo_MoreSave(object sender, EventArgs e)
        {
            SingleUserInfo_winMore.Show(Session["ID"].ToString());
        }

        protected void SingleUserInfo_MoreClose(object sender, EventArgs e)
        {
            string sSQL = "select ID from tblOperationInfo where Name='用户信息编辑' and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                SingleUserInfo_ResourceManager.AddScript("window.parent.Ext.getCmp('ExampleTabs').closeTab('" + dt.Rows[0]["ID"].ToString() + "');");
        }


    }
}
