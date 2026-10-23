using Ext.Net;
using NewTywz.baseclass;
using System;

//系统首页面，即登陆窗口
//编写人：石雷

namespace admin
{
    public partial class login : System.Web.UI.Page
    {
        private clsUserManager objUserManager = new clsUserManager();
        private DbObject cprivDBO = new DbObject();
        private clsSessionManager objSessionManager = new clsSessionManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                objSessionManager.UserExit();
        }


        [DirectMethod]
        public void InputPass()
        {
            string str = objUserManager.UserLogin(Login_txtUserName.Text.Trim(), Login_txtPassWord.Text.Trim());
            if (str == "登录成功！")
                Response.Redirect("main.aspx");
            else
                X.Msg.Alert("错误", str).Show();
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSure_Click(object sender, DirectEventArgs e)
        {            
            InputPass();  
        }


        /// <summary>
        /// 重置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, DirectEventArgs e)
        {
            Login_txtPassWord.Text = "";
            Login_txtUserName.Text = "";
        }
    }
}