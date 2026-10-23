using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;

namespace NewTywz.UserControl
{
    public partial class WinGBookManage : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return WinGBookManage_TableName.Text;
            }
            set
            {
                WinGBookManage_TableName.Text = value;
            }
        }
        public string sTableID
        {
            get
            {
                return WinGBookManage_TableID.Text;
            }
            set
            {
                WinGBookManage_TableID.Text = value;
            }
        }
        public string sName
        {
            get
            {
                return WinGBookManage_txtName.Text;
            }
            set
            {
                WinGBookManage_txtName.Text = value;
            }
        }
        public string sHead
        {
            get
            {
                return WinGBookManage_txtHead.Text;
            }
            set
            {
                WinGBookManage_txtHead.Text = value;
            }
        }

        public string sTime
        {
            get
            {
                return WinGBookManage_dateTime.Text;
            }
            set
            {
                WinGBookManage_dateTime.Text = value;
            }
        }

        public string sText
        {
            get
            {
                return WinGBookManage_txtText.Text;
            }
            set
            {
                WinGBookManage_txtText.Text = value;
            }
        }

        public string sAnswer
        {
            get
            {
                return WinGBookManage_txtAnswer.Text;
            }
            set
            {
                WinGBookManage_txtAnswer.Text = value;
            }
        }

        public string sAnswerTime
        {
            get
            {
                return WinGBookManage_dateAnswerTime.Text;
            }
            set
            {
                WinGBookManage_dateAnswerTime.Text = value;
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
                DataTable dt = cprivDBO.GetValue(sID, sTableName);
                if (dt.Rows.Count > 0)
                {
                    sHead = dt.Rows[0]["GuestBHead"].ToString();
                    sTime = dt.Rows[0]["GuestBTime"].ToString();
                    sName = dt.Rows[0]["GuestBName"].ToString();
                    sText = dt.Rows[0]["GuestBText"].ToString();
                    sAnswer = dt.Rows[0]["GuestBAnswer"].ToString();
                    if (dt.Rows[0]["GuestBAnswerTime"] != DBNull.Value)
                        sAnswerTime = Convert.ToDateTime(dt.Rows[0]["GuestBAnswerTime"]).ToString("yyyy-MM-dd");
                    else
                        WinGBookManage_dateAnswerTime.Text = DateTime.Now.ToString();
                }
            }
            WinGBookManage_winMore.Show();
        }


        protected void WinGBookManage_CloseClick(object sender, DirectEventArgs e)
        {
            WinGBookManage_winMore.Hide();
        }

        protected void WinGBookManage_SaveClick(object sender, DirectEventArgs e)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            //取当日时间
            //WinBulletinMore_dateBulletinITime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (WinGBookManage_txtAnswer.Text == "")
                X.Msg.Alert("注意", "回复不可为空！").Show();
            else
            {
                arrWordName.Add("GuestBHead"); arrWordValue.Add(WinGBookManage_txtHead.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("GuestBTime"); arrWordValue.Add(WinGBookManage_dateTime.Text); arrWordType.Add(ConstDataBase.DATETIME_TYPE);
                arrWordName.Add("GuestBName"); arrWordValue.Add(WinGBookManage_txtName.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("GuestBText"); arrWordValue.Add(WinGBookManage_txtText.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("GuestBAnswer"); arrWordValue.Add(WinGBookManage_txtAnswer.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("GuestBAnswerTime"); arrWordValue.Add(WinGBookManage_dateAnswerTime.SelectedDate.ToString("yyyy-MM-dd")); arrWordType.Add(ConstDataBase.DATETIME_TYPE);
                arrWordName.Add("GuestBAnswerFlag"); arrWordValue.Add(true); arrWordType.Add(ConstDataBase.BOOLEAN_TYPE);
                cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, sTableName, Convert.ToInt32(sTableID));
                WinGBookManage_winMore.Hide();

                if (SaveClick != null)
                    SaveClick(this, new EventArgs());
            }

        }

        public event EventHandler SaveClick;


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
