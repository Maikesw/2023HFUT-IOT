using Ext.Net;
using NewTywz.baseclass;
using NewTywz.basepage;
using System;
namespace admin.ChannelManage
{
    public partial class ChannelType : ExtPage
    {

        private DbObject cprivDBO = new DbObject();

        public override void TreeMaster_MoreInfo(string sID)
        {
            ChannelType_chkAuthor.Checked = Convert.ToBoolean(cprivDBO.GetValue(sID, "ChannelTAuthor", "tblChannelType"));
            ChannelType_winMore.Show();
        }

        protected void ChannelType_CloseClick(object sender, DirectEventArgs e)
        {
            ChannelType_winMore.Hide();
        }

        protected void ChannelType_SaveClick(object sender, DirectEventArgs e)
        {
            string sID = ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sTreeNowID;
            cprivDBO.SetValue("ChannelTAuthor", ChannelType_chkAuthor.Checked, "tblChannelType", Convert.ToInt32(sID));
            ChannelType_winMore.Hide();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sTableName = "tblChannelType";
                ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sRootName = "所有栏目";
                ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).sCodeName = "ChannelTTemplete";
                ((NewTywz.UserControl.DataTree)Master.FindControl("TreeMaster_DataTree")).BuildTree();
                ((Ext.Net.Button)Master.FindControl("TreeMaster_btnMoreInfo")).Show();
                ((Ext.Net.Label)Master.FindControl("TreeMaster_labCode")).Html = "模版名称:";
                ((NewTywz.TreeMaster)Master).SetTreePanelName("所有栏目");
                ((NewTywz.TreeMaster)Master).SetContentPanelName("栏目项详细信息");
                ((NewTywz.TreeMaster)Master).SetDataListVisiable(false);
            }
        }
    }
}
