using Ext.Net;
using NewTywz.baseclass;
using NewTywz.basepage;
using System;

//树型结构操作的母板
//编写人：石雷
namespace NewTywz
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "TreeSelMaster")]
    public partial class TreeSelMaster : System.Web.UI.MasterPage
    {
        private clsTreeManager clsTree = new clsTreeManager();
        private DbObject cprivDBO = new DbObject();

        #region 一些DirectMethod

        #endregion

        #region 一些不太好归类的

        public void SetTreePanelName(string sName)
        {
            TreeSelMaster_TreePanel.Title = sName;
        }
        public void SetContentPanelName(string sName)
        {
            TreeSelMaster_pContent.Title = sName;
        }
        #endregion

        #region 树的三个事件
        protected void TreeSelMaster_ExpandAllClick(object sender, DirectEventArgs e)
        {
            TreeSelMaster_ResourceManager.AddScript(((Ext.Net.TreePanel)TreeSelMaster_DataTree.FindControl("DataTree_TreeInfo")).ClientID + ".expandAll();");
        }
        protected void TreeSelMaster_CollapseAllClick(object sender, DirectEventArgs e)
        {
            TreeSelMaster_ResourceManager.AddScript(((Ext.Net.TreePanel)TreeSelMaster_DataTree.FindControl("DataTree_TreeInfo")).ClientID + ".collapseAll();");
        }
        protected void TreeSelMaster_NodeClick(object sender, EventArgs e)
        {
            if (!TreeSelMaster_DataList.bFirstVisit)
                TreeSelMaster_DataList.sNowSQL = "";
            ExtPage currentPage = Page as ExtPage;
            if (currentPage != null)
                currentPage.TreeSelMaster_TreeNodeClick(TreeSelMaster_DataTree.sTreeNowID);
        }
        #endregion

        #region DataList相关事件
        protected void TreeSelMaster_ListEditClick(object sender, EventArgs e)
        {
            ExtPage currentPage = Page as ExtPage;
            if (currentPage != null)
                currentPage.TreeSelMaster_DataListEditClick(TreeSelMaster_DataList.sRowNowID);
        }

        protected void TreeSelMaster_ListAddClick(object sender, EventArgs e)
        {
            ExtPage currentPage = Page as ExtPage;
            if (currentPage != null)
                currentPage.TreeSelMaster_DataListAddClick();
        }

        protected void TreeSelMaster_ListAllClick(object sender, EventArgs e)
        {
            clsTree.sTableName = TreeSelMaster_DataTree.sTableName;
            string sAllTreeID = clsTree.GetAllLeaf(TreeSelMaster_DataTree.sTreeNowID);
            TreeSelMaster_DataList.InitStore(TreeSelMaster_DataList.sInitSQL, " " + TreeSelMaster_DataTree.sTreeListName + " IN (" + sAllTreeID + ")");
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {

            }
        }


    }

}
