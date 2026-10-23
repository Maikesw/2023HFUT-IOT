/// <summary>
/// ConstValue,存放所有的常数
/// 编写人：石雷
/// </summary>

namespace NewTywz.ConstValue
{
    /// <summary>
    /// DataList顶端按钮
    /// </summary>
    public class DataListButton
    {
        public const int btnAdd = 1;
        public const int btnEdit = 2;
        public const int btnDelete = 3;
        public const int btnReset = 4;
        public const int btnSearch = 5;
        public const int btnAll = 6;
        public const int btnOrderUp = 7;
        public const int btnOrderDown = 8;
        public const int btnMore1 = 9;
    }

    /// <summary>
    /// 数据库相关常量类
    /// </summary>
    public class ConstDataBase
    {
        public string DB_CONN_STRING;
        public string PROJECT_DEPARTMENT_NAME;
        //以下为字段类型,注意如果这里修改，一定要跟着修改DBObject中相关的新增修改删除类型
        public const int STRING_TYPE = 0;
        public const int STRINGL_TYPE = 1;
        public const int INT_TYPE = 2;
        public const int FLOAT_TYPE = 3;
        public const int BOOLEAN_TYPE = 4;
        public const int DATETIME_TYPE = 5;
        public const int SELSTR_TYPE = 6;
        public const int PIC_TYPE = 7;
        public const int BOOLEANBOX_TYPE = 8;
        public const int STATUS_TYPE = 9;


        public ConstDataBase()
        {
            DB_CONN_STRING = System.Configuration.ConfigurationSettings.AppSettings["ConnStr"];
            PROJECT_DEPARTMENT_NAME = System.Configuration.ConfigurationSettings.AppSettings["ProjectDepartmentName"];
        }
    }

    /// <summary>
    /// 上传文件类型
    /// </summary>
    public class FileType
    {
        public const int PIC_TYPE = 0;
        public const int DOC_TYPE = 1;
        public const int ICON_TYPE = 2;
    }

    /// <summary>
    /// 和地址路径相关的一些const
    /// </summary>
    public class ConstAddressPath
    {
        public string UPFILE_PATH;
        public string UPFILE_WEBPATH;
        public string PIC_PATH;
        public string PIC_WEBPATH;
        public string ICON_PATH;
        public string ICON_WEBPATH;
        public string DOC_PATH;
        public string DOC_WEBPATH;

        public ConstAddressPath()
        {
            UPFILE_PATH = "/UpFile/";
            UPFILE_WEBPATH = System.Web.HttpContext.Current.Server.MapPath("~/UpFile/");
            PIC_PATH = "/attached/image/";
            PIC_WEBPATH = System.Web.HttpContext.Current.Server.MapPath("~/attached/image/");
            ICON_PATH = "/icon/";
            ICON_WEBPATH = System.Web.HttpContext.Current.Server.MapPath("~/icon/");
            DOC_PATH = "/doc/";
            DOC_WEBPATH = System.Web.HttpContext.Current.Server.MapPath("~/doc/");
        }
    }
}