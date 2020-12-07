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
    public partial class frmEditPwd : Form
    {
        public frmEditPwd()
        {
            InitializeComponent();
        }


        //******************************//
        SqlConnection conn;
        SqlDataAdapter da;
        SqlCommand com;

        private void frmEditPwd_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=localhost;uid=sa;pwd=sa123;database=studentDB");
            
            txtUserName.Text = frmLogin.userName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sqlString = "select * from 用户表 where 用户名称='"+txtUserName.Text+"' and 用户密码='"+txtUserOldPwd.Text.Trim()+"'";
            da = new SqlDataAdapter(sqlString,conn);
            DataSet ds = new DataSet();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            int count = da.Fill(ds,"用户表");
            conn.Close();
            if (count == 0)
            {
                MessageBox.Show("用户旧密码输入错误，请重新输入！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserOldPwd.Focus();
                return;
            }
            if (txtUserNewPwd.Text.Trim() != txtUserNewPwd2.Text.Trim())
            {
                MessageBox.Show("两次新密码输入不一致，请重新输入！");
                txtUserNewPwd.Focus();
                return;
            }
            try
            {
                com=new SqlCommand("update 用户表 set 用户密码='" + txtUserNewPwd.Text.Trim() + "' where 用户名称='" + txtUserName.Text.Trim() + "'",conn);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                com.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("密码修改成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
             }
            }

        }

       


    }
