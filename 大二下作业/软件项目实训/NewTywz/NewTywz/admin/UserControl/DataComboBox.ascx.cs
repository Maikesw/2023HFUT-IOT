using Ext.Net;
using NewTywz.baseclass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

//用户控件：DataComboBox。大部分comboBox都应该用它。
//编写人：石雷
namespace NewTywz.UserControl
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DataComboBox")]
    public partial class DataComboBox : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();


        #region 设定传递的一些value
        public string Value
        {
            get
            {
                return DataComboBox_cmbBox.SelectedItem.Value;
            }
            set
            {
                DataComboBox_cmbBox.SetValue(value);
            }
        }
        public string Text
        {
            get
            {
                return DataComboBox_cmbBox.SelectedItem.Text;
            }
        }
        public string sTableName
        {
            get
            {
                return DataComboBox_TableName.Text;
            }
            set
            {
                DataComboBox_TableName.Text = value;
            }
        }
        public bool bNeedOrder
        {
            get
            {
                if (DataComboBox_NeedOrder.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    DataComboBox_NeedOrder.Text = "1";
                else
                    DataComboBox_NeedOrder.Text = "0";
            }
        }
        public string sWhere
        {
            get
            {
                return DataComboBox_Where.Text;
            }
            set
            {
                DataComboBox_Where.Text = value;
            }
        }
        public bool bHasDelFlag
        {
            get
            {
                if (DataComboBox_HasDelFlag.Text == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    DataComboBox_HasDelFlag.Text = "1";
                else
                    DataComboBox_HasDelFlag.Text = "0";
            }
        }

        public string cls
        {
            get
            {
                return DataComboBox_cmbBox.Cls;
            }
            set
            {
                DataComboBox_cmbBox.Cls = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return DataComboBox_cmbBox.ReadOnly;
            }
            set
            {
                DataComboBox_cmbBox.ReadOnly = value;
            }
        }

        public string EmptyText
        {
            get
            {
                return DataComboBox_cmbBox.EmptyText;
            }
            set
            {
                DataComboBox_cmbBox.EmptyText = value;
            }
        }

        public int Width
        {
            get { return Convert.ToInt32(DataComboBox_cmbBox.Width); }
            set
            {
                Unit u = new Unit(value);
                DataComboBox_cmbBox.Width = u;
            }
        }

        public bool Editable
        {
            get { return DataComboBox_cmbBox.Editable; }
            set { DataComboBox_cmbBox.Editable = value; }
        }

        /// <summary>
        /// 设置/返回当前控件的是否隐藏
        /// </summary>
        public bool Hidden
        {
            get
            {
                return DataComboBox_cmbBox.Hidden;
            }
            set
            {
                DataComboBox_cmbBox.Hidden = value;
            }

        }

        /// <summary>
        /// 设置/返回当前控件的是否禁用
        /// </summary>
        public bool Disabled
        {
            get
            {
                return DataComboBox_cmbBox.Disabled;
            }
            set
            {
                DataComboBox_cmbBox.Disabled = value;
            }

        }
        #endregion

        #region InitCtrl
        public void InitCtrl(string sName)
        {
            InitCtrl(sName, true);
        }

        public void InitCtrl(string sName, bool order)
        {
            InitCtrl(sName, order, "");
        }
        public void InitCtrl(string sName, bool order, string sWhere)
        {
            InitCtrl(sName, order, sWhere, true);
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="sName">数据表名sTableName</param>
        /// <param name="order">要排序吗</param>
        /// <param name="sWhere">查询语句中的where</param>
        /// <param name="bDelFlag">是否考虑DelFlag，true考虑</param>
        public void InitCtrl(string name, bool order, string where, bool bDelFlag)
        {
            if (name != "")
            {
                sTableName = name;
                bNeedOrder = order;
                sWhere = where;
                bHasDelFlag = bDelFlag;
                string sSQL = "select ID,Name from " + name + " where ";
                if (bDelFlag)
                    sSQL = sSQL + " DelFlag=0 ";

                if (where != "")
                {
                    if (bDelFlag)
                        sSQL = sSQL + " and " + sWhere + " ";
                    else
                        sSQL = sSQL + sWhere + " ";
                }
                else
                {
                    if (!bDelFlag)
                        sSQL = sSQL.Remove(sSQL.Length - 7, 7);
                }

                if (order)
                    sSQL = sSQL + " Order By TheOrder";

                DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);

                List<object> data = new List<object>(dt.Rows.Count);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data.Add(new
                    {

                        Value = dt.Rows[i]["ID"].ToString(),
                        Text = dt.Rows[i]["Name"].ToString()
                    });
                }

                DataComboBox_storeInfo.DataSource = data;
                DataComboBox_storeInfo.DataBind();
                if (dt.Rows.Count == 1)
                    DataComboBox_cmbBox.SelectedItem.Value = dt.Rows[0]["ID"].ToString();
            }
            else
            {
                List<object> data = new List<object>(0);
                DataComboBox_storeInfo.DataSource = data;
                DataComboBox_storeInfo.DataBind();
            }
        }

        #endregion

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            DataComboBox_cmbBox.Clear();
        }


        /// <summary>
        /// 设置当前combo选择项
        /// </summary>
        /// <param name="sValue">sID</param>
        public void SetValue(string sValue)
        {
            if ((sValue != "0") && (sValue != "-1") && (sValue != ""))
                DataComboBox_cmbBox.SetValue(sValue);
            else
                DataComboBox_cmbBox.SetValue(null);
        }
        /// <summary>
        /// 获取当前选择的value
        /// </summary>
        /// <returns>value</returns>
        public string GetValue()
        {
            return DataComboBox_cmbBox.SelectedItem.Value;
        }
        /// <summary>
        /// 获取当前选择的text
        /// </summary>
        /// <returns>text</returns>
        public string GetText()
        {
            return DataComboBox_cmbBox.SelectedItem.Text;
        }


        protected void Page_Load(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// 选中某一条后产生的用户控件自定义事件
        /// </summary>
        public event EventHandler SelChange; //添加事件句柄 
        public void ItemChange(Object Sender, EventArgs e)
        {
            if (SelChange != null)
                SelChange(this, new EventArgs()); //激活事件    
        }

    }
}