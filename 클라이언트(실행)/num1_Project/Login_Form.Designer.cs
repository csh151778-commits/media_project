namespace num1_Project
{
    partial class Login_Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Id_Label = new Label();
            Password_Label = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            Btn_Login = new Button();
            Btn_Membership = new Button();
            Btn_findid = new Button();
            Login_Group = new GroupBox();
            Membership_Label = new Label();
            Find_Label = new Label();
            Btn_findpass = new Button();
            checkBox1 = new CheckBox();
            Login_Group.SuspendLayout();
            SuspendLayout();
            // 
            // Id_Label
            // 
            Id_Label.AutoSize = true;
            Id_Label.Location = new Point(22, 69);
            Id_Label.Margin = new Padding(4, 0, 4, 0);
            Id_Label.Name = "Id_Label";
            Id_Label.Size = new Size(100, 32);
            Id_Label.TabIndex = 0;
            Id_Label.Text = "아이디 :";
            // 
            // Password_Label
            // 
            Password_Label.AutoSize = true;
            Password_Label.Location = new Point(22, 143);
            Password_Label.Margin = new Padding(4, 0, 4, 0);
            Password_Label.Name = "Password_Label";
            Password_Label.Size = new Size(124, 32);
            Password_Label.TabIndex = 1;
            Password_Label.Text = "비밀번호 :";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.White;
            textBox1.Location = new Point(152, 65);
            textBox1.Margin = new Padding(4, 4, 4, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(539, 39);
            textBox1.TabIndex = 2;
            textBox1.Enter += textBox1_Enter;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.White;
            textBox2.Location = new Point(152, 139);
            textBox2.Margin = new Padding(4, 4, 4, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(539, 39);
            textBox2.TabIndex = 2;
            textBox2.Enter += textBox2_Enter;
            // 
            // Btn_Login
            // 
            Btn_Login.FlatStyle = FlatStyle.Flat;
            Btn_Login.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Btn_Login.Location = new Point(280, 212);
            Btn_Login.Margin = new Padding(4, 4, 4, 4);
            Btn_Login.Name = "Btn_Login";
            Btn_Login.Size = new Size(149, 60);
            Btn_Login.TabIndex = 3;
            Btn_Login.Text = "로그인";
            Btn_Login.UseVisualStyleBackColor = true;
            Btn_Login.Click += Btn_Login_Click;
            // 
            // Btn_Membership
            // 
            Btn_Membership.FlatStyle = FlatStyle.Flat;
            Btn_Membership.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Btn_Membership.ForeColor = Color.FromArgb(88, 166, 255);
            Btn_Membership.Location = new Point(203, 449);
            Btn_Membership.Margin = new Padding(4, 4, 4, 4);
            Btn_Membership.Name = "Btn_Membership";
            Btn_Membership.Size = new Size(129, 43);
            Btn_Membership.TabIndex = 4;
            Btn_Membership.Text = "가입하기";
            Btn_Membership.UseVisualStyleBackColor = true;
            Btn_Membership.Click += Btn_Membership_Click;
            // 
            // Btn_findid
            // 
            Btn_findid.FlatStyle = FlatStyle.Flat;
            Btn_findid.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Btn_findid.ForeColor = Color.FromArgb(88, 166, 255);
            Btn_findid.Location = new Point(516, 449);
            Btn_findid.Margin = new Padding(4, 4, 4, 4);
            Btn_findid.Name = "Btn_findid";
            Btn_findid.Size = new Size(130, 43);
            Btn_findid.TabIndex = 5;
            Btn_findid.Text = "아이디 찾기";
            Btn_findid.UseVisualStyleBackColor = true;
            Btn_findid.Click += Btn_findid_Click;
            // 
            // Login_Group
            // 
            Login_Group.Controls.Add(checkBox1);
            Login_Group.Controls.Add(Id_Label);
            Login_Group.Controls.Add(Password_Label);
            Login_Group.Controls.Add(textBox1);
            Login_Group.Controls.Add(Btn_Login);
            Login_Group.Controls.Add(textBox2);
            Login_Group.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Login_Group.ForeColor = Color.FromArgb(88, 166, 255);
            Login_Group.Location = new Point(149, 68);
            Login_Group.Margin = new Padding(4, 4, 4, 4);
            Login_Group.Name = "Login_Group";
            Login_Group.Padding = new Padding(4, 4, 4, 4);
            Login_Group.Size = new Size(748, 313);
            Login_Group.TabIndex = 6;
            Login_Group.TabStop = false;
            Login_Group.Text = "로그인";
            // 
            // Membership_Label
            // 
            Membership_Label.AutoSize = true;
            Membership_Label.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Membership_Label.ForeColor = Color.FromArgb(88, 166, 255);
            Membership_Label.Location = new Point(203, 413);
            Membership_Label.Margin = new Padding(4, 0, 4, 0);
            Membership_Label.Name = "Membership_Label";
            Membership_Label.Size = new Size(200, 23);
            Membership_Label.TabIndex = 7;
            Membership_Label.Text = "아직 회원이 아니신가요?";
            // 
            // Find_Label
            // 
            Find_Label.AutoSize = true;
            Find_Label.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Find_Label.ForeColor = Color.FromArgb(88, 166, 255);
            Find_Label.Location = new Point(510, 413);
            Find_Label.Margin = new Padding(4, 0, 4, 0);
            Find_Label.Name = "Find_Label";
            Find_Label.Size = new Size(365, 23);
            Find_Label.TabIndex = 8;
            Find_Label.Text = "아이디 혹은 비밀번호가 기억나지 않으시나요?";
            // 
            // Btn_findpass
            // 
            Btn_findpass.FlatStyle = FlatStyle.Flat;
            Btn_findpass.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Btn_findpass.ForeColor = Color.FromArgb(88, 166, 255);
            Btn_findpass.Location = new Point(699, 449);
            Btn_findpass.Margin = new Padding(4, 4, 4, 4);
            Btn_findpass.Name = "Btn_findpass";
            Btn_findpass.Size = new Size(141, 43);
            Btn_findpass.TabIndex = 5;
            Btn_findpass.Text = "비밀번호 찾기";
            Btn_findpass.UseVisualStyleBackColor = true;
            Btn_findpass.Click += Btn_findpass_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            checkBox1.Location = new Point(597, 240);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(94, 32);
            checkBox1.TabIndex = 4;
            checkBox1.Text = "관리자";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // Login_Form
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 27, 34);
            ClientSize = new Size(1029, 600);
            Controls.Add(Find_Label);
            Controls.Add(Membership_Label);
            Controls.Add(Login_Group);
            Controls.Add(Btn_findpass);
            Controls.Add(Btn_findid);
            Controls.Add(Btn_Membership);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Login_Form";
            Text = "Login_Form";
            Login_Group.ResumeLayout(false);
            Login_Group.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Id_Label;
        private Label Password_Label;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button Btn_Login;
        private Button Btn_Membership;
        private Button Btn_findid;
        private GroupBox Login_Group;
        private Label Membership_Label;
        private Label Find_Label;
        private Button Btn_findpass;
        private CheckBox checkBox1;
    }
}
