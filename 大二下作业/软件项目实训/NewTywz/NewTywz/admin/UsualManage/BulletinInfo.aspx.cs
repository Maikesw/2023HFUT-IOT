using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;

namespace admin.UsualManage
{
    public partial class BulletinInfo : System.Web.UI.Page
    {
        private DbObject cprivDBO = new DbObject();


        protected void BulletinInfo_ListEditClick(object sender, EventArgs e)
        {
            BulletinInfo_winMore.Show(BulletinInfo_DataList.sRowNowID);
        }

        protected void BulletinInfo_ListAddClick(object sender, EventArgs e)
        {
            BulletinInfo_winMore.Show();
        }

        protected void BulletinInfo_MoreSave(object sender, EventArgs e)
        {
            BulletinInfo_DataList.InitStore();
        }

        private void InitDataList()
        {
            BulletinInfo_DataList.sTableName = "tblBulletinInfo";
            BulletinInfo_DataList.sViewName = BulletinInfo_DataList.sTableName;
            //设置真删除
            BulletinInfo_DataList.bHasDelFlag = false;

            ArrayList arr = new ArrayList();
            arr.Add("发布时间"); arr.Add("标题"); arr.Add("内容");
            BulletinInfo_DataList.arrHeadName = arr;
            arr.Clear();
            arr.Add("BulletinITime"); arr.Add("Name"); arr.Add("Remark");
            BulletinInfo_DataList.arrDataIndex = arr;
            arr.Clear();
            arr.Add(ConstDataBase.DATETIME_TYPE); arr.Add(ConstDataBase.STRING_TYPE); arr.Add(ConstDataBase.STRING_TYPE);
            BulletinInfo_DataList.arrDataType = arr;
            arr.Clear();
            arr.Add(130); arr.Add(490); arr.Add(200);
            BulletinInfo_DataList.arrColWidth = arr;

            BulletinInfo_DataList.InitDataList();

            BulletinInfo_DataList.SetButtonVisiable(DataListButton.btnAdd);
            BulletinInfo_DataList.SetButtonVisiable(DataListButton.btnEdit);
            BulletinInfo_DataList.SetButtonVisiable(DataListButton.btnDelete);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (!X.IsAjaxRequest))
            {
                InitDataList();
                BulletinInfo_winMore.sTableName = BulletinInfo_DataList.sTableName;
            }
        }
    }
}
