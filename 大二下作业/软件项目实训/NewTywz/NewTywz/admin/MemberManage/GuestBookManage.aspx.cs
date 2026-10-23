using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;

namespace admin.MemberManage
{
    public partial class GuestBookManage : System.Web.UI.Page
    {
        private DbObject cprivDBO = new DbObject();


        protected void GBookManage_ListEditClick(object sender, EventArgs e)
        {
            GBookManage_winMore.Show(GBookManage_DataList.sRowNowID);
        }

        protected void GBookManage_ListAllClick(object sender, EventArgs e)
        {
            GBookManage_DataList.bFirstVisit = true;
            GBookManage_DataList.InitDataList();
        }

        protected void GBookManage_ListMore1Click(object sender, EventArgs e)
        {
            GBookManage_DataList.InitStore(GBookManage_DataList.sInitSQL, " GuestBAnswerFlag=0");
        }

        protected void GBookManage_MoreSave(object sender, EventArgs e)
        {
            GBookManage_DataList.InitStore();
        }

        private void InitDataList()
        {
            GBookManage_DataList.sTableName = "tblGuestBook";
            GBookManage_DataList.sViewName = GBookManage_DataList.sTableName;
            //设置真删除
            GBookManage_DataList.bHasDelFlag = false;
            GBookManage_DataList.bFirstVisit = false;

            ArrayList arr = new ArrayList();
            arr.Add("留言时间"); arr.Add("留言者"); arr.Add("标题"); arr.Add("内容"); arr.Add("回复时间"); arr.Add("回复内容");
            GBookManage_DataList.arrHeadName = arr;
            arr.Clear();
            arr.Add("GuestBTime"); arr.Add("GuestBName"); arr.Add("GuestBHead"); arr.Add("GuestBText"); arr.Add("GuestBAnswerTime"); arr.Add("GuestBAnswer");
            GBookManage_DataList.arrDataIndex = arr;
            arr.Clear();
            arr.Add(ConstDataBase.DATETIME_TYPE); arr.Add(ConstDataBase.STRING_TYPE); arr.Add(ConstDataBase.STRING_TYPE); arr.Add(ConstDataBase.STRING_TYPE); arr.Add(ConstDataBase.DATETIME_TYPE); arr.Add(ConstDataBase.STRING_TYPE);
            GBookManage_DataList.arrDataType = arr;

            arr.Clear();
            arr.Add(80); arr.Add(80); arr.Add(150); arr.Add(300); arr.Add(80); arr.Add(300);
            GBookManage_DataList.arrColWidth = arr;

            GBookManage_DataList.InitDataList();

            GBookManage_DataList.SetButtonVisiable(DataListButton.btnMore1);
            GBookManage_DataList.SetButtonVisiable(DataListButton.btnAll);
            GBookManage_DataList.SetButtonVisiable(DataListButton.btnDelete);
            GBookManage_DataList.SetButtonVisiable(DataListButton.btnEdit);
            GBookManage_DataList.SetButtonName(DataListButton.btnAll, "显示全部");
            GBookManage_DataList.SetButtonIcon(DataListButton.btnAll, Icon.BookOpen);
            GBookManage_DataList.SetButtonName(DataListButton.btnEdit, "回复");
            GBookManage_DataList.SetButtonIcon(DataListButton.btnEdit, Icon.PageEdit);
            GBookManage_DataList.SetButtonName(DataListButton.btnMore1, "显示待回复");
            GBookManage_DataList.SetButtonIcon(DataListButton.btnMore1, Icon.PageGo);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (!X.IsAjaxRequest))
            {
                InitDataList();
                GBookManage_winMore.sTableName = GBookManage_DataList.sTableName;
            }
        }
    }
}
