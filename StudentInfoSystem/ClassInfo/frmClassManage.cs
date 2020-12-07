using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Text.RegularExpressions;//正则表达式类的命名空间

namespace StudentDB.ClassInfo
{
    public partial class frmClassManage : Form
    {
        public frmClassManage()
        {
            InitializeComponent();
        }

           
        
        
        
        
        

        int AddOrEdit;//标记是添加或修改了记录
        SqlConnection conn;
        SqlCommand com;
        SqlDataAdapter da;

       

        private void FillDataGridView()//填充表格数据
        {
            da = new SqlDataAdapter("select * from viewClass order by ID", conn);//通过视图查询数据
            DataSet ds = new DataSet();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count = da.Fill(ds, "班级表");
                conn.Close();
                dgvClass.DataSource = ds.Tables["班级表"];
                lblClassNum.Text = "共有"+count.ToString()+"个班级";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ClearText()//清除控件内容
        {
            txtClassNo.Clear();
            txtClassName.Clear();
            txtManager.Clear();
            cbxDepartment.Text="";
            cbxSpeciality.Text="";
            txtStuNum.Clear();
            txtMemory.Clear();
        }

        private void LockedTextBox()//锁定文本框和组合框
        {
            txtClassNo.Enabled = false;
            txtClassName.Enabled = false;
            txtManager.Enabled =false;
            cbxDepartment.Enabled =false;
            cbxSpeciality.Enabled =false;
            txtStuNum.Enabled =false;
            txtMemory.Enabled =false;
        }

        private void UnLockedTextBox()//解除锁定
        {
            txtClassNo.Enabled = true ;
            txtClassName.Enabled = true ;
            txtManager.Enabled = true;
            cbxDepartment.Enabled = true;
            cbxSpeciality.Enabled = true ;
            txtStuNum.Enabled = true ;
            txtMemory.Enabled = true ;
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 0;
            ClearText();
            UnLockedTextBox();
            txtClassNo.Focus();
        }

        private void frmClassManage_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";

            tsbSave.Enabled = false;
            tsbCancel.Enabled = false;

            //添加查询条件列表值
            cbxCondition.Items.Clear();
            cbxCondition.Items.Add("班级编号");
            cbxCondition.Items.Add("班级名称");
            cbxCondition.Items.Add("班主任");
            cbxCondition.Items.Add("所属系别");
　　　　　　cbxCondition.Items.Add("专业名称");
            
            //绑定系别组合框
            da = new SqlDataAdapter("select 系名称 from 系别表",conn);
            DataSet ds1 = new DataSet();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            da.Fill(ds1, "系别表");
            conn.Close();
            cbxDepartment.DataSource=ds1.Tables["系别表"];
            cbxDepartment.DisplayMember = "系名称";
            
            //绑定专业组合框
            da = new SqlDataAdapter("select 专业编号,专业名称 from 专业表", conn);
            DataSet ds2 = new DataSet();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            da.Fill(ds2, "专业表");
            conn.Close();
            cbxSpeciality.DataSource = ds2.Tables["专业表"];
            cbxSpeciality.DisplayMember = "专业名称";
            cbxSpeciality.ValueMember = "专业编号";//设置返回值字段

            FillDataGridView();
            LockedTextBox();

        }

        

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 1;
            UnLockedTextBox();
            txtClassNo.Focus();
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除该系信息吗？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    com = new SqlCommand("delete from 班级表 where ID=" + Convert.ToString(dgvClass[0, dgvClass.CurrentCell.RowIndex].Value).Trim(), conn);
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
            if (txtClassNo.Text == "")
            {
                MessageBox.Show("班级编号不能为空！");
                return;
            }
            if (txtClassName.Text == "")
            {
                MessageBox.Show("班级名称不能为空！");
                return;
            }

            if (!Regex.IsMatch(txtClassNo.Text.Trim(), "^[0-9]*$"))//班级编号必须为数字
            {
                errorClassNo.SetError(txtClassNo, "班级编号必须为数字！");
                return;
            }

