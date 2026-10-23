using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;

namespace NewTywz.UserControl
{
    public partial class WinDocManage : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return WinDocManage_TableName.Text;
            }
            set
            {
                WinDocManage_TableName.Text = value;
                WinDocManage_DataList.sTableName = value;
                WinDocManage_DataList.sViewName = value;
                WinDocManage_winUpload.sTableName = value;
            }
        }
        public string sTableID
        {
            get
            {
                return WinDocManage_TableID.Text;
            }
            set
            {
                WinDocManage_TableID.Text = value;
            }
        }
        public string sRelationID
        {
            get
            {
                return WinDocManage_RelationID.Text;
            }
            set
            {
                WinDocManage_RelationID.Text = value;
            }
        }
        #endregion

        #region 按钮事件相关
        protected void WinDocManage_ListEditClick(object sender, EventArgs e)
        {
            WinDocManage_winUpload.Show(WinDocManage_DataList.sRowNowID);
        }

        protected void WinDocManage_ListAddClick(object sender, EventArgs e)
        {
            WinDocManage_winUpload.Show();
        }

        protected void WinDocManage_ListDeleteClick(object sender, EventArgs e)
        {
            string[] arrID = WinDocManage_DataList.sRowAllID.Split('|');
            for (int i = 0; i < arrID.Length; i++)
                WinDocManage_winUpload.DelOne(arrID[i]);
            WinDocManage_DataList.InitStore();
        }

        protected void WinDocManage_ListMore1Click(object sender, EventArgs e)
        {
            WinDocManage_winMore.Hide();
        }

        protected void WinDocManage_UploadSaveClick(object sender, EventArgs e)
        {
            WinDocManage_DataList.InitStore();
        }
        #endregion

        private void InitDataList()
        {
            WinDocManage_DataList.sOrderBy = " Order By TheOrder";
            WinDocManage_DataList.bHasDelFlag = false;
            WinDocManage_DataList.bFirstVisit = false;
            ArrayList arr = new ArrayList();
            arr.Add("文档标题");
            WinDocManage_DataList.arrHeadName = arr;
            arr.Clear();
            arr.Add("Name");
            WinDocManage_DataList.arrDataIndex = arr;
            WinDocManage_DataList.InitDataList();

            WinDocManage_DataList.SetButtonVisiable(DataListButton.btnAdd);
            WinDocManage_DataList.SetButtonVisiable(DataListButton.btnEdit);
            WinDocManage_DataList.SetButtonVisiable(DataListButton.btnDelete);
            WinDocManage_DataList.SetButtonVisiable(DataListButton.btnOrderDown);
            WinDocManage_DataList.SetButtonVisiable(DataListButton.btnOrderUp);
            WinDocManage_DataList.SetButtonVisiable(DataListButton.btnMore1);
            WinDocManage_DataList.SetButtonName(DataListButton.btnMore1, "退出");
            WinDocManage_DataList.SetButtonIcon(DataListButton.btnMore1, Icon.Cross);
        }


        public void Show(string sID)
        {
            string sWhere = " RelationID=" + sID;
            sRelationID = sID;
            WinDocManage_winUpload.sRelationID = sID;
            WinDocManage_winUpload.bRemarkVisit = false;
            WinDocManage_winUpload.nFileType = FileType.DOC_TYPE;
            WinDocManage_DataList.InitStore(WinDocManage_DataList.sInitSQL, sWhere);
            WinDocManage_winMore.Show();
        }


        protected void WinDocManage_CloseClick(object sender, DirectEventArgs e)
        {
            WinDocManage_winMore.Hide();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (!X.IsAjaxRequest))
            {
                InitDataList();
                WinDocManage_winUpload.nFileType = FileType.DOC_TYPE;
            }
        }
    }
}