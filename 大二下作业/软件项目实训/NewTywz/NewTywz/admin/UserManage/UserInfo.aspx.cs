using Ext.Net;
using NewTywz.baseclass;
using NewTywz.basepage;
using NewTywz.ConstValue;
using System;
using System.Collections;

namespace admin.UserManage
{
    public partial class UserInfo : ExtPage
    {
        private clsTreeManager clsTree = new clsTreeManager();

        private void InitTreeInfo()
        {
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sTableName = "tblDepartmentInfo";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sRootName = "所有部门";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).BuildTree();
            ((NewTywz.TreeSelMaster)Master).SetTreePanelName("所有部门");
            ((NewTywz.TreeSelMaster)Master).SetContentPanelName("部门所有用户");
        }

        private void InitDataList()
        {
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).sTableName = "tblUserInfo";
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).sViewName = "ViewForUserInfo";
            ArrayList arr = new ArrayList();
            arr.Add("所在部门"); arr.Add("姓名"); arr.Add("性别"); arr.Add("职务"); arr.Add("联系电话");
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).arrHeadName = arr;
            arr.Clear();
            arr.Add("DepartmentName"); arr.Add("Name"); arr.Add("UserISex"); arr.Add("DutyName"); arr.Add("UserIPhone");
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).arrDataIndex = arr;
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).InitDataList();

            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).SetButtonVisiable(DataListButton.btnAdd);
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).SetButtonVisiable(DataListButton.btnEdit);
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).SetButtonVisiable(DataListButton.btnDelete);
        }

        public override void TreeSelMaster_DataListEditClick(string sID)
        {
            UserInfo_winMore.Show(sID);
        }

        public override void TreeSelMaster_DataListAddClick()
        {
            UserInfo_winMore.Show(((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sTreeNowID, true);

        }

        public override void TreeSelMaster_TreeNodeClick(string sID)
        {
            clsTree.sTableName = "tblDepartmentInfo";
            string sAllDepartmentID = clsTree.GetAllLeaf(sID);
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).InitStore("", " UserIDepartmentIID IN (" + sAllDepartmentID + ")");
        }



        protected void UserInfo_MoreSave(object sender, EventArgs e)
        {
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).InitStore();
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
