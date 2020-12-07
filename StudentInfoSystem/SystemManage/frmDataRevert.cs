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
    public partial class frmDataRevert : Form
    {
        public frmDataRevert()
        {
            InitializeComponent();
        }

        /**********存在问题：无法还原，连接未关闭(已解决：将企业管理器关闭即可）*******************/

        //添加打开文件控件：openFileDialog1

        SqlConnection conn;
        SqlCommand com;

        private void btnSel_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "D:\\";//设定初始目录
            openFileDialog1.Filter = "bak files (*.bak)|*.bak";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();
            txtDRPath.Text = openFileDialog1.FileName.ToString().Trim();
        }

        private void btnDRevert_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                string sqlString = "use master restore database studentDB from disk='" + txtDRPath.Text.Trim() + "'";
                com = new SqlCommand(sqlString,conn);
                
                com.ExecuteNonQuery();
                com.Dispose();
                conn.Close();
                conn.Dispose();
                MessageBox.Show("数据还原成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDataRevert_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";
        }
    }
}