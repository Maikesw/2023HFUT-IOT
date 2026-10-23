using NewTywz.baseclass;
using System;

namespace NewTywz.UserControl
{
    /// <summary>
    /// 显示当前位置，“您现在的位置：首页……”
    /// </summary>
    public partial class WebHeadNavigator : System.Web.UI.UserControl
    {
        private clsMenuManager clsMenu = new clsMenuManager();

        #region 设定传递的一些value
        public string sChannelTypeID
        {
            get
            {
                return WebHeadNavigator_ChannelTypeID.Text;
            }
            set
            {
                WebHeadNavigator_ChannelTypeID.Text = value;
            }
        }
        public string sChannelContentID
        {
            get
            {
                return WebHeadNavigator_ChannelContentID.Text;
            }
            set
            {
                WebHeadNavigator_ChannelContentID.Text = value;
            }
        }
        #endregion

        public void InitHeadNavigator()
        {
            InitHeadNavigator(sChannelTypeID, sChannelContentID);
        }

        public void InitHeadNavigator(string sId1, string sId2)
        {
            sChannelTypeID = sId1;
            sChannelContentID = sId2;
            HeadNavigator.InnerHtml = clsMenu.GetNavName(sId1, sId2);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}