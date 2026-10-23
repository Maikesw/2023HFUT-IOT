using NewTywz.ConstValue;
using System;
using System.Collections;
using System.Data; // 申明变量
using System.Data.OleDb;
using System.Data.SqlClient;

namespace NewTywz.baseclass
{
    /// <summary>
    /// 直接和数据层打交道的类
    /// 编写人：石雷
    /// 新增(AddValue)和修改(SetValue)都可以 操作一个字段或多个字段。如果是一个字段，则直接以string形式传递字段名称和内容，如果是多个字段，则用ArrayList传递。
    /// 都有返回值，表示操作成功与否
    /// </summary>
    public class DataBaseFunc
    {
        protected SqlConnection sqlConn; //数据库连接字串
        protected OleDbConnection oleDbConn;
        protected SqlCommand sqlCommand;
        protected OleDbCommand oleDbCommand;
        protected SqlDataReader sqlDataReader; //可用来依次读取数据库内容
        protected OleDbDataReader oleDbDataReader;
        protected ConstDataBase con;
        protected string ConnStr;

        #region VerifySQL
        protected bool VerifySQL(string sSQL, string[] ss)
        {
            int nYin = sSQL.Length - sSQL.Replace("'", "").Length;
            if (nYin % 2 == 1)
                return false;
            bool flag = true;
            sSQL = sSQL.ToUpper();
            char[] ch = { ' ', ';', '(', ')', '=' };
            string[] arrSQL = sSQL.Split(ch);
            int i = 0;
            int j = 0;
            while ((i < arrSQL.Length) && (flag))
            {
                j = 0;
                while ((j < ss.Length) && (flag))
                {
                    if (arrSQL[i] == ss[j])
                        return false;
                    else
                        j++;
                }
                i++;
            }
            return flag;
        }

        protected bool VerifySelectSQL(string sSQL)
        {
            string[] ss = { "INSERT", "UPDATE", "DELETE", "UNICODE", "NCHAR", "SUBSTRING", "EXEC", "EXECUTE", "SP_PASSWORD", "SP_WHO", "CREATE", "DROP", "TRUNCATE", "CAST" };
            return VerifySQL(sSQL, ss);
        }

        protected bool VerifyInsertSQL(string sSQL)
        {
            string[] ss = { "UPDATE", "DELETE", "UNICODE", "NCHAR", "SUBSTRING", "EXEC", "EXECUTE", "SP_PASSWORD", "SP_WHO", "CREATE", "DROP", "TRUNCATE", "CAST" };
            return VerifySQL(sSQL, ss);
        }

        protected bool VerifyUpdateSQL(string sSQL)
        {
            string[] ss = { "INSERT", "DELETE", "UNICODE", "NCHAR", "SUBSTRING", "EXEC", "EXECUTE", "SP_PASSWORD", "SP_WHO", "CREATE", "DROP", "TRUNCATE", "CAST" };
            return VerifySQL(sSQL, ss);
        }

        protected bool VerifyDeleteSQL(string sSQL)
        {
            string[] ss = { "INSERT", "UPDATE", "UNICODE", "NCHAR", "SUBSTRING", "EXEC", "EXECUTE", "SP_PASSWORD", "SP_WHO", "CREATE", "DROP", "TRUNCATE", "CAST" };
            return VerifySQL(sSQL, ss);
        }

        protected bool VerifyTruncateSQL(string sSQL)
        {
            string[] ss = { "INSERT", "UPDATE", "UNICODE", "NCHAR", "SUBSTRING", "EXEC", "EXECUTE", "SP_PASSWORD", "SP_WHO", "CREATE", "DROP", "CAST" };
            return VerifySQL(sSQL, ss);
        }

        protected bool VerifyExtraSQL(string sSQL)
        {
            string[] ss = { "NCHAR", "SUBSTRING", "EXEC", "EXECUTE", "SP_PASSWORD", "SP_WHO", "CREATE", "DROP", "CAST" };
            return VerifySQL(sSQL, ss);
        }
        #endregion

        #region 构造函数和析构函数
        public DataBaseFunc()
        {
            InitConnStr();
        }

        ~DataBaseFunc()
        {
            //CloseConnection();
        }
        #endregion

        #region 最底层的一些东西
        protected virtual void InitConnStr()
        {

        }

