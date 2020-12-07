using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;//包含文件操作的类，如File类、FileInfo类。
using System.Data.SqlClient;

namespace StudentDB.SystemManage
{
    public partial class frmDataStore : Form
    {
        public frmDataStore()
        {
            InitializeComponent();
        }



        //添加文件夹浏览控件folderBrowserDialog1

        SqlConnection conn;
        SqlCommand com;

        private void btnSel_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            
            txtDSPath.Text = folderBrowserDialog1.SelectedPath.ToString().Trim();
        }

        private void btnDStore_Click(object sender, EventArgs e)//备份
        {
            try
            {
                if (File.Exists(txtDSPath.Text.Trim() + ".bak"))
                {
                    MessageBox.Show("该文件已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDSPath.Text = "";
                    txtDSPath.Focus();
                }
                else
                {
                        string sqlString = "backup database studentDB to disk='" + txtDSPath.Text.Trim() + ".bak'";
                        com = new SqlCommand(sqlString, conn);
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        com.ExecuteNonQuery();
                        
                            conn.Close();
                       
                        MessageBox.Show("数据备份成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
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

        private void frmDataStore_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";
        }

    }
}