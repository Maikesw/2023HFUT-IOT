using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Xml;




namespace NewTywz.UserControl
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "WinChannelContent")]
    public partial class WinChannelContent : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();
        private clsTreeManager clsTree = new clsTreeManager();
        private XmlDocument xmlDoc;
        private ArrayList arrPicAddress = new ArrayList();
        private ArrayList arrPicName = new ArrayList();
        private clsUploadFileManager clsFile = new clsUploadFileManager();
        private ConstAddressPath conPath = new ConstAddressPath();


        #region 设定传递的一些value
        public string sTableID
        {
            get
            {
                return WinChannelContent_TableID.Text;
            }
            set
            {
                WinChannelContent_TableID.Text = value;
                if (value == "")
                {
                    WinChannelContent_btnPic.Disabled = true;
                    WinChannelContent_btnDoc.Disabled = true;
                }
                else
                {
                    WinChannelContent_btnPic.Disabled = false;
                    WinChannelContent_btnDoc.Disabled = false;
                }
            }
        }
        public string sChannelID
        {
            get
            {
                return WinChannelContent_ChannelID.Text;
            }
            set
            {
                WinChannelContent_ChannelID.Text = value;
            }
        }
        public bool bOldPop
        {
            get
            {
                if (WinChannelContent_OldPop.Text == "0")
                    return false;
                else
                    return true;
            }
            set
            {
                if (value)
                    WinChannelContent_OldPop.Text = "1";
                else
                    WinChannelContent_OldPop.Text = "0";
            }
        }
        #endregion

        #region XML

        //load xml file
        private void LoadXml()
        {
            xmlDoc = new XmlDocument();
            string s = "";
            s = @System.Web.HttpContext.Current.Server.MapPath("~/UserControl/") + "bcastr.xml";
            xmlDoc.Load(s);

            deleteNode();
        }

        private string GetImageUrl(string sText, string sUrl)
        {
            if (sText.IndexOf("<img") != -1)
            {
                sText = sText.Substring(sText.IndexOf("<img"));
                sText = sText.Substring(sText.IndexOf("src=") + 4, sText.IndexOf(".jpg") - sText.IndexOf("src="));
                return sText;
            }
            else
                return sUrl;
        }

        //添加节点
        private void AddElement()
        {
            clsTree.sTableName = "tblChannelType";
            string sSQL = "select top 5 * from tblChannelContent where ChannelCPop=1 Order by ChannelCTime DESC,ID DESC";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            LoadXml();
            XmlNode xmldocSelect = xmlDoc.SelectSingleNode("bcaster");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sSQL1 = "select * from tblChannelContentPic where RelationID=" + dt.Rows[i]["ID"].ToString() + " order by TheOrder";
                DataTable dt1 = cprivDBO.ExcuteSqlTable(sSQL1);
                if (dt1.Rows.Count > 0)
                {
                    XmlElement el = xmlDoc.CreateElement("item");
                    el.SetAttribute("link", "/" + dt.Rows[i]["ChannelCTemplete"].ToString() + "?ContentId=" + dt.Rows[i]["ID"].ToString());
                    string sURL = GetImageUrl(dt.Rows[i]["ChannelCText"].ToString(), "/attached/image/" + dt1.Rows[0]["Address"].ToString());
                    el.SetAttribute("item_url", sURL);
                    el.SetAttribute("itemtitle", dt.Rows[i]["ChannelCName"].ToString().Replace(' ', ','));
                    xmldocSelect.AppendChild(el);
                }
            }
            xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath("~/UserControl/") + "bcastr.xml");
        }

        //删节点
        private void deleteNode()
        {
            xmlDoc.SelectSingleNode("bcaster").RemoveAll();
            xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath("~/UserControl/") + "bcastr.xml");
        }



        #endregion

        #region Show
        public void Show()
        {
            Show("");
        }

        public void Show(string sID)
        {
            sTableID = sID;
            if (sID != "")
            {
                DataTable dt = cprivDBO.GetValue(sID, "tblChannelContent");
                if (dt.Rows.Count > 0)
                {
                    sChannelID = dt.Rows[0]["ChannelCChannelTID"].ToString();
                    WinChannelContent_txtName.Text = dt.Rows[0]["ChannelCName"].ToString();
                    WinChannelContent_txtAuthorName.Text = dt.Rows[0]["ChannelCAuthorName"].ToString();
                    WinChannelContent_cmbTemplete.SetValue(dt.Rows[0]["ChannelCTemplete"].ToString());
                    if (dt.Rows[0]["ChannelCTime"] != DBNull.Value)
                        WinChannelContent_FoundDate.SetValue(Convert.ToDateTime(dt.Rows[0]["ChannelCTime"]).ToString("yyyy-MM-dd"));
                    WinChannelContent_chkPop.Checked = Convert.ToBoolean(dt.Rows[0]["ChannelCPop"]);
                    bOldPop = Convert.ToBoolean(dt.Rows[0]["ChannelCPop"]);
                    WinChannelContent_chkTop.Checked = Convert.ToBoolean(dt.Rows[0]["ChannelCTop"]);
                    WinChannelContent_chkAuthor.Checked = Convert.ToBoolean(dt.Rows[0]["ChannelCAuthor"]);
                    WinChannelContent_chkSendNews.Checked = Convert.ToBoolean(dt.Rows[0]["SendNews"]);
                    //string s = dt.Rows[0]["ChannelCText"].ToString().Replace("\"", "&quot;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;").Replace("\r\n", "");
                    string s = dt.Rows[0]["ChannelCText"].ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "");
                    WinChannelContent_winMore.ResourceManager.AddScript("SetText('" + s + "');");
                }
            }
            else
            {
                WinChannelContent_FoundDate.SetValue(DateTime.Now.ToString("yyyy-MM-dd"));
                WinChannelContent_cmbTemplete.SetValue("Content3.aspx");
                WinChannelContent_txtAuthorName.Text = Session["UserName"].ToString();
                WinChannelContent_txtName.Text = "";
                WinChannelContent_chkPop.Checked = false;
                bOldPop = false;
                WinChannelContent_chkTop.Checked = false;
                WinChannelContent_chkAuthor.Checked = false;
                WinChannelContent_chkSendNews.Checked = false;
                //WinChannelContent_txtChannelText.Text = "";
                WinChannelContent_winMore.ResourceManager.AddScript("SetText('');");
            }
            string sWhere = " TheLevel=3 and ID IN (select RelationID2 from tblChannelAuthorRelation where RelationID1=" + Session["ID"].ToString() + ")";
            WinChannelContent_cmbSendNews.InitCtrl("tblChannelType", true, sWhere);

            WinChannelContent_winMore.Show();
        }
        #endregion

        #region 按钮事件相关
        protected void WinChannelContent_CloseClick(object sender, DirectEventArgs e)
        {
            WinChannelContent_winMore.Hide();
        }

        protected void WinChannelContent_PicClick(object sender, DirectEventArgs e)
        {
            if (!VerifyRole())
                return;

            WinChannelContent_winPicMore.Show(sTableID);
        }

        protected void WinChannelContent_DocClick(object sender, DirectEventArgs e)
        {
            if (!VerifyRole())
                return;

            WinChannelContent_winDocMore.Show(sTableID);
        }

        private void SaveInfo(string s)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("alt=\"\"", "");

            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            arrWordName.Add("ChannelCName"); arrWordValue.Add(WinChannelContent_txtName.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("ChannelCUserIID"); arrWordValue.Add(Session["ID"].ToString()); arrWordType.Add(ConstDataBase.INT_TYPE);
            if (WinChannelContent_FoundDate.SelectedDate.ToString("yyyy-MM-dd") != "0001-01-01")
            { arrWordName.Add("ChannelCTime"); arrWordValue.Add(WinChannelContent_FoundDate.SelectedDate.ToString("yyyy-MM-dd")); arrWordType.Add(ConstDataBase.DATETIME_TYPE); }
            arrWordName.Add("ChannelCAuthorName"); arrWordValue.Add(WinChannelContent_txtAuthorName.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            //arrWordName.Add("ChannelCText"); arrWordValue.Add(WinChannelContent_txtChannelText.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("ChannelCText"); arrWordValue.Add(s); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("ChannelCTemplete"); arrWordValue.Add(WinChannelContent_cmbTemplete.SelectedItem.Value); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("ChannelCPop"); arrWordValue.Add(WinChannelContent_chkPop.Checked); arrWordType.Add(ConstDataBase.BOOLEAN_TYPE);
            arrWordName.Add("ChannelCTop"); arrWordValue.Add(WinChannelContent_chkTop.Checked); arrWordType.Add(ConstDataBase.BOOLEAN_TYPE);
            arrWordName.Add("ChannelCAuthor"); arrWordValue.Add(WinChannelContent_chkAuthor.Checked); arrWordType.Add(ConstDataBase.BOOLEAN_TYPE);
            //if (WinChannelContent_chkSendNews.Checked)
            //{ arrWordName.Add("SendNews"); arrWordValue.Add(WinChannelContent_cmbSendNews.GetValue()); arrWordType.Add(ConstDataBase.INT_TYPE); }
            //else
            //{ arrWordName.Add("SendNews"); arrWordValue.Add(0); arrWordType.Add(ConstDataBase.INT_TYPE); }

            if (sTableID != "")
                cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, "tblChannelContent", Convert.ToInt32(sTableID));
            else
            {
                arrWordName.Add("ChannelCChannelTID"); arrWordValue.Add(sChannelID); arrWordType.Add(ConstDataBase.INT_TYPE);
                sTableID = cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, "tblChannelContent");
            }
            SavePic(s);
            if (bOldPop != WinChannelContent_chkPop.Checked)
                AddElement();
        }

        private bool VerifyRole()
        {
            if (sTableID != "")
            {
                string sUserID = cprivDBO.GetValue(sTableID, "ChannelCUserIID", "tblChannelContent").ToString();
                string[] sUserRole = Session["UserRoles"].ToString().Split(',');
                string sAdminRoleID = cprivDBO.GetValueFromValue("超级管理员", "Name", "ID", "tblAuthorizationInfo");

                if ((Session["ID"].ToString() != sUserID) && (!sUserRole.Contains(sAdminRoleID)))
                {
                    X.Msg.Alert("注意", "您没有权限修改该条内容！").Show();
                    return false;
                }
                else
                    return true;
            }
            else
                return true;
        }

        protected void WinChannelContent_PreviewClick(object sender, DirectEventArgs e)
        {
            //SaveInfo();
            clsTree.sTableName = "tblChannelType";
            WinChannelContent_winMore.AddScript("window.open('/" + WinChannelContent_cmbTemplete.SelectedItem.Value + "?id=" + sChannelID + "&ContentId=" + sTableID + "','_blank')");
        }

        //protected void WinChannelContent_SaveClick(object sender, DirectEventArgs e)
        [DirectMethod]
        public void WinChannelContent_SaveClick(string s)
        {
            if (!VerifyRole())
                return;

            SaveInfo(s);
            WinChannelContent_winMore.Hide();
            if (SaveClick != null)
                SaveClick(this, new EventArgs());
        }
        #endregion

        #region 分析校验图片
        private void SavePic(string s)
        {
            ValidatePic(s);
            DelOldPic();
            AddNewPic();
        }

        //分析获取目前的图片
        private void ValidatePic(string s)
        {
            arrPicName.Clear();
            arrPicAddress.Clear();
            int n = s.IndexOf(".jpg") + 4;
            while (n != 3)
            {
                string str = s.Substring(0, n);
                s = s.Substring(n);
                if (str.Contains("alt="))
                {
                    string str1 = str.Substring(str.LastIndexOf("alt=\"") + 5, n - str.LastIndexOf("alt=\"") - 5);
                    str1 = str1.Substring(0, str1.IndexOf("\"") - 1);
                    arrPicName.Add(str1);
                }
                else
                    arrPicName.Add("");
                str = str.Substring(str.LastIndexOf("/") + 1, n - str.LastIndexOf("/") - 1);
                arrPicAddress.Add(str);
                n = s.IndexOf(".jpg") + 4;
            }
        }

        //删除旧的图片，以及数据库记录
        private void DelOldPic()
        {
            string sSQL = "select * from tblChannelContentPic where RelationID=" + sTableID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool flag = false;
                int j = 0;
                while ((!flag) && (j < arrPicAddress.Count))
                {
                    if (dt.Rows[i]["Address"].ToString() == arrPicAddress[j].ToString())
                        flag = true;
                    else
                        j++;
                }
                if (!flag)//新的没有旧的有
                    clsFile.DelOneFile(conPath.PIC_WEBPATH + dt.Rows[i]["Address"].ToString());
                cprivDBO.DeleteValue(Convert.ToInt32(dt.Rows[i]["ID"]), "tblChannelContentPic");
            }
        }

        private void AddNewPic()
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            arrWordName.Add("RelationID"); arrWordValue.Add(sTableID); arrWordType.Add(ConstDataBase.INT_TYPE);
            arrWordName.Add("Address"); arrWordValue.Add(""); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("Name"); arrWordValue.Add(""); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("TheOrder"); arrWordValue.Add(1); arrWordType.Add(ConstDataBase.INT_TYPE);


            for (int i = 0; i < arrPicAddress.Count; i++)
            {
                arrWordValue[1] = arrPicAddress[i];
                arrWordValue[2] = arrPicName[i];
                arrWordValue[3] = i;
                cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, "tblChannelContentPic");
            }
        }
        #endregion

        public event EventHandler SaveClick;


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (!X.IsAjaxRequest))
            {
                WinChannelContent_winDocMore.sTableName = "tblChannelContentDoc";
                WinChannelContent_winPicMore.sTableName = "tblChannelContentPic";
            }
        }
    }
}