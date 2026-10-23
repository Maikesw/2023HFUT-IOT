using Ext.Net;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;
using System.IO;

namespace NewTywz.baseclass
{
    /// <summary>
    /// clsUploadFileManager，用来管理上传文件的类
    /// 创建人：石雷
    /// </summary>
    public class clsUploadFileManager
    {
        public FileUploadField fuHand;
        protected ConstAddressPath conAddPath = new ConstAddressPath();
        protected DbObject cprivDBO = new DbObject();

        public clsUploadFileManager()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        bool upFile(string sFileName)
        {
            if (this.fuHand.HasFile)
            {
                string name = this.fuHand.PostedFile.FileName;

                FileInfo file = new FileInfo(name);
                string fileName = file.Name;
                string webFilePath = sFileName;
                {
                    try
                    {
                        this.fuHand.PostedFile.SaveAs(webFilePath);
                    }
                    catch (Exception ex)
                    {
                        string errText = webFilePath + "提示：文件上传失败，失败原因：" + ex.Message;
                        return false;
                    }
                }
                return true;

            }
            return false;
        }

        public bool UploadFile(FileUploadField fuTransHand, string sFilter, string sFileName)
        {
            this.fuHand = fuTransHand;
            string sFileContentType = this.fuHand.PostedFile.ContentType;
            bool flag = false;
            int i = 0;
            if (sFilter.Length == 0)
                flag = true;
            else
            {
                string[] arrFilter = sFilter.Split('|');
                while ((i < arrFilter.Length) && (!flag))
                    if (sFileContentType == arrFilter[i])
                        flag = true;
                    else
                        i++;
            }
            if (flag)
                return upFile(sFileName);
            else
                return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fuTransHand"></param>
        /// <param name="arrFilter"></param>
        /// <param name="nType">1,doc;2,images;3,icon</param>
        /// <returns></returns>
        public bool UploadWholeFile(FileUploadField fuTransHand, string sSingleFileName, string sDBName, string sTableName, string sID, int nType)
        {
            if (fuTransHand.HasFile)
            {
                string sFilePath = "";
                string sFileName = "";
                if (nType == FileType.DOC_TYPE)
                    sFilePath = conAddPath.DOC_WEBPATH;
                else if (nType == FileType.PIC_TYPE)
                    sFilePath = conAddPath.PIC_WEBPATH;
                else if (nType == FileType.ICON_TYPE)
                    sFilePath = conAddPath.ICON_WEBPATH;

                string[] arrFileName = fuTransHand.FileName.Split('\\');
                sSingleFileName = sSingleFileName + arrFileName[arrFileName.Length - 1];
                sFileName = sFilePath + sSingleFileName;
                string sFilter = "";
                if (nType == FileType.DOC_TYPE)
                    sFilter = "application/msword";
                else if ((nType == FileType.PIC_TYPE) || (nType == FileType.ICON_TYPE))
                    sFilter = "image/pjpeg";
                if (UploadFile(fuTransHand, sFilter, sFileName))
                {
                    string sSQL = "select " + sDBName + " from " + sTableName + " where ID=" + sID;
                    DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                    if (dt.Rows.Count > 0)
                        DelOneFile(sFilePath + dt.Rows[0][sDBName].ToString());

                    ArrayList arrWordName = new ArrayList();
                    ArrayList arrWordValue = new ArrayList();
                    ArrayList arrWordType = new ArrayList();

                    arrWordName.Add(sDBName);
                    arrWordValue.Add(sSingleFileName);
                    arrWordType.Add(ConstDataBase.STRING_TYPE);

                    cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, sTableName, "ID=" + sID);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }



        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        public void DelOneFile(string sFileName)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(sFileName);
            if (file.Exists)
                file.Delete();
        }
        /// <summary>
        /// 修改一个文件名称
        /// </summary>
        /// <param name="sOldFileName">源文件名称</param>
        /// <param name="sNewFileName">新文件名称</param>
        public void RenameFile(string sOldFileName, string sNewFileName)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(sOldFileName);
            if (file.Exists)
            {
                file.CopyTo(sNewFileName);
                file.Delete();
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="sFileName">文件名称</param>
        /// <returns></returns>
        public bool HasFile(string sFileName)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(sFileName);
            return file.Exists;
        }

        /// <summary>
        /// 删除目录下的所有文件
        /// </summary>
        /// <param name="sPath">目录名称</param>
        public void DelPath(string sPath)
        {
            string[] fileList = Directory.GetFileSystemEntries(sPath);//获取目录下所有文件目录和文件
            foreach (string file in fileList)
                DelOneFile(file);
        }
    }
}