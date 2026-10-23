using Ext.Net;
using NewTywz.baseclass;
using NewTywz.ConstValue;
using System;
using System.Collections;

//简单窗口的管理
//编写人：石雷
namespace admin
{
    public partial class SimpleDlg : System.Web.UI.Page
    {
        private DbObject cprivDBO = new DbObject();

        #region 按钮事件相关
        protected void SimpleDlg_ListEditClick(object sender, EventArgs e)
        {
            SimpleDlg_winSimpleMore.Show(SimpleDlg_DataList.sRowNowID);
        }

        protected void SimpleDlg_ListAddClick(object sender, EventArgs e)
        {
            SimpleDlg_winSimpleMore.Show();
        }


        protected void SimpleDlg_SimpleMoreSave(object sender, EventArgs e)
        {
            SimpleDlg_DataList.InitStore();
        }
        #endregion

        private void InitDataList()
        {
            SimpleDlg_DataList.sTableName = Request["keyword"].ToString();
            SimpleDlg_DataList.sViewName = SimpleDlg_DataList.sTableName;
            SimpleDlg_DataList.sOrderBy = " Order By TheOrder";
            ArrayList arr = new ArrayList();
            arr.Add("名称"); arr.Add("说明");
            SimpleDlg_DataList.arrHeadName = arr;
            arr.Clear();
            arr.Add("Name"); arr.Add("Remark");
            SimpleDlg_DataList.arrDataIndex = arr;
            SimpleDlg_DataList.InitDataList();

            SimpleDlg_DataList.SetButtonVisiable(DataListButton.btnAdd);
            SimpleDlg_DataList.SetButtonVisiable(DataListButton.btnEdit);
            SimpleDlg_DataList.SetButtonVisiable(DataListButton.btnDelete);
            SimpleDlg_DataList.SetButtonVisiable(DataListButton.btnOrderDown);
            SimpleDlg_DataList.SetButtonVisiable(DataListButton.btnOrderUp);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (!X.IsAjaxRequest))
            {
                if (Request["keyword"] != null)
                {
                    InitDataList();
                    SimpleDlg_winSimpleMore.sTableName = Request["keyword"].ToString();
                }
            }
        }
    }
}