            if (!Regex.IsMatch(txtStuNum.Text.Trim(), "^[0-9]*$"))//班级人数必须为数字
            {
                errorStuNum.SetError(txtStuNum, "班级人数输入必须为数字！");
                return;
            }

            errorClassNo.Clear();
            errorStuNum.Clear();
            if (AddOrEdit == 0)
            {
                //添加记录后的保存
                try
                {

                    //判断班级编号是否已存在
                    da = new SqlDataAdapter("select * from 班级表 where 班级编号='" + txtClassNo.Text + "'", conn);
                    DataSet ds = new DataSet();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    int count = da.Fill(ds, "班级表");
                    conn.Close();
                    if (count != 0)
                    {
                        MessageBox.Show("该班级号已存在，请重新输入！");
                        txtClassNo.Focus();
                        return;
                    }
                    
                    com = new SqlCommand("insert into 班级表(班级编号,班级名称,班主任,所属系别,专业编号,班级人数,备注)values('" + txtClassNo.Text.Trim() + "','" + txtClassName.Text.Trim() + "','" + txtManager.Text.Trim() +"','"+cbxDepartment.Text+ "','"+cbxSpeciality.SelectedValue.ToString()+"',"+txtStuNum.Text.Trim() +",'"+txtMemory.Text.Trim()+"')", conn);
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
                    com = new SqlCommand("update 班级表 set 班级编号='" + txtClassNo.Text.Trim() + "',班级名称='" + txtClassName.Text.Trim() + "',班主任='" + txtManager.Text.Trim() + "',所属系别='" + cbxDepartment.Text.Trim()+ "',专业编号='" + cbxSpeciality.SelectedValue.ToString()+"',班级人数=" + txtStuNum.Text.Trim()+",备注='"+txtMemory.Text.Trim()+"' where ID=" + Convert.ToString(dgvClass[0, dgvClass.CurrentCell.RowIndex].Value).Trim(), conn);
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

       
        private void dgvClass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            txtClassNo.Text = Convert.ToString(dgvClass[1, dgvClass.CurrentCell.RowIndex].Value).Trim();
            txtClassName.Text = Convert.ToString(dgvClass[2, dgvClass.CurrentCell.RowIndex].Value).Trim();
            txtManager.Text = Convert.ToString(dgvClass[3, dgvClass.CurrentCell.RowIndex].Value).Trim();
            cbxDepartment.Text = Convert.ToString(dgvClass[4, dgvClass.CurrentCell.RowIndex].Value).Trim();
            

            cbxSpeciality.Text=Convert.ToString(dgvClass[5, dgvClass.CurrentCell.RowIndex].Value).Trim();
            txtStuNum.Text = Convert.ToString(dgvClass[6, dgvClass.CurrentCell.RowIndex].Value).Trim();
            txtMemory.Text = Convert.ToString(dgvClass[7, dgvClass.CurrentCell.RowIndex].Value).Trim();
        }

        private void tsbFind_Click(object sender, EventArgs e)//查询
        {
            if (cbxCondition.Text.Length != 0)
            {
                try
                {
                    if (txtFindValue.Text.Length == 0)
                    {
                        this.FillDataGridView();//显示所有记录
                    }
                    //分别按"班级编号"、"班级名称"、"班主任"、"所属系别"、"专业名称"进行查询;
                    else
                    {
                        string fieldName = cbxCondition.Text;
                        string findValue = txtFindValue.Text.Trim();
                        da = new SqlDataAdapter("select * from viewClass where " + fieldName + " like '%" + findValue + "%'", conn);//通过视图查询
                        DataSet ds = new DataSet();
                        conn.Open();
                        int count = da.Fill(ds, "班级表");
                        conn.Close();
                        if (count != 0)
                        {
                            dgvClass.DataSource = ds.Tables["班级表"];
                            lblClassNum.Text = "共有" + count.ToString() + "个班级";
                        }
                        else
                        {
                            MessageBox.Show("没有查询到符合条件的记录！");
                        }
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {

                MessageBox.Show("请选择查询条件");
            }
        }

        

        
    }
}