using Ext.Net;
using NewTywz.basepage;
using System;
using System.Collections;

namespace admin.UserManage
{
    public partial class DepartmentInfo : ExtPage
    {

        private void InitTreeInfo()
        {
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sTableName = "tblDepartmentInfo";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sRootName = "所有部门";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sCodeName = "DepartmentICode";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).BuildTree();
            ((Ext.Net.Button)Master.FindControl("TreeMaster_btnMoreInfo")).Show();
            ((Ext.Net.Label)Master.FindControl("TreeMaster_labCode")).Html = "部门编码:";
            ((NewTywz.TreeMaster)Master).SetTreePanelName("所有部门");
            ((NewTywz.TreeMaster)Master).SetContentPanelName("部门详细信息");
        }

        private void InitDataList()
        {
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeMaster_DataList")).sTableName = "tblUserInfo";
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeMaster_DataList")).sViewName = "ViewForUserInfo";
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeMaster_DataList")).bFirstVisit = false;
            ArrayList arr = new ArrayList();
            arr.Add("所在部门"); arr.Add("姓名"); arr.Add("性别"); arr.Add("职务"); arr.Add("联系电话");
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeMaster_DataList")).arrHeadName = arr;
            arr.Clear();
            arr.Add("DepartmentName"); arr.Add("Name"); arr.Add("UserISex"); arr.Add("DutyName"); arr.Add("UserIPhone");
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeMaster_DataList")).arrDataIndex = arr;
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeMaster_DataList")).InitDataList();
        }

        public override void TreeMaster_MoreInfo(string sID)
        {
            DepartmentInfo_winMore.Show(sID);
        }

        protected void DepartmentInfo_MoreSave(object sender, EventArgs e)
        {
            ((NewTywz.TreeMaster)Master).refreshText();
        }

        public override void TreeMaster_TreeNodeClick(string sID)
        {
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeMaster_DataList")).InitStore("", "UserIDepartmentIID=" + sID);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                InitTreeInfo();
                InitDataList();
            }
        }
    }
}
