using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;//先通过“项目”---“添加引用”,在COM选项卡中选择“Microsoft Excel 11.0 Object Library”.再在这儿添加该语句。
using Microsoft.Office.Core;


namespace StudentDB.StudentInfo
{
    public partial class frmStudentReport : Form
    {
        public frmStudentReport()
        {
            InitializeComponent();
        }


        /******** 将数据库中的数据导出到EXCEL中进行打印**********/
        
        
        //仍存在问题：数值太长时自动转换成科学记数法**********已解决



        SqlConnection conn;
        SqlDataAdapter da;

        private void frmStudentReport_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";

            //绑定班级组合框
            da = new SqlDataAdapter("select 班级名称 from 班级表", conn);
            DataSet ds1 = new DataSet();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            da.Fill(ds1, "班级表");
           
                conn.Close();
           
            cbxClassName.DataSource = ds1.Tables["班级表"];
            cbxClassName.DisplayMember = "班级名称";
            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            da = new SqlDataAdapter("select 学号,姓名,性别,民族,身份证号,出生日期,家庭住址,家庭电话 from viewStudent where 班级名称='" + cbxClassName.Text + "'" ,conn);
            System.Data.DataTable dt = new System.Data.DataTable();//否则会出现错误：“DataTable”是“System.Data.DataTable”和“Microsoft.Office.Interop.Excel.DataTable”之间的不明确的引用	H:\CSharp教材\教材实例\学生信息管理系统2005\StudentInfoSystem\StudentInfoSystem\StudentInfo\frmStudentReport.cs	58	13	StudentInfoSystem
            int count=da.Fill(dt);
            if (count==0)
            {
                MessageBox.Show("该班级没有学生记录！");
                return;
            }
            //导出学生信息到EXCEL中 
           //************** 也可先将数据放入到数组中，然后再写入EXCEL中即可。这样可使速度提高。数据量较大时建议采用此方法。
            
            ApplicationClass acExcel;//声明EXCEL应用程序对象
            Workbook wb;//声明工作簿对象
            Worksheet ws;//声明工作表对象

            acExcel = new ApplicationClass();
            acExcel.Visible = true;
            if (acExcel == null)
            {
                MessageBox.Show("EXCEL无法启动！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
            wb = acExcel.Workbooks.Add(true);//新建工作簿对象
            ws = (Worksheet)wb.Worksheets[1];//引用工作簿中的第1个工作表
           

            //设置EXCEL表格中开头显示的内容
            ws.Cells[1, 2] = "班级学生名册报表";//在第1行第2个单元格中显示内容
            ws.Cells[2, 1] = "班级名称：" + cbxClassName.Text;
            ws.Cells[3, 1] = "打印日期：" + DateTime.Now.ToLongDateString();

            //设置行和列的索引
            int rowindex = 5;//从第5行、第1列开始显示数据
            int colindex = 1;

            foreach (DataColumn dc in dt.Columns)
            {
                //添加列名
                
                ws.Cells[rowindex, colindex] = dc.ColumnName;//从第5行开始显示列名
                colindex++;
            }

            int rowCount = dt.Rows.Count;
            int colCount = dt.Columns.Count;
            ws.get_Range(ws.Cells[rowindex + 1, 1], ws.Cells[rowCount + rowindex, colCount]).NumberFormatLocal = "@";//设置显示的文本为字符格式,以防长数字出现科学记数法的数字形式
            foreach (DataRow dr in dt.Rows)
            {
                //添加数据
                rowindex++;//从第6行第1列开始显示数据
                colindex = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                    ws.Cells[rowindex, colindex] = dr[dc.ColumnName].ToString();
                    colindex++;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}