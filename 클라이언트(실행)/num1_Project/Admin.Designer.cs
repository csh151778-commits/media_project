namespace num1_Project
{
    partial class Admin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            comboBox1 = new ComboBox();
            DbTable = new ListBox();
            DBSave = new Button();
            btnLoad = new Button();
            label3 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(DbTable);
            panel1.Controls.Add(DBSave);
            panel1.Controls.Add(btnLoad);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1512, 643);
            panel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(78, 12);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(231, 23);
            comboBox1.TabIndex = 19;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // DbTable
            // 
            DbTable.FormattingEnabled = true;
            DbTable.ItemHeight = 15;
            DbTable.Location = new Point(29, 50);
            DbTable.Name = "DbTable";
            DbTable.Size = new Size(1323, 559);
            DbTable.TabIndex = 18;
            // 
            // DBSave
            // 
            DBSave.BackColor = Color.FromArgb(33, 38, 45);
            DBSave.FlatStyle = FlatStyle.Flat;
            DBSave.ForeColor = SystemColors.Control;
            DBSave.Location = new Point(1374, 91);
            DBSave.Name = "DBSave";
            DBSave.Size = new Size(75, 23);
            DBSave.TabIndex = 17;
            DBSave.Text = "저장하기";
            DBSave.UseVisualStyleBackColor = false;
            DBSave.Click += DBSave_Click;
            // 
            // btnLoad
            // 
            btnLoad.BackColor = Color.FromArgb(33, 38, 45);
            btnLoad.FlatStyle = FlatStyle.Flat;
            btnLoad.ForeColor = SystemColors.Control;
            btnLoad.Location = new Point(1374, 50);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 17;
            btnLoad.Text = "불러오기";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += btnLoad_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(88, 166, 255);
            label3.Location = new Point(29, 15);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 16;
            label3.Text = "테이블";
            // 
            // Admin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 27, 34);
            ClientSize = new Size(1512, 643);
            Controls.Add(panel1);
            Name = "Admin";
            Text = "Admin";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox songList;
        private Button btnCancel;
        private Button btnSave;
        private Button btnRemoveSong;
        private ListBox DbTable;
        private Button btnLoad;
        private Label label3;
        private Button DBSave;
        private ComboBox comboBox1;
    }
}