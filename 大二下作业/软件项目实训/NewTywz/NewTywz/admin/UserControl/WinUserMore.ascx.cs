using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;

namespace NewTywz.UserControl
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "winUser")]
    public partial class WinUserMore : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return WinUserMore_TableName.Text;
            }
            set
            {
                WinUserMore_TableName.Text = value;
            }
        }

        public string sTableID
        {
            get
            {
                return WinUserMore_TableID.Text;
            }
            set
            {
                WinUserMore_TableID.Text = value;
            }
        }


        public string sRowID
        {
            get
            {
                return WinUserMore_RowID.Text;
            }
            set
            {
                WinUserMore_RowID.Text = value;
            }
        }



        public string sName
        {
            get
            {
                return WinUserMore_txtName.Text;
            }
            set
            {
                WinUserMore_txtName.Text = value;
            }
        }
        public string sRemark
        {
            get
            {
                return WinUserMore_txtRemark.Text;
            }
            set
            {
                WinUserMore_txtRemark.Text = value;
            }
        }

        #endregion


        //初始化角色面板
        private void InitStoreRoles(string sID)
        {
            string sSQL = "select UserARAuthorIID from tblUserAuthorRelation where UserARUserIID=" + sID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            string sSelIDs = "(";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    sSelIDs = sSelIDs + dt.Rows[i]["UserARAuthorIID"].ToString() + ",";
                sSelIDs = sSelIDs.Remove(sSelIDs.Length - 1, 1);
                sSelIDs = sSelIDs + ")";
            }
            else
                sSelIDs = sSelIDs + "0)";


            sSQL = "select ID,Name from tblAuthorizationInfo where DelFlag=0 and ID IN " + sSelIDs + " order by TheOrder ASC";
            dt = cprivDBO.ExcuteSqlTable(sSQL);
            object[] data = new object[0];
            if (dt.Rows.Count > 0)
            {
                data = new object[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] dataline = new object[2];
                    dataline[0] = dt.Rows[i]["ID"].ToString();
                    dataline[1] = dt.Rows[i]["Name"].ToString();
                    data[i] = dataline;
                }
            }
            WinUserMore_storeSelRoles.DataSource = data;
            WinUserMore_storeSelRoles.DataBind();

            sSQL = "select ID,Name from tblAuthorizationInfo where DelFlag=0 and ID NOT IN " + sSelIDs + " order by TheOrder ASC";
            dt = cprivDBO.ExcuteSqlTable(sSQL);
            data = new object[0];
            if (dt.Rows.Count > 0)
            {
                data = new object[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] dataline = new object[2];
                    dataline[0] = dt.Rows[i]["ID"].ToString();
                    dataline[1] = dt.Rows[i]["Name"].ToString();
                    data[i] = dataline;
                }
            }
            WinUserMore_storeAllRoles.DataSource = data;
            WinUserMore_storeAllRoles.DataBind();
        }

        private void InitBaseTab(string sID)
        {
            string sSQL = "select * from tblUserInfo where ID=" + sID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserIBirth"].ToString() == "")
                    WinUserMore_dateBirth.Clear();
                else
                    WinUserMore_dateBirth.SetValue(Convert.ToDateTime(dt.Rows[0]["UserIBirth"]).ToString("yyyy-MM-dd"));
                if (dt.Rows[0]["UserIRegisterDate"].ToString() == "")
                    WinUserMore_dateRegister.Clear();
                else
                    WinUserMore_dateRegister.SetValue(Convert.ToDateTime(dt.Rows[0]["UserIRegisterDate"]).ToString("yyyy-MM-dd"));

                WinUserMore_cmbSex.SetValue(dt.Rows[0]["UserISex"].ToString());
                WinUserMore_cmbDepartment.SetValue(dt.Rows[0]["UserIDepartmentIID"].ToString());
                WinUserMore_cmbDuty.SetValue(dt.Rows[0]["UserIDutyIID"].ToString());

                WinUserMore_txtCode.Text = dt.Rows[0]["UserICode"].ToString();
                WinUserMore_txtName.Text = dt.Rows[0]["Name"].ToString();
                WinUserMore_txtIDCard.Text = dt.Rows[0]["UserIIDCard"].ToString();

                WinUserMore_txtAddress.Text = dt.Rows[0]["UserIAddress"].ToString();
                WinUserMore_txtPostalCode.Text = dt.Rows[0]["UserIPostalCode"].ToString();
                WinUserMore_txtMobilePhone.Text = dt.Rows[0]["UserIMobilePhone"].ToString();
                WinUserMore_txtPhone.Text = dt.Rows[0]["UserIPhone"].ToString();

                WinUserMore_txtRemark.Text = dt.Rows[0]["UserIRemarks"].ToString();
                WinUserMore_txtPsd.Text = dt.Rows[0]["UserIPsd"].ToString();
                WinUserMore_txtEmail.Text = dt.Rows[0]["UserIeMail"].ToString();
            }
            else
            {
                WinUserMore_dateBirth.Value = DateTime.Now;
                WinUserMore_dateRegister.Value = DateTime.Now;
                WinUserMore_cmbSex.Clear();
                WinUserMore_cmbDepartment.SetValue("");
                WinUserMore_cmbDuty.Clear();

                WinUserMore_txtCode.Text = "";
                WinUserMore_txtName.Text = "";
                WinUserMore_txtIDCard.Text = "";

                WinUserMore_txtAddress.Text = "";
                WinUserMore_txtPostalCode.Text = "";
                WinUserMore_txtMobilePhone.Text = "";
                WinUserMore_txtPhone.Text = "";

                WinUserMore_txtRemark.Text = "";
                WinUserMore_txtPsd.Text = "";
                WinUserMore_txtEmail.Text = "";
            }
        }

        private void InitAllStore(string sID)
        {
            InitBaseTab(sID);
            InitStoreRoles(sID);
        }

        //保存角色面板信息
        private void WinUserMore_SaveRole(string s)
        {
            string[] arr = s.Split(',');

            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();
            arrWordName.Add("UserARUserIID"); arrWordValue.Add(sRowID); arrWordType.Add(ConstDataBase.INT_TYPE);
            cprivDBO.DeleteValue(arrWordName, arrWordValue, arrWordType, "tblUserAuthorRelation");
            arrWordName.Add("UserARAuthorIID"); arrWordValue.Add("0"); arrWordType.Add(ConstDataBase.INT_TYPE);
            for (int i = 0; i < arr.Length; i++)
            {
                arrWordValue[1] = arr[i];
                cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, "tblUserAuthorRelation");
            }

        }

        [DirectMethod]
        public void WinUserMore_SaveClick(string s)
        {
            if (s == "")
                X.Msg.Alert("注意", "至少得分配一个权限！").Show();
            else
            {
                ArrayList arrWordName = new ArrayList();
                ArrayList arrWordValue = new ArrayList();
                ArrayList arrWordType = new ArrayList();

                if (WinUserMore_dateBirth.SelectedDate.ToString("yyyy-MM-dd") != "0001-01-01")
                { arrWordName.Add("UserIBirth"); arrWordValue.Add(WinUserMore_dateBirth.SelectedDate.ToString("yyyy-MM-dd")); arrWordType.Add(ConstDataBase.DATETIME_TYPE); }
                if (WinUserMore_dateRegister.SelectedDate.ToString("yyyy-MM-dd") != "0001-01-01")
                { arrWordName.Add("UserIRegisterDate"); arrWordValue.Add(WinUserMore_dateRegister.SelectedDate.ToString("yyyy-MM-dd")); arrWordType.Add(ConstDataBase.DATETIME_TYPE); }

                arrWordName.Add("UserISex"); arrWordValue.Add(WinUserMore_cmbSex.SelectedItem.Value); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("UserIDepartmentIID"); arrWordValue.Add(WinUserMore_cmbDepartment.GetValue()); arrWordType.Add(ConstDataBase.INT_TYPE);


                arrWordName.Add("Name"); arrWordValue.Add(WinUserMore_txtName.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("UserIRemarks"); arrWordValue.Add(WinUserMore_txtRemark.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("UserICode"); arrWordValue.Add(WinUserMore_txtCode.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("UserIPhone"); arrWordValue.Add(WinUserMore_txtPhone.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("UserIeMail"); arrWordValue.Add(WinUserMore_txtEmail.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("UserIIDCard"); arrWordValue.Add(WinUserMore_txtIDCard.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("UserIAddress"); arrWordValue.Add(WinUserMore_txtAddress.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);
                arrWordName.Add("UserIPsd"); arrWordValue.Add(WinUserMore_txtPsd.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);

                arrWordName.Add("UserIMobilePhone"); arrWordValue.Add(WinUserMore_txtMobilePhone.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);

                arrWordName.Add("UserIPostalCode"); arrWordValue.Add(WinUserMore_txtPostalCode.Text); arrWordType.Add(ConstDataBase.STRING_TYPE);

                if (WinUserMore_cmbDuty.Text != "")
                { arrWordName.Add("UserIDutyIID"); arrWordValue.Add(WinUserMore_cmbDuty.GetValue()); arrWordType.Add(ConstDataBase.INT_TYPE); }

                if (sRowID != "0")
                    cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, "tblUserInfo", "ID=" + sRowID);
                else
                    sRowID = cprivDBO.AddValue(arrWordName, arrWordValue, arrWordType, "tblUserInfo");

                WinUserMore_winMore.Hide();
                WinUserMore_SaveRole(s);

                if (SaveClick != null)
                    SaveClick(this, new EventArgs());
            }
        }
        public event EventHandler SaveClick;
        public event EventHandler CloseClick;


        public void Show(string sID)
        {
            Show(sID, false);
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="sID">修改=userId；新增=departmentId</param>
        /// <param name="newflag">修改还是新增</param>
        public void Show(string sID, bool newflag)
        {
            if (!newflag)
            {
                InitAllStore(sID);
                sRowID = sID;
                WinUserMore_winMore.Show();
            }
            else
            {
                InitAllStore("0");
                sRowID = "0";
                WinUserMore_cmbDepartment.SetValue(sID);
                WinUserMore_winMore.Show();
            }
        }


        protected void WinUserMore_CloseClick(object sender, DirectEventArgs e)
        {
            WinUserMore_winMore.Hide();

            if (CloseClick != null)
                CloseClick(this, new EventArgs());

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                WinUserMore_cmbDuty.InitCtrl("tblDutyInfo");
                WinUserMore_cmbDepartment.InitCtrl("tblDepartmentInfo");
            }
        }

    }
}
