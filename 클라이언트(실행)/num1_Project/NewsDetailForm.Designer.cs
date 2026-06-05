namespace num1_Project
{
    partial class NewsDetailForm
    {
     
        private System.ComponentModel.IContainer components = null;

       
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


        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblDate = new Label();
            lblHeadline = new Label();
            picMain = new PictureBox();
            lblImgCaption = new Label();
            btnClose = new Button();
            pnlBody = new Panel();
            lblBody = new Label();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picMain).BeginInit();
            pnlBody.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(13, 17, 23);
            pnlTop.Controls.Add(lblDate);
            pnlTop.Controls.Add(lblHeadline);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.MaximumSize = new Size(0, 80);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(642, 80);
            pnlTop.TabIndex = 0;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("맑은 고딕", 8.5F);
            lblDate.ForeColor = Color.FromArgb(139, 148, 158);
            lblDate.Location = new Point(21, 45);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(0, 20);
            lblDate.TabIndex = 1;
            // 
            // lblHeadline
            // 
            lblHeadline.AutoEllipsis = true;
            lblHeadline.AutoSize = true;
            lblHeadline.Font = new Font("맑은 고딕", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblHeadline.ForeColor = Color.FromArgb(88, 166, 255);
            lblHeadline.Location = new Point(21, 12);
            lblHeadline.Name = "lblHeadline";
            lblHeadline.Size = new Size(0, 31);
            lblHeadline.TabIndex = 0;
            // 
            // picMain
            // 
            picMain.BackColor = Color.FromArgb(13, 17, 23);
            picMain.Dock = DockStyle.Top;
            picMain.Location = new Point(0, 80);
            picMain.Name = "picMain";
            picMain.Size = new Size(642, 240);
            picMain.SizeMode = PictureBoxSizeMode.Zoom;
            picMain.TabIndex = 1;
            picMain.TabStop = false;
            // 
            // lblImgCaption
            // 
            lblImgCaption.AutoSize = true;
            lblImgCaption.BackColor = Color.FromArgb(13, 17, 23);
            lblImgCaption.Dock = DockStyle.Top;
            lblImgCaption.Font = new Font("맑은 고딕", 7.5F);
            lblImgCaption.ForeColor = Color.FromArgb(100, 110, 120);
            lblImgCaption.Location = new Point(0, 320);
            lblImgCaption.Name = "lblImgCaption";
            lblImgCaption.Padding = new Padding(0, 0, 12, 0);
            lblImgCaption.Size = new Size(12, 17);
            lblImgCaption.TabIndex = 2;
            lblImgCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(46, 117, 182);
            btnClose.Cursor = Cursors.Hand;
            btnClose.Dock = DockStyle.Bottom;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("맑은 고딕", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(0, 513);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(642, 40);
            btnClose.TabIndex = 3;
            btnClose.Text = "닫기";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // pnlBody
            // 
            pnlBody.AutoScroll = true;
            pnlBody.Controls.Add(lblBody);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 337);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(21, 16, 21, 60);
            pnlBody.Size = new Size(642, 176);
            pnlBody.TabIndex = 4;
            // 
            // lblBody
            // 
            lblBody.AutoSize = true;
            lblBody.Dock = DockStyle.Fill;
            lblBody.Font = new Font("맑은 고딕", 10F);
            lblBody.ForeColor = Color.FromArgb(200, 210, 220);
            lblBody.Location = new Point(21, 16);
            lblBody.MaximumSize = new Size(600, 0);
            lblBody.Name = "lblBody";
            lblBody.Size = new Size(0, 23);
            lblBody.TabIndex = 0;
            // 
            // NewsDetailForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 27, 34);
            ClientSize = new Size(642, 553);
            Controls.Add(pnlBody);
            Controls.Add(btnClose);
            Controls.Add(lblImgCaption);
            Controls.Add(picMain);
            Controls.Add(pnlTop);
            ForeColor = Color.FromArgb(230, 237, 243);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewsDetailForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "뉴스 상세 보기";
            Load += NewsDetailForm_Load;
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picMain).EndInit();
            pnlBody.ResumeLayout(false);
            pnlBody.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlTop;
        private Label lblDate;
        private Label lblHeadline;
        private PictureBox picMain;
        private Label lblImgCaption;
        private Button btnClose;
        private Panel pnlBody;
        private Label lblBody;
    }
}