using Ext.Net;
using NewTywz.baseclass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

//用户控件：TreeListDataComboBox。适用于：省市县、部门一级二级三级……
//编写人：石雷
namespace NewTywz.UserControl
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "ThreeListComboBox")]
    public partial class ThreeListComboBox : System.Web.UI.UserControl
    {
        private DbObject cprivDBO = new DbObject();

        #region 设定传递的一些value
        public string sTableName
        {
            get
            {
                return ThreeListComboBox_TableName.Text;
            }
            set
            {
                ThreeListComboBox_TableName.Text = value;
            }
        }
        public int Width
        {
            get { return Convert.ToInt32(ThreeListComboBox_cmbBox1.Width); }
            set
            {
                Unit u = new Unit(value);
                ThreeListComboBox_cmbBox1.Width = u;
                ThreeListComboBox_cmbBox2.Width = u;
                ThreeListComboBox_cmbBox3.Width = u;
            }
        }

        public string cls
        {
            get { return ThreeListComboBox_cmbBox1.Cls; }
            set
            {
                ThreeListComboBox_cmbBox1.Cls = value;
                ThreeListComboBox_cmbBox2.Cls = value;
                ThreeListComboBox_cmbBox3.Cls = value;
            }
        }

        public bool ReadOnly
        {
            get { return ThreeListComboBox_cmbBox1.ReadOnly; }
            set
            {
                ThreeListComboBox_cmbBox1.ReadOnly = value;
                ThreeListComboBox_cmbBox2.ReadOnly = value;
                ThreeListComboBox_cmbBox3.ReadOnly = value;
            }
        }

        public bool Disabled
        {
            get { return ThreeListComboBox_cmbBox1.Disabled; }
            set
            {
                ThreeListComboBox_cmbBox1.Disabled = value;
                ThreeListComboBox_cmbBox2.Disabled = value;
                ThreeListComboBox_cmbBox3.Disabled = value;
            }
        }

        #endregion 

        /// <summary>
        /// 选中某一条后产生的用户控件自定义事件
        /// </summary>
        public event EventHandler SelChange; //添加事件句柄 
        public void ItemChange1(Object Sender, EventArgs e)
        {
            InitStore2();
            if (SelChange != null)
                SelChange(this, new EventArgs()); //激活事件    
        }
        public void ItemChange2(Object Sender, EventArgs e)
        {
            InitStore3();
            if (SelChange != null)
                SelChange(this, new EventArgs()); //激活事件    
        }
        public void ItemChange3(Object Sender, EventArgs e)
        {
            if (SelChange != null)
                SelChange(this, new EventArgs()); //激活事件    
        }

        public void InitStore2()
        {
            ThreeListComboBox_storeCombo3.RemoveAll();
            InitSingleCtrl("2", ThreeListComboBox_cmbBox1.SelectedItem.Value);
        }

        public void InitStore3()
        {
            InitSingleCtrl("3", ThreeListComboBox_cmbBox2.SelectedItem.Value);
        }

        /// <summary>
        /// 初始化控件。宽度默认80
        /// </summary>
        /// <param name="sName">数据表名sTableName</param>
        public void InitCtrl(string sName)
        {
            sTableName = sName;
            ThreeListComboBox_storeCombo2.RemoveAll();
            ThreeListComboBox_storeCombo3.RemoveAll();
            InitSingleCtrl("1", "-1");
        }

        /// <summary>
        /// 初始化combo的store
        /// </summary>
        /// <param name="sLevel">等级。只能为"1"、"2"、"3"</param>
        /// <param name="sParentIndex">父节点编号</param>
        public void InitSingleCtrl(string sLevel, string sParentIndex)
        {
            string sSQL = "select ID,Name from " + sTableName + " where DelFlag=0 and TheLevel=" + sLevel + " and ParentIndex=" + sParentIndex + " order by TheOrder";
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

            if (sLevel == "1")
            {
                ThreeListComboBox_storeCombo1.DataSource = data;
                ThreeListComboBox_storeCombo1.DataBind();
                ThreeListComboBox_cmbBox2.SetValue(null);
                ThreeListComboBox_cmbBox3.SetValue(null);
            }
            else if (sLevel == "2")
            {
                ThreeListComboBox_storeCombo2.DataSource = data;
                ThreeListComboBox_storeCombo2.DataBind();
                ThreeListComboBox_cmbBox2.SetValue(null);
                ThreeListComboBox_cmbBox3.SetValue(null);
            }
            else if (sLevel == "3")
            {
                ThreeListComboBox_storeCombo3.DataSource = data;
                ThreeListComboBox_storeCombo3.DataBind();
                ThreeListComboBox_cmbBox3.SetValue(null);
            }
        }

        /// <summary>
        /// 获取当前combo选择项的value
        /// </summary>
        /// <returns>Value</returns>
        public string GetValue()
        {
            if (ThreeListComboBox_cmbBox3.SelectedItem.Value != "")
                return ThreeListComboBox_cmbBox3.SelectedItem.Value;
            else if (ThreeListComboBox_cmbBox2.SelectedItem.Value != "")
                return ThreeListComboBox_cmbBox2.SelectedItem.Value;
            else if (ThreeListComboBox_cmbBox1.SelectedItem.Value != "")
                return ThreeListComboBox_cmbBox1.SelectedItem.Value;
            else
                return "0";
        }
        /// <summary>
        /// 获取当前combo选择项的text
        /// </summary>
        /// <returns>Text</returns>
        public string GetText()
        {
            string sText;
            if (ThreeListComboBox_cmbBox3.SelectedItem.Value != "")
                sText = ThreeListComboBox_cmbBox3.SelectedItem.Text;
            else if (ThreeListComboBox_cmbBox2.SelectedItem.Value != "")
                sText = ThreeListComboBox_cmbBox2.SelectedItem.Text;// + sText;
            else
                sText = ThreeListComboBox_cmbBox1.SelectedItem.Text;// +sText;
            return sText;
        }

        /// <summary>
        /// 获取当前combo选择项的全部text
        /// </summary>
        /// <returns>Text</returns>
        public string GetAllText()
        {
            string sText;
            if (ThreeListComboBox_cmbBox3.SelectedItem.Value != "")
                sText = ThreeListComboBox_cmbBox1.SelectedItem.Text + ThreeListComboBox_cmbBox2.SelectedItem.Text + ThreeListComboBox_cmbBox3.SelectedItem.Text;
            else if (ThreeListComboBox_cmbBox2.SelectedItem.Value != "")
                sText = ThreeListComboBox_cmbBox1.SelectedItem.Text + ThreeListComboBox_cmbBox2.SelectedItem.Text;
            else
                sText = ThreeListComboBox_cmbBox1.SelectedItem.Text;
            return sText;
        }

        /// <summary>
        /// 返回当前所处Level
        /// </summary>
        /// <returns></returns>
        public int GetLevel()
        {
            if (ThreeListComboBox_cmbBox3.SelectedItem.Value != "")
                return 3;
            if (ThreeListComboBox_cmbBox2.SelectedItem.Value != "")
                return 2;
            if (ThreeListComboBox_cmbBox1.SelectedItem.Value != "")
                return 1;
            return 0;
        }

        /// <summary>
        /// 设置当前combo选择项
        /// </summary>
        /// <param name="sValue">sID</param>
        public void SetValue(string sValue)
        {
            if ((sValue != "0") && (sValue != "-1") && (sValue != ""))
            {
                string sSQL = "select ID,TheLevel,ParentIndex from " + sTableName + " where ID=" + sValue;
                DataTable dt = cprivDBO.ExcuteSqlTable(sSQL);
                string sID1, sID2, sID3;

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt16(dt.Rows[0]["TheLevel"]) == 3) //三层
                    {
                        InitSingleCtrl("3", dt.Rows[0]["ParentIndex"].ToString());
                        sID3 = dt.Rows[0]["ID"].ToString();
                        //二层
                        sSQL = "select ID,ParentIndex from " + sTableName + " where ID=" + dt.Rows[0]["ParentIndex"].ToString();
                        dt = cprivDBO.ExcuteSqlTable(sSQL);
                        InitSingleCtrl("2", dt.Rows[0]["ParentIndex"].ToString());
                        sID2 = dt.Rows[0]["ID"].ToString();
                        //一层
                        sID1 = dt.Rows[0]["ParentIndex"].ToString();

                        ThreeListComboBox_cmbBox3.SetValue(sID3);
                        ThreeListComboBox_cmbBox2.SetValue(sID2);
                        ThreeListComboBox_cmbBox1.SetValue(sID1);
                    }
                    else if (Convert.ToInt16(dt.Rows[0]["TheLevel"]) == 2) //二层
                    {
                        //三层
                        InitSingleCtrl("3", dt.Rows[0]["ID"].ToString());
                        sID3 = "0";
                        //二层
                        InitSingleCtrl("2", dt.Rows[0]["ParentIndex"].ToString());
                        sID2 = dt.Rows[0]["ID"].ToString();
                        //一层
                        sID1 = dt.Rows[0]["ParentIndex"].ToString();

                        ThreeListComboBox_cmbBox3.SetValue(null);
                        ThreeListComboBox_cmbBox2.SetValue(sID2);
                        ThreeListComboBox_cmbBox1.SetValue(sID1);
                    }
                    else if (Convert.ToInt16(dt.Rows[0]["TheLevel"]) == 1) //一层
                    {
                        //三层
                        ThreeListComboBox_storeCombo3.RemoveAll();
                        sID3 = "0";
                        //二层
                        InitSingleCtrl("2", dt.Rows[0]["ID"].ToString());
                        sID2 = "0";
                        //一层
                        sID1 = dt.Rows[0]["ID"].ToString();

                        ThreeListComboBox_cmbBox3.SetValue(null);
                        ThreeListComboBox_cmbBox2.SetValue(null);
                        ThreeListComboBox_cmbBox1.SetValue(sID1);
                    }
                }
            }
            else
            {
                ThreeListComboBox_storeCombo3.RemoveAll();
                ThreeListComboBox_storeCombo2.RemoveAll();
                ThreeListComboBox_cmbBox3.SetValue(null);
                ThreeListComboBox_cmbBox2.SetValue(null);
                ThreeListComboBox_cmbBox1.SetValue(null);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}