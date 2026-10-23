using Ext.Net;
using NewTywz.baseclass;
using NewTywz.basepage;
using NewTywz.ConstValue;
using System;
using System.Collections;

namespace admin.ChannelManage
{
    public partial class ChannelContent : ExtPage
    {
        private clsTreeManager clsTree = new clsTreeManager();
        private DbObject cprivDBO = new DbObject();

        #region 初始化相关
        private void InitTreeInfo()
        {
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sTableName = "tblChannelType";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sRootName = "所有栏目";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sTreeListName = "ChannelCChannelTID";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sSomeSQL = "select tblChannelType.ID from tblChannelType,tblChannelAuthorRelation where tblChannelType.DelFlag=0 and tblChannelType.ID=tblChannelAuthorRelation.RelationID2 and tblChannelAuthorRelation.RelationID1 IN (select UserARAuthorIID from tblUserAuthorRelation where UserARUserIID=" + Session["ID"].ToString() + ")";
            ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).BuildTree();
            ((NewTywz.TreeSelMaster)Master).SetTreePanelName("所有栏目");
            ((NewTywz.TreeSelMaster)Master).SetContentPanelName("栏目所有内容");
        }

        private void InitDataList()
        {
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).sTableName = "tblChannelContent";
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).sViewName = "tblChannelContent";
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).bFirstVisit = false;
            ArrayList arr = new ArrayList();
            arr.Add("置顶"); arr.Add("首页显示"); arr.Add("发布时间"); arr.Add("标题");
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).arrHeadName = arr;
            arr.Clear();
            arr.Add("ChannelCTop"); arr.Add("ChannelCPop"); arr.Add("ChannelCTime"); arr.Add("ChannelCName");
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).arrDataIndex = arr;
            arr.Clear();
            arr.Add(ConstDataBase.BOOLEANBOX_TYPE); arr.Add(ConstDataBase.BOOLEAN_TYPE); arr.Add(ConstDataBase.DATETIME_TYPE); arr.Add(ConstDataBase.STRING_TYPE);
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).arrDataType = arr;
            arr.Clear();
            arr.Add(40); arr.Add(80); arr.Add(100); arr.Add(200);
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).arrColWidth = arr;
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).InitDataList();
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).SetButtonVisiable(DataListButton.btnAdd);
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).SetButtonVisiable(DataListButton.btnEdit);
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).SetButtonVisiable(DataListButton.btnDelete);
        }
        #endregion

        #region DataList和tree上的各种按钮相关
        public override void TreeSelMaster_DataListEditClick(string sID)
        {
            WinChannelContent_winMore.Show(sID);
        }
        public override void TreeSelMaster_DataListAddClick()
        {
            clsTree.sTableName = "tblChannelType";
            if (clsTree.IsLeaf(((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sTreeNowID))
            {
                WinChannelContent_winMore.sChannelID = ((NewTywz.UserControl.DataTree)Master.FindControl("TreeSelMaster_DataTree")).sTreeNowID;
                WinChannelContent_winMore.Show();
            }
            else
                X.Msg.Alert("注意", "只有叶子节点才可以增加内容！").Show();
        }

        private bool VerifyRole()
        {
            string[] sUserRole = Session["UserRoles"].ToString().Split(',');
            string sAdminRoleID = cprivDBO.GetValueFromValue("超级管理员", "Name", "ID", "tblAuthorizationInfo");
            bool flag = true;
            int i = 0;
            while ((i < sUserRole.Length) && (flag))
            {
                if (sUserRole[i] == sAdminRoleID)
                    flag = false;
                else
                    i++;
            }
            return false;
        }

        public override void TreeSelMaster_TreeNodeClick(string sID)
        {
            clsTree.sTableName = "tblChannelType";
            string sChannelTypeID = sID;
            string sWhere = " ChannelCChannelTID=" + sChannelTypeID;
            if (!VerifyRole())
                sWhere = sWhere + " and ChannelCUserIID=" + Session["ID"].ToString();
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).InitStore(((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).sInitSQL, sWhere);
        }



        #endregion

        protected void WinChannelContent_SaveClick(object sender, EventArgs e)
        {
            ((NewTywz.UserControl.DataList)Master.FindControl("TreeSelMaster_DataList")).InitStore();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                InitTreeInfo();
                InitDataList();
            }
        }
    }
}
