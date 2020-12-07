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
    public partial class frmStudentBrowse : Form
    {
        public frmStudentBrowse()
        {
            InitializeComponent();
        }
        
        
        /******************存在问题：一开始就选中“山东信息职业技术学院”后，没有显示被选中;
        //　　　　　                  
         *                            
        * **************************/
        




        //TreeView从数据库得到数据              

        //1.从数据库查询出DATASET;
        //2.然后用FOREACH从表中查询每一行
        //3.再者就是实例化父节点TreeNode,把要显示的数据字段付给TreeNode对象,设置有关字段的对象.
        //4.实例化子节点,然后被添加到父节点对象
        //5.最后把父节点添加到treeView控件里



        SqlConnection conn;
        //SqlCommand com;
        SqlDataAdapter da;


        private void fillTree()//填充treeView控件
        {
            this.treeView1.Nodes.Clear();

            DataTable dt = new DataTable();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            da = new SqlDataAdapter("select 系名称 from 系别表", conn);

            da.Fill(dt);
            conn.Close();
            TreeNode tnMain = new TreeNode("沈阳建筑大学", 0, 0);
            for (int iRows = 0; iRows < dt.Rows.Count; iRows++)
            {
                TreeNode tnChild = new TreeNode(dt.Rows[iRows][0].ToString(), 1, 2);

                tnMain.Nodes.Add(tnChild);
                LoadTreeS(tnChild, dt.Rows[iRows][0].ToString());//填加系别的所有的班级子节点的方法
            }
            this.treeView1.Nodes.Add(tnMain);

            this.treeView1.Nodes[0].ExpandAll();//展开所有子树节点

            this.treeView1.SelectedNode = tnMain;//选中根节点************此语句没起作用***********************
        }


        public void LoadTreeS(TreeNode tn, string depName)//填加系别的班级子节点
        {

            da = new SqlDataAdapter("select 班级名称 from 班级表 where 所属系别='" + depName + "'", conn); ;
            DataTable dt = new DataTable();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            da.Fill(dt);
            conn.Close();
            foreach (DataRow dr in dt.Rows)
            {
                TreeNode tns = new TreeNode();
                tns.Text = dr["班级名称"].ToString();
        
                tn.Nodes.Add(tns);
            }
        }


        private void ShowInfo(string NodeText)//分别显示所有学生、选择的系的学生、选择的班级的所有学生信息。
        {
            try
            {
                listView1.Clear();//清除列表控件的所有内容
                da = new SqlDataAdapter("select 学号,姓名,性别,民族,身份证号,出生日期,家庭住址,家庭电话,班级名称,所属系别,专业名称,个人简历 from viewStudent where 班级名称='" + NodeText + "'", conn);//通过视图查询
                           
                if (NodeText == "沈阳建筑大学")//显示所有学生信息
                {
                    da = new SqlDataAdapter("select 学号,姓名,性别,民族,身份证号,出生日期,家庭住址,家庭电话,班级名称,所属系别,专业名称,个人简历 from viewStudent", conn);//通过视图查询
                }
                else //显示选择的某系或某班级的学生信息
                {
                    
                    char []ch = NodeText.ToCharArray();
                    for (int i = 0; i < ch.Length; i++)
                    {
                        if (ch[i]=='系')
                        {

                            da = new SqlDataAdapter("select 学号,姓名,性别,民族,身份证号,出生日期,家庭住址,家庭电话,班级名称,所属系别,专业名称,个人简历 from viewStudent where 所属系别='" + NodeText + "'", conn);//通过视图查询
                            break;
                        }
                    } 
                }
                DataTable dt = new DataTable();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                int count = da.Fill(dt);
                conn.Close();
                lblStudentNum.Text = "共有" + count.ToString() + "名学生";



                SetupHeaderText();//设置列标头


                //填充listview控件
                foreach (DataRow dr in dt.Rows)
                {
                    ListViewItem lvi = new ListViewItem();

                    lvi.SubItems.Clear();

                    lvi.SubItems[0].Text = dr[0].ToString();//得到第1列的值
                    lvi.SubItems.Add(dr[1].ToString());//得到第2列的值
                    lvi.SubItems.Add(dr[2].ToString());//得到第3列的值
                    lvi.SubItems.Add(dr[3].ToString());//得到第4列的值
                    lvi.SubItems.Add(dr[4].ToString());//得到第5列的值
                    lvi.SubItems.Add(dr[5].ToString());//得到第6列的值
                    lvi.SubItems.Add(dr[6].ToString());//得到第7列的值
                    lvi.SubItems.Add(dr[7].ToString());//得到第8列的值
                    lvi.SubItems.Add(dr[8].ToString());//得到第9列的值
                    lvi.SubItems.Add(dr[9].ToString());//得到第10列的值
                    lvi.SubItems.Add(dr[10].ToString());//得到第11列的值
                    lvi.SubItems.Add(dr[11].ToString());//得到第12列的值
                    listView1.Items.Add(lvi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void SetupHeaderText()//设置listView1的列标头
        {

            listView1.Columns.Add("学号", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("姓名", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("性别", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("民族", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("身份证号", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("出生日期", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("家庭住址", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("家庭电话", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("班级名称", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("所属系别", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("专业名称", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("个人简历", 100, HorizontalAlignment.Left);
　　　　　　//listView1.Columns.Add("照片", 100, HorizontalAlignment.Left);
        }

        private void frmStudentBrowse_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = "server=localhost;uid=sa;pwd=sa123;database=studentDB";

            fillTree();//填充树
            // 初始化ListView 
            listView1.GridLines = true;//显示各个记录的分隔线 
            listView1.FullRowSelect = true;//要选择就是一行
            listView1.View = View.Details;//定义列表显示的方式
            listView1.Scrollable = true; //需要时候显示滚动条 
            listView1.MultiSelect = false; // 不可以多行选择 
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;//列表头不响应鼠标单击
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string nodeText = e.Node.Text.ToString();
            this.ShowInfo(nodeText);
        }

        private void lblStudentsNum_Click(object sender, EventArgs e)
        {

        }
    }
}