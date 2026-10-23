using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;

namespace NewTywz
{
    public partial class WebForm11 : System.Web.UI.Page
    {

        private int a = 0;
        private int b = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hiddenValue.Text = a.ToString() + "|" + b.ToString();
            }
        }


        protected void btnSure1_Click(object sender, DirectEventArgs e)
        {
            X.Msg.Alert("确定1", "show" + hiddenValue.Text).Show();
        }
        protected void btnSure2_Click(object sender, DirectEventArgs e)
        {
            a = 1;b = 2;
            hiddenValue.Text = a.ToString() + "|" + b.ToString();
            X.Msg.Alert("确定2", "show" + hiddenValue.Text).Show();
        }


    }
}