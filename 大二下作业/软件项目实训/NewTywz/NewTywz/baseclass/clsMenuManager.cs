using System.Data;
using System.Web.UI;


namespace NewTywz.baseclass
{
    /// <summary>
    /// 处理菜单的相关业务层
    /// 编写人：石雷
    /// </summary>
    public class clsMenuManager : Page
    {
        private DbObject cprivDBO;

        public clsMenuManager()
        {
            this.cprivDBO = new DbObject();
        }

        /// <summary>
        /// 根据传过来的用户角色（编号，数组），返回包含菜单项各种信息的数据库表 
        /// </summary>
        /// <param name="sUserRoles">用户角色（编号，数组）</param>
        /// <returns>包含菜单项各种信息的数据库表</returns>
        public DataTable GetRoleMenu(string[] sUserRoles)
        {

            string sSQL = "select distinct tblOperationInfo.ID,tblOperationInfo.Name, tblOperationInfo.OperationIURL,tblOperationInfo.ParentIndex,tblOperationInfo.IsLeaf,tblOperationInfo.TheOrder from tblOperationAuthorRelation,tblOperationInfo " +
                        " where tblOperationInfo.ID=tblOperationAuthorRelation.RelationID2 " +
                        "AND ( tblOperationAuthorRelation.RelationID1=" + sUserRoles[0] + " ";
            for (int i = 1; i < sUserRoles.Length - 1; i++) //sUserRoles.Length-1,是因为最后多一个,
                sSQL = sSQL + "OR tblOperationAuthorRelation.RelationID1 =" + sUserRoles[i] + " ";
            sSQL = sSQL + ")";
            sSQL = sSQL + " and tblOperationInfo.DelFlag=0 ";
            sSQL = sSQL + " Order By TheOrder";
            return cprivDBO.ExcuteSqlTable(sSQL);
        }

        public string GetNavName(string sTypeID)
        {
            return GetNavName(sTypeID, "");
        }

        public string GetNavName(string sTypeID, string sContentID)
        {
            string sReturn = "";
            string sId = sTypeID;
            while (sId != "-1")
            {
                string sLink = cprivDBO.GetValue(sId, "ChannelTTemplete", "tblChannelType").ToString();
                if (sLink[sLink.Length - 1] == '/')
                    sLink = sLink.Remove(sLink.Length - 1, 1);
                else
                    sLink = sLink + "?id=" + sId;
                sReturn = "<a href=" + sLink + " class=Keyword>"
                    + cprivDBO.GetValue(sId, "Name", "tblChannelType").ToString() + "</a>&nbsp;&gt;&nbsp;" + sReturn;
                sId = cprivDBO.GetValue(sId, "ParentIndex", "tblChannelType").ToString();
            }
            if (sContentID == "")
                sReturn = "您现在的位置：" + sReturn.Remove(sReturn.Length - 10, 10);
            else
                sReturn = "您现在的位置：" + sReturn + cprivDBO.GetValue(sContentID, "ChannelCName", "tblChannelContent").ToString();

            return sReturn;
        }

    }
}
