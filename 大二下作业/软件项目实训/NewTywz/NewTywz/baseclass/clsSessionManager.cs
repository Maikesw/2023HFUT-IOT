using System.Web.UI;


namespace NewTywz.baseclass
{
    /// <summary>
    /// 管理session的业务层类。被很多其他的业务层调用。
    /// 编写人：石雷
    /// </summary>
    public class clsSessionManager : Page
    {

        public clsSessionManager()
        {

        }
        /// <summary>
        /// 用户退出时,将用户信息的sessio清空
        /// </summary>
        public void UserExit()
        {
            Session.Remove("UserRoles");
            Session.Remove("ID");
            Session.Remove("UserName");
            Session.Remove("RegisterName");
        }
        /// <summary>
        /// 用户登录时，写入相关session
        /// </summary>
        /// <param name="sID">用户ID</param>
        /// <param name="sRegisterName">用户登录帐号</param>
        /// <param name="sUserName">用户姓名</param>
        /// <param name="sDepartmentID">用户所在单位</param>
        /// <param name="strRole">用户权限</param>
        public void UserLogin(string sID, string sRegisterName, string sUserName, string strRole)
        {
            Session.Add("UserRoles", strRole);
            Session.Add("ID", sID);
            Session.Add("UserName", sUserName);
            Session.Add("RegisterName", sRegisterName);
        }
    }
}
