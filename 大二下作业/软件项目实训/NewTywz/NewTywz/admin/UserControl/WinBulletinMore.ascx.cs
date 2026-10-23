using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;

namespace NewTywz.UserControl
{
    public partial class WinBulletinMore : System.Web.UI.UserControl
    {

        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return WinBulletinMore_TableName.Text;
            }
            set
            {
                WinBulletinMore_TableName.Text = value;
            }
        }
        public string sTableID
        {
            get
            {
                return WinBulletinMore_TableID.Text;
            }
            set
            {
                WinBulletinMore_TableID.Text = value;
            }
        }
        public string sName
        {
            get
            {
                return WinBulletinMore_txtName.Text;
            }
            set
            {
                WinBulletinMore_txtName.Text = value;
            }
        }
        public string sRemark
        {
            get
            {
                return WinBulletinMore_txtRemark.Text;
            }
            set
            {
                WinBulletinMore_txtRemark.Text = value;
            }
        }

        public string sBulletinTime
        {
            get
            {
                return WinBulletinMore_dateBulletinITime.Text;
            }
            set
            {
                WinBulletinMore_dateBulletinITime.Text = value;
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
                    sName = dt.Rows[0]["Name"].ToString();
                    sRemark = dt.Rows[0]["Remark"].ToString();
                    sBulletinTime = dt.Rows[0]["BulletinITime"].ToString();
                }
                else
                {
                    sName = "";
                    sRemark = "";
                    sBulletinTime = "";
                }
            }
            else
            {
                sName = "";
                sRemark = "";
                sBulletinTime = "";
                //取当日时间
                WinBulletinMore_dateBulletinITime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            WinBulletinMore_winMore.Show();
        }


        protected void WinBulletinMore_CloseClick(object sender, DirectEventArgs e)
        {
            WinBulletinMore_winMore.Hide();
        }

        protected void WinBulletinMore_SaveClick(object sender, DirectEventArgs e)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            //取当日时间
            //WinBulletinMore_dateBulletinITime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (WinBulletinMore_txtName.Text == "") X.Msg.Alert("注意", "标题不可为空！").Show();
            else
            {
                arrWordName.Add("Name"); arrWordValue.Add(WinBulletinMore_txtName.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("Remark"); arrWordValue.Add(WinBulletinMore_txtRemark.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("BulletinITime"); arrWordValue.Add(WinBulletinMore_dateBulletinITime.Text); arrWordType.Add(ConstDataBase.DATETIME_TYPE);

                if (sTableID != "")
                    cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, sTableName, Convert.ToInt32(sTableID));
                else
                {
                    cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, sTableName);
                }

                WinBulletinMore_winMore.Hide();

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
