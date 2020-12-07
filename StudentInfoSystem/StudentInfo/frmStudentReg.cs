using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Text.RegularExpressions;//引用正则表达式类的命名空间


namespace StudentDB.StudentInfo
{
    public partial class frmStudentReg : Form
    {
        public frmStudentReg()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        SqlCommand com;
        SqlDataAdapter da;
        DataTable dt;//声明存储数据的临时表dt

        private void ClearText()
        {
            this.txtStudentNo.Clear();
            this.txtStudentName.Clear();
            this.cbxSex.Text = "";
            this.cbxNationlity.Text = "";
            this.txtCardID.Clear();
            this.dtpBirthday.Value = Convert.ToDateTime("1990-1-1");//设定初始值
            this.txtAddress.Clear();
            this.txtTel.Clear();
            this.txtMemory.Clear();
        }

        private void frmStudentReg_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";

            this.cbxDepartment.Items.Clear();
            da = new SqlDataAdapter("select 系名称 from 系别表",conn);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
           
            da.Fill(ds,"系别表");
            
            conn.Close();
          
            this.cbxDepartment.DataSource=ds.Tables[0];
            this.cbxDepartment.DisplayMember = "系名称";
            this.cbxDepartment.ValueMember = "系名称";

            this.cbxSex.Items.Add("男");
            this.cbxSex.Items.Add("女");

            this.cbxNationlity.Items.Add("汉族");
            this.cbxNationlity.Items.Add("回族");
            this.cbxNationlity.Items.Add("朝鲜族");
            this.cbxNationlity.Items.Add("满族");
            this.cbxNationlity.Items.Add("藏族");
            this.cbxNationlity.Items.Add("维吾尔族");


            //创建存储数据的临时表(也可不设置主键，但设为主键后，可以保证学号值不会重复)
            dt = new DataTable("学生基本信息表");
            DataColumn[] mykey = new DataColumn[1];//创建包含一个元素的数据列数组
            DataColumn column1 = new DataColumn();
            column1.ColumnName = "学号";
            dt.Columns.Add(column1);
            mykey[0] = column1;
            dt.PrimaryKey = mykey;//设置主键
            //添加其它列
            dt.Columns.Add("姓名",typeof(string));
            dt.Columns.Add("性别",typeof(string));
            dt.Columns.Add("民族",typeof(string));
            dt.Columns.Add("身份证号",typeof(string));
            dt.Columns.Add("出生日期", typeof(string));
            dt.Columns.Add("家庭住址", typeof(string));
            dt.Columns.Add("家庭电话", typeof(string));
            dt.Columns.Add("班级编号", typeof(string));
            dt.Columns.Add("个人简历", typeof(string));
        }

        private void cbxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取选中系别的所有班级名称信息
            da = new SqlDataAdapter("select 班级编号,班级名称 from 班级表 where 所属系别='"+cbxDepartment.Text+"'", conn);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            da.Fill(ds, "班级表");
            
                conn.Close();
           
            this.cbxClassName.DataSource = ds.Tables[0];
            this.cbxClassName.DisplayMember = "班级名称";
            this.cbxClassName.ValueMember = "班级编号"; 
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //应判断是否为空值，是否是合法数据等。errorProvider控件显示错误提示
            if (txtStudentNo.Text == "")
            {
                MessageBox.Show("学号不能为空！");
                return;
            }
            if (txtStudentName.Text == "")
            {
                MessageBox.Show("姓名不能为空！");
                return;
            }

            if (this.cbxSex.Text == "")
            {
                MessageBox.Show("性别不能为空！");
                return;
            }
            if (!Regex.IsMatch(txtStudentNo.Text.Trim(), "^[0-9]*$"))//学号必须为数字
            {
                errorStudentNo.SetError(txtStudentNo, "学号必须为数字！");
                return;
            }

            if (!Regex.IsMatch(txtCardID.Text.Trim(), "\\d{15}|\\d{18}"))//验证身份证号
            {
                errorCardID.SetError(txtCardID, "身份证号输入不合法！");
                return;
            }
            if (!Regex.IsMatch(txtTel.Text.Trim(), "\\d{3}-\\d{8}|\\d{4}-\\d{7}"))//验证电话号码
            {
                errorTel.SetError(txtTel, "电话号码输入不合法！");
                return;
            }

            errorStudentNo.Clear();
            errorCardID.Clear();
            errorTel.Clear();
            txtStudentNo.Focus();
            try
            {
                DataRow dr = dt.NewRow();//添加新的空白行
                dr["学号"] = txtStudentNo.Text.Trim();
                dr["姓名"] = txtStudentName.Text.Trim();
                dr["性别"] = cbxSex.Text.Trim();
                dr["民族"] = cbxNationlity.Text.Trim();
                dr["身份证号"] = txtCardID.Text.Trim();
                dr["出生日期"] = dtpBirthday.Value.ToString();
                dr["家庭住址"] = txtAddress.Text.Trim();
                dr["家庭电话"] = txtTel.Text.Trim();
                dr["班级编号"] = cbxClassName.SelectedValue.ToString();
                dr["个人简历"] = txtMemory.Text.Trim();


                dt.Rows.Add(dr);//加入新行
                ClearText();
            }
            catch (Exception ex)
            {
                MessageBox.Show("当前记录输入有错！　" + ex.Message.ToString());
            }


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sqlString = "insert into 学生基本信息表(学号,姓名,性别,民族,身份证号,出生日期,家庭住址,家庭电话,班级编号,个人简历) values(@学号,@姓名,@性别,@民族,@身份证号,@出生日期,@家庭住址,@家庭电话,@班级编号,@个人简历)";
            com = new SqlCommand(sqlString,conn);
           
            com.Parameters.Add(new SqlParameter("@学号",SqlDbType.NChar));
            com.Parameters.Add(new SqlParameter("@姓名", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@性别", SqlDbType.NChar));
            com.Parameters.Add(new SqlParameter("@民族", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@身份证号", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@出生日期", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@家庭住址", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@家庭电话", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@班级编号", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@个人简历", SqlDbType.NVarChar));
            
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                com.Parameters["@学号"].Value = dt.Rows[i][0].ToString();
                com.Parameters["@姓名"].Value = dt.Rows[i][1].ToString();
                com.Parameters["@性别"].Value = dt.Rows[i][2].ToString();
                com.Parameters["@民族"].Value = dt.Rows[i][3].ToString();
                com.Parameters["@身份证号"].Value = dt.Rows[i][4].ToString();
                com.Parameters["@出生日期"].Value = dt.Rows[i][5].ToString();
                com.Parameters["@家庭住址"].Value = dt.Rows[i][6].ToString();
                com.Parameters["@家庭电话"].Value = dt.Rows[i][7].ToString();
                com.Parameters["@班级编号"].Value = dt.Rows[i][8].ToString();
                com.Parameters["@个人简历"].Value = dt.Rows[i][9].ToString();
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    com.ExecuteNonQuery();
                    
                        conn.Close();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存到数据库时出错！　"+ex.Message.ToString());
                }
            }
            dt.Rows.Clear();
            this.Close();
            
        }
    }
}