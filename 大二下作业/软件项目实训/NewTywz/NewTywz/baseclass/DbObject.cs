using NewTywz.ConstValue;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace NewTywz.baseclass
{
    /// <summary>
    /// DbObject，直接和数据层打交道的类
    /// </summary>
    public class DbObject : DataBaseFunc
    {
        public DbObject()
        {

        }

        ~DbObject()
        {

        }

        protected override void InitConnStr()
        {
            this.con = new ConstDataBase();
            ConnStr = con.DB_CONN_STRING;
            if (ConnStr.Contains("Provider"))
                this.oleDbConn = new OleDbConnection(ConnStr);
            else
                this.sqlConn = new SqlConnection(ConnStr);
        }

    }
}
