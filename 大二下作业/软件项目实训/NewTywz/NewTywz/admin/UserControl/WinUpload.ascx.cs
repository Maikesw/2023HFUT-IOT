using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace NewTywz.UserControl
{
    public partial class WinUpload : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();
        private clsUploadFileManager clsFile = new clsUploadFileManager();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return WinUpload_TableName.Text;
            }
            set
            {
                WinUpload_TableName.Text = value;
            }
        }
        public string sTableID
        {
            get
            {
                return WinUpload_TableID.Text;
            }
            set
            {
                WinUpload_TableID.Text = value;
            }
        }
        public string sRelationID
        {
            get
            {
                return WinUpload_RelationID.Text;
            }
            set
            {
                WinUpload_RelationID.Text = value;
            }
        }
        public string sFileName
        {
            get
            {
                return WinUpload_txtName.Text;
            }
            set
            {
                WinUpload_txtName.Text = value;
            }
        }
        public string sFileRemark
        {
            get
            {
                return WinUpload_txtRemark.Text;
            }
            set
            {
                WinUpload_txtRemark.Text = value;
            }
        }
        public string sRemarkText
        {
            get
            {
                return WinUpload_labRemark.Text;
            }
            set
            {
                WinUpload_labRemark.Text = value;
            }
        }
        public string sFilePath
        {
            get
            {
                return WinUpload_FilePath.Text;
            }
            set
            {
                WinUpload_FilePath.Text = value;
            }
        }
        public string sFilter
        {
            get
            {
                return WinUpload_Filter.Text;
            }
            set
            {
                WinUpload_Filter.Text = value;
            }
        }
        public bool bRemarkVisit
        {
            get
            {
                if (WinUpload_RemarkVisit.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    WinUpload_RemarkVisit.Text = "1";
                    WinUpload_winMore.Height = new Unit(150);
                    WinUpload_labRemark.Show();
                    WinUpload_txtRemark.Show();
                    winUpload_txtupFile.Y = 60;
                    WinUpload_upFile.Y = 58;
                    WinUpload_btnDownLoad.Y = 83;
                    WinUpload_btnSave.Y = 83;
                    WinUpload_btnClose.Y = 83;
                    WinUpload_winMore.UpdateContent();
                }
                else
                {
                    WinUpload_RemarkVisit.Text = "0";
                    WinUpload_winMore.Height = new Unit(120);
                    WinUpload_labRemark.Hide();
                    WinUpload_txtRemark.Hide();
                    winUpload_txtupFile.Y = 35;
                    WinUpload_upFile.Y = 33;
                    WinUpload_btnDownLoad.Y = 58;
                    WinUpload_btnSave.Y = 58;
                    WinUpload_btnClose.Y = 58;
                    WinUpload_winMore.UpdateContent();
                }
            }
        }
        public string sFileAddress
        {
            get
            {
                return WinUpload_FileAddress.Text;
            }
            set
            {
                WinUpload_FileAddress.Text = value;
            }
        }
        public int nFileType
        {
            get
            {
                return Convert.ToInt16(WinUpload_FileType.Text);
            }
            set
            {
                WinUpload_FileType.Text = value.ToString();
                ConstAddressPath path = new ConstAddressPath();
                switch (value)
                {
                    case FileType.PIC_TYPE:
                        sFilePath = path.PIC_WEBPATH;
                        sFilter = "image/pjpeg";
                        break;
                    case FileType.DOC_TYPE:
                        sFilePath = path.DOC_WEBPATH;
                        sFilter = "application/msword|application/vnd.ms-powerpoint";
                        break;
                    case FileType.ICON_TYPE:
                        sFilePath = path.PIC_WEBPATH;
                        sFilter = "image/pjpeg";
                        break;
                }
            }
        }

        #endregion

        public void Show()
        {
            Show("");
        }

        public void Show(string sID)
        {
            sTableID = sID;
            if (sID != "")
            {
                string sSQL = "select * from " + sTableName + " where ID=" + sID;
                DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                if (dt.Rows.Count > 0)
                {
                    sFileName = dt.Rows[0]["Name"].ToString();
                    sFileAddress = dt.Rows[0]["Address"].ToString();
                    if (bRemarkVisit)
                        sFileRemark = dt.Rows[0]["Remark"].ToString();
                    WinUpload_btnDownLoad.Disabled = false;
                }
            }
            else
            {
                sFileAddress = "";
                sFileName = "";
                sFileRemark = "";
                WinUpload_btnDownLoad.Disabled = true;
            }
            WinUpload_winMore.Show();
        }

        protected void WinUpload_SaveClick(object sender, DirectEventArgs e)
        {
            if (sTableID != "")
            {
                cprivDBO.SetValue("Name", sFileName, sTableName, Convert.ToInt32(sTableID));
                if (bRemarkVisit)
                    cprivDBO.SetValue("Remark", sFileRemark, sTableName, Convert.ToInt32(sTableID));
                if (WinUpload_upFile.HasFile)
                {
                    if (!clsFile.UploadFile(WinUpload_upFile, sFilter, sFilePath + sFileAddress))
                    {
                        X.Msg.Alert("错误", "上传文件出错！").Show();
                        return;
                    }
                }
            }
            else
            {
                if (WinUpload_upFile.HasFile)
                {
                    sFileAddress = DateTime.Now.ToFileTimeUtc().ToString() + WinUpload_upFile.FileName.Substring(WinUpload_upFile.FileName.Length - 4).ToLower();
                    if (clsFile.UploadFile(WinUpload_upFile, sFilter, sFilePath + sFileAddress))
                    {
                        object objOrder = cprivDBO.GetMaxValue("TheOrder", sTableName, "RelationID=" + sRelationID);
                        int nOrder = 1;
                        if (objOrder != DBNull.Value)
                            nOrder = Convert.ToInt16(objOrder) + 1;

                        ArrayList arrWordName = new ArrayList(); ArrayList arrWordValue = new ArrayList(); ArrayList arrWordType = new ArrayList();
                        arrWordName.Add("Name"); arrWordValue.Add(sFileName); arrWordType.Add(ConstDataBase.STRING_TYPE);
                        arrWordName.Add("RelationID"); arrWordValue.Add(sRelationID); arrWordType.Add(ConstDataBase.STRING_TYPE);
                        arrWordName.Add("TheOrder"); arrWordValue.Add(nOrder); arrWordType.Add(ConstDataBase.INT_TYPE);
                        arrWordName.Add("Address"); arrWordValue.Add(sFileAddress); arrWordType.Add(ConstDataBase.STRING_TYPE);
                        if (bRemarkVisit)
                        { arrWordName.Add("Remark"); arrWordValue.Add(sFileRemark); arrWordType.Add(ConstDataBase.STRING_TYPE); }
                        cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, sTableName);
                    }
                    else
                    {
                        X.Msg.Alert("错误", "文件上传失败！").Show();
                        return;
                    }
                }
                else
                {
                    X.Msg.Alert("错误", "尚未选择文件！").Show();
                    return;
                }
            }
            WinUpload_winMore.Hide();

            if (SaveClick != null)
                SaveClick(this, new EventArgs());
        }

        public event EventHandler SaveClick;

        protected void WinUpload_DownLoadClick(object sender, DirectEventArgs e)
        {
            if (clsFile.HasFile(sFilePath + sFileAddress))
            {
                System.IO.FileInfo DownloadFile = new System.IO.FileInfo(sFilePath + sFileAddress);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = sFilter;
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(sFileAddress, System.Text.Encoding.UTF8));
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }
            else
                X.Msg.Alert("错误", "文件不存在！").Show();
        }

        protected void WinUpload_CloseClick(object sender, DirectEventArgs e)
        {
            WinUpload_winMore.Hide();
        }

        public void DelOne(string sID)
        {
            object obj = cprivDBO.GetValue(sID, "Address", sTableName);
            if (obj != null)
            {
                string sAddress = obj.ToString();
                clsFile.DelOneFile(sFilePath + sAddress);
                cprivDBO.DeleteValue(sID, sTableName);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (!X.IsAjaxRequest))
            {

            }
        }
    }
}