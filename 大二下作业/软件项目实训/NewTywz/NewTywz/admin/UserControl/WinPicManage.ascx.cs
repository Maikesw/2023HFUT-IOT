using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;

namespace NewTywz.UserControl
{
    public partial class WinPicManage : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return WinPicManage_TableName.Text;
            }
            set
            {
                WinPicManage_TableName.Text = value;
                WinPicManage_PicList.sTableName = value;
                WinPicManage_winUpload.sTableName = value;
            }
        }
        public string sTableID
        {
            get
            {
                return WinPicManage_TableID.Text;
            }
            set
            {
                WinPicManage_TableID.Text = value;
            }
        }
        public string sRelationID
        {
            get
            {
                return WinPicManage_RelationID.Text;
            }
            set
            {
                WinPicManage_RelationID.Text = value;
            }
        }
        #endregion

        #region 按钮事件相关
        protected void WinPicManage_ListEditClick(object sender, EventArgs e)
        {
            WinPicManage_winUpload.Show(WinPicManage_PicList.sRowNowID);
        }

        protected void WinPicManage_ListAddClick(object sender, EventArgs e)
        {
            WinPicManage_winUpload.Show();
        }

        protected void WinPicManage_ListDeleteClick(object sender, EventArgs e)
        {
            WinPicManage_winUpload.DelOne(WinPicManage_PicList.sRowNowID);
            WinPicManage_PicList.InitStore();
        }

        protected void WinPicManage_ListMore1Click(object sender, EventArgs e)
        {
            WinPicManage_winMore.Hide();
        }

        protected void WinPicManage_UploadSaveClick(object sender, EventArgs e)
        {
            WinPicManage_PicList.InitStore();
        }
        #endregion

        private void InitDataList()
        {
            WinPicManage_PicList.bHasDelFlag = false;
            WinPicManage_PicList.sFileNameColumn = "Name";
            WinPicManage_PicList.sFileAddressColumn = "Address";

            WinPicManage_PicList.SetButtonVisiable(DataListButton.btnAdd);
            WinPicManage_PicList.SetButtonVisiable(DataListButton.btnEdit);
            WinPicManage_PicList.SetButtonVisiable(DataListButton.btnDelete);
            WinPicManage_PicList.SetButtonVisiable(DataListButton.btnOrderDown);
            WinPicManage_PicList.SetButtonVisiable(DataListButton.btnOrderUp);
            WinPicManage_PicList.SetButtonVisiable(DataListButton.btnMore1);
            WinPicManage_PicList.SetButtonName(DataListButton.btnMore1, "退出");
            WinPicManage_PicList.SetButtonIcon(DataListButton.btnMore1, Icon.Cross);
        }


        public void Show(string sID)
        {
            string sWhere = " RelationID=" + sID;
            sRelationID = sID;
            WinPicManage_winUpload.sRelationID = sID;
            WinPicManage_winUpload.bRemarkVisit = false;
            WinPicManage_winUpload.nFileType = FileType.PIC_TYPE;
            WinPicManage_PicList.InitStore(sWhere);
            WinPicManage_winMore.Show();
        }


        protected void WinPicManage_CloseClick(object sender, DirectEventArgs e)
        {
            WinPicManage_winMore.Hide();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (!X.IsAjaxRequest))
            {
                InitDataList();
                WinPicManage_winUpload.nFileType = FileType.DOC_TYPE;
            }
        }
    }
}