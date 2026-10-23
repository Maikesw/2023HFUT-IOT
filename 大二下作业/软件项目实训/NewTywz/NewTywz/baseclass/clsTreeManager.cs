using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;

namespace NewTywz.baseclass
{
    /// <summary>
    /// 专门用来处理树形结构表的manager
    /// 编写人：石雷
    /// </summary>
    public class clsTreeManager
    {
        private DbObject cprivDBO = new DbObject();

        public string sTableName;  //关联的数据库表名
        public string sRootName;   //根名
        public bool bNeedRoot;   //是否需要树根

        public clsTreeManager()
        {
            //
            // TODO: 在此处添加构造函数逻辑
        }


        #region 和树的初始化相关

        private void DFS(Ext.Net.TreePanel tpTransTree, Ext.Net.TreeNode tnParent, ArrayList arrTransValue, bool check, string sWhere)
        {
            string sSQL;
            int i;
            sSQL = "select * from " + sTableName + " where ParentIndex=" + tnParent.NodeID + " and DelFlag=0 ";
            if (sWhere != "")
                sSQL += " and " + sWhere;
            sSQL = sSQL + "Order By TheOrder";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (arrTransValue.Count == 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode tnTemp = new Ext.Net.TreeNode(dt.Rows[i]["Name"].ToString());
                    tnTemp.NodeID = dt.Rows[i]["ID"].ToString();
                    tnTemp.Qtip = dt.Rows[i]["Name"].ToString();
                    if (Convert.ToBoolean(dt.Rows[i]["IsLeaf"]) == false)//不是叶子节点,则继续
                        DFS(tpTransTree, tnTemp, arrTransValue, check, sWhere);
                    else
                    {
                        if (check)
                            tnTemp.Checked = Ext.Net.ThreeStateBool.False;
                    }
                    tnParent.Nodes.Add(tnTemp);
                }
            }
            else
            {
                bool bflag;
                int j;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    bflag = false;
                    j = 0;
                    while ((j < arrTransValue.Count) && (!bflag))
                    {
                        if (dt.Rows[i]["ID"].ToString() == arrTransValue[j].ToString())
                            bflag = true;
                        else
                            j++;
                    }
                    if (bflag)
                    {
                        Ext.Net.TreeNode tnTemp = new Ext.Net.TreeNode(dt.Rows[i]["Name"].ToString());
                        tnTemp.NodeID = dt.Rows[i]["ID"].ToString();
                        tnTemp.Qtip = dt.Rows[i]["Name"].ToString();
                        if (Convert.ToBoolean(dt.Rows[i]["IsLeaf"]) == false)
                            DFS(tpTransTree, tnTemp, arrTransValue, check, sWhere);
                        else
                        {
                            if (check)
                                tnTemp.Checked = Ext.Net.ThreeStateBool.False;
                        }
                        tnParent.Nodes.Add(tnTemp);
                    }
                }
            }
        }

        #region InitTree
        /// <summary>
        /// 初始化树
        /// </summary>
        /// <param name="tvTransTree">传过来的TreeView</param>
        /// <param name="arrTransValue">如果只取一部分内容，用到该数组</param>
        /// <returns>初始化好的TreeView</returns>
        public void InitTree(Ext.Net.TreePanel tpTransTree, ArrayList arrTransValue, bool check)
        {
            InitTree(tpTransTree, arrTransValue, check, "");
        }

        /// <summary>
        /// 初始化树
        /// </summary>
        /// <param name="tvTransTree">传过来的TreeView</param>
        /// <param name="arrTransValue">如果只取一部分内容，用到该数组</param>
        /// <returns>初始化好的TreeView</returns>
        /// <param name="sWhere">查询条件</param>
        public void InitTree(Ext.Net.TreePanel tpTransTree, ArrayList arrTransValue, bool check, string sWhere)
        {
            if (bNeedRoot)
            {
                Ext.Net.TreeNode tnRoot = new Ext.Net.TreeNode(sRootName);
                tnRoot.NodeID = "-1";
                DFS(tpTransTree, tnRoot, arrTransValue, check, sWhere);
                tpTransTree.Root.Clear();
                tpTransTree.Root.Add(tnRoot);
            }
            else
            {
                string sSQL = "select * from " + sTableName + " where ParentIndex=-1 and DelFlag=0";
                DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                if (dt.Rows.Count > 0)
                {
                    Ext.Net.TreeNode tnRoot = new Ext.Net.TreeNode(dt.Rows[0]["Name"].ToString());
                    tnRoot.NodeID = dt.Rows[0]["ID"].ToString();
                    tnRoot.Qtip = dt.Rows[0]["Name"].ToString();
                    DFS(tpTransTree, tnRoot, arrTransValue, check, sWhere);
                    tpTransTree.Root.Clear();
                    tpTransTree.Root.Add(tnRoot);
                }
                else
                {
                    ArrayList arrWordName = new ArrayList();
                    ArrayList arrWordValue = new ArrayList();
                    ArrayList arrWordType = new ArrayList();

                    arrWordName.Add("Name"); arrWordValue.Add(sRootName); arrWordType.Add(ConstDataBase.STRING_TYPE);
                    string sID = cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, sTableName);
                    Ext.Net.TreeNode tnRoot = new Ext.Net.TreeNode(sRootName);
                    tnRoot.NodeID = sID;
                    tnRoot.Qtip = sRootName;
                    DFS(tpTransTree, tnRoot, arrTransValue, check, sWhere);
                    tpTransTree.Root.Clear();
                    tpTransTree.Root.Add(tnRoot);
                }
            }
        }
        #endregion

        #endregion

        #region 节点的新增删除移动相关
        /// <summary>
        /// 增加一个树节点。
        /// </summary>
        /// <param name="arrName">字段名数组</param>
        /// <param name="arrValue">字段值数组</param>
        /// <param name="arrType">字段类型数组</param>
        /// <param name="sParentID">父节点</param>
        /// <returns>新增节点的ID</returns>
        public string AddTreeNode(ArrayList arrName, ArrayList arrValue, ArrayList arrType, string sParentID)
        {
            arrName.Add("TheOrder");
            arrType.Add(ConstDataBase.INT_TYPE);

            object objOrder = cprivDBO.GetMaxValue("TheOrder", sTableName, " ParentIndex=" + sParentID + " and DelFlag=0");
            if (objOrder != DBNull.Value)
            {
                int i = Convert.ToInt16(objOrder) + 1;
                if (i <= 255)
                    arrValue.Add(i);
                else
                    arrValue.Add(255);
            }
            else
                arrValue.Add(1);

            if (sParentID != "-1")
            {
                //获取父节点的level，并计算本层节点的level
                object obj = cprivDBO.GetValue(sParentID, "TheLevel", sTableName);
                if (obj.ToString() != "")
                {
                    int itemp = Convert.ToInt32(obj);
                    itemp++;

                    arrName.Add("TheLevel");
                    arrValue.Add(itemp.ToString());
                    arrType.Add(ConstDataBase.INT_TYPE);
                }
            }
            string sReturn = cprivDBO.AddValue(arrName, arrValue, arrType, sTableName);


            //对父节点相关信息进行修改
            if (sParentID != "-1")
            {
                object obj = cprivDBO.GetValue(sParentID, "ChildNum", sTableName);
                if (obj.ToString() != "")
                {
                    int itemp = Convert.ToInt32(obj);
                    itemp++;

                    ArrayList arrWordName = new ArrayList();
                    ArrayList arrWordValue = new ArrayList();
                    ArrayList arrWordType = new ArrayList();

                    arrWordName.Add("ChildNum");
                    arrWordValue.Add(itemp.ToString());
                    arrWordType.Add(ConstDataBase.INT_TYPE);

                    arrWordName.Add("IsLeaf");
                    arrWordValue.Add(false);
                    arrWordType.Add(ConstDataBase.BOOLEAN_TYPE);

                    cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, sTableName, "ID=" + sParentID);
                }
            }
            return sReturn;
        }


        /// <summary>
        /// 删除树中的一个节点。注意只能删除叶子节点。
        /// </summary>
        /// <param name="sID">删除节点编号</param>
        public void DelTreeNode(string sID)
        {
            //首先获取父节点
            string sParentIndex = cprivDBO.GetValue(sID, "ParentIndex", sTableName).ToString();
            //删除当前节点
            cprivDBO.DelValueByFlag(sID, sTableName);
            //对父节点的操作
            if ((sParentIndex != "-1") && (sParentIndex != ""))
            {
                object obj = cprivDBO.GetValue(sParentIndex, "ChildNum", sTableName);
                if (obj.ToString() != "")
                {
                    int nChildNum = Convert.ToInt32(obj);
                    nChildNum--;
                    if (nChildNum == 0)
                    {
                        ArrayList arrName = new ArrayList();
                        ArrayList arrValue = new ArrayList();
                        ArrayList arrType = new ArrayList();
                        arrName.Add("ChildNum");
                        arrValue.Add(nChildNum);
                        arrType.Add(ConstDataBase.INT_TYPE);
                        arrName.Add("IsLeaf");
                        arrValue.Add(true);
                        arrType.Add(ConstDataBase.BOOLEAN_TYPE);
                        cprivDBO.SetValue(arrName, arrValue, arrType, sTableName, Convert.ToInt32(sParentIndex));
                    }
                    else
                        cprivDBO.SetValue("ChildNum", nChildNum, sTableName, Convert.ToInt32(sParentIndex));
                }
            }
        }

        /// <summary>
        /// 改变节点的位置（order），向上移动。
        /// </summary>
        /// <param name="sID">节点ID</param>
        public void OrderUp(string sID)
        {
            string sSQL = "select * from " + sTableName + " where ParentIndex=(select ParentIndex from " + sTableName + " where ID=" + sID + ") and DelFlag=0 Order By TheOrder DESC";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                bool flag = false;
                int i = 0;
                //对当前节点的定位
                while ((i < dt.Rows.Count) && (!flag))
                {
                    if (dt.Rows[i]["ID"].ToString() == sID)
                        flag = true;
                    else
                        i++;
                }
                //找到了并且不是第一个点
                if ((flag) && i < dt.Rows.Count - 1)
                {
                    int nTemp = Convert.ToInt32(dt.Rows[i]["TheOrder"]);
                    cprivDBO.SetValue("TheOrder", Convert.ToInt32(dt.Rows[i + 1]["TheOrder"]), sTableName, Convert.ToInt32(dt.Rows[i]["ID"]));
                    cprivDBO.SetValue("TheOrder", nTemp, sTableName, Convert.ToInt32(dt.Rows[i + 1]["ID"]));
                }
            }
        }
        /// <summary>
        /// 改变节点的位置（order），向下移动。
        /// </summary>
        /// <param name="sID">节点ID</param>
        public void OrderDown(string sID)
        {
            string sSQL = "select * from " + sTableName + " where ParentIndex=(select ParentIndex from " + sTableName + " where ID=" + sID + ") and DelFlag=0 Order By TheOrder DESC";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                bool flag = false;
                int i = 0;
                //对当前节点的定位
                while ((i < dt.Rows.Count) && (!flag))
                {
                    if (dt.Rows[i]["ID"].ToString() == sID)
                        flag = true;
                    else
                        i++;
                }
                //找到了并且不是最后一个点
                if ((flag) && (i > 0))
                {
                    int nTemp = Convert.ToInt32(dt.Rows[i]["TheOrder"]);
                    cprivDBO.SetValue("TheOrder", Convert.ToInt32(dt.Rows[i - 1]["TheOrder"]), sTableName, Convert.ToInt32(dt.Rows[i]["ID"]));
                    cprivDBO.SetValue("TheOrder", nTemp, sTableName, Convert.ToInt32(dt.Rows[i - 1]["ID"]));
                }
            }
        }
        #endregion

        #region 树的一些简单操作

        /// <summary>
        /// 判断当前节点是否为叶子节点
        /// </summary>
        /// <param name="sID">当前节点ID</param>
        /// <returns>true叶子false非叶子</returns>
        public bool IsLeaf(string sID)
        {
            object obj = cprivDBO.GetValue(sID, "IsLeaf", sTableName);
            if (obj.ToString() == "")
                return false;
            else
                return Convert.ToBoolean(obj);
        }

        /// <summary>
        /// 获得当前节点的父结点ID
        /// </summary>
        /// <param name="sID">当前节点</param>
        /// <returns>父结点ID</returns>
        public string GetParent(string sID)
        {
            object obj = cprivDBO.GetValue(sID, "ParentIndex", sTableName);
            if (obj.ToString() == "")
                return "";
            else
                return obj.ToString();
        }
        /// <summary>
        /// 返回当前节点的最上层父节点
        /// </summary>
        /// <param name="sID">当前节点的ID</param>
        /// <returns>返回节点的ID</returns>
        public string GetBigParent(string sID)
        {
            string sNowID = sID;
            object obj = cprivDBO.GetValue(sID, "ParentIndex", sTableName);
            if (obj.ToString() == "")
                return "";
            else
            {
                while (obj.ToString() != "-1")
                {
                    sNowID = obj.ToString();
                    obj = cprivDBO.GetValue(obj.ToString(), "ParentIndex", sTableName);
                }
            }
            return sNowID;

        }

        /// <summary>
        /// 返回当前节点的第一个子节点ID
        /// </summary>
        /// <param name="sID">当前节点ID</param>
        /// <returns>子节点ID，如果不存在，则返回当前节点ID</returns>
        public string GetFirstLeaf(string sID)
        {
            string sSQL = "select ID from " + sTableName + " where " + "ParentIndex=" + sID + " and DelFlag=0 Order By TheOrder";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["ID"].ToString();
            else
                return sID;
        }

        #region GetAllLeaf
        private string GetSubLeaf(string sID)
        {
            string sReturn = "";
            string sSQL = "select ID,IsLeaf from " + sTableName + " where " + "ParentIndex=" + sID + " and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dt.Rows[i]["IsLeaf"]))
                    sReturn = sReturn + dt.Rows[i]["ID"].ToString() + ",";
                else
                    sReturn = sReturn + dt.Rows[i]["ID"].ToString() + "," + GetSubLeaf(dt.Rows[i]["ID"].ToString());
            }
            return sReturn;
        }

        /// <summary>
        /// 以字符串形式返回从当前节点开始的所有子节点
        /// </summary>
        /// <param name="sID">传过来的节点ID</param>
        /// <returns>包括当前节点的所有字节点组成的字符串</returns>
        public string GetAllLeaf(string sID)
        {
            return GetAllLeaf(sID, true);
        }

        /// <summary>
        /// 以字符串形式返回从当前节点开始的所有子节点
        /// </summary>
        /// <param name="sID">传过来的节点ID</param>
        /// <param name="flag">true：严格来；false：如果sID=0，则返回全部节点</param>
        /// <returns>包括当前节点的所有字节点组成的字符串</returns>
        public string GetAllLeaf(string sID, bool flag)
        {
            if (!flag)
                if (sID == "0")
                    sID = "-1";
            string sReturn = sID + "," + GetSubLeaf(sID);
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
        #endregion

        #region GetAllLeafName
        private string GetSubLeafName(string sID)
        {
            string sReturn = "";
            string sSQL = "select ID,Name from " + sTableName + " where " + "ParentIndex=" + sID + " and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
                sReturn = sReturn + "'" + dt.Rows[i]["Name"].ToString() + "'," + GetSubLeafName(dt.Rows[i]["ID"].ToString());
            return sReturn;
        }
        /// <summary>
        /// 以字符串形式返回从当前节点开始的所有子节点
        /// </summary>
        /// <param name="sID">传过来的节点ID</param>
        /// <returns>包括当前节点的所有字节点组成的字符串</returns>
        public string GetAllLeafName(string sID)
        {
            return GetAllLeafName(sID, true);
        }
        /// <summary>
        /// 以字符串形式返回从当前节点开始的所有子节点
        /// </summary>
        /// <param name="sID">传过来的节点ID</param>
        /// <param name="flag">true：严格来；false：如果sID=0，则返回全部节点</param>
        /// <returns>包括当前节点的所有字节点组成的字符串</returns>
        public string GetAllLeafName(string sID, bool flag)
        {
            if (!flag)
                if (sID == "0")
                    sID = "-1";

            string sReturn = "";
            string sSQL = "select ID,Name from " + sTableName + " where " + "ID=" + sID + " and DelFlag=0";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                sReturn = "'" + dt.Rows[0]["Name"].ToString() + "'," + GetSubLeafName(sID);
                sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            }
            return sReturn;
        }
        #endregion

        #endregion

    }
}