        #region Open and Close Connection
        public bool OpenConnection()//打开数据库
        {
            if (ConnStr.Contains("Provider"))
            {
                try
                {
                    this.oleDbConn.Open();
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    this.sqlConn.Open();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public void CloseConnection()//关闭数据库
        {
            if (ConnStr.Contains("Provider"))
                this.oleDbConn.Close();
            else
                this.sqlConn.Close();
        }
        #endregion


        #region ExcuteSqlTable
        /// <summary>
        /// 通过传来的sql语句，返回一个datatable，只能查询
        /// </summary>
        /// <param name="sSQL">传来的sql语句</param>
        /// <returns>返回一个datatable</returns>
        public DataTable ExcuteSqlTable(string sSQL)
        {
            DataTable dt = new DataTable();
            if (sSQL != "")
            {
                if (!VerifySelectSQL(sSQL))
                    return dt;

                if (OpenConnection())
                {
                    if (ConnStr.Contains("Provider"))
                    {
                        OleDbDataAdapter sda = new OleDbDataAdapter(sSQL, this.oleDbConn);
                        sda.Fill(dt);
                    }
                    else
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(sSQL, this.sqlConn);
                        sda.Fill(dt);
                    }
                    CloseConnection();
                }
            }
            return dt;
        }


        /// <summary>
        /// 通过传来的sql语句，返回一个datatable，UARD各种操作，主要用于批量
        /// </summary>
        /// <param name="sSQL">传来的sql语句</param>
        /// <returns>返回一个datatable</returns>
        public DataTable ExcuteExtraSqlTable(string sSQL)
        {
            DataTable dt = new DataTable();
            if (sSQL != "")
            {
                if (!VerifyExtraSQL(sSQL))
                    return dt;

                if (OpenConnection())
                {
                    if (ConnStr.Contains("Provider"))
                    {
                        OleDbDataAdapter sda = new OleDbDataAdapter(sSQL, this.oleDbConn);
                        sda.Fill(dt);
                    }
                    else
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(sSQL, this.sqlConn);
                        sda.Fill(dt);
                    }
                    CloseConnection();
                }
            }
            return dt;
        }
        #endregion

        #endregion

        #region GetValue

        public DataTable GetValue(string sID, string sTableName)
        {
            string sSQL = "select * from " + sTableName + " where ID=" + sID;
            return this.ExcuteSqlTable(sSQL);
        }

        public object GetValue(string sID, string sToName, string sTableName)
        {
            string sSQL = "select " + sToName + " from " + sTableName + " where ID=" + sID;
            DataTable dt = this.ExcuteSqlTable(sSQL);
            if ((dt.Rows.Count > 0) && (dt.Rows[0][0] != DBNull.Value))
                return dt.Rows[0][0];
            else
                return "";
        }

        public object GetMaxValue(string sWord, string sTableName)
        {
            return GetMaxValue(sWord, sTableName, "");
        }
        public object GetMaxValue(string sWord, string sTableName, string sWhere)
        {
            string sSQL = "select max(" + sWord + ") from " + sTableName;
            if (sWhere != "")
                sSQL = sSQL + " where " + sWhere;
            DataTable dt = ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0];
            else
                return 0;
        }
        public int GetCount(string sWhere, string sTableName)
        {
            string sSQL = "select count(*) from " + sTableName + " where " + sWhere;
            DataTable dt = this.ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0][0]);
            else
                return 0;
        }

        #endregion

        #region GetValueFromValue
        protected string GetReturn(string sSQL)
        {
            DataTable dt = ExcuteSqlTable(sSQL);
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return "";
        }
        /// <summary>
        /// 通过一个字段在数据库中寻找另外一个字段的值。如果有多个符合条件的值，只返回第一个值。不考虑伪删除
        /// </summary>
        /// <param name="sFromValue">源字段值</param>
        /// <param name="sFromWords">源字段在数据库表中的字段名</param>
        /// <param name="sToWords">目的字段在数据库表中的字段名</param>
        /// <param name="sTableName">数据库表名</param>
        /// <returns>目的字段值</returns>
        public string GetValueFromValue(string sFromValue, string sFromWords, string sToWords, string sTableName)
        {
            string sSQL = "select " + sToWords + " from " + sTableName + " where " + sFromWords + "='" + sFromValue + "'";
            return GetReturn(sSQL);
        }


        /// <summary>
        /// 通过一个字段在数据库中寻找另外一个字段的值。如果有多个符合条件的值，只返回第一个值
        /// </summary>
        /// <param name="sFromValue">源字段值</param>
        /// <param name="sFromWords">源字段在数据库表中的字段名</param>
        /// <param name="sToWords">目的字段在数据库表中的字段名</param>
        /// <param name="sTableName">数据库表名</param>
        /// <param name="delflag">伪删除情况。true考虑没有删除的，flase考虑已经删除的</param>
        /// <returns>目的字段值</returns>
        public string GetValueFromValue(string sFromValue, string sFromWords, string sToWords, string sTableName, bool delflag)
        {
            string sSQL = "select " + sToWords + " from " + sTableName + " where " + sFromWords + "='" + sFromValue + "'";
            if (delflag)
                sSQL = sSQL + " and DelFlag=0";
            else
                sSQL = sSQL + " and DelFlag=1";
            return GetReturn(sSQL);
        }

        /// <summary>
        /// 通过一个字段在数据库中寻找另外一个字段的值。如果有多个符合条件的值，只返回第一个值。不考虑伪删除
        /// </summary>
        /// <param name="sFromValue">源字段值</param>
        /// <param name="sFromWords">源字段在数据库表中的字段名</param>
        /// <param name="sToWords">目的字段在数据库表中的字段名</param>
        /// <param name="sTableName">数据库表名</param>
        /// <returns>目的字段值</returns>
        public string GetValueFromValue(long sFromValue, string sFromWords, string sToWords, string sTableName)
        {
            string sSQL = "select " + sToWords + " from " + sTableName + " where " + sFromWords + "=" + sFromValue.ToString();
            return GetReturn(sSQL);
        }

        /// <summary>
        /// 通过一个字段在数据库中寻找另外一个字段的值。如果有多个符合条件的值，只返回第一个值
        /// </summary>
        /// <param name="sFromValue">源字段值</param>
        /// <param name="sFromWords">源字段在数据库表中的字段名</param>
        /// <param name="sToWords">目的字段在数据库表中的字段名</param>
        /// <param name="sTableName">数据库表名</param>
        /// <param name="delflag">伪删除情况。true考虑没有删除的，flase考虑已经删除的</param>        
        /// <returns>目的字段值</returns>
        public string GetValueFromValue(long sFromValue, string sFromWords, string sToWords, string sTableName, bool delflag)
        {
            string sSQL = "select " + sToWords + " from " + sTableName + " where " + sFromWords + "=" + sFromValue.ToString();
            if (delflag)
                sSQL = sSQL + " and DelFlag=0";
            else
                sSQL = sSQL + " and DelFlag=1";
            return GetReturn(sSQL);
        }

        /// <summary>
        /// 通过一个字段在数据库中寻找另外一个字段的值。可用于有多条记录的值。不考虑伪删除
        /// </summary>
        /// <param name="sFromValue">源字段值</param>
        /// <param name="sFromWords">源字段在数据库表中的字段名</param>
        /// <param name="sToWords">目的字段在数据库表中的字段名</param>
        /// <param name="sTableName">数据库表名</param>
        /// <returns>目的字段值列表</returns>
        public ArrayList GetArrValueFromValue(string sFromValue, string sFromWords, string sToWords, string sTableName)
        {
            string sSQL = "select " + sToWords + " from " + sTableName + " where " + sFromWords + "='" + sFromValue + "'";
            ArrayList arrReturnValue = new ArrayList();
            DataTable dt = ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
                arrReturnValue.Add(dt.Rows[i][0].ToString());
            return arrReturnValue;
        }

        /// <summary>
        /// 通过一个字段在数据库中寻找另外一个字段的值。可用于有多条记录的值
        /// </summary>
        /// <param name="sFromValue">源字段值</param>
        /// <param name="sFromWords">源字段在数据库表中的字段名</param>
        /// <param name="sToWords">目的字段在数据库表中的字段名</param>
        /// <param name="sTableName">数据库表名</param>
        /// <param name="delflag">伪删除情况。true考虑没有删除的，flase考虑已经删除的</param>        
        /// <returns>目的字段值列表</returns>
        public ArrayList GetArrValueFromValue(string sFromValue, string sFromWords, string sToWords, string sTableName, bool delflag)
        {
            string sSQL = "select " + sToWords + " from " + sTableName + " where " + sFromWords + "='" + sFromValue + "'";
            if (delflag)
                sSQL = sSQL + " and DelFlag=0";
            else
                sSQL = sSQL + " and DelFlag=1";
            ArrayList arrReturnValue = new ArrayList();
            DataTable dt = ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
                arrReturnValue.Add(dt.Rows[i][0].ToString());
            return arrReturnValue;
        }
        /// <summary>
        /// 通过一个字段在数据库中寻找另外一个字段的值。可用于有多条记录的值。不考虑伪删除
        /// </summary>
        /// <param name="sFromValue">源字段值</param>
        /// <param name="sFromWords">源字段在数据库表中的字段名</param>
        /// <param name="sToWords">目的字段在数据库表中的字段名</param>
        /// <param name="sTableName">数据库表名</param>
        /// <returns>目的字段值列表</returns>
        public ArrayList GetArrValueFromValue(long sFromValue, string sFromWords, string sToWords, string sTableName)
        {
            string sSQL = "select " + sToWords + " from " + sTableName + " where " + sFromWords + "=" + sFromValue.ToString();
            ArrayList arrReturnValue = new ArrayList();
            DataTable dt = ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
                arrReturnValue.Add(dt.Rows[i][0].ToString());
            return arrReturnValue;
        }

        /// <summary>
        /// 通过一个字段在数据库中寻找另外一个字段的值。可用于有多条记录的值
        /// </summary>
        /// <param name="sFromValue">源字段值</param>
        /// <param name="sFromWords">源字段在数据库表中的字段名</param>
        /// <param name="sToWords">目的字段在数据库表中的字段名</param>
        /// <param name="sTableName">数据库表名</param>
        /// <param name="delflag">伪删除情况。true考虑没有删除的，flase考虑已经删除的</param>         
        /// <returns>目的字段值列表</returns>
        public ArrayList GetArrValueFromValue(long sFromValue, string sFromWords, string sToWords, string sTableName, bool delflag)
        {
            string sSQL = "select " + sToWords + " from " + sTableName + " where " + sFromWords + "=" + sFromValue.ToString();
            if (delflag)
                sSQL = sSQL + " and DelFlag=0";
            else
                sSQL = sSQL + " and DelFlag=1";
            ArrayList arrReturnValue = new ArrayList();
            DataTable dt = ExcuteSqlTable(sSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
                arrReturnValue.Add(dt.Rows[i][0].ToString());
            return arrReturnValue;
        }
        #endregion         

        #region AddValue

        #region AddValue，增加一个字段，并且根据字段内容的类型自动判断调用
        public string AddValue(string sWordName, string sWordValue, string sTableName)
        {
            return AddValue(sWordName, sWordValue, ConstDataBase.STRING_TYPE, sTableName);
        }
        public string AddValue(string sWordName, int nWordValue, string sTableName)
        {
            return AddValue(sWordName, nWordValue.ToString(), ConstDataBase.INT_TYPE, sTableName);
        }
        public string AddValue(string sWordName, float fWordValue, string sTableName)
        {
            return AddValue(sWordName, fWordValue.ToString(), ConstDataBase.FLOAT_TYPE, sTableName);
        }
        public string AddValue(string sWordName, bool bWordValue, string sTableName)
        {
            if (bWordValue)
                return AddValue(sWordName, "1", ConstDataBase.BOOLEAN_TYPE, sTableName);
            else
                return AddValue(sWordName, "0", ConstDataBase.BOOLEAN_TYPE, sTableName);
        }
        public string AddValue(string sWordName, DateTime dtWordValue, string sTableName)
        {
            return AddValue(sWordName, dtWordValue.ToString("yyyy-MM-dd"), ConstDataBase.DATETIME_TYPE, sTableName);
        }
        #endregion

        public string AddValue(string sWordName, string sWordValue, int nConstDataValue, string sTableName)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            arrWordName.Add(sWordName);
            arrWordValue.Add(sWordValue);
            arrWordType.Add(nConstDataValue);

            return AddValue(arrWordName, arrWordValue, arrWordType, sTableName);
        }

        public string AddValue(ArrayList arrWordName, ArrayList arrWordValue, ArrayList arrWordType, string sTableName)
        {
            if ((arrWordName.Count != arrWordValue.Count) || (arrWordName.Count != arrWordType.Count))
                return "0";

            if (OpenConnection())
            {
                string sSQL = GetInsertSql(arrWordName, arrWordValue, arrWordType, sTableName);

                if (!VerifyInsertSQL(sSQL))
                {
                    this.CloseConnection();
                    return "0";
                }

                if (ConnStr.Contains("Provider"))
                {
                    this.oleDbCommand = new OleDbCommand(sSQL, this.oleDbConn);
                    this.oleDbCommand.ExecuteNonQuery();
                    sSQL = "SELECT @@IDENTITY";
                    this.oleDbCommand = new OleDbCommand(sSQL, this.oleDbConn);
                    string stemp = this.oleDbCommand.ExecuteScalar().ToString();
                    this.CloseConnection();
                    return stemp;
                }
                else
                {
                    this.sqlCommand = new SqlCommand(sSQL, this.sqlConn);
                    this.sqlCommand.ExecuteNonQuery();
                    sSQL = "SELECT @@IDENTITY";
                    this.sqlCommand = new SqlCommand(sSQL, this.sqlConn);
                    string stemp = this.sqlCommand.ExecuteScalar().ToString();
                    this.CloseConnection();
                    return stemp;
                }
            }
            else
                return "0";
        }

        //获取一条insert的sql
        public string GetInsertSql(ArrayList arrWordName, ArrayList arrWordValue, ArrayList arrWordType, string sTableName)
        {
            string sSQL;
            sSQL = "INSERT INTO " + sTableName + " (";
            int i;
            for (i = 0; i < arrWordName.Count; i++)
                sSQL = sSQL + arrWordName[i] + ",";
            sSQL = sSQL.Remove(sSQL.Length - 1, 1);
            sSQL = sSQL + ") Values (";

            for (i = 0; i < arrWordType.Count; i++)
            {
                if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRING_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRINGL_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.PIC_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.SELSTR_TYPE))
                    sSQL = sSQL + "'" + arrWordValue[i].ToString().Replace("'", "").Replace("\"", "") + "',";
                else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.INT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.FLOAT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STATUS_TYPE))
                    sSQL = sSQL + arrWordValue[i] + ",";
                else if (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.DATETIME_TYPE)
                {

                    if (ConnStr.Contains("Provider"))
                        sSQL = sSQL + "#" + arrWordValue[i] + "#,";
                    else
                        sSQL = sSQL + "'" + arrWordValue[i] + "',";
                }
                else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEAN_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEANBOX_TYPE))
                {
                    if (Convert.ToBoolean(arrWordValue[i]))
                        sSQL = sSQL + "1,";
                    else
                        sSQL = sSQL + "0,";
                }
            }
            sSQL = sSQL.Remove(sSQL.Length - 1, 1);
            sSQL = sSQL + ")";
            return sSQL;
        }

        #endregion

        #region SetValue

        #region SetValue，修改一个字段，并且根据字段内容的类型自动判断调用，并且自动处理ID
        public bool SetValue(string sWordName, string sWordValue, string sTableName, Int32 nID)
        {
            return SetValue(sWordName, sWordValue, ConstDataBase.STRING_TYPE, sTableName, "ID=" + nID.ToString());
        }
        public bool SetValue(string sWordName, int nWordValue, string sTableName, Int32 nID)
        {
            return SetValue(sWordName, nWordValue.ToString(), ConstDataBase.INT_TYPE, sTableName, "ID=" + nID.ToString());
        }
        public bool SetValue(string sWordName, float fWordValue, string sTableName, Int32 nID)
        {
            return SetValue(sWordName, fWordValue.ToString(), ConstDataBase.FLOAT_TYPE, sTableName, "ID=" + nID.ToString());
        }
        public bool SetValue(string sWordName, double dWordValue, string sTableName, Int32 nID)
        {
            return SetValue(sWordName, dWordValue.ToString(), ConstDataBase.FLOAT_TYPE, sTableName, "ID=" + nID.ToString());
        }
        public bool SetValue(string sWordName, bool bWordValue, string sTableName, Int32 nID)
        {
            if (bWordValue)
                return SetValue(sWordName, "1", ConstDataBase.BOOLEAN_TYPE, sTableName, "ID=" + nID.ToString());
            else
                return SetValue(sWordName, "0", ConstDataBase.BOOLEAN_TYPE, sTableName, "ID=" + nID.ToString());
        }
        public bool SetValue(string sWordName, DateTime dtWordValue, string sTableName, Int32 nID)
        {
            return SetValue(sWordName, dtWordValue.ToString("yyyy-MM-dd"), ConstDataBase.DATETIME_TYPE, sTableName, "ID=" + nID.ToString());
        }
        public bool SetValue(ArrayList arrWordName, ArrayList arrWordValue, ArrayList arrWordType, string sTableName, Int32 nID)
        {
            return SetValue(arrWordName, arrWordValue, arrWordType, sTableName, "ID=" + nID.ToString());
        }
        #endregion

        #region SetValue，修改一个字段，并且根据字段内容的类型自动判断调用
        public bool SetValue(string sWordName, string sWordValue, string sTableName, string sWhereValue)
        {
            return SetValue(sWordName, sWordValue, ConstDataBase.STRING_TYPE, sTableName, sWhereValue);
        }
        public bool SetValue(string sWordName, int nWordValue, string sTableName, string sWhereValue)
        {
            return SetValue(sWordName, nWordValue.ToString(), ConstDataBase.INT_TYPE, sTableName, sWhereValue);
        }
        public bool SetValue(string sWordName, float fWordValue, string sTableName, string sWhereValue)
        {
            return SetValue(sWordName, fWordValue.ToString(), ConstDataBase.FLOAT_TYPE, sTableName, sWhereValue);
        }
        public bool SetValue(string sWordName, bool bWordValue, string sTableName, string sWhereValue)
        {
            if (bWordValue)
                return SetValue(sWordName, "1", ConstDataBase.BOOLEAN_TYPE, sTableName, sWhereValue);
            else
                return SetValue(sWordName, "0", ConstDataBase.BOOLEAN_TYPE, sTableName, sWhereValue);
        }
        public bool SetValue(string sWordName, DateTime dtWordValue, string sTableName, string sWhereValue)
        {
            return SetValue(sWordName, dtWordValue.ToString("yyyy-MM-dd"), ConstDataBase.DATETIME_TYPE, sTableName, sWhereValue);
        }
        #endregion 

        public bool SetValue(string sWordName, string sWordValue, int nConstDataValue, string sTableName, string sWhereValue)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            arrWordName.Add(sWordName);
            arrWordValue.Add(sWordValue);
            arrWordType.Add(nConstDataValue);

            return SetValue(arrWordName, arrWordValue, arrWordType, sTableName, sWhereValue);
        }

        public bool SetValue(ArrayList arrWordName, ArrayList arrWordValue, ArrayList arrWordType, string sTableName, string sWhereValue)
        {
            if ((arrWordName.Count != arrWordValue.Count) || (arrWordName.Count != arrWordType.Count))
                return false;

            string sSQL = GetUpdateSql(arrWordName, arrWordValue, arrWordType, sTableName, sWhereValue);

            if (!VerifyUpdateSQL(sSQL))
                return false;


            if (OpenConnection())
                return ExcuteSql(sSQL);
            else
                return false;
        }

        //获取一条update的sql
        public string GetUpdateSql(ArrayList arrWordName, ArrayList arrWordValue, ArrayList arrWordType, string sTableName, string sWhereValue)
        {
            string sSQL = "Update " + sTableName;
            int i = 0;
            if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRING_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRINGL_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.PIC_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.SELSTR_TYPE))
                sSQL = sSQL + " Set " + arrWordName[i] + "='" + arrWordValue[i] + "' ";
            else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.INT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.FLOAT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STATUS_TYPE))
                sSQL = sSQL + " Set " + arrWordName[i] + "=" + arrWordValue[i];
            else if (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.DATETIME_TYPE)
            {
                if (arrWordValue[i].ToString() == "")
                    sSQL = sSQL + " Set " + arrWordName[i] + "=null";
                else
                    sSQL = sSQL + " Set " + arrWordName[i] + "='" + arrWordValue[i] + "'";
            }
            else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEAN_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEANBOX_TYPE))
            {
                if (Convert.ToBoolean(arrWordValue[i]))
                    sSQL = sSQL + " Set " + arrWordName[i] + "=1 ";// +arrWordValue[i];
                else
                    sSQL = sSQL + " Set " + arrWordName[i] + "=0 ";
            }

            for (i = 1; i < arrWordName.Count; i++)
            {
                if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRING_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRINGL_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.PIC_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.SELSTR_TYPE))
                    sSQL = sSQL + ", " + arrWordName[i] + "='" + arrWordValue[i].ToString().Replace("'", "").Replace("\"", "") + "' ";
                else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.INT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.FLOAT_TYPE))
                    sSQL = sSQL + ", " + arrWordName[i] + "=" + arrWordValue[i];
                else if (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.DATETIME_TYPE)
                {
                    if (arrWordValue[i].ToString() == "")
                        sSQL = sSQL + ", " + arrWordName[i] + "=null";
                    else
                        sSQL = sSQL + ", " + arrWordName[i] + "='" + arrWordValue[i] + "'";
                }
                else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEAN_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEANBOX_TYPE))
                    sSQL = sSQL + " ," + arrWordName[i] + "=" + Convert.ToInt16(arrWordValue[i]);
            }
            sSQL = sSQL + " where " + sWhereValue;
            return sSQL;
        }

        #endregion

        #region DeleteValue

        #region DelValueByFlag
        #region Delete Value By ID With Flag
        public bool DelValueByFlag(Int64 nID, string sTableName)
        {
            return DelValueByFlag(nID.ToString(), sTableName);
        }
        public bool DelValueByFlag(string sID, string sTableName)
        {
            return DelValueByFlag("ID", sID, ConstDataBase.BOOLEAN_TYPE, sTableName);
        }
        #endregion

        #region Delete Value Direct With Flag
        public bool DelValueByFlag(string sWordName, string sWordValue, string sTableName)
        {
            return DelValueByFlag(sWordName, sWordValue, ConstDataBase.STRING_TYPE, sTableName);
        }
        public bool DelValueByFlag(string sWordName, int nWordValue, string sTableName)
        {
            return DelValueByFlag(sWordName, nWordValue.ToString(), ConstDataBase.INT_TYPE, sTableName);
        }
        public bool DelValueByFlag(string sWordName, float fWordValue, string sTableName)
        {
            return DelValueByFlag(sWordName, fWordValue.ToString(), ConstDataBase.FLOAT_TYPE, sTableName);
        }
        public bool DelValueByFlag(string sWordName, DateTime dtWordValue, string sTableName)
        {
            return DelValueByFlag(sWordName, dtWordValue.ToString("yyyy-MM-dd"), ConstDataBase.DATETIME_TYPE, sTableName);
        }
        public bool DelValueByFlag(string sWordName, bool bWordValue, string sTableName)
        {
            if (bWordValue)
                return DelValueByFlag(sWordName, "1", ConstDataBase.BOOLEAN_TYPE, sTableName);
            else
                return DelValueByFlag(sWordName, "0", ConstDataBase.BOOLEAN_TYPE, sTableName);
        }
        #endregion

        public bool DelValueByFlag(string sWordName, string sWordValue, int nConstDataValue, string sTableName)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            arrWordName.Add(sWordName);
            arrWordValue.Add(sWordValue);
            arrWordType.Add(nConstDataValue);

            return DelValueByFlag(arrWordName, arrWordValue, arrWordType, sTableName);
        }

        public bool DelValueByFlag(ArrayList arrWordName, ArrayList arrWordValue, ArrayList arrWordType, string sTableName)
        {
            if ((arrWordName.Count != arrWordValue.Count) || (arrWordName.Count != arrWordType.Count))
                return false;

            string sSQL = "update " + sTableName + " set DelFlag=1 where ";
            int i;
            for (i = 0; i < arrWordName.Count; i++)
            {
                if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRING_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRINGL_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.PIC_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.SELSTR_TYPE))
                    sSQL = sSQL + arrWordName[i] + "='" + arrWordValue[i] + "' and ";
                else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.INT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.FLOAT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STATUS_TYPE))
                    sSQL = sSQL + arrWordName[i] + "=" + arrWordValue[i] + " and ";
                else if (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.DATETIME_TYPE)
                    sSQL = sSQL + arrWordName[i] + "='" + arrWordValue[i] + "' and ";
                else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEAN_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEANBOX_TYPE))
                    sSQL = sSQL + arrWordName[i] + "=" + Convert.ToInt16(arrWordValue[i]) + " and ";
            }
            sSQL = sSQL.Remove(sSQL.Length - 5, 5);

            if (!VerifyUpdateSQL(sSQL))
                return false;

            if (OpenConnection())
                return ExcuteSql(sSQL);
            else
                return false;
        }
        #endregion

        #region DeleteValue
        #region Delete Value By ID
        public bool DeleteValue(Int64 nID, string sTableName)
        {
            return DeleteValue(nID.ToString(), sTableName);
        }
        public bool DeleteValue(string sID, string sTableName)
        {
            return DeleteValue("ID", sID, ConstDataBase.BOOLEAN_TYPE, sTableName);
        }
        #endregion

        #region Delete Value Direct
        public bool DeleteValue(string sWordName, string sWordValue, string sTableName)
        {
            return DeleteValue(sWordName, sWordValue, ConstDataBase.STRING_TYPE, sTableName);
        }
        public bool DeleteValue(string sWordName, int nWordValue, string sTableName)
        {
            return DeleteValue(sWordName, nWordValue.ToString(), ConstDataBase.INT_TYPE, sTableName);
        }
        public bool DeleteValue(string sWordName, float fWordValue, string sTableName)
        {
            return DeleteValue(sWordName, fWordValue.ToString(), ConstDataBase.FLOAT_TYPE, sTableName);
        }
        public bool DeleteValue(string sWordName, DateTime dtWordValue, string sTableName)
        {
            return DeleteValue(sWordName, dtWordValue.ToString("yyyy-MM-dd"), ConstDataBase.DATETIME_TYPE, sTableName);
        }
        public bool DeleteValue(string sWordName, bool bWordValue, string sTableName)
        {
            if (bWordValue)
                return DeleteValue(sWordName, "1", ConstDataBase.BOOLEAN_TYPE, sTableName);
            else
                return DeleteValue(sWordName, "0", ConstDataBase.BOOLEAN_TYPE, sTableName);
        }
        #endregion

        public bool DeleteValue(string sWordName, string sWordValue, int nConstDataValue, string sTableName)
        {
            ArrayList arrWordName = new ArrayList();
            ArrayList arrWordValue = new ArrayList();
            ArrayList arrWordType = new ArrayList();

            arrWordName.Add(sWordName);
            arrWordValue.Add(sWordValue);
            arrWordType.Add(nConstDataValue);

            return DeleteValue(arrWordName, arrWordValue, arrWordType, sTableName);
        }

        public bool DeleteValue(ArrayList arrWordName, ArrayList arrWordValue, ArrayList arrWordType, string sTableName)
        {
            if ((arrWordName.Count != arrWordValue.Count) || (arrWordName.Count != arrWordType.Count))
                return false;

            string sSQL = "delete from " + sTableName + " where ";
            int i;
            for (i = 0; i < arrWordName.Count; i++)
            {
                if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRING_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STRINGL_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.PIC_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.SELSTR_TYPE))
                    sSQL = sSQL + arrWordName[i] + "='" + arrWordValue[i] + "' and ";
                else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.INT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.FLOAT_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.STATUS_TYPE))
                    sSQL = sSQL + arrWordName[i] + "=" + arrWordValue[i] + " and ";
                else if (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.DATETIME_TYPE)
                    sSQL = sSQL + arrWordName[i] + "='" + arrWordValue[i] + "' and ";
                else if ((Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEAN_TYPE) || (Convert.ToInt16(arrWordType[i].ToString()) == ConstDataBase.BOOLEANBOX_TYPE))
                    sSQL = sSQL + arrWordName[i] + "=" + Convert.ToInt16(arrWordValue[i]) + " and ";
            }
            sSQL = sSQL.Remove(sSQL.Length - 5, 5);

            if (!VerifyDeleteSQL(sSQL))
                return false;

            if (OpenConnection())
                return ExcuteSql(sSQL);
            else
                return false;
        }
        #endregion

        /// <summary>
        /// 删除回收站(DelFlag=1)
        /// </summary>
        /// <param name="sTableName">表名</param>
        public void DeleteRecycle(string sTableName)
        {
            DeleteValue("DelFlag", true, sTableName);
        }

        /// <summary>
        /// 删除一张表中的所有内容。注意慎用此函数。
        /// </summary>
        /// <param name="sTableName">表名</param>
        public bool DeleteAll(string sTableName)
        {
            string sSQL = "truncate table " + sTableName;
            if (!VerifyTruncateSQL(sSQL))
                return false;

            if (OpenConnection())
                return ExcuteSql(sSQL);
            else
                return false;
        }

        #endregion       

        protected bool ExcuteSql(string sSQL)
        {
            if (ConnStr.Contains("Provider"))
            {
                this.oleDbCommand = new OleDbCommand(sSQL, this.oleDbConn);
                this.oleDbCommand.ExecuteNonQuery();
                this.CloseConnection();
                return true;
            }
            else
            {
                this.sqlCommand = new SqlCommand(sSQL, this.sqlConn);
                this.sqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                return true;
            }
        }

    }
}
