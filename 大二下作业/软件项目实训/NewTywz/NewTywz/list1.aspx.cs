using NewTywz.basepage;
using System;

namespace NewTywz
{
    public partial class list1 : WebPage
    {
        private int MAXBODYNUM = 16;

        public int nPageNow
        {
            get
            {
                return Convert.ToInt16(list1_PageNow.Text);
            }
            set
            {
                list1_PageNow.Text = value.ToString();
            }
        }

        public override void InitMainList(string sDisID)
        {
            tabMain.nListNum = MAXBODYNUM;
            tabMain.nNameMaxLength = 40;
            tabMain.bNeedPageToolbar = true;
            tabMain.sChannelTypeID = sDisID;
            tabMain.nNowPageNum = nPageNow;
            tabMain.InitWebList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["PageID"] == null)
                nPageNow = 1;
            else
                nPageNow = Convert.ToInt16(Request["PageID"]);

        }
    }
}
