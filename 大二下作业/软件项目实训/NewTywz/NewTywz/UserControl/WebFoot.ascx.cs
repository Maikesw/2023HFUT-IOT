using NewTywz.baseclass;
using System;

namespace NewTywz.UserControl
{
    /// <summary>
    /// 脚注
    /// </summary>
    public partial class WebFoot : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();


        protected void Page_Load(object sender, EventArgs e)
        {
            string sFoot = cprivDBO.GetValueFromValue(1, "ID", "MainIFootMark", "tblMainInfo");
            WebFoot_span.InnerHtml = sFoot;
        }
    }
}


