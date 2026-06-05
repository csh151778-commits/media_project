namespace num1_Project
{
    partial class Pay
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
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            groupBox1 = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            button1 = new Button();
            label3 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(31, 46);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(164, 35);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "일반요금제: ";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.ForeColor = Color.FromArgb(88, 166, 255);
            radioButton2.Location = new Point(31, 129);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(154, 35);
            radioButton2.TabIndex = 0;
            radioButton2.TabStop = true;
            radioButton2.Text = "VIP 요금제:";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Font = new Font("맑은 고딕", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBox1.ForeColor = Color.FromArgb(88, 166, 255);
            groupBox1.Location = new Point(26, 40);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(681, 230);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "요금제 선택";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(184, 131);
            label2.Name = "label2";
            label2.Size = new Size(491, 62);
            label2.TabIndex = 2;
            label2.Text = "365일 노래 선택 제약 없이 무제한 듣기 가능. \r\n최대 5개 다운로드 가능!";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(184, 48);
            label1.Name = "label1";
            label1.Size = new Size(422, 62);
            label1.TabIndex = 1;
            label1.Text = "노래 선택 제약 없이 무제한 듣기 가능! \r\n한 달 주기로 결제 진행.";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(radioButton4);
            groupBox2.Controls.Add(radioButton3);
            groupBox2.Font = new Font("맑은 고딕", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 129);
            groupBox2.ForeColor = Color.FromArgb(88, 166, 255);
            groupBox2.Location = new Point(26, 287);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(402, 152);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "결제수단 선택";
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(211, 67);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(158, 35);
            radioButton4.TabIndex = 0;
            radioButton4.TabStop = true;
            radioButton4.Text = "무통장 입금";
            radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(31, 67);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(135, 35);
            radioButton3.TabIndex = 0;
            radioButton3.TabStop = true;
            radioButton3.Text = "카드 결제";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(88, 166, 255);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("맑은 고딕", 15F);
            button1.ForeColor = SystemColors.ControlText;
            button1.Location = new Point(494, 344);
            button1.Name = "button1";
            button1.Size = new Size(213, 95);
            button1.TabIndex = 3;
            button1.Text = "결제창 이동";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 18F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label3.ForeColor = Color.FromArgb(88, 166, 255);
            label3.Location = new Point(558, 287);
            label3.Name = "label3";
            label3.Size = new Size(65, 41);
            label3.TabIndex = 4;
            label3.Text = "0원";
            // 
            // Pay
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 27, 34);
            ClientSize = new Size(736, 469);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Pay";
            Text = "Pay";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private GroupBox groupBox2;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private Button button1;
        private Label label3;
    }
}