using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;


namespace NewTywz.UserControl
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.ClientID)]
    public partial class DataTree : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();
        private clsTreeManager clsTree = new clsTreeManager();


        #region 设定传递的一些value
        public bool Disabled
        {
            get
            {
                return DataTree_TreeInfo.Disabled;
            }
            set
            {
                DataTree_TreeInfo.Disabled = value;
            }
        }


        public int Width
        {
            get { return Convert.ToInt32(DataTree_TreeInfo.Width); }
            set
            {
                Unit u = new Unit(value - 25);
                DataTree_TreeInfo.Width = u;
            }
        }

        public string sTableName
        {
            get
            {
                return DataTree_TableName.Text;
            }
            set
            {
                DataTree_TableName.Text = value;
            }
        }
        public string sTreeNowID
        {
            get
            {
                return DataTree_TreeNowID.Text;
            }
            set
            {
                DataTree_TreeNowID.Text = value;
                DataTree_TreeInfo.ResourceManager.AddScript(DataTree_TreeInfo.ClientID + ".getNodeById(" + value + ").select();");
            }
        }
        public string sTreeNewID
        {
            get
            {
                return DataTree_TreeNewID.Text;
            }
            set
            {
                DataTree_TreeNewID.Text = value;
            }
        }
        public string sSomeSQL
        {
            get
            {
                return DataTree_SomeSQL.Text;
            }
            set
            {
                DataTree_SomeSQL.Text = value;
            }
        }
        public bool bNeedRoot
        {
            get
            {
                if (DataTree_NeedRoot.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    DataTree_NeedRoot.Text = "1";
                else
                    DataTree_NeedRoot.Text = "0";
            }
        }
        public bool bNewFlag
        {
            get
            {
                if (DataTree_NewFlag.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    DataTree_NewFlag.Text = "1";
                    DataTree_TreeInfo.Enabled = false;
                }
                else
                {
                    DataTree_NewFlag.Text = "0";
                    DataTree_TreeInfo.Enabled = true;
                }
            }
        }

        public bool bIsCheckTree
        {
            get
            {
                if (DataTree_IsCheckTree.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    DataTree_IsCheckTree.Text = "1";
                else
                    DataTree_IsCheckTree.Text = "0";
            }
        }

        public string sRootName
        {
            get
            {
                return DataTree_RootName.Text;
            }
            set
            {
                DataTree_RootName.Text = value;
            }
        }
        public string sCodeName
        {
            get
            {
                return DataTree_CodeName.Text;
            }
            set
            {
                DataTree_CodeName.Text = value;
            }
        }
        public int nMaxLevel
        {
            get
            {
                return Convert.ToInt16(DataTree_MaxLevel.Text);
            }
            set
            {
                DataTree_MaxLevel.Text = value.ToString();
            }
        }
        public string sTreeListName
        {
            get
            {
                return DataTree_TreeListName.Text;
            }
            set
            {
                DataTree_TreeListName.Text = value;
            }
        }
        public string sRelationTableName
        {
            get
            {
                return DataTree_RelationTableName.Text;
            }
            set
            {
                DataTree_RelationTableName.Text = value;
            }
        }
        public string sRelationID1
        {
            get
            {
                return DataTree_RelationID1.Text;
            }
            set
            {
                DataTree_RelationID1.Text = value;
            }
        }
        public string sOldCheckListIDs
        {
            get
            {
                return DataTree_OldCheckListIDs.Text;
            }
            set
            {
                DataTree_OldCheckListIDs.Text = value;
            }
        }
        public string sNewCheckListIDs
        {
            get
            {
                return DataTree_NewCheckListIDs.Text;
            }
            set
            {
                DataTree_NewCheckListIDs.Text = value;
            }
        }
        #endregion

        #region 树的新增修改删除上下移等操作

        #region 节点修改操作
        public void SaveTreeNode(ArrayList arrName, ArrayList arrValue, ArrayList arrType)
        {
            cprivDBO.SetValue(arrName, arrValue, arrType, sTableName, "ID=" + sTreeNowID);
            DataTree_TreeInfo.SetNodeText(sTreeNowID, arrValue[0].ToString());
        }
        public void SetNodeText(string sName)
        {
            DataTree_TreeInfo.SetNodeText(sTreeNowID, sName);
        }
        #endregion

        #region 节点新增操作
        private void ClientAddTreeNode(string sParentID, string sNodeID, string sNodeText)
        {
            DataTree_TreeInfo.ResourceManager.AddScript("var node=new Ext.tree.TreeNode();");
            DataTree_TreeInfo.ResourceManager.AddScript("node.id=" + sNodeID + ";");
            DataTree_TreeInfo.ResourceManager.AddScript("node.text='" + sNodeText + "';");
            DataTree_TreeInfo.ResourceManager.AddScript(DataTree_TreeInfo.ClientID + ".getNodeById(" + sParentID + ").expand();");
            DataTree_TreeInfo.ResourceManager.AddScript(DataTree_TreeInfo.ClientID + ".getNodeById(" + sParentID + ").appendChild(node);");
        }

        public void AddTreeNode(ArrayList arrName, ArrayList arrValue, ArrayList arrType)
        {
            clsTree.sTableName = sTableName;
            sTreeNewID = clsTree.AddTreeNode(arrName, arrValue, arrType, sTreeNowID);
            ClientAddTreeNode(sTreeNowID, sTreeNewID, arrValue[0].ToString());
        }

        public void EndAddNew()
        {
            sTreeNowID = sTreeNewID;
            bNewFlag = false;
        }
        #endregion

        #region 节点删除操作
        private void ClientDelTreeNode()
        {
            DataTree_TreeInfo.ResourceManager.AddScript("var node=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ").previousSibling;");
            DataTree_TreeInfo.ResourceManager.AddScript("if (node==null) node=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ").nextSibling;");
            DataTree_TreeInfo.ResourceManager.AddScript("if (node==null) node=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ").parentNode;");
            DataTree_TreeInfo.ResourceManager.AddScript(DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ").remove();");
            DataTree_TreeInfo.ResourceManager.AddScript("if (node!=null) node.select();");
            DataTree_TreeInfo.ResourceManager.AddScript("Ext.net.DirectMethods." + this.ClientID + "._TreeNodeClick(node.id);");
        }

        protected void DataTree_DelSureClick(object sender, EventArgs e)
        {
            clsTree.sTableName = sTableName;
            if (clsTree.IsLeaf(sTreeNowID))
            {
                clsTree.DelTreeNode(sTreeNowID);
                ClientDelTreeNode();
            }
            else
                X.Msg.Alert("错误", "删除失败！只有叶子节点才能删除！").Show();
        }

        public void DelTreeNode()
        {
            if ((sTreeNowID != "0") && (sTreeNowID != "-1"))
                DataTree_winDelSure.Show("您确定删除该内容吗？");
            else
                X.Msg.Alert("注意", "请先选中要删除的节点！").Show();
        }
        #endregion

        #region 节点OrderUp操作
        private void ClientOrderUpNode()
        {
            DataTree_TreeInfo.ResourceManager.AddScript("var node=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ").previousSibling;");
            DataTree_TreeInfo.ResourceManager.AddScript("if (node==null) Ext.Msg.alert('提示', '已经是第一个节点了！');");
            DataTree_TreeInfo.ResourceManager.AddScript("else {");
            DataTree_TreeInfo.ResourceManager.AddScript("var nNode=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ");");
            DataTree_TreeInfo.ResourceManager.AddScript("var pNode=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ").parentNode;");
            DataTree_TreeInfo.ResourceManager.AddScript("pNode.insertBefore(nNode,node);");
            DataTree_TreeInfo.ResourceManager.AddScript("Ext.net.DirectMethods." + this.ClientID + ".OrderUp('" + sTableName + "');}");
        }

        public void OrderUpNode()
        {
            if ((sTreeNowID != "0") && (sTreeNowID != "-1"))
                ClientOrderUpNode();
            else
                X.Msg.Confirm("注意", "请先选中要移动的节点！").Show();
        }
        [DirectMethod]
        public void OrderUp(string _sTableName)
        {
            clsTree.sTableName = _sTableName;
            clsTree.OrderUp(sTreeNowID);
        }
        #endregion

        #region 节点OrderDown操作

        private void ClientOrderDownNode()
        {
            DataTree_TreeInfo.ResourceManager.AddScript("var node=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ").nextSibling;");
            DataTree_TreeInfo.ResourceManager.AddScript("if (node==null) Ext.Msg.alert('提示', '已经是最后一个节点了！');");
            DataTree_TreeInfo.ResourceManager.AddScript("else {");
            DataTree_TreeInfo.ResourceManager.AddScript("var nNode=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ");");
            DataTree_TreeInfo.ResourceManager.AddScript("var pNode=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sTreeNowID + ").parentNode;");
            DataTree_TreeInfo.ResourceManager.AddScript("pNode.insertBefore(node,nNode);");
            DataTree_TreeInfo.ResourceManager.AddScript("Ext.net.DirectMethods." + this.ClientID + ".OrderDown('" + sTableName + "');}");
        }

        public void OrderDownNode()
        {
            if ((sTreeNowID != "0") && (sTreeNowID != "-1"))
                ClientOrderDownNode();
            else
                X.Msg.Confirm("注意", "请先选中要移动的节点！").Show();
        }
        [DirectMethod]
        public void OrderDown(string _sTableName)
        {
            clsTree.sTableName = _sTableName;
            clsTree.OrderDown(sTreeNowID);
        }
        #endregion

        #endregion

        #region BuildTree
        /// <summary>
        /// 建立树
        /// </summary>        
        public void BuildTree()
        {
            BuildTree(false);
        }
        public void BuildTree(bool check)
        {
            bIsCheckTree = check;
            ArrayList arrValue = new ArrayList();
            if (DataTree_SomeSQL.Value != null)
            {
                DataTable dt = cprivDBO.ExcuteSqlTable(DataTree_SomeSQL.Value.ToString());
                for (int i = 0; i < dt.Rows.Count; i++)
                    arrValue.Add(dt.Rows[i]["ID"].ToString());
            }
            clsTree.sTableName = sTableName;
            clsTree.sRootName = sRootName;
            clsTree.bNeedRoot = bNeedRoot;
            clsTree.InitTree(DataTree_TreeInfo, arrValue, check);
        }
        #endregion

        #region SaveCheck
        public void SaveAllCheck()
        {
            DataTree_TreeInfo.ResourceManager.AddScript("var nodes=" + DataTree_TreeInfo.ClientID + ".getChecked();");
            DataTree_TreeInfo.ResourceManager.AddScript("var s='';");
            DataTree_TreeInfo.ResourceManager.AddScript("for (var i = 0; i < nodes.length; i++) ");
            DataTree_TreeInfo.ResourceManager.AddScript("s=s+nodes[i].id+'|';");
            DataTree_TreeInfo.ResourceManager.AddScript("Ext.net.DirectMethods." + this.ClientID + ".SaveCheck(s,'" + sRelationTableName + "','" + sOldCheckListIDs + "');");
        }

        private string GetAllCheckStr(string s)
        {
            string sReturn = s.Substring(0, s.Length - 1);
            string[] ss = sReturn.Split('|');
            for (int i = 0; i < ss.Length; i++)
            {
                string sNowID = ss[i];
                while (sNowID != "-1")
                {
                    sNowID = cprivDBO.GetValue(sNowID, "ParentIndex", sTableName).ToString();
                    if (sNowID != "-1")
                    {
                        string[] ssTemp = sReturn.Split('|');
                        if (!ssTemp.Contains(sNowID))
                            sReturn = sReturn + "|" + sNowID;
                    }
                }
            }
            return sReturn;
        }


        [DirectMethod]
        public void SaveCheck(string s, string _sTableName, string _sOldCheck)
        {
            string[] arrNewCheckListID = GetAllCheckStr(s).Split('|');
            string[] arrOldCheckListID = _sOldCheck.Split('|');
            for (int i = 0; i < arrNewCheckListID.Length; i++)//新的有旧的没有
            {
                bool flag = false;
                int j = 0;
                while ((!flag) && (j < arrOldCheckListID.Length))
                {
                    if (arrNewCheckListID[i] == arrOldCheckListID[j])
                        flag = true;
                    else
                        j++;
                }
                if (!flag)//新的有旧的没有，新增
                {
                    ArrayList arrWordName = new ArrayList(); ArrayList arrWordValue = new ArrayList(); ArrayList arrWordType = new ArrayList();
                    arrWordName.Add("RelationID1"); arrWordValue.Add(sRelationID1); arrWordType.Add(ConstDataBase.INT_TYPE);
                    arrWordName.Add("RelationID2"); arrWordValue.Add(arrNewCheckListID[i]); arrWordType.Add(ConstDataBase.INT_TYPE);
                    cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, _sTableName);
                }
            }

            for (int i = 0; i < arrOldCheckListID.Length; i++)//旧的有新的没有
            {
                bool flag = false;
                int j = 0;
                while ((!flag) && (j < arrNewCheckListID.Length))
                {
                    if (arrOldCheckListID[i] == arrNewCheckListID[j])
                        flag = true;
                    else
                        j++;
                }
                if (!flag)//旧的有新的没有，删除
                {
                    ArrayList arrWordName = new ArrayList(); ArrayList arrWordValue = new ArrayList(); ArrayList arrWordType = new ArrayList();
                    arrWordName.Add("RelationID1"); arrWordValue.Add(sRelationID1); arrWordType.Add(ConstDataBase.INT_TYPE);
                    arrWordName.Add("RelationID2"); arrWordValue.Add(arrOldCheckListID[i]); arrWordType.Add(ConstDataBase.INT_TYPE);
                    cprivDBO.DeleteValue(arrWordName, arrWordValue, arrWordType, sRelationTableName);
                }
            }
        }
        #endregion

        #region SetCheck
        public void RefreashCheck(string sRelationID)
        {
            sRelationID1 = sRelationID;
            string sSQL = "select * from " + sRelationTableName + " where RelationID1=" + sRelationID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            //初始原有的菜单权限
            ArrayList arrOldOperation = new ArrayList();
            for (int i = 0; i < dt.Rows.Count; i++)
                arrOldOperation.Add(dt.Rows[i]["RelationID2"]);
            SetAllCheck(arrOldOperation);
        }


        private void ClearAllCheck()
        {
            DataTree_TreeInfo.ResourceManager.AddScript("var nodes=" + DataTree_TreeInfo.ClientID + ".getChecked();");
            DataTree_TreeInfo.ResourceManager.AddScript("for (var i = 0; i < nodes.length; i++)");
            DataTree_TreeInfo.ResourceManager.AddScript("nodes[i].getUI().toggleCheck(false);");
        }

        public void SetAllCheck(ArrayList arrTransCheck)
        {
            if (bIsCheckTree)
            {
                ClearAllCheck();
                string s = "";
                for (int i = 0; i < arrTransCheck.Count; i++)
                {
                    SetCheck(arrTransCheck[i].ToString(), true);
                    s = s + arrTransCheck[i].ToString() + "|";
                }
                if (s != "")
                    sOldCheckListIDs = s.Remove(s.Length - 1, 1);
                else
                    sOldCheckListIDs = "";
            }
        }

        public void SetCheck(string sID, bool flag)
        {
            DataTree_TreeInfo.ResourceManager.AddScript(" var node=" + DataTree_TreeInfo.ClientID + ".getNodeById(" + sID + ");");
            if (flag)
                DataTree_TreeInfo.ResourceManager.AddScript(" if (node!=null) node.getUI().toggleCheck(true);");
            else
                DataTree_TreeInfo.ResourceManager.AddScript(" if (node!=null) node.getUI().toggleCheck(false);");
        }
        #endregion

        #region TreeNodeClick

        [DirectMethod]
        public void _TreeNodeClick(string sID)
        {
            if (sID == "")
                sTreeNowID = DataTree_Select.SelectedNode.NodeID;
            else
                sTreeNowID = sID;


            if (TreeClick != null)
                TreeClick(this, new EventArgs());
        }

        protected void TreeNodeClick(object sender, DirectEventArgs e)
        {
            _TreeNodeClick("");
        }
        #endregion

        public event EventHandler TreeClick; //在外面定义的树结点点击

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {

            }
        }
    }
}