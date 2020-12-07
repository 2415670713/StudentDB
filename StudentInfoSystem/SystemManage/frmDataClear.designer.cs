namespace StudentDB.SystemManage
{
    partial class frmDataClear
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkClass = new System.Windows.Forms.CheckBox();
            this.chkSpeciality = new System.Windows.Forms.CheckBox();
            this.chkUser = new System.Windows.Forms.CheckBox();
            this.chkDepartment = new System.Windows.Forms.CheckBox();
            this.chkStudent = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Location = new System.Drawing.Point(253, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(101, 102);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(9, 24);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(76, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "清理";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(9, 57);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(76, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // textBox1
            // 
            this.textBox1.ForeColor = System.Drawing.Color.Red;
            this.textBox1.Location = new System.Drawing.Point(30, 119);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(324, 85);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "                                        \r\n　　　注意：系统数据清理，将清理数据库中所有相关表的数据，因此在系统数据清理前" +
                "，请作好备份工作，以免造成大量数据丢失，带来不必要的损失。";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkClass);
            this.groupBox1.Controls.Add(this.chkSpeciality);
            this.groupBox1.Controls.Add(this.chkUser);
            this.groupBox1.Controls.Add(this.chkDepartment);
            this.groupBox1.Controls.Add(this.chkStudent);
            this.groupBox1.Location = new System.Drawing.Point(30, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 102);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据表信息";
            // 
            // chkClass
            // 
            this.chkClass.AutoSize = true;
            this.chkClass.ForeColor = System.Drawing.Color.Blue;
            this.chkClass.Location = new System.Drawing.Point(125, 15);
            this.chkClass.Name = "chkClass";
            this.chkClass.Size = new System.Drawing.Size(60, 16);
            this.chkClass.TabIndex = 0;
            this.chkClass.Text = "班级表";
            this.chkClass.UseVisualStyleBackColor = true;
            // 
            // chkSpeciality
            // 
            this.chkSpeciality.AutoSize = true;
            this.chkSpeciality.ForeColor = System.Drawing.Color.Blue;
            this.chkSpeciality.Location = new System.Drawing.Point(125, 37);
            this.chkSpeciality.Name = "chkSpeciality";
            this.chkSpeciality.Size = new System.Drawing.Size(60, 16);
            this.chkSpeciality.TabIndex = 0;
            this.chkSpeciality.Text = "专业表";
            this.chkSpeciality.UseVisualStyleBackColor = true;
            // 
            // chkUser
            // 
            this.chkUser.AutoSize = true;
            this.chkUser.ForeColor = System.Drawing.Color.Blue;
            this.chkUser.Location = new System.Drawing.Point(10, 58);
            this.chkUser.Name = "chkUser";
            this.chkUser.Size = new System.Drawing.Size(60, 16);
            this.chkUser.TabIndex = 0;
            this.chkUser.Text = "用户表";
            this.chkUser.UseVisualStyleBackColor = true;
            // 
            // chkDepartment
            // 
            this.chkDepartment.AutoSize = true;
            this.chkDepartment.ForeColor = System.Drawing.Color.Blue;
            this.chkDepartment.Location = new System.Drawing.Point(10, 37);
            this.chkDepartment.Name = "chkDepartment";
            this.chkDepartment.Size = new System.Drawing.Size(60, 16);
            this.chkDepartment.TabIndex = 0;
            this.chkDepartment.Text = "系别表";
            this.chkDepartment.UseVisualStyleBackColor = true;
            //this.chkDepartment.CheckedChanged += new System.EventHandler(this.chkDepartment_CheckedChanged);
            // 
            // chkStudent
            // 
            this.chkStudent.AutoSize = true;
            this.chkStudent.ForeColor = System.Drawing.Color.Blue;
            this.chkStudent.Location = new System.Drawing.Point(10, 15);
            this.chkStudent.Name = "chkStudent";
            this.chkStudent.Size = new System.Drawing.Size(108, 16);
            this.chkStudent.TabIndex = 0;
            this.chkStudent.Text = "学生基本信息表";
            this.chkStudent.UseVisualStyleBackColor = true;
            // 
            // frmDataClear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 225);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmDataClear";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据清理";
            this.Load += new System.EventHandler(this.frmDataClear_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkClass;
        private System.Windows.Forms.CheckBox chkSpeciality;
        private System.Windows.Forms.CheckBox chkUser;
        private System.Windows.Forms.CheckBox chkDepartment;
        private System.Windows.Forms.CheckBox chkStudent;
    }
}