using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data;

namespace admin.UsualManage
{
    public partial class BaseInfo : System.Web.UI.Page
    {
        private DbObject cprivDBO = new DbObject();
        private ConstDataBase conDataBase = new ConstDataBase();
        private ConstAddressPath conAddPath = new ConstAddressPath();


        private void InitAll()
        {
            string sSQL = "select * from tblMainInfo";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count == 1)
            {
                BaseInfo_txtName.Text = dt.Rows[0]["MainIName"].ToString();
                BaseInfo_txtURL.Text = dt.Rows[0]["MainIURL"].ToString();
                BaseInfo_txtEmail.Text = dt.Rows[0]["MainIEmail"].ToString();
                BaseInfo_txtAboutUS.Text = dt.Rows[0]["MainIAboutUS"].ToString();
                BaseInfo_txtInformation.Text = dt.Rows[0]["MainIInformation"].ToString();
                BaseInfo_txtFootMark.Text = dt.Rows[0]["MainIFootMark"].ToString();
                //imgLogo.ImageUrl = conAddPath.LOGOPIC_WEBPATH;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                //if (Session["UserRoles"] == null)
                //Response.Redirect("login.aspx");
                InitAll();
            }
        }

        protected void BaseInfo_SaveClick(object sender, EventArgs e)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();


            arrWordName.Add("MainIName");
            arrWordValue.Add(BaseInfo_txtName.Text);
            arrWordType.Add(ConstDataBase.STRING_TYPE);

            arrWordName.Add("MainIURL");
            arrWordValue.Add(BaseInfo_txtURL.Text);
            arrWordType.Add(ConstDataBase.STRING_TYPE);

            arrWordName.Add("MainIEmail");
            arrWordValue.Add(BaseInfo_txtEmail.Text);
            arrWordType.Add(ConstDataBase.STRING_TYPE);

            arrWordName.Add("MainIAboutUS");
            arrWordValue.Add(BaseInfo_txtAboutUS.Text);
            arrWordType.Add(ConstDataBase.STRING_TYPE);

            arrWordName.Add("MainIInformation");
            arrWordValue.Add(BaseInfo_txtInformation.Text);
            arrWordType.Add(ConstDataBase.STRING_TYPE);

            arrWordName.Add("MainIFootMark");
            arrWordValue.Add(BaseInfo_txtFootMark.Text);
            arrWordType.Add(ConstDataBase.STRING_TYPE);

            cprivDBO.SetValue(arrWordName, arrWordValue, arrWordType, "tblMainInfo", " ID=1");
            X.Msg.Alert("成功", "　　保存完毕！　　").Show();
        }


    }
}
