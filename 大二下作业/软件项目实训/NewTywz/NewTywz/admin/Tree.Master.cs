using Ext.Net;
using NewTywz.baseclass;
using NewTywz.basepage;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;

//树型结构操作的母板
//编写人：石雷
namespace NewTywz
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "TreeMaster")]
    public partial class TreeMaster : System.Web.UI.MasterPage
    {
        private clsTreeManager clsTree = new clsTreeManager();
        private DbObject cprivDBO = new DbObject();


        #region 一些DirectMethod
        /// <summary>
        /// 结束新增
        /// </summary>
        [DirectMethod]
        public void EndAddNew()
        {
            InitAllButton(false);
            TreeMaster_DataTree.EndAddNew();
        }

        /// <summary>
        /// 重新初始化所有的text
        /// </summary>
        [DirectMethod]
        public void ClearAllText()
        {
            TreeMaster_txtName.Text = "";
            TreeMaster_txtRemark.Text = "";
            TreeMaster_txtTemplete.Text = "";
        }
        #endregion

        #region 一些不太好归类的
        /// <summary>
        /// 重新初始化所有的button的enable
        /// </summary>
        /// <param name="flag">true or false</param>
        private void InitAllButton(bool flag)
        {
            TreeMaster_btnDelete.Disabled = flag;
            TreeMaster_TreePanel.Disabled = flag;
            TreeMaster_btnOrderUp.Disabled = flag;
            TreeMaster_btnOrderDown.Disabled = flag;
            TreeMaster_btnNew.Disabled = flag;
            TreeMaster_btnMoreInfo.Disabled = flag;
            TreeMaster_DataTree.bNewFlag = flag;
        }
        public void SetTreePanelName(string sName)
        {
            TreeMaster_TreePanel.Title = sName;
        }
        public void SetContentPanelName(string sName)
        {
            TreeMaster_pContent.Title = sName;
        }
        public void SetDataListVisiable(bool flag)
        {
            TreeMaster_DataListPanel.Hidden = !flag;
            if (!flag)
                TreeMaster_txtRemark.AnchorVertical = "99%";
        }
        public void refreshText()
        {
            DataTable dt = cprivDBO.GetValue(TreeMaster_DataTree.sTreeNowID, TreeMaster_DataTree.sTableName);
            if (dt.Rows.Count > 0)
            {
                TreeMaster_txtName.Text = dt.Rows[0]["Name"].ToString();
                TreeMaster_txtRemark.Text = dt.Rows[0]["Remark"].ToString();
                TreeMaster_txtTemplete.Text = dt.Rows[0][TreeMaster_DataTree.sCodeName].ToString();
                TreeMaster_DataTree.SetNodeText(dt.Rows[0]["Name"].ToString());
            }
        }
        #endregion

        #region 树的三个事件
        protected void TreeMaster_ExpandAllClick(object sender, DirectEventArgs e)
        {
            TreeMaster_ResourceManager.AddScript(((Ext.Net.TreePanel)TreeMaster_DataTree.FindControl("DataTree_TreeInfo")).ClientID + ".expandAll();");
        }
        protected void TreeMaster_CollapseAllClick(object sender, DirectEventArgs e)
        {
            TreeMaster_ResourceManager.AddScript(((Ext.Net.TreePanel)TreeMaster_DataTree.FindControl("DataTree_TreeInfo")).ClientID + ".collapseAll();");
        }
        protected void TreeMaster_NodeClick(object sender, EventArgs e)
        {
            DataTable dt = cprivDBO.GetValue(TreeMaster_DataTree.sTreeNowID, TreeMaster_DataTree.sTableName);
            if (dt.Rows.Count > 0)
            {
                TreeMaster_txtName.Text = dt.Rows[0]["Name"].ToString();
                TreeMaster_txtTemplete.Text = dt.Rows[0][TreeMaster_DataTree.sCodeName].ToString();
                TreeMaster_txtRemark.Text = dt.Rows[0]["Remark"].ToString();
            }
            else
            {
                TreeMaster_txtName.Text = "";
                TreeMaster_txtTemplete.Text = "";
                TreeMaster_txtRemark.Text = "";
            }
            ExtPage currentPage = Page as ExtPage;
            if (currentPage != null)
                currentPage.TreeMaster_TreeNodeClick(TreeMaster_DataTree.sTreeNowID);
        }
        #endregion

        #region 详细操作的各种按钮
        /// <summary>
        /// 点击创建下级（新增）按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeMaster_AddNextLevelClick(object sender, DirectEventArgs e)
        {
            if (TreeMaster_DataTree.sTreeNowID != "0")
            {
                string sSQL = "select ID,TheLevel from " + TreeMaster_DataTree.sTableName + " where ID=" + TreeMaster_DataTree.sTreeNowID;
                DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                object obj = cprivDBO.GetValue(TreeMaster_DataTree.sTreeNowID, "TheLevel", TreeMaster_DataTree.sTableName);
                if ((obj != null) && (obj != ""))
                {
                    int nLevel = Convert.ToInt16(obj);
                    if (nLevel == TreeMaster_DataTree.nMaxLevel)
                        X.Msg.Alert("错误", "已经是最底层节点了！").Show();
                    else
                    {
                        InitAllButton(true);
                        TreeMaster_DataTree.bNewFlag = true;
                        ClearAllText();
                    }
                }
                else if (TreeMaster_DataTree.sTreeNowID == "-1")
                {
                    InitAllButton(true);
                    TreeMaster_DataTree.bNewFlag = true;
                    ClearAllText();
                }
                else
                    X.Msg.Alert("错误", "未知错误！").Show();
            }
            else
                X.Msg.Alert("提示", "请先选中要新增的节点！").Show();
        }

        /// <summary>
        /// 点击保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeMaster_SaveClick(object sender, DirectEventArgs e)
        {
            if (TreeMaster_DataTree.sTreeNowID != "0")
            {
                if ((!TreeMaster_DataTree.bNewFlag) && (TreeMaster_DataTree.sTreeNowID != "-1"))  //修改，不能修改根节点
                {
                    ArrayList arrWordName = new ArrayList();
                    ArrayList arrWordValue = new ArrayList();
                    ArrayList arrWordType = new ArrayList();

                    arrWordName.Add("Name");
                    arrWordValue.Add(TreeMaster_txtName.Text.Trim());
                    arrWordType.Add(ConstDataBase.STRING_TYPE);

                    arrWordName.Add("Remark");
                    arrWordValue.Add(TreeMaster_txtRemark.Text.Trim());
                    arrWordType.Add(ConstDataBase.STRING_TYPE);

                    arrWordName.Add(TreeMaster_DataTree.sCodeName);
                    arrWordValue.Add(TreeMaster_txtTemplete.Text.Trim());
                    arrWordType.Add(ConstDataBase.STRING_TYPE);

                    TreeMaster_DataTree.SaveTreeNode(arrWordName, arrWordValue, arrWordType);
                }
                else if (TreeMaster_DataTree.bNewFlag)  //新增
                {
                    ArrayList arrWordName = new ArrayList();
                    ArrayList arrWordValue = new ArrayList();
                    ArrayList arrWordType = new ArrayList();

                    arrWordName.Add("Name");
                    arrWordValue.Add(TreeMaster_txtName.Text.Trim());
                    arrWordType.Add(ConstDataBase.STRING_TYPE);

                    arrWordName.Add("Remark");
                    arrWordValue.Add(TreeMaster_txtRemark.Text.Trim());
                    arrWordType.Add(ConstDataBase.STRING_TYPE);

                    arrWordName.Add(TreeMaster_DataTree.sCodeName);
                    arrWordValue.Add(TreeMaster_txtTemplete.Text.Trim());
                    arrWordType.Add(ConstDataBase.STRING_TYPE);

                    arrWordName.Add("ParentIndex");
                    arrWordValue.Add(TreeMaster_DataTree.sTreeNowID);
                    arrWordType.Add(ConstDataBase.INT_TYPE);

                    TreeMaster_DataTree.AddTreeNode(arrWordName, arrWordValue, arrWordType);


                    X.Msg.Confirm("提示", "新增成功！继续新增吗？", new JFunction { Fn = "TreeMaster_ConfirmAddNew" }).Show();
                }
                else
                    X.Msg.Alert("提示", "根节点不能修改！").Show();
            }
            else
                X.Msg.Alert("提示", "请先选中要修改的节点！").Show();
        }
        /// <summary>
        /// 点击删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeMaster_DeleteClick(object sender, DirectEventArgs e)
        {
            TreeMaster_DataTree.DelTreeNode();
        }
        /// <summary>
        /// 点击重置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeMaster_ResetClick(object sender, DirectEventArgs e)
        {
            ClearAllText();
            InitAllButton(false);
            TreeMaster_DataTree.sTreeNowID = "-1";
        }
        /// <summary>
        /// 点击上移按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeMaster_OrderUpClick(object sender, DirectEventArgs e)
        {
            TreeMaster_DataTree.OrderUpNode();
        }
        /// <summary>
        /// 点击上移按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeMaster_OrderDownClick(object sender, DirectEventArgs e)
        {
            TreeMaster_DataTree.OrderDownNode();
        }

        /// <summary>
        /// 点击更多内容按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeMaster_MoreInfoClick(object sender, DirectEventArgs e)
        {
            if ((TreeMaster_DataTree.sTreeNowID == "0") || (TreeMaster_DataTree.sTreeNowID == "-1"))
                X.Msg.Alert("注意", "请先选中操作节点！").Show();
            else
            {
                ExtPage currentPage = Page as ExtPage;
                if (currentPage != null)
                    currentPage.TreeMaster_MoreInfo(TreeMaster_DataTree.sTreeNowID);
            }
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
