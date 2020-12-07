using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;


namespace StudentDB.ClassInfo
{
    public partial class frmClassBrowse : Form
    {
        public frmClassBrowse()
        {
            InitializeComponent();
        }
       

//TreeView从数据库得到数据的步骤：             

//1.从数据库查询出DATASET;
//2.然后用FOREACH从表中查询每一行
//3.再者就是实例化父节点TreeNode,把要显示的数据字段付给TreeNode对象,设置有关字段的对象.
//4.实例化子节点,然后被添加到父节点对象
//5.最后把父节点添加到treeView控件里


        
        SqlConnection conn;
        SqlDataAdapter da;

        private void frmClassBrowse_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";

            fillTree();//填充树
            // 初始化ListView 
            listView1.View = View.Details;//定义列表显示的方式
            listView1.GridLines = true;//显示各个记录的分隔线 
            listView1.FullRowSelect = true;//要选择就是一行
            
            listView1.Scrollable = true; //需要时候显示滚动条 
            listView1.MultiSelect = false; // 不可以多行选择 
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;//列表头不响应鼠标单击
        }

        private void fillTree()//填充treeView控件
        {
            this.treeView1.Nodes.Clear();//清空所有节点
            
            DataTable dt = new DataTable();//创建数据表对象


            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            da = new SqlDataAdapter("select 系名称 from 系别表", conn);

            da.Fill(dt);//填充数据表
            conn.Close();
            TreeNode tnMain = new TreeNode("沈阳建筑大学");//创建根节点
            for (int iRows = 0; iRows < dt.Rows.Count; iRows++)
            {
                TreeNode tnChild = new TreeNode(dt.Rows[iRows][0].ToString());//分别创建系名称节点
                LoadTreeS(tnChild, dt.Rows[iRows][0].ToString());//填加系别的所有的班级子节点
                tnMain.Nodes.Add(tnChild);//添加到根节点中
                
            }
            this.treeView1.Nodes.Add(tnMain);

            this.treeView1.ExpandAll();//展开所有子树节点

            this.treeView1.SelectedNode = tnMain;//选中根节点
        }

        public void LoadTreeS(TreeNode tn, string depName)//填加系别的班级子节点
        {
            
            da = new SqlDataAdapter("select 班级名称 from 班级表 where 所属系别='" + depName  + "'", conn); ;
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            foreach (DataRow dr in dt.Rows)
            {
                TreeNode tns = new TreeNode();
                tns.Text = dr["班级名称"].ToString();
                tn.Nodes.Add(tns);
                //tn.Nodes.Add(dr["班级名称"].ToString());
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string nodeText = e.Node.Text.ToString();
            this.ShowInfo(nodeText);
        }
       
        private void ShowInfo(string NodeText)//根据选择的节点，查询相应的班级信息
        {
            try
            {
                listView1.Clear();//清除列表控件的所有内容

                da = new SqlDataAdapter("select 班级编号,班级名称,班主任,专业名称,班级人数,备注 from viewClass where 班级名称='" + NodeText + "'", conn);//通过视图查询

                if (NodeText == "沈阳建筑大学")//显示所有班级信息
                {
                    da = new SqlDataAdapter("select 班级编号,班级名称,班主任,专业名称,班级人数,备注 from viewClass", conn);//通过视图查询
                }
                else //显示选择的系的班级信息
                {
                    char[] ch = NodeText.ToCharArray();
                    for (int i = 0; i < ch.Length; i++)
                    {
                        if (ch[i] == '系')
                        {

                            da = new SqlDataAdapter("select 班级编号,班级名称,班主任,专业名称,班级人数,备注 from viewClass where 所属系别='" + NodeText + "'", conn);//通过视图查询
                            break;
                        }
                    } 
                }
                DataTable dt = new DataTable();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count=da.Fill(dt);
                conn.Close();
                lblClassNum.Text = "共有"+count.ToString()+"个班级";

               

                 SetupHeaderText();//设置列标头

                
                //填充listview控件
                foreach (DataRow dr in dt.Rows)
                {
                    ListViewItem lvi=new ListViewItem();
                    
                    lvi.SubItems.Clear();
                    lvi.SubItems[0].Text = dr[0].ToString();//得到第1列的值
                    lvi.SubItems.Add(dr[1].ToString());//得到第2列的值
                    lvi.SubItems.Add(dr[2].ToString());//得到第3列的值
                    lvi.SubItems.Add(dr[3].ToString());//得到第4列的值
                    lvi.SubItems.Add(dr[4].ToString());//得到第5列的值
                    lvi.SubItems.Add(dr[5].ToString());//得到第6列的值
                    listView1.Items.Add(lvi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );

            }
        }

        private void SetupHeaderText()//设置listView1的列标头
        {

            listView1.Columns.Add("班级编号", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("班级名称", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("班主任", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("专业名称", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("班级人数", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("备注", 200, HorizontalAlignment.Left);
        }

        

        





        
        }
    }
