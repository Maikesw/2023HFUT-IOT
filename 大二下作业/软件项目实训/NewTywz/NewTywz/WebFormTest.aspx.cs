using Ext.Net;
using System;

namespace NewTywz
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public int a = 0, b = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                temp.Text = a.ToString() + "|" + b.ToString();

            }
        }
        protected void btnSure_Click(object sender, DirectEventArgs e)
        {
            a = 1; b = 2;
            temp.Text = a.ToString() + "|" + b.ToString();
            X.Msg.Alert("title", temp.Text).Show();
        }
        protected void btnReset_Click(object sender, DirectEventArgs e)
        {
            X.Msg.Alert("title", temp.Text).Show();
        }
    }
}