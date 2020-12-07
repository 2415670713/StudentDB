using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace StudentDB
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        public static string userName;//记录登录用户名字//主窗体中使用
        public static string userRight;//记录登录用户的权限//主窗体中使用

        SqlConnection conn;
        SqlCommand com;
        SqlDataAdapter da;
        SqlDataReader dr;
        private void frmLogin_Load(object sender, EventArgs e)
        {

            try
            {

                conn = new SqlConnection();
                conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";
                DataSet ds = new DataSet();
                da = new SqlDataAdapter("select 用户名称 from 用户表", conn);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(ds, "用户表");
                conn.Close();
                cbxUserName.DataSource = ds.Tables["用户表"];
                cbxUserName.DisplayMember = "用户名称";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void cbxUserName_SelectedIndexChanged(object sender, EventArgs e)//查询权限值
        {
            try
            {
                com = new SqlCommand("select 用户名称,用户权限 from 用户表 where 用户名称='" + cbxUserName.Text + "'", conn);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                dr = com.ExecuteReader();


                if (dr.Read())
                {
                    lblUserRight.Text = dr["用户权限"].ToString();
                    userRight = lblUserRight.Text;
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)//可以用读取器类也可以用数据集类实现
        {
            //用数据集类实现
            try
            {
                da = new SqlDataAdapter("select * from 用户表 where 用户名称='" + cbxUserName.Text.Trim() + "' and 用户密码='" + txtUserPwd.Text.Trim() + "'", conn);
                DataSet ds = new DataSet();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(ds, "用户表");
                conn.Close();
                if (ds.Tables["用户表"].Rows.Count > 0)
                {
                    userName = cbxUserName.Text;

                    frmMain frmmain = new frmMain();
                    this.Hide();
                    frmmain.Show();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserPwd.Text = "";
                    cbxUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            //用读取器类实现
            try
            {
                com = new SqlCommand("select * from 用户表 where 用户名称='" + cbxUserName.Text.Trim() + "' and 用户密码='" + txtUserPwd.Text.Trim() + "'", conn);

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                dr = com.ExecuteReader();

                if (dr.Read())
                {
                    userName = cbxUserName.Text;

                    frmMain frmmain = new frmMain();
                    this.Hide();
                    frmmain.Show();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserPwd.Text = "";
                    cbxUserName.Focus();
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }                                                            

        

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}