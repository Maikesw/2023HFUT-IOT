using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;

//设置公司部门的权限信息。
//编写人：石雷
namespace admin.UserManage
{
    public partial class RoleAuthorization : System.Web.UI.Page
    {
        private clsTreeManager clsRoleTree = new clsTreeManager();
        private clsTreeManager clsOperationTree = new clsTreeManager();
        private DbObject cprivDBO = new DbObject();


        #region 树的相关事件
        protected void RoleAuthorization_ExpandOperationAllClick(object sender, DirectEventArgs e)
        {
            RoleAuthorization_ResourceManager.AddScript(((Ext.Net.TreePanel)RoleAuthorization_DataTreeOperation.FindControl("DataTree_TreeInfo")).ClientID + ".expandAll();");
        }
        protected void RoleAuthorization_CollapseOperationAllClick(object sender, DirectEventArgs e)
        {
            RoleAuthorization_ResourceManager.AddScript(((Ext.Net.TreePanel)RoleAuthorization_DataTreeOperation.FindControl("DataTree_TreeInfo")).ClientID + ".collapseAll();");
        }
        protected void RoleAuthorization_ExpandChannelAllClick(object sender, DirectEventArgs e)
        {
            RoleAuthorization_ResourceManager.AddScript(((Ext.Net.TreePanel)RoleAuthorization_DataTreeChannel.FindControl("DataTree_TreeInfo")).ClientID + ".expandAll();");
        }
        protected void RoleAuthorization_CollapseChannelAllClick(object sender, DirectEventArgs e)
        {
            RoleAuthorization_ResourceManager.AddScript(((Ext.Net.TreePanel)RoleAuthorization_DataTreeChannel.FindControl("DataTree_TreeInfo")).ClientID + ".collapseAll();");
        }
        #endregion

        #region Init
        private void InitDataList()
        {
            RoleAuthorization_DataList.sTableName = "tblAuthorizationInfo";
            RoleAuthorization_DataList.sViewName = RoleAuthorization_DataList.sTableName;
            RoleAuthorization_DataList.sOrderBy = " Order By TheOrder";
            ArrayList arr = new ArrayList();
            arr.Add("角色名称");
            RoleAuthorization_DataList.arrHeadName = arr;
            arr.Clear();
            arr.Add("Name");
            RoleAuthorization_DataList.arrDataIndex = arr;
            RoleAuthorization_DataList.InitDataList();

            RoleAuthorization_DataList.SetButtonVisiable(DataListButton.btnMore1);
            RoleAuthorization_DataList.SetButtonName(DataListButton.btnMore1, "保存权限");
            RoleAuthorization_DataList.SetButtonIcon(DataListButton.btnMore1, Icon.Disk);
        }

        private void InitOperationTree()
        {
            RoleAuthorization_DataTreeOperation.sTableName = "tblOperationInfo";
            RoleAuthorization_DataTreeOperation.sRootName = "所有菜单";
            RoleAuthorization_DataTreeOperation.sRelationTableName = "tblOperationAuthorRelation";
            RoleAuthorization_DataTreeOperation.BuildTree(true);
        }

        private void InitChannelTree()
        {
            RoleAuthorization_DataTreeChannel.sTableName = "tblChannelType";
            RoleAuthorization_DataTreeChannel.sRootName = "所有栏目";
            RoleAuthorization_DataTreeChannel.sRelationTableName = "tblChannelAuthorRelation";
            RoleAuthorization_DataTreeChannel.BuildTree(true);
        }
        #endregion

        #region SetCheck
        private void RefreshOperationTree()
        {
            RoleAuthorization_DataTreeOperation.RefreashCheck(RoleAuthorization_DataList.sRowNowID);
        }
        private void RefreshChannelTree()
        {
            RoleAuthorization_DataTreeChannel.RefreashCheck(RoleAuthorization_DataList.sRowNowID);
        }
        protected void RoleAuthorization_ListRowClick(object sender, EventArgs e)
        {
            RoleAuthorization_ResourceManager.AddScript(((Ext.Net.TreePanel)RoleAuthorization_DataTreeOperation.FindControl("DataTree_TreeInfo")).ClientID + ".expandAll();");
            RoleAuthorization_ResourceManager.AddScript(((Ext.Net.TreePanel)RoleAuthorization_DataTreeChannel.FindControl("DataTree_TreeInfo")).ClientID + ".expandAll();");
            RefreshOperationTree();
            RefreshChannelTree();
        }
        #endregion

        protected void RoleAuthorization_ListMore1Click(object sender, EventArgs e)
        {
            RoleAuthorization_DataTreeOperation.SaveAllCheck();
            RoleAuthorization_DataTreeChannel.SaveAllCheck();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                InitDataList();
                InitOperationTree();
                InitChannelTree();
                ((Ext.Net.GridPanel)RoleAuthorization_DataList.FindControl("DataList_gpInfo")).DirectEvents.Click.EventMask.ShowMask = true;
                ((Ext.Net.GridPanel)RoleAuthorization_DataList.FindControl("DataList_gpInfo")).DirectEvents.Click.EventMask.Msg = "正在操作,请稍候...";
            }
        }
    }
}
