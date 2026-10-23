using System.Web.UI;

namespace NewTywz.basepage
{
    /// <summary>
    /// 操作相关页面的基类page，提供各种页面自定义供模板调用函数的虚实现
    // 编写人：石雷
    /// </summary>
    public class ExtPage : Page
    {
        #region TreeMaster
        public virtual void TreeMaster_MoreInfo(string sID) { }
        public virtual void TreeMaster_TreeNodeClick(string sID) { }
        #endregion

        #region TreeSelMaster
        public virtual void TreeSelMaster_TreeNodeClick(string sID) { }
        public virtual void TreeSelMaster_DataListEditClick(string sID) { }
        public virtual void TreeSelMaster_DataListAddClick() { }
        #endregion
    }
}
