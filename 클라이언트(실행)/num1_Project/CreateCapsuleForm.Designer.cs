namespace num1_Project
{
    partial class CreateCapsuleForm
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
            label1 = new Label();
            txtTitle = new TextBox();
            label2 = new Label();
            dtpOpenDate = new DateTimePicker();
            label3 = new Label();
            btnAddSong = new Button();
            lstSongs = new ListBox();
            btnRemoveSong = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            songList = new ComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(88, 166, 255);
            label1.Location = new Point(36, 23);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 0;
            label1.Text = "캡슐 이름";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(101, 20);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(206, 23);
            txtTitle.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(88, 166, 255);
            label2.Location = new Point(52, 55);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 2;
            label2.Text = "개봉일";
            // 
            // dtpOpenDate
            // 
            dtpOpenDate.Location = new Point(101, 49);
            dtpOpenDate.Name = "dtpOpenDate";
            dtpOpenDate.Size = new Size(206, 23);
            dtpOpenDate.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(88, 166, 255);
            label3.Location = new Point(36, 81);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 4;
            label3.Text = "노래 제목";
            // 
            // btnAddSong
            // 
            btnAddSong.BackColor = Color.FromArgb(33, 38, 45);
            btnAddSong.FlatStyle = FlatStyle.Flat;
            btnAddSong.ForeColor = SystemColors.Control;
            btnAddSong.Location = new Point(322, 127);
            btnAddSong.Name = "btnAddSong";
            btnAddSong.Size = new Size(75, 23);
            btnAddSong.TabIndex = 6;
            btnAddSong.Text = "노래 추가";
            btnAddSong.UseVisualStyleBackColor = false;
            btnAddSong.Click += btnAddSong_Click;
            // 
            // lstSongs
            // 
            lstSongs.FormattingEnabled = true;
            lstSongs.ItemHeight = 15;
            lstSongs.Location = new Point(101, 107);
            lstSongs.Name = "lstSongs";
            lstSongs.Size = new Size(206, 169);
            lstSongs.TabIndex = 7;
            // 
            // btnRemoveSong
            // 
            btnRemoveSong.BackColor = Color.FromArgb(33, 38, 45);
            btnRemoveSong.FlatStyle = FlatStyle.Flat;
            btnRemoveSong.ForeColor = SystemColors.Control;
            btnRemoveSong.Location = new Point(322, 183);
            btnRemoveSong.Name = "btnRemoveSong";
            btnRemoveSong.Size = new Size(75, 23);
            btnRemoveSong.TabIndex = 8;
            btnRemoveSong.Text = "노래 삭제";
            btnRemoveSong.UseVisualStyleBackColor = false;
            btnRemoveSong.Click += btnRemoveSong_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(33, 38, 45);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = SystemColors.Control;
            btnSave.Location = new Point(101, 345);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 9;
            btnSave.Text = "저장";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(33, 38, 45);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = SystemColors.Control;
            btnCancel.Location = new Point(232, 345);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "취소";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // songList
            // 
            songList.FormattingEnabled = true;
            songList.Items.AddRange(new object[] { "'", ";", "kjo" });
            songList.Location = new Point(101, 78);
            songList.Name = "songList";
            songList.Size = new Size(206, 23);
            songList.TabIndex = 11;
            // 
            // CreateCapsuleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 27, 34);
            ClientSize = new Size(419, 401);
            Controls.Add(songList);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnRemoveSong);
            Controls.Add(lstSongs);
            Controls.Add(btnAddSong);
            Controls.Add(label3);
            Controls.Add(dtpOpenDate);
            Controls.Add(label2);
            Controls.Add(txtTitle);
            Controls.Add(label1);
            Name = "CreateCapsuleForm";
            Text = "캡슐 생성";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtTitle;
        private Label label2;
        private DateTimePicker dtpOpenDate;
        private Label label3;
        private Button btnAddSong;
        private ListBox lstSongs;
        private Button btnRemoveSong;
        private Button btnSave;
        private Button btnCancel;
        private ComboBox songList;
    }
}