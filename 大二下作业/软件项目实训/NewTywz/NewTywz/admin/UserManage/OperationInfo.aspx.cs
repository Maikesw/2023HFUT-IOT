using Ext.Net;
using NewTywz.basepage;
using System;
namespace admin.UserManage
{
    public partial class OperationInfo : ExtPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sTableName = "tblOperationInfo";
                ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sRootName = "所有菜单";
                ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sCodeName = "OperationIURL";
                ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).BuildTree();
                ((Ext.Net.Label)Master.FindControl("TreeMaster_labCode")).Html = "模版名称:";
                ((NewTywz.TreeMaster)Master).SetTreePanelName("所有菜单");
                ((NewTywz.TreeMaster)Master).SetContentPanelName("菜单项详细信息");
                ((NewTywz.TreeMaster)Master).SetDataListVisiable(false);
            }
        }
    }
}
