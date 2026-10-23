using Ext.Net;
using System;

namespace NewTywz.UserControl
{
    public partial class WinNeedConfirm : System.Web.UI.UserControl
    {
        public void Show(string sTip)
        {
            WinNeedConfirm_labTip.Text = sTip;
            WinNeedConfirm_winMore.Show();
        }


        protected void WinNeedConfirm_CloseClick(object sender, DirectEventArgs e)
        {
            WinNeedConfirm_winMore.Hide();
        }

        protected void WinNeedConfirm_SureClick(object sender, DirectEventArgs e)
        {
            WinNeedConfirm_winMore.Hide();
            if (SureClick != null)
                SureClick(this, new EventArgs());
        }

        public event EventHandler SureClick;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}