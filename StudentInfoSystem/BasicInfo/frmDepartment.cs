using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using System.Data.SqlClient;
using System.Text.RegularExpressions;//正则表达式类的命名空间

namespace StudentDB.BasicInfo
{
    public partial class frmDepartment : Form
    {
        public frmDepartment()
        {
            InitializeComponent();
        }

        //表格控件的ReadOnly属性设为true,SelectionMode属性设置为FullRowSelect.
        

        int AddOrEdit;//标记是添加或修改了记录
        SqlConnection conn;
        SqlCommand com;
        SqlDataAdapter da;

        private void FillDataGridView()//填充表格数据
        {
            da = new SqlDataAdapter("select * from 系别表 order by ID", conn);
            DataSet ds = new DataSet();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count = da.Fill(ds, "系别表");
                conn.Close();
                dgvDepartment.DataSource = ds.Tables["系别表"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void frmDepartment_Load(object sender, EventArgs e)
        {

            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";
            FillDataGridView();
            LockedTextBox();
            tsbSave.Enabled = false;//保存按钮无效
            tsbCancel.Enabled = false;//取消按钮无效

        }

        private void dgvDepartment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtDName.Text = Convert.ToString(dgvDepartment[1, dgvDepartment.CurrentCell.RowIndex].Value).Trim();
            txtDManager.Text = Convert.ToString(dgvDepartment[2, dgvDepartment.CurrentCell.RowIndex].Value).Trim();
            txtTel.Text = Convert.ToString(dgvDepartment[3, dgvDepartment.CurrentCell.RowIndex].Value).Trim();

        }

        private void ClearText()//清除文本框内容
        {
            txtDName.Clear();
            txtDManager.Clear();
            txtTel.Clear();
        }

        private void LockedTextBox()//锁定文本框
        {
            txtDName.Enabled = false;
            txtDManager.Enabled = false;
            txtTel.Enabled = false;
        }

        private void UnLockedTextBox()//解除锁定
        {
            txtDName.Enabled = true ;
            txtDManager.Enabled = true ;
            txtTel.Enabled = true ;
        }


        private void tsbAdd_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 0;
            ClearText();
            UnLockedTextBox();
            txtDName.Focus();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 1;
            UnLockedTextBox();
            txtDName.Focus();
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除该系信息吗？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    com = new SqlCommand("delete from 系别表 where ID=" + Convert.ToString(dgvDepartment[0, dgvDepartment.CurrentCell.RowIndex].Value).Trim(), conn);
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
            if (txtDName.Text == "")
            {
                MessageBox.Show("系名称不能为空！");
                return;
            }
            if (txtDManager.Text == "")
            {
                MessageBox.Show("系主任不能为空！");
                return;
            }
            
           
            if (!Regex.IsMatch(txtTel.Text.Trim(), @"\d{3}-\d{8}|\d{4}-\d{7}"))//电话号码的正则表达式,位数超过后验证不出来????????

            
            {
                errorTel.SetError(txtTel,"电话号码格式不正确！");
                return ;
            }

            
            
            errorTel.Clear();
            if (AddOrEdit == 0)
            {
                //添加记录后的保存
                try
                {
                    
                    //判断系名称是否已存在
                    da = new SqlDataAdapter("select * from 系别表 where 系名称='" + txtDName.Text + "'", conn);
                    DataSet ds = new DataSet();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    int count = da.Fill(ds, "系别表");
                    conn.Close();
                    if (count != 0)
                    {
                        MessageBox.Show("该系已存在，请重新输入！");
                        txtDName.Focus();
                        return;
                    }

                    com = new SqlCommand("insert into 系别表(系名称,系主任,办公电话)values('"+txtDName.Text.Trim()+"','"+txtDManager.Text.Trim()+"','"+txtTel.Text.Trim()+"')",conn);
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
                    com = new SqlCommand("update 系别表 set 系名称='"+txtDName.Text.Trim() + "',系主任='"+txtDManager.Text.Trim()+"',办公电话='"+txtTel.Text.Trim() + "' where ID="+Convert.ToString(dgvDepartment[0, dgvDepartment.CurrentCell.RowIndex].Value).Trim(),conn);
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
    }
}