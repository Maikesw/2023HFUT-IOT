using Ext.Net;
using NewTywz.baseclass;
using System;
using System.Data;

//系统整体框架。主要工作是根据权限生成左边不同菜单，以及作为右边功能的容器。
//编写人：石雷
namespace admin
{
    public partial class main : System.Web.UI.Page
    {
        private clsMenuManager objMenuManager = new clsMenuManager();
        private clsSessionManager objSessionManager = new clsSessionManager();

        public string sFirstPage;
        protected DataRow[] father;//一级菜单的内容
        protected DataRow[] first;//二级菜单的内容
        protected DataRow[] second;//二级菜单的内容
        protected string[] strUserRoles;
        protected DataTable dtFather;


        /// <summary>
        /// 校验当前的session，如果没有了，则跳转到登陆页
        /// </summary>
        [DirectMethod]
        public void VerifySession()
        {
            if ((Session["UserRoles"] == null) || (Session["UserRoles"].ToString() == ""))
                Response.Redirect("login.aspx");
        }


        /// <summary>
        /// 根据权限创建左边的Menu
        /// </summary>
        private void CreateMenuPanel()
        {
            strUserRoles = Session["UserRoles"].ToString().Split(',');
            dtFather = (DataTable)objMenuManager.GetRoleMenu(strUserRoles);
            father = dtFather.Select("ParentIndex=-1", "TheOrder");
            bool bflag = false;
            string sUrl;
            //依次创建一级菜单
            foreach (DataRow drfather in father)
            {
                Ext.Net.Panel myPanel = new Ext.Net.Panel();
                myPanel.BodyStyle = "background-color: #F0F0F0;";
                MenuPanel myMenuPanel = new MenuPanel();
                myMenuPanel.Border = false;
                myPanel.Title = drfather["Name"].ToString();
                sUrl = drfather["OperationIURL"].ToString();
                //创建二级菜单
                first = dtFather.Select("ParentIndex=" + drfather["ID"].ToString(), "TheOrder");
                //依次创建二级菜单
                foreach (DataRow drfirst in first)
                {
                    Ext.Net.MenuItem myMenuItem = new Ext.Net.MenuItem(drfirst["Name"].ToString());
                    string sUrl1 = "admin/" + sUrl + "/" + drfirst["OperationIURL"].ToString();
                    if (Convert.ToBoolean(drfirst["IsLeaf"]) == true)
                    {
                        myMenuItem.Icon = Icon.ArrowRight;
                        string s;
                        if (drfirst["OperationIURL"].ToString().Remove(1) == "/")
                            s = "'admin/" + drfirst["OperationIURL"].ToString().Substring(1) + "', '" + drfirst["ID"] + "','" + drfirst["Name"] + "'";
                        else
                            s = "'" + sUrl1 + "', '" + drfirst["ID"] + "','" + drfirst["Name"] + "'";
                        myMenuItem.Listeners.Click.Handler = "JavaScript:e.stopEvent();loadExample(" + s + ");";
                        if (bflag == false)
                        {
                            sFirstPage = s;
                            bflag = true;
                        }
                    }
                    else//最多三级菜单
                    {
                        Ext.Net.Menu subMenu = new Ext.Net.Menu();
                        second = dtFather.Select("ParentIndex=" + drfirst["ID"].ToString(), "TheOrder");
                        foreach (DataRow drsecond in second)
                        {
                            string sUrl2 = sUrl1 + "/" + drsecond["OperationIURL"].ToString();
                            Ext.Net.MenuItem mySubMenuItem = new Ext.Net.MenuItem(drsecond["Name"].ToString());
                            mySubMenuItem.Icon = Icon.ArrowRight;
                            string s;
                            if (drsecond["OperationIURL"].ToString().Remove(1) == "/")
                                s = "'" + drsecond["OperationIURL"].ToString().Substring(1) + "', '" + drfirst["ID"] + "','" + drfirst["Name"] + "'";
                            else
                                s = "'" + sUrl2 + "', '" + drsecond["ID"] + "','" + drsecond["Name"] + "'";
                            mySubMenuItem.Listeners.Click.Handler = "JavaScript:e.stopEvent();loadExample(" + s + ");";
                            if (bflag == false)
                            {
                                sFirstPage = s;
                                bflag = true;
                            }
                            subMenu.Items.Add(mySubMenuItem);
                        }
                        myMenuItem.Icon = Icon.PlayGreen;
                        myMenuItem.Menu.Add(subMenu);
                    }
                    myMenuPanel.Menu.Items.Add(myMenuItem);
                }
                myPanel.AutoScroll = true;
                myMenuPanel.Header = false;
                myPanel.Items.Add(myMenuPanel);
                Main_Menu.Items.Add(myPanel);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !X.IsAjaxRequest)
            {
                VerifySession();
                sFirstPage = "";
                CreateMenuPanel();
                if (sFirstPage != "")
                {
                    string loadExample = string.Concat("loadExample(" + sFirstPage + ");");
                    this.Main_ResourceManager.Listeners.DocumentReady.Handler = loadExample;
                    labWelcome.Text = "您好，" + objSessionManager.Session["RegisterName"].ToString() + "，欢迎使用本系统--" + "今天是" + DateTime.Now.ToString("yyyy年MM月dd日");
                }
            }
        }
    }
}