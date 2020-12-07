using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Text.RegularExpressions;//引用正则表达式类的命名空间
using System.IO;//该命名空间包含文件操作的类


//TabControl控件：提供页标签。TabPages属性：添加页标签。
//dateTimePicker控件：Text（得到文本值）和Value属性（得到日期值）
//PictureBox控件：显示照片。Image（设置图像）,Cursor（设置鼠标形状）,SizeMode属性（设置图像的显示方式）
//选择查询条件后，自动显示查询值系别名或班级名称的列表
//将照片文件显示到PictureBox控件，并保存到数据库
//将表格中的照片显示到PictureBox控件
//执行带参数的命令行

namespace StudentDB.StudentInfo
{
    public partial class frmStudentManage : Form
    {
        public frmStudentManage()
        {
            InitializeComponent();
        }

        //表格控件的ReadOnly属性设为true,SelectionMode属性设置为FullRowSelect.
        //保存和取消按钮的Enabled属性设置为false.
        //日历控件通过Value属性设定初始值

        int AddOrEdit;//标记是添加或修改了记录
        SqlConnection conn;
        SqlCommand com;
        SqlDataAdapter da;
        string oldStudentNo;//修改记录之前的学号值 

        private void FillDataGridView()//填充表格数据
        {

            dgvStudent.DataSource = null;
            da = new SqlDataAdapter("select * from viewStudent", conn);//通过视图查询数据
            DataSet ds = new DataSet();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count = da.Fill(ds, "学生基本信息表");
                
                    conn.Close();
                
                this.dgvStudent.DataSource = ds.Tables["学生基本信息表"];
                lblStudentNum.Text = "共有" + count.ToString() + "个学生";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ClearText()//清除控件内容
        {
            this.txtStudentNo.Clear();
            this.txtStudentName.Clear();
            this.cbxSex.Text = "";
            this.cbxNationlity.Text = "";
            this.txtCardID.Clear();
            this.dtpBirthday.Value =Convert.ToDateTime("1990-1-1");//设定初始值
            this.txtAddress.Clear();
            this.txtTel.Clear();
            this.cbxClassName.Text = "";
            this.txtMemory.Clear();
            this.picPhoto.Image = null;

            
        }

        private void LockedTextBox()//锁定文本框和组合框
        {
            this.txtStudentNo.Enabled = false;
            this.txtStudentName.Enabled =false ;
            this.cbxSex.Enabled =false;
            this.cbxNationlity.Enabled =false ;
            this.txtCardID.Enabled =false ;
            this.dtpBirthday.Enabled =false;
            this.txtAddress.Enabled =false;
            this.txtTel.Enabled =false ;
            this.cbxClassName.Enabled =false ;
            this.txtMemory.Enabled =false ;
            this.picPhoto.Enabled = false;
        
        }

        private void UnLockedTextBox()//解除锁定
        {
             this.txtStudentNo.Enabled = true ;
             this.txtStudentName.Enabled = true;
             this.cbxSex.Enabled = true;
             this.cbxNationlity.Enabled = true;
             this.txtCardID.Enabled = true;
             this.dtpBirthday.Enabled = true;
             this.txtAddress.Enabled = true;
             this.txtTel.Enabled = true;
             this.cbxClassName.Enabled = true;
             this.txtMemory.Enabled = true;
             this.picPhoto.Enabled = true ;
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 0;
            ClearText();
            UnLockedTextBox();
            txtStudentNo.Focus();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
            AddOrEdit = 1;
            UnLockedTextBox();
            oldStudentNo = txtStudentNo.Text;// 根据学号进行修改

            
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除该生信息吗？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    com = new SqlCommand("delete from 学生基本信息表 where 学号=" + Convert.ToString(dgvStudent[0, dgvStudent.CurrentCell.RowIndex].Value).Trim(), conn);
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

            string SQLString;
            if (AddOrEdit == 0)
            {
                //判断学号是否已存在
                try
                {


                    da = new SqlDataAdapter("select * from 学生基本信息表 where 学号='" + txtStudentNo.Text + "'", conn);
                    DataSet ds = new DataSet();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    int count = da.Fill(ds, "学生基本信息表");
                    
                        conn.Close();
                    
                    if (count != 0)
                    {
                        MessageBox.Show("该学号已存在，请重新输入！");
                        txtStudentNo.Focus();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }



                SQLString = "insert into 学生基本信息表(学号,姓名,性别,民族,身份证号,出生日期,家庭住址,家庭电话,班级编号,个人简历,照片)values(@学号,@姓名,@性别,@民族,@身份证号,@出生日期,@家庭住址,@家庭电话,@班级编号,@个人简历,@照片)";
            }
            else
            {
                SQLString = "update 学生基本信息表 set 学号=@学号,姓名=@姓名,性别=@性别,民族=@民族,身份证号=@身份证号,出生日期=@出生日期,家庭住址=@家庭住址,家庭电话=@家庭电话,班级编号=@班级编号,个人简历=@个人简历,照片=@照片 where 学号='"+oldStudentNo+"'";

            }
                
                

                    com = new SqlCommand(SQLString, conn);
                    com.Parameters.Clear();//清除所有命令参数对象
                    
                    SqlParameter snoParameter = new SqlParameter();//创建命令参数
                    snoParameter.ParameterName = "@学号";//获取命令参数名称
                    snoParameter.Value = this.txtStudentNo.Text;//获取命令参数值
                    com.Parameters.Add(snoParameter);//往集合中添加参数
                    
                    SqlParameter nameParameter = new SqlParameter();//创建命令参数
                    nameParameter.ParameterName = "@姓名";//获取命令参数名称
                    nameParameter.Value = this.txtStudentName.Text;//获取命令参数值
                    com.Parameters.Add(nameParameter);//往集合中添加参数


                    SqlParameter sexParameter = new SqlParameter();//创建命令参数
                    sexParameter.ParameterName = "@性别";//获取命令参数名称
                    sexParameter.Value = this.cbxSex.Text;//获取命令参数值
                    com.Parameters.Add(sexParameter);//往集合中添加参数

                    SqlParameter nationParameter = new SqlParameter();//创建命令参数
                    nationParameter.ParameterName = "@民族";//获取命令参数名称
                    nationParameter.Value = this.cbxNationlity.Text;//获取命令参数值
                    com.Parameters.Add(nationParameter);//往集合中添加参数

                    SqlParameter IDParameter = new SqlParameter();//创建命令参数
                    IDParameter.ParameterName = "@身份证号";//获取命令参数名称
                    IDParameter.Value = this.txtCardID.Text;//获取命令参数值
                    com.Parameters.Add(IDParameter);//往集合中添加参数
                    
                    SqlParameter birthdayParameter = new SqlParameter();//创建命令参数
                    birthdayParameter.ParameterName = "@出生日期";//获取命令参数名称
                    birthdayParameter.Value = this.dtpBirthday.Text ;//获取命令参数值
                    com.Parameters.Add(birthdayParameter);//往集合中添加参数


                    SqlParameter addressParameter = new SqlParameter();//创建命令参数
                    addressParameter.ParameterName = "@家庭住址";//获取命令参数名称
                    addressParameter.Value = this.txtAddress.Text;//获取命令参数值
                    com.Parameters.Add(addressParameter);//往集合中添加参数

                    SqlParameter telParameter = new SqlParameter();//创建命令参数
                    telParameter.ParameterName = "@家庭电话";//获取命令参数名称
                    telParameter.Value = this.txtTel.Text;//获取命令参数值
                    com.Parameters.Add(telParameter);//往集合中添加参数

                    
                    SqlParameter classNoParameter = new SqlParameter();//创建命令参数
                    classNoParameter.ParameterName = "@班级编号";//获取命令参数名称
                    classNoParameter.Value = this.cbxClassName.SelectedValue.ToString();//获取命令参数值
                    com.Parameters.Add(classNoParameter);//往集合中添加参数

                    SqlParameter memoryParameter = new SqlParameter();//创建命令参数
                    memoryParameter.ParameterName = "@个人简历";//获取命令参数名称
                    memoryParameter.Value = this.txtMemory.Text;//获取命令参数值
                    com.Parameters.Add(memoryParameter);//往集合中添加参数
                    
                    //设置照片参数,若无照片，可设置为一缺省图片

                    
                    if (openFileDialog1.FileName == "")
                    {
                        openFileDialog1.FileName = "缺省照片.bmp";//该照片文件在当前项目的StudentInfoSystem\StudentInfoSystem\bin\Debug文件夹中
                    }

                    //读取照片文件到文件流对象，通过文件流创建字节数组.
                    FileStream fsBLOBFile = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                    Byte[] bytBLOBData = new Byte[fsBLOBFile.Length];
                    //从流中读取字节块，并将该数据写入给定缓冲区中
                    fsBLOBFile.Read(bytBLOBData, 0, bytBLOBData.Length);
                    fsBLOBFile.Close();

                    //创建命令参数，并将参数添加到命令对象的参数集合中.
                    SqlParameter paramImageData = new SqlParameter("@照片", SqlDbType.Image, bytBLOBData.Length);
                    paramImageData.Value =bytBLOBData ;
                    com.Parameters.Add( paramImageData);

                    
                 try
                 {
                     if (conn.State == ConnectionState.Closed)
                     {
                         conn.Open();
                     }
                    com.ExecuteNonQuery();
                    
                        conn.Close();
                    
                    MessageBox.Show("操作成功！");
                    openFileDialog1.FileName = "";
                    FillDataGridView();
                    LockedTextBox();
                    tsbSave.Enabled = false;
                    tsbCancel.Enabled = false;
                 }
                catch (Exception ex)
                {

                    MessageBox.Show("保存有误!  "+ex.Message.ToString());
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


        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            this.txtStudentNo.Text = Convert.ToString(dgvStudent[0, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.txtStudentName.Text = Convert.ToString(dgvStudent[1, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.cbxSex.Text = Convert.ToString(dgvStudent[2, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.cbxNationlity.Text = Convert.ToString(dgvStudent[3, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.txtCardID.Text = Convert.ToString(dgvStudent[4, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.dtpBirthday.Text = Convert.ToString(dgvStudent[5, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.txtAddress.Text = Convert.ToString(dgvStudent[6, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.txtTel.Text = Convert.ToString(dgvStudent[7, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.cbxClassName.Text = Convert.ToString(dgvStudent[8, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            this.txtMemory.Text = Convert.ToString(dgvStudent[11, dgvStudent.CurrentCell.RowIndex].Value).Trim();
            //显示照片
            if (!(dgvStudent[12, dgvStudent.CurrentCell.RowIndex].Value is DBNull))
            {
                Byte[] byteBLOBData = new Byte[0];
                byteBLOBData = (Byte[])(dgvStudent[12, dgvStudent.CurrentCell.RowIndex].Value);
                MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);//创建流对象
                this.picPhoto.Image = Image.FromStream(stmBLOBData);//从流对象中得到数据显示在图片框控件中
            }
            else
            {
                this.picPhoto.Image = null ;
            }
        }
        
        private void tsbFind_Click(object sender, EventArgs e)//查询
        {
            if (cbxCondition.Text.Length != 0)
            {
                
                    string fieldName;
                    if (cbxFindValue.Text.Length == 0)
                    {
                        this.FillDataGridView();//显示所有记录
                        return; 
                    }
                    //分别按"所属系别"、"班级名称"进行查询;
                    else if (cbxCondition.Text == "系别")
                    {
                        fieldName = "所属系别";

                    }
                    else
                    {
                        fieldName = "班级名称";
                    }

                   try
                    { 
                       
                        string findValue = cbxFindValue.Text.Trim();
                        da = new SqlDataAdapter("select * from viewStudent where " + fieldName + " like '%" + findValue + "%'", conn);//通过视图查询
                        DataSet ds = new DataSet();
                        conn.Open();
                        int count = da.Fill(ds, "学生基本信息表");
                        conn.Close();
                        if (count != 0)
                        {
                            dgvStudent.DataSource = ds.Tables["学生基本信息表"];
                            lblStudentNum.Text = "共有" + count.ToString() + "个学生";
                        }
                        else
                        {
                            MessageBox.Show("没有查询到符合条件的记录！");
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

        
        private void frmStudentManage_Load(object sender, EventArgs e)
        {

            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";

            //添加查询条件列表值
            cbxCondition.Items.Clear();
            cbxCondition.Items.Add("系别");
            cbxCondition.Items.Add("班级");
            
            
            
            this.cbxSex.Items.Add("男");
            this.cbxSex.Items.Add("女");

            this.cbxNationlity.Items.Add("汉族");
            this.cbxNationlity.Items.Add("回族");
            this.cbxNationlity.Items.Add("朝鲜族");
            this.cbxNationlity.Items.Add("满族");
            this.cbxNationlity.Items.Add("藏族");
            this.cbxNationlity.Items.Add("维吾尔族");

            //绑定班级组合框
            da = new SqlDataAdapter("select 班级编号,班级名称 from 班级表", conn);
            DataSet ds1 = new DataSet();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            da.Fill(ds1, "班级表");
            
                conn.Close();
           
            cbxClassName.DataSource = ds1.Tables["班级表"];
            cbxClassName.DisplayMember = "班级名称";
            cbxClassName.ValueMember = "班级编号";

            openFileDialog1.FileName = "";//设置打开文件的初始文件名为空值

            FillDataGridView();
            LockedTextBox();




        }

        private void cbxCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            //给查询值组合框添加列表值
            cbxFindValue.Items.Clear();
            cbxFindValue.Text = "";
            string conditionString = cbxCondition.Text;
            if (conditionString == "系别")
            {

                da = new SqlDataAdapter("select 系名称 from 系别表", conn);
                DataSet ds1 = new DataSet();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(ds1, "系别表");
                
                    conn.Close();
                
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)//这儿无法绑定，只好手动添加
                {
                    cbxFindValue.Items.Add(ds1.Tables[0].Rows[i][0]);
                }
                
            }
            else
            {
                da = new SqlDataAdapter("select 班级名称 from 班级表", conn);
                DataSet ds1 = new DataSet();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                da.Fill(ds1, "班级表");
                
                    conn.Close();
              
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)//这儿无法绑定，只好手动添加
                {
                    cbxFindValue.Items.Add(ds1.Tables[0].Rows[i][0]);
                }

            }


        }

        private void picPhoto_Click(object sender, EventArgs e)
        {

            try
            {
                openFileDialog1.FileName = "";
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName != "")
                {
                    picPhoto.Image = Image.FromFile(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载照片有误"+ex.Message.ToString());
            }
           
            
        }

      

        


      
       
    }
}