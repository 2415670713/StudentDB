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
    public partial class frmUserManage : Form
    {
        public frmUserManage()
        {
            InitializeComponent();
        }
        //表格控件的ReadOnly属性设为true,SelectionMode属性设置为FullRowSelect.
        //保存和取消按钮的Enabled属性设置为false.

        int AddOrEdit;//标记是添加或修改了记录
        SqlConnection conn;
        SqlCommand com;
        SqlDataAdapter da;

        private void FillDataGridView()//填充表格数据
        {
            da = new SqlDataAdapter("select * from 用户表 order by ID", conn);
            DataSet ds = new DataSet();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count = da.Fill(ds, "用户表");
                conn.Close();
                dgvUser.DataSource = ds.Tables["用户表"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        

        private void dgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUserName.Text = Convert.ToString(dgvUser[1, dgvUser.CurrentCell.RowIndex].Value).Trim();
            txtUserPwd.Text = Convert.ToString(dgvUser[2, dgvUser.CurrentCell.RowIndex].Value).Trim();
            cbxUserRight.Text = Convert.ToString(dgvUser[3, dgvUser.CurrentCell.RowIndex].Value).Trim();

        }

        private void ClearText()//清除文本框内容
        {
            txtUserName.Clear();
            txtUserPwd.Clear();
            
        }

        private void LockedTextBox()//锁定文本框
        {
            txtUserName.Enabled = false;
            txtUserPwd.Enabled = false;
            cbxUserRight.Enabled = false;
        }

        private void UnLockedTextBox()//解除锁定
        {
            txtUserName.Enabled = true;
            txtUserPwd.Enabled = true ;
            cbxUserRight.Enabled =true ;
        }


        private void tsbAdd_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 0;
            ClearText();
            UnLockedTextBox();
            txtUserName.Focus();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 1;
            UnLockedTextBox();
            txtUserName.Focus();
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除该用户信息吗？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    com = new SqlCommand("delete from 用户表 where ID=" + Convert.ToString(dgvUser[0, dgvUser.CurrentCell.RowIndex].Value).Trim(), conn);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    com.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("删除数据成功！");
                    ClearText();
                    FillDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                MessageBox.Show("用户名称不能为空！");
                return;
            }
            if (txtUserPwd.Text == "")
            {
                MessageBox.Show("用户密码不能为空！");
                return;
            }

            if (cbxUserRight.Text == "")
            {
                MessageBox.Show("用户权限不能为空！");
                return;
            }
            
            if (AddOrEdit == 0)
            {
                //添加记录后的保存
                try
                {

                    //判断用户名称是否已存在
                    da = new SqlDataAdapter("select * from 用户表 where 用户名称='" + txtUserName.Text + "'", conn);
                    DataSet ds = new DataSet();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    int count = da.Fill(ds, "用户表");
                    conn.Close();
                    if (count != 0)
                    {
                        MessageBox.Show("该用户名称已存在，请重新输入！");
                        txtUserName.Focus();
                        return;
                    }

                    com = new SqlCommand("insert into 用户表(用户名称,用户密码,用户权限)values('" + txtUserName.Text.Trim() + "','" + txtUserPwd.Text.Trim() + "','" + cbxUserRight.Text.Trim() + "')", conn);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    com.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("添加数据成功！");
                    FillDataGridView();
                    LockedTextBox();
                    tsbSave.Enabled = false;
                    tsbCancel.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
            if (AddOrEdit == 1)
            {
                //修改记录后的保存
                try
                {
                    com = new SqlCommand("update 用户表 set 用户名称='" + txtUserName.Text.Trim() + "',用户密码='" + txtUserPwd.Text.Trim() + "',用户权限='" + cbxUserRight.Text.Trim() + "' where ID=" + Convert.ToString(dgvUser[0, dgvUser.CurrentCell.RowIndex].Value).Trim(), conn);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    com.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("修改数据成功！");
                    FillDataGridView();
                    LockedTextBox();
                    tsbSave.Enabled = false;
                    tsbCancel.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            LockedTextBox();
            tsbCancel.Enabled = false;
            tsbSave.Enabled = false;
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUserManage_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";

            tsbSave.Enabled = false;//保存按钮无效
            tsbCancel.Enabled = false;//取消按钮无效
            
            cbxUserRight.Items.Add("administrator");
            cbxUserRight.Items.Add("user");

            FillDataGridView();
            LockedTextBox();

        }



    }
}