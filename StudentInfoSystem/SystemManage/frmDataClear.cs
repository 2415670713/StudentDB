using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace StudentDB.SystemManage
{
    public partial class frmDataClear : Form
    {
        public frmDataClear()
        {
            InitializeComponent();
        }



        SqlConnection conn;
        SqlCommand com;

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            try
            {
                if (chkStudent.Checked)
                {
                    string sqlString = "delete from 学生基本信息表";
                    com = new SqlCommand(sqlString, conn);
                    com.ExecuteNonQuery();
                }
                if (chkUser.Checked)
                {
                    string sqlString = "delete from 用户表";
                    com = new SqlCommand(sqlString, conn);
                    com.ExecuteNonQuery();
                }
                if (chkClass.Checked)
                {
                    string sqlString = "delete from 班级表";
                    com = new SqlCommand(sqlString, conn);
                    com.ExecuteNonQuery();
                }
                if (chkDepartment.Checked)
                {
                    string sqlString = "delete from 系别表";
                    com = new SqlCommand(sqlString, conn);
                    com.ExecuteNonQuery();
                }
                if (chkSpeciality.Checked)
                {
                    string sqlString = "delete from 专业表";
                    com = new SqlCommand(sqlString, conn);
                    com.ExecuteNonQuery();
                }
                MessageBox.Show("数据清理成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             
             catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            
             
                conn.Close();
              
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDataClear_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";
        }

       
    }
}