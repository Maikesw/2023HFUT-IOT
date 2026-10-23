using System;
using System.Data;

namespace NewTywz.baseclass
{

    /// <summary>
    /// 用来管理用户的业务层类。
    /// 编写人：石雷
    /// </summary>
    public class clsUserManager
    {
        private DbObject cprivDBO;
        private clsSessionManager objSessionManager;

        public clsUserManager()
        {
            // TODO: 在此处添加构造函数逻辑
            this.cprivDBO = new DbObject();
            this.objSessionManager = new clsSessionManager();
        }
        /// <summary>
        /// 用户登录的判断函数
        /// </summary>
        /// <param name="sRegisterName">登录帐号</param>
        /// <param name="sPassWord">登录密码</param>
        /// <returns></returns>
        public string UserLogin(string sRegisterName, string sPassWord)
        {
            string sGetPass = cprivDBO.GetValueFromValue(sRegisterName, "UserICode", "UserIPsd", "tblUserInfo", true);
            if (sGetPass == "")
                return "该用户不存在！";
            else if (sGetPass != sPassWord)
                return "密码错误！";
            else//正确的用户
            {
                string strRole = GetUserRoleByRegisterName(sRegisterName);
                if (strRole == "")
                    return "您还没有登录后台的权限！请等待管理员审核。";
                else
                {
                    string sSQL = "select * from tblUserInfo where UserICode='" + sRegisterName + "'";
                    DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                    objSessionManager.UserLogin(dt.Rows[0]["ID"].ToString(), sRegisterName, dt.Rows[0]["Name"].ToString(), strRole);
                    return "登录成功！";
                }
            }
        }

        /// <summary>
        /// 根据用户ID获得用户名（登录名）
        /// </summary>
        /// <param name="sUserID">用户ID（主码）</param>
        /// <returns></returns>
        private string GetUserName(string sUserID)
        {
            string sSQL = "select * from tblUserInfo where ID=" + sUserID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["UserICode"].ToString();
            else
                return "no user";
        }


        /// <summary>
        /// 按用户登陆名查找用户ID值
        /// </summary>
        /// <param name="sUserICode">用户登陆帐号</param>
        /// <returns>用户在数据库中的ID</returns>
        private long GetUserIDFromRegisterName(string sRegisterName)
        {
            string stemp;
            stemp = cprivDBO.GetValueFromValue(sRegisterName, "UserICode", "ID", "tblUserInfo", true);
            if (stemp != "")
                return Convert.ToInt32(stemp);
            else
                return -1;//错误情况
        }



        /// <summary>
        /// 按照用户名获取用户的RoleID
        /// </summary>
        /// <param name="sRegisterName">用户帐号</param>
        /// <returns></returns>
        private string GetUserRoleByRegisterName(string sRegisterName)
        {
            string sUserID = cprivDBO.GetValueFromValue(sRegisterName, "UserICode", "ID", "tblUserInfo");
            string sSQL = "select * from tblUserAuthorRelation where UserARUserIID=" + sUserID;
            DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
            string sReturn = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    sReturn = sReturn + "," + dt.Rows[i]["UserARAuthorIID"].ToString();
                sReturn = sReturn.Remove(0, 1);
            }
            return sReturn;
        }

    }
}