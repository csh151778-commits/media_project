namespace num1_Project
{
    partial class Membership
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Membership));
            Membership_label = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            richTextBox1 = new RichTextBox();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            Address_Label = new Label();
            comboBox1 = new ComboBox();
            textBox5 = new TextBox();
            Id_Label = new Label();
            textBox4 = new TextBox();
            Pass_Label = new Label();
            Phone_Label = new Label();
            Name_Label = new Label();
            Address2_Label = new Label();
            listBox1 = new ListBox();
            Btn_newmem = new Button();
            Group_Agree = new GroupBox();
            Group_Agree.SuspendLayout();
            SuspendLayout();
            // 
            // Membership_label
            // 
            Membership_label.AutoSize = true;
            Membership_label.BackColor = Color.FromArgb(22, 27, 34);
            Membership_label.FlatStyle = FlatStyle.Flat;
            Membership_label.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Membership_label.ForeColor = Color.FromArgb(88, 166, 255);
            Membership_label.Location = new Point(602, 28);
            Membership_label.Margin = new Padding(4, 0, 4, 0);
            Membership_label.Name = "Membership_label";
            Membership_label.Size = new Size(110, 32);
            Membership_label.TabIndex = 0;
            Membership_label.Text = "회원가입";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(219, 100);
            textBox1.Margin = new Padding(4, 4, 4, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(405, 27);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.White;
            textBox2.Location = new Point(804, 100);
            textBox2.Margin = new Padding(4, 4, 4, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(405, 27);
            textBox2.TabIndex = 2;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(219, 163);
            textBox3.Margin = new Padding(4, 4, 4, 4);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(405, 27);
            textBox3.TabIndex = 2;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(22, 27, 34);
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            richTextBox1.ForeColor = Color.FromArgb(88, 166, 255);
            richTextBox1.Location = new Point(379, 301);
            richTextBox1.Margin = new Padding(4, 4, 4, 4);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(644, 253);
            richTextBox1.TabIndex = 4;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            richTextBox1.MouseDown += richTextBox1_MouseDown;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            radioButton1.ForeColor = Color.FromArgb(88, 166, 255);
            radioButton1.Location = new Point(27, 32);
            radioButton1.Margin = new Padding(4, 4, 4, 4);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(120, 27);
            radioButton1.TabIndex = 5;
            radioButton1.TabStop = true;
            radioButton1.Tag = "";
            radioButton1.Text = "동의합니다.";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            radioButton2.ForeColor = Color.FromArgb(88, 166, 255);
            radioButton2.Location = new Point(156, 32);
            radioButton2.Margin = new Padding(4, 4, 4, 4);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(86, 27);
            radioButton2.TabIndex = 5;
            radioButton2.TabStop = true;
            radioButton2.Text = "아니오.";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // Address_Label
            // 
            Address_Label.AutoSize = true;
            Address_Label.FlatStyle = FlatStyle.Flat;
            Address_Label.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Address_Label.ForeColor = Color.FromArgb(88, 166, 255);
            Address_Label.Location = new Point(157, 236);
            Address_Label.Margin = new Padding(4, 0, 4, 0);
            Address_Label.Name = "Address_Label";
            Address_Label.Size = new Size(52, 28);
            Address_Label.TabIndex = 6;
            Address_Label.Text = "주소";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(219, 236);
            comboBox1.Margin = new Padding(4, 4, 4, 4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(174, 28);
            comboBox1.TabIndex = 7;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(804, 236);
            textBox5.Margin = new Padding(4, 4, 4, 4);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(405, 27);
            textBox5.TabIndex = 8;
            // 
            // Id_Label
            // 
            Id_Label.AutoSize = true;
            Id_Label.FlatStyle = FlatStyle.Flat;
            Id_Label.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Id_Label.ForeColor = Color.FromArgb(88, 166, 255);
            Id_Label.Location = new Point(136, 100);
            Id_Label.Margin = new Padding(4, 0, 4, 0);
            Id_Label.Name = "Id_Label";
            Id_Label.Size = new Size(72, 28);
            Id_Label.TabIndex = 6;
            Id_Label.Text = "아이디";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(804, 163);
            textBox4.Margin = new Padding(4, 4, 4, 4);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(405, 27);
            textBox4.TabIndex = 2;
            // 
            // Pass_Label
            // 
            Pass_Label.AutoSize = true;
            Pass_Label.FlatStyle = FlatStyle.Flat;
            Pass_Label.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Pass_Label.ForeColor = Color.FromArgb(88, 166, 255);
            Pass_Label.Location = new Point(701, 100);
            Pass_Label.Margin = new Padding(4, 0, 4, 0);
            Pass_Label.Name = "Pass_Label";
            Pass_Label.Size = new Size(92, 28);
            Pass_Label.TabIndex = 6;
            Pass_Label.Text = "비밀번호";
            // 
            // Phone_Label
            // 
            Phone_Label.AutoSize = true;
            Phone_Label.FlatStyle = FlatStyle.Flat;
            Phone_Label.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Phone_Label.ForeColor = Color.FromArgb(88, 166, 255);
            Phone_Label.Location = new Point(701, 163);
            Phone_Label.Margin = new Padding(4, 0, 4, 0);
            Phone_Label.Name = "Phone_Label";
            Phone_Label.Size = new Size(92, 28);
            Phone_Label.TabIndex = 6;
            Phone_Label.Text = "전화번호";
            // 
            // Name_Label
            // 
            Name_Label.AutoSize = true;
            Name_Label.FlatStyle = FlatStyle.Flat;
            Name_Label.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Name_Label.ForeColor = Color.FromArgb(88, 166, 255);
            Name_Label.Location = new Point(157, 163);
            Name_Label.Margin = new Padding(4, 0, 4, 0);
            Name_Label.Name = "Name_Label";
            Name_Label.Size = new Size(52, 28);
            Name_Label.TabIndex = 6;
            Name_Label.Text = "이름";
            // 
            // Address2_Label
            // 
            Address2_Label.AutoSize = true;
            Address2_Label.FlatStyle = FlatStyle.Flat;
            Address2_Label.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Address2_Label.ForeColor = Color.FromArgb(88, 166, 255);
            Address2_Label.Location = new Point(701, 236);
            Address2_Label.Margin = new Padding(4, 0, 4, 0);
            Address2_Label.Name = "Address2_Label";
            Address2_Label.Size = new Size(92, 28);
            Address2_Label.TabIndex = 6;
            Address2_Label.Text = "상세주소";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(449, 236);
            listBox1.Margin = new Padding(4, 4, 4, 4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(174, 44);
            listBox1.TabIndex = 9;
            // 
            // Btn_newmem
            // 
            Btn_newmem.FlatStyle = FlatStyle.Flat;
            Btn_newmem.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Btn_newmem.ForeColor = Color.FromArgb(88, 166, 255);
            Btn_newmem.Location = new Point(616, 664);
            Btn_newmem.Margin = new Padding(4, 4, 4, 4);
            Btn_newmem.Name = "Btn_newmem";
            Btn_newmem.Size = new Size(130, 43);
            Btn_newmem.TabIndex = 10;
            Btn_newmem.Text = "가입하기";
            Btn_newmem.UseVisualStyleBackColor = true;
            Btn_newmem.Click += Btn_newmem_Click;
            // 
            // Group_Agree
            // 
            Group_Agree.Controls.Add(radioButton1);
            Group_Agree.Controls.Add(radioButton2);
            Group_Agree.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Group_Agree.ForeColor = Color.FromArgb(88, 166, 255);
            Group_Agree.Location = new Point(538, 562);
            Group_Agree.Margin = new Padding(4, 4, 4, 4);
            Group_Agree.Name = "Group_Agree";
            Group_Agree.Padding = new Padding(4, 4, 4, 4);
            Group_Agree.Size = new Size(271, 80);
            Group_Agree.TabIndex = 11;
            Group_Agree.TabStop = false;
            Group_Agree.Text = "동의여부";
            // 
            // Membership
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 27, 34);
            ClientSize = new Size(1292, 784);
            Controls.Add(Group_Agree);
            Controls.Add(Btn_newmem);
            Controls.Add(listBox1);
            Controls.Add(textBox5);
            Controls.Add(Address2_Label);
            Controls.Add(Phone_Label);
            Controls.Add(Pass_Label);
            Controls.Add(Name_Label);
            Controls.Add(Id_Label);
            Controls.Add(comboBox1);
            Controls.Add(Address_Label);
            Controls.Add(richTextBox1);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(Membership_label);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Membership";
            Text = "Membership";
            Group_Agree.ResumeLayout(false);
            Group_Agree.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Membership_label;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private RichTextBox richTextBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label Address_Label;
        private ComboBox comboBox1;
        private TextBox textBox5;
        private Label Id_Label;
        private TextBox textBox4;
        private Label Pass_Label;
        private Label Phone_Label;
        private Label Name_Label;
        private Label Address2_Label;
        private ListBox listBox1;
        private Button Btn_newmem;
        private GroupBox Group_Agree;
    }
}