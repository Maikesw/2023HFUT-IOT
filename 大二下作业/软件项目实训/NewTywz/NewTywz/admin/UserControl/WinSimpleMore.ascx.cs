using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;

namespace NewTywz.UserControl
{
    public partial class WinSimpleMore : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return WinSimpleMore_TableName.Text;
            }
            set
            {
                WinSimpleMore_TableName.Text = value;
            }
        }
        public string sTableID
        {
            get
            {
                return WinSimpleMore_TableID.Text;
            }
            set
            {
                WinSimpleMore_TableID.Text = value;
            }
        }
        public string sName
        {
            get
            {
                return WinSimpleMore_txtName.Text;
            }
            set
            {
                WinSimpleMore_txtName.Text = value;
            }
        }
        public string sRemark
        {
            get
            {
                return WinSimpleMore_txtRemark.Text;
            }
            set
            {
                WinSimpleMore_txtRemark.Text = value;
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
                }
                else
                {
                    sName = "";
                    sRemark = "";
                }
            }
            else
            {
                sName = "";
                sRemark = "";
            }
            WinSimpleMore_winMore.Show();
        }


        protected void WinSimpleMore_CloseClick(object sender, DirectEventArgs e)
        {
            WinSimpleMore_winMore.Hide();
        }

        protected void WinSimpleMore_SaveClick(object sender, DirectEventArgs e)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            if (WinSimpleMore_txtName.Text == "") X.Msg.Alert("注意", "名称不可为空！").Show();
            else
            {
                arrWordName.Add("Name"); arrWordValue.Add(WinSimpleMore_txtName.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("Remark"); arrWordValue.Add(WinSimpleMore_txtRemark.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);

                if (sTableID != "")
                    cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, sTableName, Convert.ToInt32(sTableID));
                else
                {
                    object objOrder = cprivDBO.GetMaxValue("TheOrder", sTableName, " DelFlag=0");
                    int nOrder = 1;
                    if (objOrder != DBNull.Value)
                        nOrder = Convert.ToInt32(objOrder) + 1;
                    arrWordName.Add("TheOrder"); arrWordValue.Add(nOrder); arrWordType.Add(ConstDataBase.INT_TYPE);
                    cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, sTableName);
                }

                WinSimpleMore_winMore.Hide();

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