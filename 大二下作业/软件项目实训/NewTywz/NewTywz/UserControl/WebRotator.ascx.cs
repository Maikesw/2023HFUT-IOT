using NewTywz.baseclass;
using System;

namespace NewTywz.UserControl
{
    /// <summary>
    /// 轮转广告条
    /// </summary>
    public partial class WebRotator : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();
        private clsTreeManager clsTree = new clsTreeManager();
        public string sXMLFileName;

        public int Width
        {
            set
            {
                WebRotator_Width.Text = value.ToString();
            }
            get
            {
                return Convert.ToInt32(WebRotator_Width.Text);
            }
        }

        public int Height
        {
            set
            {
                WebRotator_Height.Text = value.ToString();
            }
            get
            {
                return Convert.ToInt32(WebRotator_Height.Text);
            }
        }






        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}