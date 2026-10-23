using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;


namespace NewTywz.UserControl
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DataList")]
    public partial class DataList : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return DataList_TableName.Text;
            }
            set
            {
                DataList_TableName.Text = value;
            }
        }
        public string sViewName
        {
            get
            {
                return DataList_ViewName.Text;
            }
            set
            {
                DataList_ViewName.Text = value;
            }
        }
        private ArrayList GetArr(string trans)
        {
            ArrayList arr = new ArrayList();
            if (trans != "")
            {
                string[] s = trans.Split('|');
                for (int i = 0; i < s.Length; i++)
                    arr.Add(s[i]);
            }
            return arr;
        }
        private void SetArr(ArrayList arr, Ext.Net.Hidden hideCtrl)
        {
            string s = "";
            if (arr.Count > 0)
            {
                for (int i = 0; i < arr.Count; i++)
                    s = s + arr[i].ToString() + "|";
                s = s.Remove(s.Length - 1, 1);
            }
            hideCtrl.Text = s;

        }

        public ArrayList arrHeadName
        {
            get
            {
                return GetArr(DataList_HeadName.Text);
            }
            set
            {
                SetArr(value, DataList_HeadName);
            }
        }
        public ArrayList arrDataIndex
        {
            get
            {
                return GetArr(DataList_DataIndex.Text);
            }
            set
            {
                SetArr(value, DataList_DataIndex);
            }
        }
        public ArrayList arrDataType
        {
            get
            {
                return GetArr(DataList_DataType.Text);
            }
            set
            {
                SetArr(value, DataList_DataType);
            }
        }
        public ArrayList arrColWidth
        {
            get
            {
                return GetArr(DataList_ColWidth.Text);
            }
            set
            {
                SetArr(value, DataList_ColWidth);
            }
        }
        public bool bFirstVisit
        {
            get
            {
                if (DataList_FirstVisit.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    DataList_FirstVisit.Text = "1";
                else
                    DataList_FirstVisit.Text = "0";
            }
        }
        public bool bHasDelFlag
        {
            get
            {
                if (DataList_HasDelFlag.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    DataList_HasDelFlag.Text = "1";
                else
                    DataList_HasDelFlag.Text = "0";
            }
        }
        public bool bDelFlagValue
        {
            get
            {
                if (DataList_DelFlagValue.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    DataList_DelFlagValue.Text = "1";
                else
                    DataList_DelFlagValue.Text = "0";
            }
        }
        public string sOrderBy
        {
            get
            {
                return DataList_OrderBy.Text;
            }
            set
            {
                DataList_OrderBy.Text = value;
            }
        }
        public string sInitWhere
        {
            get
            {
                return DataList_InitWhere.Text.Replace("♀", "'").Replace("♂", "\"");
            }
            set
            {
                DataList_InitWhere.Text = value.Replace("'", "♀").Replace("\"", "♂");
            }
        }
        public string sNowWhere
        {
            get
            {
                return DataList_NowWhere.Text.Replace("♀", "'").Replace("♂", "\"");
            }
            set
            {
                DataList_NowWhere.Text = value.Replace("'", "♀").Replace("\"", "♂");
            }
        }
        public string sInitSQL
        {
            get
            {
                return DataList_InitSQL.Text.Replace("♀", "'").Replace("♂", "\"");
            }
            set
            {
                DataList_InitSQL.Text = value.Replace("'", "♀").Replace("\"", "♂");
            }
        }
        public string sNowSQL
        {
            get
            {
                return DataList_NowSQL.Text.Replace("♀", "'").Replace("♂", "\"");
            }
            set
            {
                DataList_NowSQL.Text = value.Replace("'", "♀").Replace("\"", "♂");
            }
        }
        public string sRowNowID
        {
            get
            {
                return DataList_RowNowID.Text;
            }
            set
            {
                DataList_RowNowID.Text = value;
            }
        }
        public string sRowAllID
        {
            get
            {
                return DataList_RowAllID.Text;
            }
            set
            {
                DataList_RowAllID.Text = value;
            }
        }

        #endregion

        #region 一些private
        #region setStoreField
        /// <summary>
        /// 设定/初始化一列storeField。
        /// </summary>
        /// <param name="sFieldName">field名</param>
        protected void setStoreField(string sFieldName)
        {
            setStoreField(sFieldName, ConstDataBase.STRING_TYPE);
        }

        /// <summary>
        /// 设定/初始化一列storeField。
        /// </summary>
        /// <param name="sFieldName">field名</param>
        /// <param name="rfType">field类型</param>
        protected void setStoreField(string sFieldName, int rfType)
        {
            RecordField rField = new RecordField(sFieldName);
            if ((rfType == ConstDataBase.STRING_TYPE) || (rfType == ConstDataBase.STRINGL_TYPE) || (rfType == ConstDataBase.SELSTR_TYPE) || (rfType == ConstDataBase.BOOLEAN_TYPE) || (rfType == ConstDataBase.STATUS_TYPE))
                rField.Type = RecordFieldType.String;
            else if (rfType == ConstDataBase.DATETIME_TYPE)
                rField.Type = RecordFieldType.String;
            else if (rfType == ConstDataBase.INT_TYPE)
                rField.Type = RecordFieldType.Int;
            else if (rfType == ConstDataBase.FLOAT_TYPE)
                rField.Type = RecordFieldType.Float;
            else if (rfType == ConstDataBase.BOOLEANBOX_TYPE)
                rField.Type = RecordFieldType.Boolean;
            DataList_storeInfo.AddField(rField);
        }

        #endregion

        #region setPanelColumn
        /// <summary>
        /// 设定/初始化一列。
        /// </summary>
        /// <param name="sHeadName">列名</param>
        /// <param name="sDataIndex">列的DataIndex</param>
        /// <param name="nColWidth">列宽</param>
        /// <param name="rfType">列的类型</param>
        protected void setPanelColumn(string sHeadName, string sDataIndex, int nColWidth, int rfType)
        {
            setPanelColumn(sHeadName, sDataIndex, nColWidth, rfType, true);

        }
        /// <summary>
        /// 设定/初始化一列。
        /// </summary>
        /// <param name="sHeadName">列名</param>
        /// <param name="sDataIndex">列的DataIndex</param>
        /// <param name="nColWidth">列宽</param>
        /// <param name="rfType">列的类型</param>
        /// <param name="flag">是否使用AutoExpandColumn</param>
        protected void setPanelColumn(string sHeadName, string sDataIndex, int nColWidth, int rfType, bool flag)
        {
            if ((rfType == ConstDataBase.STRING_TYPE) || (rfType == ConstDataBase.INT_TYPE) || (rfType == ConstDataBase.STRINGL_TYPE) || (rfType == ConstDataBase.SELSTR_TYPE) || (rfType == ConstDataBase.DATETIME_TYPE) || (rfType == ConstDataBase.BOOLEAN_TYPE) || (rfType == ConstDataBase.STATUS_TYPE))
            {
                Column col = new Column();
                col.Header = sHeadName;
                col.DataIndex = sDataIndex;
                col.ColumnID = sDataIndex;
                Unit width = new Unit(nColWidth);
                col.Width = width;
                DataList_gpInfo.ColumnModel.Columns.Add(col);
            }
            else if (rfType == ConstDataBase.FLOAT_TYPE)
            {
                NumberColumn col = new NumberColumn();
                col.Header = sHeadName;
                col.DataIndex = sDataIndex;
                col.ColumnID = sDataIndex;
                col.Format = "0.0000";
                Unit width = new Unit(nColWidth);
                col.Width = width;
                DataList_gpInfo.ColumnModel.Columns.Add(col);
            }
            else if (rfType == ConstDataBase.BOOLEANBOX_TYPE)
            {
                CheckColumn col = new CheckColumn();
                col.Header = sHeadName;
                col.DataIndex = sDataIndex;
                col.ColumnID = sDataIndex;
                Unit width = new Unit(35);
                col.Width = width;
                DataList_gpInfo.ColumnModel.Columns.Add(col);
            }
            if (flag)
                DataList_gpInfo.AutoExpandColumn = sDataIndex;
        }

        #endregion

        #endregion

        #region 顶部的按钮事件及其相关
        public event EventHandler ListRowClick;
        public event EventHandler ListAddClick;
        public event EventHandler ListEditClick;
        public event EventHandler ListDeleteClick;
        public event EventHandler ListResetClick;
        public event EventHandler ListSearchClick;
        public event EventHandler ListAllClick;
        public event EventHandler ListOrderUpClick;
        public event EventHandler ListOrderDownClick;
        public event EventHandler ListMore1Click;

        public void SetButtonName(int nBtn, string sName)
        {
            if (nBtn == DataListButton.btnAdd)
                DataList_btnAdd.Text = sName;
            else if (nBtn == DataListButton.btnEdit)
                DataList_btnEdit.Text = sName;
            else if (nBtn == DataListButton.btnDelete)
                DataList_btnDelete.Text = sName;
            else if (nBtn == DataListButton.btnSearch)
                DataList_btnSearch.Text = sName;
            else if (nBtn == DataListButton.btnAll)
                DataList_btnAll.Text = sName;
            else if (nBtn == DataListButton.btnReset)
                DataList_btnReset.Text = sName;
            else if (nBtn == DataListButton.btnOrderUp)
                DataList_btnOrderUp.Text = sName;
            else if (nBtn == DataListButton.btnOrderDown)
                DataList_btnOrderDown.Text = sName;
            else if (nBtn == DataListButton.btnMore1)
                DataList_btnMore1.Text = sName;
        }

        public void SetButtonIcon(int nBtn, Icon icon)
        {
            if (nBtn == DataListButton.btnAdd)
                DataList_btnAdd.Icon = icon;
            else if (nBtn == DataListButton.btnEdit)
                DataList_btnEdit.Icon = icon;
            else if (nBtn == DataListButton.btnDelete)
                DataList_btnDelete.Icon = icon;
            else if (nBtn == DataListButton.btnSearch)
                DataList_btnSearch.Icon = icon;
            else if (nBtn == DataListButton.btnAll)
                DataList_btnAll.Icon = icon;
            else if (nBtn == DataListButton.btnReset)
                DataList_btnReset.Icon = icon;
            else if (nBtn == DataListButton.btnOrderUp)
                DataList_btnOrderUp.Icon = icon;
            else if (nBtn == DataListButton.btnOrderDown)
                DataList_btnOrderDown.Icon = icon;
            else if (nBtn == DataListButton.btnMore1)
                DataList_btnMore1.Icon = icon;
        }

        public void SetButtonVisiable(int i)
        {
            SetButtonVisiable(i, true);
        }

        /// <summary>
        /// 设置gird顶部按钮是否显示
        /// </summary>
        /// <param name="i">1新增2重置3查询4全部5导入6导出7删除全部</param>
        /// <param name="flag">显示还是隐藏，默认显示</param>
        public void SetButtonVisiable(int i, bool flag)
        {
            if (i == DataListButton.btnAdd)
                DataList_btnAdd.Visible = flag;
            else if (i == DataListButton.btnReset)
                DataList_btnReset.Visible = flag;
            else if (i == DataListButton.btnSearch)
                DataList_btnSearch.Visible = flag;
            else if (i == DataListButton.btnAll)
                DataList_btnAll.Visible = flag;
            else if (i == DataListButton.btnEdit)
                DataList_btnEdit.Visible = flag;
            else if (i == DataListButton.btnDelete)
                DataList_btnDelete.Visible = flag;
            else if (i == DataListButton.btnOrderUp)
                DataList_btnOrderUp.Visible = flag;
            else if (i == DataListButton.btnOrderDown)
                DataList_btnOrderDown.Visible = flag;
            else if (i == DataListButton.btnMore1)
                DataList_btnMore1.Visible = flag;
        }

        protected void DataList_PageClick(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = DataList_gpInfo.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
            {
                sm.SelectedRows.Clear();
                sm.UpdateSelection();
            }
        }

        private void ListEdit()
        {
            RowSelectionModel sm = DataList_gpInfo.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
            {
                sRowNowID = sm.SelectedRows[sm.SelectedRows.Count - 1].RecordID;
                if (ListEditClick != null)
                    ListEditClick(this, new EventArgs());
            }
            else
                X.Msg.Alert("注意", "尚未选择任何行！").Show();

        }

        protected void DataList_DBClick(object sender, DirectEventArgs e)
        {
            ListEdit();
        }

        protected void DataList_Click(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = DataList_gpInfo.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
            {
                sRowNowID = sm.SelectedRows[sm.SelectedRows.Count - 1].RecordID;
                if (ListRowClick != null)
                    ListRowClick(this, new EventArgs());
            }
        }

        protected void DataList_AddClick(object sender, DirectEventArgs e)
        {
            if (ListAddClick != null)
                ListAddClick(this, new EventArgs());
        }

        protected void DataList_EditClick(object sender, DirectEventArgs e)
        {
            ListEdit();
        }

        protected void DataList_DeleteClick(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = DataList_gpInfo.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
                DataList_winSure.Show("您确定删除选中行吗？");
            else
                X.Msg.Alert("注意", "尚未选择任何行！").Show();
        }

        protected void DataList_SureClick(object sender, EventArgs e)
        {
            RowSelectionModel sm = DataList_gpInfo.SelectionModel.Primary as RowSelectionModel;
            if (ListDeleteClick != null)
            {
                sRowAllID = "";
                foreach (SelectedRow row in sm.SelectedRows)
                    sRowAllID = sRowAllID + row.RecordID + "|";
                sRowAllID = sRowAllID.Remove(sRowAllID.Length - 1, 1);
                ListDeleteClick(this, new EventArgs());
            }
            else
            {
                if (bHasDelFlag)
                    foreach (SelectedRow row in sm.SelectedRows)
                        cprivDBO.DelValueByFlag(row.RecordID, sTableName);
                else
                    foreach (SelectedRow row in sm.SelectedRows)
                        cprivDBO.DeleteValue(row.RecordID, sTableName);
                InitStore();
            }
        }

        protected void DataList_ResetClick(object sender, DirectEventArgs e)
        {
            if (ListResetClick != null)
                ListResetClick(this, new EventArgs());
        }

        protected void DataList_SearchClick(object sender, DirectEventArgs e)
        {
            if (ListSearchClick != null)
                ListSearchClick(this, new EventArgs());
        }

        protected void DataList_AllClick(object sender, DirectEventArgs e)
        {
            if (ListAllClick != null)
                ListAllClick(this, new EventArgs());
        }

        protected void DataList_OrderUpClick(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = DataList_gpInfo.SelectionModel.Primary as RowSelectionModel;
            if (ListOrderUpClick != null)
                ListOrderUpClick(this, new EventArgs());
            else
            {
                if (sm.SelectedRows.Count > 0)
                {
                    object obj = cprivDBO.GetValue(sm.SelectedRows[sm.SelectedRows.Count - 1].RecordID, "TheOrder", sTableName);
                    if (obj != null)
                    {
                        int nOldOrder = Convert.ToInt32(obj);
                        string sSQL = "select * from " + sTableName + " where TheOrder<" + nOldOrder;
                        if (sNowWhere != "")
                            sSQL = sSQL + " and " + sNowWhere;
                        if (bHasDelFlag)
                            sSQL = sSQL + " and DelFlag=0";
                        sSQL = sSQL + " Order By TheOrder DESC";
                        DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                        if (dt.Rows.Count > 0)
                        {
                            cprivDBO.SetValue("TheOrder", Convert.ToInt32(dt.Rows[0]["TheOrder"]), sTableName, Convert.ToInt32(sm.SelectedRows[0].RecordID));
                            cprivDBO.SetValue("TheOrder", nOldOrder, sTableName, Convert.ToInt32(dt.Rows[0]["ID"]));
                            InitStore();
                        }
                        else
                            X.Msg.Alert("注意", "已经是第一条记录了！").Show();
                    }
                }
            }
        }

        protected void DataList_OrderDownClick(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = DataList_gpInfo.SelectionModel.Primary as RowSelectionModel;
            if (ListOrderDownClick != null)
                ListOrderDownClick(this, new EventArgs());
            else
            {
                if (sm.SelectedRows.Count > 0)
                {
                    object obj = cprivDBO.GetValue(sm.SelectedRows[sm.SelectedRows.Count - 1].RecordID, "TheOrder", sTableName);
                    if (obj != null)
                    {
                        int nOldOrder = Convert.ToInt32(obj);
                        string sSQL = "select * from " + sTableName + " where TheOrder>" + nOldOrder;
                        if (sNowWhere != "")
                            sSQL = sSQL + " and " + sNowWhere;
                        if (bHasDelFlag)
                            sSQL = sSQL + " and DelFlag=0";
                        sSQL = sSQL + " Order By TheOrder ASC";
                        DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                        if (dt.Rows.Count > 0)
                        {
                            cprivDBO.SetValue("TheOrder", Convert.ToInt32(dt.Rows[0]["TheOrder"]), sTableName, Convert.ToInt32(sm.SelectedRows[0].RecordID));
                            cprivDBO.SetValue("TheOrder", nOldOrder, sTableName, Convert.ToInt32(dt.Rows[0]["ID"]));
                            InitStore();
                        }
                        else
                            X.Msg.Alert("注意", "已经是最后一条记录了！").Show();
                    }
                }
            }
        }

        protected void DataList_More1Click(object sender, DirectEventArgs e)
        {
            if (ListMore1Click != null)
                ListMore1Click(this, new EventArgs());
        }

        #endregion

        #region 初始化和InitStore
        public void InitStore()
        {
            InitStore(sNowSQL);
        }
        public void InitStore(string sSQL)
        {
            InitStore(sSQL, "");

        }
        public void InitStore(string sSQL, string sWhere)
        {
            ((RowSelectionModel)DataList_gpInfo.SelectionModel.Primary).ClearSelections();

            if (sSQL == "")
                sSQL = sNowSQL;

            if (sWhere != "")
            {
                sSQL = sInitSQL;
                sNowWhere = sWhere;
                if (sSQL.IndexOf("where") != -1)
                    sSQL = sSQL.Remove(sSQL.IndexOf("where"));
                else if (sSQL.IndexOf("Order By") != -1)
                    sSQL = sSQL.Remove(sSQL.IndexOf("Order By"));
                sSQL = sSQL + " where " + sWhere;
                if (sInitWhere != "")
                    sSQL = sSQL + " and " + sInitWhere;
                if ((bHasDelFlag) && (!bDelFlagValue))
                    sSQL = sSQL + " and DelFlag=0";
                else if ((bHasDelFlag) && (bDelFlagValue))
                    sSQL = sSQL + " and DelFlag=1";
                sSQL = sSQL + sOrderBy;
            }
            sNowSQL = sSQL;

            if (sSQL != "")
            {
                DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                object[] data = new object[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] dataline = new object[arrHeadName.Count + 1];
                    dataline[0] = dt.Rows[i]["ID"].ToString();
                    for (int j = 1; j <= arrHeadName.Count; j++)
                    {
                        string sVal = "";
                        if ((Convert.ToInt16(arrDataType[j - 1]) == ConstDataBase.DATETIME_TYPE) && (dt.Rows[i][j] != DBNull.Value))
                        {
                            sVal = Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd");
                            dataline[j] = sVal;
                        }
                        else if (Convert.ToInt16(arrDataType[j - 1]) == ConstDataBase.BOOLEAN_TYPE)
                        {
                            if (Convert.ToBoolean(dt.Rows[i][j]))
                                sVal = "是";
                            else
                                sVal = "否";
                            dataline[j] = sVal;
                        }
                        else
                            dataline[j] = dt.Rows[i][j];
                    }
                    data[i] = dataline;
                }
                DataList_storeInfo.DataSource = data;
                DataList_storeInfo.DataBind();
            }
            else
            {
                object[] data = new object[0];
                DataList_storeInfo.DataSource = data;
                DataList_storeInfo.DataBind();
            }
        }

        public void InitDataList()
        {
            if (arrHeadName.Count > 0)
            {
                if (arrDataType.Count == 0)
                {
                    ArrayList arr = new ArrayList();
                    for (int i = 0; i < arrHeadName.Count; i++)
                        arr.Add(ConstDataBase.STRING_TYPE);
                    arrDataType = arr;
                }
                if (arrColWidth.Count == 0)
                {
                    ArrayList arr = new ArrayList();
                    for (int i = 0; i < arrHeadName.Count; i++)
                        arr.Add(80);
                    arrColWidth = arr;
                }

                DataList_gpInfo.ColumnModel.Columns.Clear();
                RowNumbererColumn col = new RowNumbererColumn();
                DataList_gpInfo.ColumnModel.Columns.Add(col);
                DataList_storeInfo.RemoveFields();

                setStoreField("ID", ConstDataBase.INT_TYPE);
                string sSQL = "select ID,";
                for (int i = 0; i < arrHeadName.Count; i++)
                {
                    setPanelColumn(arrHeadName[i].ToString(), arrDataIndex[i].ToString(), Convert.ToInt16(arrColWidth[i]), Convert.ToInt16(arrDataType[i]));
                    setStoreField(arrDataIndex[i].ToString(), Convert.ToInt16(arrDataType[i]));
                    sSQL = sSQL + arrDataIndex[i].ToString() + ",";
                }
                sSQL = sSQL.Remove(sSQL.Length - 1);
                //如何处理伪删除
                sSQL = sSQL + " from " + sViewName;
                sSQL = sSQL + " where ";
                if (sInitWhere != "")
                {
                    sSQL = sSQL + sInitWhere;
                    sNowWhere = sInitSQL;
                }

                if ((bHasDelFlag) && (!bDelFlagValue))
                    sSQL = sSQL + " DelFlag=0";
                else if ((bHasDelFlag) && (bDelFlagValue))
                    sSQL = sSQL + " DelFlag=1";
                else
                    sSQL = sSQL.Remove(sSQL.Length - 7, 7);
                sSQL = sSQL + sOrderBy;
                sInitSQL = sSQL;
                if (bFirstVisit)
                    InitStore(sSQL);
            }
        }
        #endregion

        public void SetVisiable(bool flag)
        {
            if (flag)
                DataList_gpInfo.Show();
            else
                DataList_gpInfo.Hide();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}