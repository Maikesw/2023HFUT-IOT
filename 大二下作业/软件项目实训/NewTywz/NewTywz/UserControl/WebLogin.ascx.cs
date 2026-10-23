using Ext.Net;
using NewTywz.baseclass;
using System;

namespace NewTywz.UserControl
{
    /// <summary>
    /// 登陆小窗口
    /// </summary>
    public partial class WebLogin : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();
        private clsUserManager objUserManager = new clsUserManager();


        protected void LoginClick(object sender, DirectEventArgs e)
        {
            if (Request.Cookies["CheckCode"] == null)
            {
                X.Msg.Alert("注意", "您的浏览器设置已被禁用 Cookies，您必须设置浏览器允许使用 Cookies 选项后才能使用本系统。").Show();
                return;
            }

            if (String.Compare(Request.Cookies["CheckCode"].Value, WebLogin_ValidatePass.Text, true) != 0)
            {
                X.Msg.Alert("注意", "验证码错误，请输入正确的验证码。").Show();
                return;
            }

            string str = objUserManager.UserLogin(WebLogin_RegisterName.Text.Trim(), WebLogin_RegisterPass.Text.Trim());
            if (str == "登录成功！")
                Response.Redirect(Request.FilePath);
            else
                X.Msg.Alert("错误", str).Show();

        }

        private void InitCell2()
        {
            if (Session["ID"] != null)
            {
                labWelcome.Html = "您好，<br />";
                labWelcome.Html = labWelcome.Html + Session["UserName"].ToString() + "，<br />";
                labWelcome.Html = labWelcome.Html + "欢迎登录。";
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ID"] != null)
            {
                cell1.Visible = false;
                cell2.Visible = true;
                InitCell2();
            }
            else
            {
                cell1.Visible = true;
                cell2.Visible = false;
            }
        }
    }
}