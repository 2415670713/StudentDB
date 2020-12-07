using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using StudentDB.BasicInfo;//引入自定义命名空间，以使用其中的类
using StudentDB.StudentInfo;
using StudentDB.ClassInfo ;
using StudentDB.SystemManage ;
using StudentDB.Help ;

namespace StudentDB
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void 系统维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 系别设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDepartment frmdep = new frmDepartment();
            frmdep.ShowDialog();
        }

        private void 退出本系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 专业设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSpeciality frmspe = new frmSpeciality();
            frmspe.ShowDialog();
        }

        private void 学生基本信息导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudentInput frmsi = new frmStudentInput();
            frmsi.ShowDialog();
            
        }

        private void 班级信息维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmClassManage frmcla = new frmClassManage();
            frmcla.ShowDialog();
        }

        private void 班级信息浏览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmClassBrowse frmcb = new frmClassBrowse();
            frmcb.ShowDialog();
        }

        private void 学生基本信息录入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudentReg frmsr = new frmStudentReg();
            frmsr.ShowDialog();
        }

        private void 学生基本信息维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudentManage frmsm = new frmStudentManage();
            frmsm.ShowDialog();
        }

        

       

        private void 学生基本信息浏览toolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudentBrowse frmsb =new frmStudentBrowse();
            frmsb.ShowDialog();

        }

        private void 学生基本信息查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudentFind frmsf = new frmStudentFind();
            frmsf.ShowDialog();
        }

        private void 班级名册表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudentReport frmsr = new frmStudentReport();
            frmsr.ShowDialog();
        }

        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserManage frmum = new frmUserManage();
            frmum.ShowDialog();
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditPwd frmep = new frmEditPwd();
            frmep.ShowDialog();
        }

        private void 数据备份ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDataStore frmds = new frmDataStore();
            frmds.ShowDialog();
        }

        private void 数据还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDataRevert frmdr = new frmDataRevert();
            frmdr.ShowDialog();
        }

        private void 数据清理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDataClear frmdc = new frmDataClear();
            frmdc.ShowDialog();
        }

        private void 关于本系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAboutUs frmau = new frmAboutUs();
            frmau.ShowDialog();
            
        }

        private void 记事本ToolStripMenuItem_Click(object sender, EventArgs e)//System.Diagnostics 命名空间提供特定的类，使您能够与系统进程、事件日志和性能计数器进行交互。
        {
            System.Diagnostics.Process.Start("notepad.exe");//Process类提供下列功能：监视整个网络的系统进程以及启动和停止本地系统进程。
        }

        private void 计算器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");

        }

        private void 万年历ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://www.hao123.com/rili"); 
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.tslUserName.Text = "||操作用户：" + frmLogin.userName;
            this.tslUserRight.Text = "||用户权限：" + frmLogin.userRight;

            if (frmLogin.userRight.ToString().Trim() != "administrator")//普通用户仅能浏览信息
            {
                this.基础信息设置ToolStripMenuItem.Visible = false;
                this.班级信息维护ToolStripMenuItem.Visible = false;
                this.学生基本信息导入ToolStripMenuItem.Visible = false;
                this.学生基本信息录入ToolStripMenuItem.Visible = false;
                this.学生基本信息维护ToolStripMenuItem.Visible = false;
                this.数据备份ToolStripMenuItem.Visible = false;
                this.数据还原ToolStripMenuItem.Visible = false;
                this.数据清理ToolStripMenuItem.Visible = false;
                this.用户管理ToolStripMenuItem.Visible = false;


            }
        }

       
       

       
    }
}