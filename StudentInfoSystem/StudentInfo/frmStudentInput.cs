using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;//包含对EXCEL文件进行连接或操作的类，EXCEL文件也是一种数据源
using System.Data.SqlClient;

namespace StudentDB.StudentInfo
{
    public partial class frmStudentInput : Form
    {
        public frmStudentInput()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        SqlCommand com;
        DataTable dt;//存放导入的数据


        public DataSet ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn1 = new OleDbConnection(strConn);
            string strExcel = "";
            OleDbDataAdapter da1 = null;
            DataSet ds = null;
            strExcel = "select 学号,姓名,性别,民族,身份证号,出生日期,家庭住址,家庭电话,班级编号,个人简历 from [Sheet1$]";
            da1 = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            if (conn1.State == ConnectionState.Closed)
            {
                conn1.Open();
            }
            da1.Fill(ds);
            conn1.Close();
            return ds;
        }

        private void tsbInput_Click(object sender, EventArgs e)//导入EXCEL文件
        {
            string fileName="";
            try
            {
                openFileDialog1.Filter = "Excel文件|*.xls";
                openFileDialog1.ShowDialog();
                
                fileName = openFileDialog1.FileName.ToString();
               
             }
            catch (Exception ex)
             {
                MessageBox.Show("打开文件出错！" + ex.Message.ToString());
             }
             try
             {

                 if (fileName != "")//如果已选了EXCEL文件
                 {
                     DataSet ds = ExcelToDS(fileName);
                     dt = new DataTable();
                     dt = ds.Tables[0];
                     this.dgvStudent.DataSource = dt;//将数据在表格中显示
                     tsbSave.Enabled = true;
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show("导入文件出错！" + ex.Message.ToString());
             }
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmStudentInput_Load(object sender, EventArgs e)
        {
            this.dgvStudent.ReadOnly = true; //设置为只读
            tsbSave.Enabled = false;

        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {

            MessageBox.Show("您可直接在表格中对数据进行修改！");
            this.dgvStudent.ReadOnly = false ; 
            
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            this.dgvStudent.ReadOnly = false; 
            MessageBox.Show("请单击表格列的左侧，选中要删除的行，然后直接按“Delete”键删除该行!");
        }

        private void tsbSave_Click(object sender, EventArgs e)//保存到数据库
        {
            //添加到后台数据库中
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";
            com = conn.CreateCommand();
            com.CommandText = "insert into 学生基本信息表(学号,姓名,性别,民族,身份证号,出生日期,家庭住址,家庭电话,班级编号,个人简历) values(@学号,@姓名,@性别,@民族,@身份证号,@出生日期,@家庭住址,@家庭电话,@班级编号,@个人简历)";
            com.Parameters.Add(new SqlParameter("@学号", SqlDbType.NChar));
            com.Parameters.Add(new SqlParameter("@姓名", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@性别", SqlDbType.NChar));
            com.Parameters.Add(new SqlParameter("@民族", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@身份证号", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@出生日期", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@家庭住址", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@家庭电话", SqlDbType.NVarChar));
            com.Parameters.Add(new SqlParameter("@班级编号", SqlDbType.NChar));
            com.Parameters.Add(new SqlParameter("@个人简历", SqlDbType.NVarChar));

            try
            {
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
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    com.ExecuteNonQuery();
                    conn.Close();
                }
                MessageBox.Show("已成功将EXCEL数据导入到数据库中！");
                dgvStudent.DataSource = null;
                dgvStudent.ReadOnly = true;
            }
            
            catch (Exception ex)
              {
                   MessageBox.Show("保存数据有误！　"+ex.Message.ToString());
              }
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("您可直接在表格中的最后添加新的记录！");
            this.dgvStudent.ReadOnly = false; 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}