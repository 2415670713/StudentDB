using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace StudentDB.StudentInfo
{
    public partial class frmStudentFind : Form
    {
        public frmStudentFind()
        {
            InitializeComponent();
        }

      


 　　　//知识点：打印功能的实现
        
        //通过学生视图查询学生信息
        SqlConnection conn;
        SqlDataAdapter da;

        private void FillDataGridView(string sqlString)  //填充表格数据
        {

            dgvStudent.DataSource = null;
            da = new SqlDataAdapter(sqlString, conn);//通过视图查询数据
            DataSet ds = new DataSet();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count = da.Fill(ds, "学生基本信息表");
                conn.Close();
                if (count !=0)
                {
                this.dgvStudent.DataSource = ds.Tables["学生基本信息表"];
                lblStudentNum.Text = "共有" + count.ToString() + "个学生";
                }
                else 
                {
                  MessageBox.Show("没有查询到符合条件的记录！");
                  this.dgvStudent.DataSource =null ;
                  lblStudentNum.Text = "共有0个学生";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void frmStudentFind_Load(object sender, EventArgs e)
        {
            dgvStudent.ReadOnly = true;

            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";

            //添加查询条件列表值
            cbxCondition.Items.Clear();
            string[] fieldname = { "学号", "姓名", "性别", "民族", "身份证号", "出生日期", "家庭住址", "家庭电话", "班级名称", "所属系别", "专业名称" };
            for (int i = 0; i < fieldname.Length; i++)
            {
               cbxCondition.Items.Add(fieldname[i]);
            }

            string sqlString="select * from viewStudent";
            FillDataGridView(sqlString);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxFindValue.Items.Clear();
            txtFindValue.Text = "";

            if (cbxCondition.Text == "学号" || cbxCondition.Text == "姓名" || cbxCondition.Text == "身份证号" || cbxCondition.Text == "出生日期" || cbxCondition.Text == "家庭住址" || cbxCondition.Text == "家庭电话")
            {
                cbxFindValue.Visible = false;
                txtFindValue.Visible = true; 

            }
            else
            {
                cbxFindValue.Visible = true ;
                txtFindValue.Visible = false ; 
            }

            if (cbxCondition.Text=="性别")
            {
                cbxFindValue.Items.Add("男");
                cbxFindValue.Items.Add("女");
            }
            if (cbxCondition.Text == "民族")
            {
                string[] nationlity = { "汉族", "回族", "朝鲜族", "满族", "藏族", "维吾尔族" };
           
                for (int i = 0; i < nationlity.Length; i++)
                {
                    cbxFindValue.Items.Add(nationlity[i]);
                }
            }
            if (cbxCondition.Text == "班级名称")
            {
                //绑定查询值组合框
                da = new SqlDataAdapter("select 班级名称 from 班级表", conn);
                DataSet ds1 = new DataSet();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count=da.Fill(ds1, "班级表");
                                    conn.Close();
                
                for (int i = 0; i < count; i++)
                {
                    cbxFindValue.Items.Add(ds1.Tables[0].Rows[i][0]);
                    
                }
            }
            if (cbxCondition.Text == "所属系别")
            {
                //绑定查询值组合框
                da = new SqlDataAdapter("select 系名称 from 系别表", conn);
                DataSet ds1 = new DataSet();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count=da.Fill(ds1, "系别表");
               
                    conn.Close();
                
                for (int i = 0; i < count; i++)
                {
                    cbxFindValue.Items.Add(ds1.Tables[0].Rows[i][0]);
                }
            }
            if (cbxCondition.Text == "专业名称")
            {
                //绑定查询值组合框
                da = new SqlDataAdapter("select 专业名称 from 专业表", conn);
                DataSet ds1 = new DataSet();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count=da.Fill(ds1, "专业表");
                
                    conn.Close();
               
                for (int i = 0; i < count; i++)
                {
                    cbxFindValue.Items.Add(ds1.Tables[0].Rows[i][0]);

                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (cbxCondition.Text.Length == 0)
            {
                MessageBox.Show("请选择查询条件");
                cbxCondition.Focus();
               
                
            }
            else  
            {
               if (cbxFindValue.Text.Length == 0 && txtFindValue.Text.Trim().Length ==0)//没输入查询值
                {
                    this.FillDataGridView("select * from viewStudent");//显示所有记录
                    
                }
                
               else if (cbxCondition.Text == "学号" || cbxCondition.Text == "姓名" || cbxCondition.Text == "身份证号" || cbxCondition.Text == "出生日期" || cbxCondition.Text == "家庭住址" || cbxCondition.Text == "家庭电话")
               {
                string sqlString="select * from viewStudent where "+cbxCondition.Text +" like '%"+txtFindValue.Text.Trim()+"%'";//模糊查询
                this.FillDataGridView(sqlString );
               }
               else
               {

                string sqlString="select * from viewStudent where "+cbxCondition.Text +" = '"+cbxFindValue.Text.Trim()+"'";
                this.FillDataGridView(sqlString );
               }
            }
        }

    }
}