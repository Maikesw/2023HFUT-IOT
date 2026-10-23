using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;

namespace NewTywz.UserControl
{
    public partial class WinDepartmentMore : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value

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
                return WinDepartmentMore_txtName.Text;
            }
            set
            {
                WinDepartmentMore_txtName.Text = value;
            }
        }
        public string sRemark
        {
            get
            {
                return WinDepartmentMore_txtRemark.Text;
            }
            set
            {
                WinDepartmentMore_txtRemark.Text = value;
            }
        }

        #endregion


        public void Show(string sID)
        {
            sTableID = sID;
            DataTable dt = cprivDBO.GetValue(sID, "tblDepartmentInfo");
            if (dt.Rows.Count > 0)
            {
                WinDepartmentMore_txtName.Text = dt.Rows[0]["Name"].ToString();
                WinDepartmentMore_txtCode.Text = dt.Rows[0]["DepartmentICode"].ToString();
                WinDepartmentMore_txtPhone.Text = dt.Rows[0]["DepartmentIPhone"].ToString();
                WinDepartmentMore_txtEmail.Text = dt.Rows[0]["DepartmentIeMail"].ToString();
                WinDepartmentMore_txtFax.Text = dt.Rows[0]["DepartmentIFax"].ToString();
                WinDepartmentMore_txtAddress.Text = dt.Rows[0]["DepartmentIADD"].ToString();
                WinDepartmentMore_txtPostalCode.Text = dt.Rows[0]["DepartmentIPostalCode"].ToString();
                WinDepartmentMore_txtRemark.Text = dt.Rows[0]["Remark"].ToString();
            }
            WinDepartmentMore_winMore.Show();
        }


        protected void WinDepartmentMore_CloseClick(object sender, DirectEventArgs e)
        {
            WinDepartmentMore_winMore.Hide();
        }

        protected void WinDepartmentMore_SaveClick(object sender, DirectEventArgs e)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            arrWordName.Add("Name"); arrWordValue.Add(WinDepartmentMore_txtName.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("Remark"); arrWordValue.Add(WinDepartmentMore_txtRemark.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("DepartmentICode"); arrWordValue.Add(WinDepartmentMore_txtCode.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("DepartmentIPhone"); arrWordValue.Add(WinDepartmentMore_txtPhone.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("DepartmentIeMail"); arrWordValue.Add(WinDepartmentMore_txtEmail.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("DepartmentIFax"); arrWordValue.Add(WinDepartmentMore_txtFax.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("DepartmentIADD"); arrWordValue.Add(WinDepartmentMore_txtAddress.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
            arrWordName.Add("DepartmentIPostalCode"); arrWordValue.Add(WinDepartmentMore_txtPostalCode.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);

            cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, "tblDepartmentInfo", Convert.ToInt32(sTableID));

            WinDepartmentMore_winMore.Hide();

            if (SaveClick != null)
                SaveClick(this, new EventArgs());
        }

        public event EventHandler SaveClick;


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}