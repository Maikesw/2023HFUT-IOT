using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;

namespace NewTywz.UserControl
{
    public partial class WinLeaveMessage : System.Web.UI.UserControl
    {

        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return WinLeaveMessage_TableName.Text;
            }
            set
            {
                WinLeaveMessage_TableName.Text = value;
            }
        }
        //public string sTableID
        //{
        //    get
        //    {
        //        return WinGBookManage_TableID.Text;
        //    }
        //    set
        //    {
        //        WinGBookManage_TableID.Text = value;
        //    }
        //}
        public string sGuestBName
        {
            get
            {
                return WinLeaveMessage_txtGuestBName.Text;
            }
            set
            {
                WinLeaveMessage_txtGuestBName.Text = value;
            }
        }
        public string sHead
        {
            get
            {
                return WinLeaveMessage_txtHead.Text;
            }
            set
            {
                WinLeaveMessage_txtHead.Text = value;
            }
        }

        //public string sTime
        //{
        //    get
        //    {
        //        return WinLeaveMessage_dateTime.Text;
        //    }
        //    set
        //    {
        //        WinLeaveMessage_dateTime.Text = value;
        //    }
        //}

        public string sMessage
        {
            get
            {
                return WinLeaveMessage_txtMsg.Text;
            }
            set
            {
                WinLeaveMessage_txtMsg.Text = value;
            }
        }

        #endregion


        public void Show()
        {
            sGuestBName = "";
            sHead = "";
            sMessage = "";

            WinLeaveMessage_winMsg.Show();
        }

        protected void WinLeaveMessage_CloseClick(object sender, DirectEventArgs e)
        {
            WinLeaveMessage_winMsg.Hide();
        }

        protected void WinLeaveMessage_SaveClick(object sender, DirectEventArgs e)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            sTableName = "tblGuestBook";

            //取当日时间
            //WinBulletinMore_dateBulletinITime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (WinLeaveMessage_txtMsg.Text == "") X.Msg.Alert("注意", "留言内容为空，请填写！").Show();
            else
            {
                arrWordName.Add("GuestBName"); arrWordValue.Add(WinLeaveMessage_txtGuestBName.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("GuestBHead"); arrWordValue.Add(WinLeaveMessage_txtHead.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("GuestBTime"); arrWordValue.Add(DateTime.Now.ToString("yyyy-MM-dd")); arrWordType.Add(ConstDataBase.DATETIME_TYPE);
                arrWordName.Add("GuestBText"); arrWordValue.Add(WinLeaveMessage_txtMsg.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);

                cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, sTableName);

                X.Msg.Alert("提示", "留言已提交，请等待回复！").Show();
                WinLeaveMessage_winMsg.Hide();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
