using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Text.RegularExpressions;//正则表达式类的命名空间,验证专业编号必须为数字


namespace StudentDB.BasicInfo
{
    public partial class frmSpeciality : Form
    {
        public frmSpeciality()
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
            da = new SqlDataAdapter("select * from 专业表 order by ID", conn);
            DataSet ds = new DataSet();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count = da.Fill(ds, "专业表");
                conn.Close();
                dgvSpeciality.DataSource = ds.Tables["专业表"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void ClearText()//清除文本框内容
        {
            txtSpeNo.Clear();
            txtSpeName.Clear();
        }

        private void LockedTextBox()//锁定文本框
        {
            txtSpeNo.Enabled = false;
            txtSpeName.Enabled = false;
        }

        private void UnLockedTextBox()//解除锁定
        {
            txtSpeNo.Enabled = true;
            txtSpeName.Enabled = true;

        }
        private void frmSpeciality_Load(object sender, EventArgs e)
        {
            tsbSave.Enabled = false ;//保存按钮无效
            tsbCancel.Enabled = false ;//取消按钮无效

            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";
            FillDataGridView();
            LockedTextBox();
        }

        private void dgvSpeciality_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSpeNo.Text = Convert.ToString(dgvSpeciality[1, dgvSpeciality.CurrentCell.RowIndex].Value).Trim();
            txtSpeName.Text = Convert.ToString(dgvSpeciality[2, dgvSpeciality.CurrentCell.RowIndex].Value).Trim();
        }
       

        private void tsbAdd_Click(object sender, EventArgs e)//添加
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 0;
            ClearText();
            UnLockedTextBox();
            txtSpeNo.Focus();
        }

        private void tsbEdit_Click(object sender, EventArgs e)//修改
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 1;
            UnLockedTextBox();
            txtSpeNo.Focus();
        }

        private void tsbDel_Click(object sender, EventArgs e)//删除
        {
            if (MessageBox.Show("确定要删除该专业信息吗？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    com = new SqlCommand("delete from 专业表 where ID=" + Convert.ToString(dgvSpeciality[0, dgvSpeciality.CurrentCell.RowIndex].Value).Trim(), conn);
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

        private void tsbSave_Click(object sender, EventArgs e)//保存
        {
            if (txtSpeNo.Text == "")
            {
                MessageBox.Show("专业编号不能为空！");
                return;
            }
            if (txtSpeName.Text == "")
            {
                MessageBox.Show("专业名称不能为空！");
                return;
            }


            if (!Regex.IsMatch(txtSpeNo.Text.Trim(), "^[0-9]*$"))//专业编号必须为数字
            {
                errorSpeNo.SetError(txtSpeNo, "专业编号必须为数字！");
                return;
            }

            

            errorSpeNo.Clear();
            if (AddOrEdit == 0)
            {
                //添加记录后的保存
                try
                {

                    //判断该专业编号是否已存在
                    da = new SqlDataAdapter("select * from 专业表 where 专业编号='" + txtSpeNo.Text + "'", conn);
                    DataSet ds = new DataSet();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    int count = da.Fill(ds, "专业表");
                    conn.Close();
                    if (count != 0)
                    {
                        MessageBox.Show("该专业编号已存在，请重新输入！");
                        txtSpeNo.Focus();
                        return;
                    }

                    com = new SqlCommand("insert into 专业表(专业编号,专业名称)values('" + txtSpeNo.Text.Trim() + "','" + txtSpeName.Text.Trim() + "')", conn);
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
                    com = new SqlCommand("update 专业表 set 专业编号='" + txtSpeNo.Text.Trim() + "',专业名称='" + txtSpeName.Text.Trim()+ "' where ID=" + Convert.ToString(dgvSpeciality[0, dgvSpeciality.CurrentCell.RowIndex].Value).Trim(), conn);
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

        private void tsbCancel_Click(object sender, EventArgs e)//取消
        {
            LockedTextBox();
            tsbCancel.Enabled = false;
            tsbSave.Enabled = false;
        }

        private void tsbExit_Click(object sender, EventArgs e)//退出
        {
            this.Close();
        }


    }
}