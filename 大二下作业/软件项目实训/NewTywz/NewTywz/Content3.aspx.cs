using NewTywz.basepage;
using System;
using System.Data;

namespace NewTywz
{
    public partial class Content3 : WebPage
    {

        public override void InitMainList(string sDisID)
        {
            string sChannelContentID = "";
            if (Request["ContentId"] != null)
                sChannelContentID = Request["ContentId"].ToString();
            else
            {
                if (Request["Id"] != null)
                {
                    string sSQL = "select * from tblChannelContent where ChannelCChannelTID=" + Request["Id"] + " and DelFlag=0 order by ID DESC";
                    DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                    if (dt.Rows.Count > 0)
                        sChannelContentID = dt.Rows[0]["ID"].ToString();
                }
                else
                    Response.Redirect("index.aspx");
            }

            tabMainList.sContentID = sChannelContentID;
            tabMainList.nType = 3;
            tabMainList.InitMain();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
