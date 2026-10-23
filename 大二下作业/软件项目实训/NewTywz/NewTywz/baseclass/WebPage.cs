
using NewTywz.baseclass;
using System.Web.UI;

//using Ext.Net;

namespace NewTywz.basepage
{
    /// <summary>
    /// 操作相关页面的基类page，提供各种页面自定义供模板调用函数的虚实现
    // 编写人：石雷
    /// </summary>
    public class WebPage : Page
    {

        public DbObject cprivDBO = new DbObject();
        public clsMenuManager clsMenu = new clsMenuManager();

        public virtual void InitMainList(string sDisID) { }


    }
}
