using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections.Generic;
using System.Data;


namespace NewTywz.UserControl
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "PicList")]
    public partial class PicList : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();
        private ConstAddressPath conAddPath = new ConstAddressPath();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return PicList_TableName.Text;
            }
            set
            {
                PicList_TableName.Text = value;
            }
        }
        public string sFileNameColumn
        {
            get
            {
                return PicList_FileNameColumn.Text;
            }
            set
            {
                PicList_FileNameColumn.Text = value;
            }
        }
        public string sFileAddressColumn
        {
            get
            {
                return PicList_FileAddressColumn.Text;
            }
            set
            {
                PicList_FileAddressColumn.Text = value;
            }
        }

        public bool bHasDelFlag
        {
            get
            {
                if (PicList_HasDelFlag.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    PicList_HasDelFlag.Text = "1";
                else
                    PicList_HasDelFlag.Text = "0";
            }
        }

        public string sOrderBy
        {
            get
            {
                return PicList_OrderBy.Text;
            }
            set
            {
                PicList_OrderBy.Text = value;
            }
        }
        public string sInitWhere
        {
            get
            {
                return PicList_InitWhere.Text.Replace("♀", "'").Replace("♂", "\"");
            }
            set
            {
                PicList_InitWhere.Text = value.Replace("'", "♀").Replace("\"", "♂");
            }
        }
        public string sNowWhere
        {
            get
            {
                return PicList_NowWhere.Text.Replace("♀", "'").Replace("♂", "\"");
            }
            set
            {
                PicList_NowWhere.Text = value.Replace("'", "♀").Replace("\"", "♂");
            }
        }
        public string sInitSQL
        {
            get
            {
                return PicList_InitSQL.Text.Replace("♀", "'").Replace("♂", "\"");
            }
            set
            {
                PicList_InitSQL.Text = value.Replace("'", "♀").Replace("\"", "♂");
            }
        }
        public string sNowSQL
        {
            get
            {
                return PicList_NowSQL.Text.Replace("♀", "'").Replace("♂", "\"");
            }
            set
            {
                PicList_NowSQL.Text = value.Replace("'", "♀").Replace("\"", "♂");
            }
        }
        public string sRowNowID
        {
            get
            {
                return PicList_RowNowID.Text;
            }
            set
            {
                PicList_RowNowID.Text = value;
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
            PicList_storeInfo.AddField(rField);
        }

        #endregion



        #endregion

        #region 顶部的按钮事件及其相关
        public event EventHandler ListRowClick;
        public event EventHandler ListAddClick;
        public event EventHandler ListEditClick;
        public event EventHandler ListDeleteClick;
        public event EventHandler ListOrderUpClick;
        public event EventHandler ListOrderDownClick;
        public event EventHandler ListMore1Click;

        public void SetButtonName(int nBtn, string sName)
        {
            if (nBtn == DataListButton.btnAdd)
                PicList_btnAdd.Text = sName;
            else if (nBtn == DataListButton.btnEdit)
                PicList_btnEdit.Text = sName;
            else if (nBtn == DataListButton.btnDelete)
                PicList_btnDelete.Text = sName;
            else if (nBtn == DataListButton.btnOrderUp)
                PicList_btnOrderUp.Text = sName;
            else if (nBtn == DataListButton.btnOrderDown)
                PicList_btnOrderDown.Text = sName;
            else if (nBtn == DataListButton.btnMore1)
                PicList_btnMore1.Text = sName;
        }

        public void SetButtonIcon(int nBtn, Icon icon)
        {
            if (nBtn == DataListButton.btnAdd)
                PicList_btnAdd.Icon = icon;
            else if (nBtn == DataListButton.btnEdit)
                PicList_btnEdit.Icon = icon;
            else if (nBtn == DataListButton.btnDelete)
                PicList_btnDelete.Icon = icon;
            else if (nBtn == DataListButton.btnOrderUp)
                PicList_btnOrderUp.Icon = icon;
            else if (nBtn == DataListButton.btnOrderDown)
                PicList_btnOrderDown.Icon = icon;
            else if (nBtn == DataListButton.btnMore1)
                PicList_btnMore1.Icon = icon;
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
                PicList_btnAdd.Visible = flag;
            else if (i == DataListButton.btnEdit)
                PicList_btnEdit.Visible = flag;
            else if (i == DataListButton.btnDelete)
                PicList_btnDelete.Visible = flag;
            else if (i == DataListButton.btnOrderUp)
                PicList_btnOrderUp.Visible = flag;
            else if (i == DataListButton.btnOrderDown)
                PicList_btnOrderDown.Visible = flag;
            else if (i == DataListButton.btnMore1)
                PicList_btnMore1.Visible = flag;
        }

        private void ListEdit()
        {
            if (PicList_dvInfo.SelectedRecordID != "")
            {
                sRowNowID = PicList_dvInfo.SelectedRecordID;
                if (ListEditClick != null)
                    ListEditClick(this, new EventArgs());
            }
            else
                X.Msg.Alert("注意", "尚未选择任何行！").Show();

        }

        protected void PicList_DBClick(object sender, DirectEventArgs e)
        {
            ListEdit();
        }

        protected void PicList_Click(object sender, DirectEventArgs e)
        {
            if (PicList_dvInfo.SelectedRecordID != "")
            {
                sRowNowID = PicList_dvInfo.SelectedRecordID;
                if (ListRowClick != null)
                    ListRowClick(this, new EventArgs());
            }
        }

        protected void PicList_AddClick(object sender, DirectEventArgs e)
        {
            if (ListAddClick != null)
                ListAddClick(this, new EventArgs());
        }

        protected void PicList_EditClick(object sender, DirectEventArgs e)
        {
            ListEdit();
        }

        protected void PicList_DeleteClick(object sender, DirectEventArgs e)
        {
            if (PicList_dvInfo.SelectedRecordID != "")
                PicList_winSure.Show("您确定删除选中的图片吗？");
            else
                X.Msg.Alert("注意", "尚未选择任何图片！").Show();
        }

        protected void PicList_SureClick(object sender, EventArgs e)
        {
            if (ListDeleteClick != null)
                ListDeleteClick(this, new EventArgs());
            else
            {
                if (bHasDelFlag)
                    cprivDBO.DelValueByFlag(PicList_dvInfo.SelectedRecordID, sTableName);
                else
                    cprivDBO.DeleteValue(PicList_dvInfo.SelectedRecordID, sTableName);
                InitStore();
            }
        }

        protected void PicList_OrderUpClick(object sender, DirectEventArgs e)
        {
            if (ListOrderUpClick != null)
                ListOrderUpClick(this, new EventArgs());
            else
            {
                if (PicList_dvInfo.SelectedRecordID != "")
                {
                    object obj = cprivDBO.GetValue(PicList_dvInfo.SelectedRecordID, "TheOrder", sTableName);
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
                            cprivDBO.SetValue("TheOrder", Convert.ToInt32(dt.Rows[0]["TheOrder"]), sTableName, Convert.ToInt32(PicList_dvInfo.SelectedRecordID));
                            cprivDBO.SetValue("TheOrder", nOldOrder, sTableName, Convert.ToInt32(dt.Rows[0]["ID"]));
                            InitStore();
                        }
                        else
                            X.Msg.Alert("注意", "已经是第一条记录了！").Show();
                    }
                }
                else
                    X.Msg.Alert("注意", "清先选中要操作的图片！").Show();
            }
        }

        protected void PicList_OrderDownClick(object sender, DirectEventArgs e)
        {
            if (ListOrderDownClick != null)
                ListOrderDownClick(this, new EventArgs());
            else
            {
                if (PicList_dvInfo.SelectedRecordID != "")
                {
                    object obj = cprivDBO.GetValue(PicList_dvInfo.SelectedRecordID, "TheOrder", sTableName);
                    if (obj != null)
                    {
                        int nOldOrder = Convert.ToInt32(obj);
                        string sSQL = "select * from " + sTableName + " where TheOrder>" + nOldOrder;
                        if (sNowWhere != "")
                            sSQL = sSQL + " and " + sNowWhere;
                        if (bHasDelFlag)
                            sSQL = sSQL + " and DelFlag=0";
                        sSQL = sSQL + " Order By TheOrder DESC";
                        DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                        if (dt.Rows.Count > 0)
                        {
                            cprivDBO.SetValue("TheOrder", Convert.ToInt32(dt.Rows[0]["TheOrder"]), sTableName, Convert.ToInt32(PicList_dvInfo.SelectedRecordID));
                            cprivDBO.SetValue("TheOrder", nOldOrder, sTableName, Convert.ToInt32(dt.Rows[0]["ID"]));
                            InitStore();
                        }
                        else
                            X.Msg.Alert("注意", "已经是最后一条记录了！").Show();
                    }
                }
                else
                    X.Msg.Alert("注意", "清先选中要操作的图片！").Show();
            }
        }

        protected void PicList_More1Click(object sender, DirectEventArgs e)
        {
            if (ListMore1Click != null)
                ListMore1Click(this, new EventArgs());
        }

        #endregion

        #region 初始化和InitStore
        public void InitStore()
        {
            InitStore("");
        }
        public void InitStore(string sWhere)
        {
            if (sNowSQL == "")
            {
                sNowSQL = "select * from " + sTableName;
                if (bHasDelFlag)
                    sNowSQL = sNowSQL + " where DelFlag=0";
                sNowSQL = sNowSQL + sOrderBy;
            }
            string sSQL = sNowSQL;
            if (sSQL != "")
            {
                if (sWhere != "")
                {
                    sNowWhere = sWhere;
                    if (sSQL.IndexOf("where") != -1)
                        sSQL = sSQL.Remove(sSQL.IndexOf("where"));
                    else if (sSQL.IndexOf("Order By") != -1)
                        sSQL = sSQL.Remove(sSQL.IndexOf("Order By"));
                    sSQL = sSQL + " where " + sWhere;
                    if (sInitWhere != "")
                        sSQL = sSQL + " and " + sInitWhere;
                    if (bHasDelFlag)
                        sSQL = sSQL + " and DelFlag=0";
                    sSQL = sSQL + sOrderBy;
                }
                sNowSQL = sSQL;
                DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                List<object> data = new List<object>(dt.Rows.Count);
                Random dm = new Random();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string fileName = conAddPath.PIC_WEBPATH + dt.Rows[i][sFileAddressColumn].ToString();
                    System.IO.FileInfo fi = new System.IO.FileInfo(fileName);

                    if (fi.Exists)
                        data.Add(new
                        {
                            id = dt.Rows[i]["ID"].ToString(),
                            name = dt.Rows[i][sFileNameColumn].ToString(),
                            url = conAddPath.PIC_PATH + dt.Rows[i][sFileAddressColumn].ToString() + "?tmp=" + dm.Next(1000),
                            size = fi.Length,
                            lastmod = fi.LastAccessTime
                        });
                }
                PicList_storeInfo.DataSource = data;
                PicList_storeInfo.DataBind();
            }
            else
            {
                object[] data = new object[0];
                PicList_storeInfo.DataSource = data;
                PicList_storeInfo.DataBind();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}