using NewTywz.basepage;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace NewTywz
{
    public partial class guestbook : WebPage
    {

        public string sChannelID;
        private int MAXBODYNUM = 6;
        private int nAllNum;
        public int nPageNow
        {
            get
            {
                return Convert.ToInt16(GBook_PageNow.Text);
            }
            set
            {
                GBook_PageNow.Text = value.ToString();
            }
        }

        private Table InitMessageBody()
        {

            Table tabMain = new Table();
            tabMain.Width = new Unit(622);
            //tabMain.CellPadding = 0;
            //tabMain.CellSpacing = 0;
            string sSQL = "select * from tblGuestBook where GuestBAnswerFlag=1 order by GuestBTime DESC";
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            nAllNum = cprivDBO.GetCount(" GuestBAnswerFlag=1", "tblGuestBook");
            nPageNow = nPageNow - 1;

            TableRow tBigRow;
            TableCell tBigCell;

            int num, i;
            i = 0;
            num = nPageNow * MAXBODYNUM;
            while ((num < dt.Rows.Count) && (i < MAXBODYNUM))
            {
                tBigRow = new TableRow();
                tBigCell = new TableCell();

                Table tab = new Table();
                //下面是第一行
                TableRow tSmallRow = new TableRow();
                TableCell tSmallCell = new TableCell();
                Label lab = new Label();
                lab.Text = "留言主题：";
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("bgColor", "#d3dabf");
                tSmallCell.Attributes.Add("width", "14%");
                tSmallCell.Attributes.Add("height", "20");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = dt.Rows[num]["GuestBHead"].ToString();
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("bgColor", "#f6f6f6");
                tSmallCell.Attributes.Add("width", "34%");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = "称    呼：";
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("bgColor", "#d3dabf");
                tSmallCell.Attributes.Add("width", "10%");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = dt.Rows[num]["GuestBName"].ToString();
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("bgColor", "#f6f6f6");
                tSmallCell.Attributes.Add("width", "20%");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = "留言日期：";
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("bgColor", "d3dabf");
                tSmallCell.Attributes.Add("width", "12%");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = Convert.ToDateTime(dt.Rows[num]["GuestBTime"]).ToString("yyyy-MM-dd");
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("bgColor", "#f6f6f6");
                tSmallCell.Attributes.Add("width", "10%");
                tSmallRow.Cells.Add(tSmallCell);

                tab.Rows.Add(tSmallRow);


                //下面是第二行
                tSmallRow = new TableRow();
                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = "留言内容：";
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("bgColor", "#d3dabf");
                tSmallCell.Attributes.Add("height", "22");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = dt.Rows[num]["GuestBText"].ToString();
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("colspan", "5");
                tSmallCell.Attributes.Add("bgColor", "#f6f6f6");
                tSmallCell.Attributes.Add("align", "left");
                tSmallRow.Cells.Add(tSmallCell);

                tab.Rows.Add(tSmallRow);

                //下面是第三行
                tSmallRow = new TableRow();

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = "管理员回复：";
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("height", "22");
                tSmallCell.Attributes.Add("bgColor", "#d3dabf");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = dt.Rows[num]["GuestBAnswer"].ToString();
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("colspan", "3");
                tSmallCell.Attributes.Add("bgColor", "#efefef");
                tSmallCell.Attributes.Add("align", "left");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = "回复时间：";
                tSmallCell.Controls.Add(lab);
                tSmallCell.Attributes.Add("bgColor", "#d3dabf");
                tSmallRow.Cells.Add(tSmallCell);

                tSmallCell = new TableCell();
                lab = new Label();
                lab.Text = Convert.ToDateTime(dt.Rows[num]["GuestBAnswerTime"]).ToString("yyyy-MM-dd");
                tSmallCell.Controls.Add(lab);
                //tSmallCell.Attributes.Add("colspan", "3");
                tSmallCell.Attributes.Add("bgColor", "#efefef");
                //tSmallCell.Attributes.Add("align", "left");
                tSmallRow.Cells.Add(tSmallCell);

                tab.Rows.Add(tSmallRow);
                //表格样式
                tab.Attributes.Add("width", "95%");
                tab.Attributes.Add("border", "0");
                tab.Attributes.Add("bgcolor", "#7a8952");
                tab.CellPadding = 3; tab.CellSpacing = 1;


                tBigCell.Controls.Add(tab);
                tBigCell.Attributes.Add("align", "center");
                tBigCell.Attributes.Add("valign", "top");
                tBigRow.Cells.Add(tBigCell);
                tabMain.Rows.Add(tBigRow);

                tBigCell = new TableCell();
                tBigRow = new TableRow();
                tBigCell.Height = 30;
                tBigRow.Cells.Add(tBigCell);
                tabMain.Rows.Add(tBigRow);

                num++; i++;

            }
            return tabMain;
        }

        public override void InitMainList(string sDisID)
        {
            TableRow tRow = new TableRow();
            TableCell tCell = new TableCell();

            tRow = new TableRow();
            tCell = new TableCell();
            tCell.Controls.Add(InitMessageBody());
            tRow.Cells.Add(tCell);
            tabBigBook.Rows.Add(tRow);

            nPageNow = nPageNow + 1;

            LeaveMessage_Page.nAllNum = nAllNum;
            LeaveMessage_Page.nPageNow = nPageNow;
            LeaveMessage_Page.MAXBODYNUM = MAXBODYNUM;
            LeaveMessage_Page.sChannelID = sDisID;
            LeaveMessage_Page.visiable = true;
            LeaveMessage_Page.InitPageToolBar();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["PageID"] == null)
                nPageNow = 1;
            else
                nPageNow = Convert.ToInt16(Request["PageID"]);

        }

        protected void LeaveMessage_btnClick(object sender, EventArgs e)
        {
            guestbook_winMsg.Show();
        }

    }
}
