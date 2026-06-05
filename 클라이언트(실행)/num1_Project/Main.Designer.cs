namespace num1_Project
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            tabControl1 = new TabControl();
            tabPage2 = new TabPage();
            tabControl2 = new TabControl();
            tabPage4 = new TabPage();
            pnlPlaylist = new Panel();
            lblPlaylistHdr = new Label();
            tprecent = new TabPage();
            pnlrecent = new Panel();
            lstrecent = new ListBox();
            tpmost = new TabPage();
            pnlmost = new Panel();
            lstmost = new ListBox();
            pnlVisualizer = new Panel();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            albumArt = new PictureBox();
            pictureBox3 = new PictureBox();
            lblTotal = new Label();
            lblCurrent = new Label();
            pictureBox2 = new PictureBox();
            song_DownLoad = new PictureBox();
            btnPlay1 = new Button();
            pictureBoxWave = new PictureBox();
            pnlControls = new Panel();
            volBar = new TrackBar();
            btnRepeat = new Button();
            btnNext = new Button();
            btnPlay = new Button();
            btnShuffle = new Button();
            btnPrev = new Button();
            progressBar1 = new SeekBar();
            lblArtist = new Label();
            lblTitle = new Label();
            tabPage1 = new TabPage();
            tabMain = new TabControl();
            tpNews = new TabPage();
            lstNews = new ListBox();
            lblNewsTitle = new Label();
            tpChart = new TabPage();
            dgvChart = new DataGridView();
            colRank = new DataGridViewTextBoxColumn();
            colTitle = new DataGridViewTextBoxColumn();
            colArtist = new DataGridViewTextBoxColumn();
            colGenre = new DataGridViewTextBoxColumn();
            colNote = new DataGridViewTextBoxColumn();
            lblChartTitle = new Label();
            pnlFooter = new Panel();
            lblFooter = new Label();
            pnlYearCtrl = new Panel();
            lblYearNum = new Label();
            lblYearSub = new Label();
            btnpreView = new Button();
            trkYear = new TrackBar();
            btnNexts = new Button();
            lblMin = new Label();
            lblMax = new Label();
            pnlHeader = new Panel();
            lblMainTitle = new Label();
            lblDesc = new Label();
            tabPage3 = new TabPage();
            flowCards = new FlowLayoutPanel();
            panelTop = new Panel();
            btnCreateTop = new Button();
            label1 = new Label();
            timerWave = new System.Windows.Forms.Timer(components);
            timer1 = new System.Windows.Forms.Timer(components);
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabControl2.SuspendLayout();
            tabPage4.SuspendLayout();
            tprecent.SuspendLayout();
            pnlrecent.SuspendLayout();
            tpmost.SuspendLayout();
            pnlmost.SuspendLayout();
            pnlVisualizer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)albumArt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)song_DownLoad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxWave).BeginInit();
            pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)progressBar1).BeginInit();
            tabPage1.SuspendLayout();
            tabMain.SuspendLayout();
            tpNews.SuspendLayout();
            tpChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChart).BeginInit();
            pnlFooter.SuspendLayout();
            pnlYearCtrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkYear).BeginInit();
            pnlHeader.SuspendLayout();
            tabPage3.SuspendLayout();
            panelTop.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Appearance = TabAppearance.Buttons;
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.ItemSize = new Size(130, 36);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1397, 711);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.TabIndex = 31;
            tabControl1.DrawItem += tabControl1_DrawItem;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.FromArgb(13, 17, 23);
            tabPage2.Controls.Add(tabControl2);
            tabPage2.Controls.Add(pnlVisualizer);
            tabPage2.Location = new Point(4, 40);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1389, 667);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "🎵플레이어";
            // 
            // tabControl2
            // 
            tabControl2.Controls.Add(tabPage4);
            tabControl2.Controls.Add(tprecent);
            tabControl2.Controls.Add(tpmost);
            tabControl2.Dock = DockStyle.Fill;
            tabControl2.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl2.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tabControl2.Location = new Point(569, 3);
            tabControl2.Margin = new Padding(2);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(817, 661);
            tabControl2.TabIndex = 8;
            tabControl2.DrawItem += tabControl2_DrawItem;
            // 
            // tabPage4
            // 
            tabPage4.BackColor = Color.FromArgb(22, 27, 34);
            tabPage4.Controls.Add(pnlPlaylist);
            tabPage4.Controls.Add(lblPlaylistHdr);
            tabPage4.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Margin = new Padding(2);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(2);
            tabPage4.Size = new Size(809, 633);
            tabPage4.TabIndex = 0;
            tabPage4.Text = "🎵 플레이리스트";
            // 
            // pnlPlaylist
            // 
            pnlPlaylist.Location = new Point(2, 0);
            pnlPlaylist.Margin = new Padding(2);
            pnlPlaylist.Name = "pnlPlaylist";
            pnlPlaylist.Size = new Size(810, 647);
            pnlPlaylist.TabIndex = 2;
            // 
            // lblPlaylistHdr
            // 
            lblPlaylistHdr.AutoSize = true;
            lblPlaylistHdr.Dock = DockStyle.Top;
            lblPlaylistHdr.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblPlaylistHdr.ForeColor = Color.FromArgb(139, 148, 158);
            lblPlaylistHdr.Location = new Point(2, 2);
            lblPlaylistHdr.Name = "lblPlaylistHdr";
            lblPlaylistHdr.Size = new Size(0, 15);
            lblPlaylistHdr.TabIndex = 1;
            // 
            // tprecent
            // 
            tprecent.BackColor = Color.FromArgb(22, 27, 34);
            tprecent.Controls.Add(pnlrecent);
            tprecent.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tprecent.Location = new Point(4, 24);
            tprecent.Margin = new Padding(2);
            tprecent.Name = "tprecent";
            tprecent.Padding = new Padding(2);
            tprecent.Size = new Size(809, 633);
            tprecent.TabIndex = 1;
            tprecent.Text = "🕐 최근 재생 목록";
            // 
            // pnlrecent
            // 
            pnlrecent.Controls.Add(lstrecent);
            pnlrecent.Dock = DockStyle.Fill;
            pnlrecent.Location = new Point(2, 2);
            pnlrecent.Name = "pnlrecent";
            pnlrecent.Size = new Size(805, 629);
            pnlrecent.TabIndex = 0;
            // 
            // lstrecent
            // 
            lstrecent.BackColor = Color.FromArgb(13, 17, 23);
            lstrecent.BorderStyle = BorderStyle.None;
            lstrecent.Cursor = Cursors.Hand;
            lstrecent.Dock = DockStyle.Fill;
            lstrecent.DrawMode = DrawMode.OwnerDrawFixed;
            lstrecent.Font = new Font("맑은 고딕", 11F);
            lstrecent.ForeColor = Color.FromArgb(200, 210, 220);
            lstrecent.ItemHeight = 48;
            lstrecent.Location = new Point(0, 0);
            lstrecent.Name = "lstrecent";
            lstrecent.Size = new Size(805, 629);
            lstrecent.TabIndex = 1;
            // 
            // tpmost
            // 
            tpmost.BackColor = Color.FromArgb(22, 27, 34);
            tpmost.Controls.Add(pnlmost);
            tpmost.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tpmost.Location = new Point(4, 24);
            tpmost.Name = "tpmost";
            tpmost.Size = new Size(809, 633);
            tpmost.TabIndex = 2;
            tpmost.Text = "🏆 가장 많이 들은 음악";
            // 
            // pnlmost
            // 
            pnlmost.Controls.Add(lstmost);
            pnlmost.Dock = DockStyle.Fill;
            pnlmost.Location = new Point(0, 0);
            pnlmost.Name = "pnlmost";
            pnlmost.Size = new Size(809, 633);
            pnlmost.TabIndex = 0;
            // 
            // lstmost
            // 
            lstmost.BackColor = Color.FromArgb(13, 17, 23);
            lstmost.BorderStyle = BorderStyle.None;
            lstmost.Cursor = Cursors.Hand;
            lstmost.Dock = DockStyle.Fill;
            lstmost.DrawMode = DrawMode.OwnerDrawFixed;
            lstmost.Font = new Font("맑은 고딕", 11F);
            lstmost.ForeColor = Color.FromArgb(200, 210, 220);
            lstmost.ItemHeight = 48;
            lstmost.Location = new Point(0, 0);
            lstmost.Name = "lstmost";
            lstmost.Size = new Size(809, 633);
            lstmost.TabIndex = 1;
            // 
            // pnlVisualizer
            // 
            pnlVisualizer.BackColor = Color.FromArgb(22, 27, 34);
            pnlVisualizer.Controls.Add(button3);
            pnlVisualizer.Controls.Add(button2);
            pnlVisualizer.Controls.Add(button1);
            pnlVisualizer.Controls.Add(albumArt);
            pnlVisualizer.Controls.Add(pictureBox3);
            pnlVisualizer.Controls.Add(lblTotal);
            pnlVisualizer.Controls.Add(lblCurrent);
            pnlVisualizer.Controls.Add(pictureBox2);
            pnlVisualizer.Controls.Add(song_DownLoad);
            pnlVisualizer.Controls.Add(btnPlay1);
            pnlVisualizer.Controls.Add(pictureBoxWave);
            pnlVisualizer.Controls.Add(pnlControls);
            pnlVisualizer.Controls.Add(progressBar1);
            pnlVisualizer.Controls.Add(lblArtist);
            pnlVisualizer.Controls.Add(lblTitle);
            pnlVisualizer.Dock = DockStyle.Left;
            pnlVisualizer.Location = new Point(3, 3);
            pnlVisualizer.Name = "pnlVisualizer";
            pnlVisualizer.Size = new Size(566, 661);
            pnlVisualizer.TabIndex = 5;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.None;
            button3.Location = new Point(23, 39);
            button3.Name = "button3";
            button3.Size = new Size(93, 23);
            button3.TabIndex = 11;
            button3.Text = "로그아웃";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(459, 10);
            button2.Name = "button2";
            button2.Size = new Size(102, 23);
            button2.TabIndex = 10;
            button2.Text = "관리자 테스트";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(23, 10);
            button1.Name = "button1";
            button1.Size = new Size(93, 23);
            button1.TabIndex = 10;
            button1.Text = "요금제 가입";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // albumArt
            // 
            albumArt.Location = new Point(149, 40);
            albumArt.Name = "albumArt";
            albumArt.Size = new Size(285, 272);
            albumArt.TabIndex = 0;
            albumArt.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.테두리;
            pictureBox3.Location = new Point(23, 10);
            pictureBox3.Margin = new Padding(2);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(538, 338);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 9;
            pictureBox3.TabStop = false;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Consolas", 8F);
            lblTotal.ForeColor = Color.FromArgb(139, 148, 158);
            lblTotal.Location = new Point(486, 388);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(31, 13);
            lblTotal.TabIndex = 3;
            lblTotal.Text = "5:55";
            // 
            // lblCurrent
            // 
            lblCurrent.AutoSize = true;
            lblCurrent.Font = new Font("Consolas", 8F);
            lblCurrent.ForeColor = Color.FromArgb(139, 148, 158);
            lblCurrent.Location = new Point(93, 388);
            lblCurrent.Name = "lblCurrent";
            lblCurrent.Size = new Size(31, 13);
            lblCurrent.TabIndex = 2;
            lblCurrent.Text = "1:28";
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(93, 555);
            pictureBox2.Margin = new Padding(2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(143, 50);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // song_DownLoad
            // 
            song_DownLoad.BackgroundImageLayout = ImageLayout.Zoom;
            song_DownLoad.Image = (Image)resources.GetObject("song_DownLoad.Image");
            song_DownLoad.Location = new Point(297, 548);
            song_DownLoad.Margin = new Padding(2);
            song_DownLoad.Name = "song_DownLoad";
            song_DownLoad.Size = new Size(170, 56);
            song_DownLoad.SizeMode = PictureBoxSizeMode.StretchImage;
            song_DownLoad.TabIndex = 7;
            song_DownLoad.TabStop = false;
            song_DownLoad.Click += song_DownLoad_Click;
            // 
            // btnPlay1
            // 
            btnPlay1.BackgroundImage = Properties.Resources.프로젝트_유튜브_버튼;
            btnPlay1.BackgroundImageLayout = ImageLayout.Zoom;
            btnPlay1.FlatStyle = FlatStyle.Flat;
            btnPlay1.Location = new Point(3, 626);
            btnPlay1.Margin = new Padding(2);
            btnPlay1.Name = "btnPlay1";
            btnPlay1.Size = new Size(61, 43);
            btnPlay1.TabIndex = 6;
            btnPlay1.UseVisualStyleBackColor = true;
            btnPlay1.Click += btnPlay1_Click;
            btnPlay1.MouseDown += btnPlay1_MouseDown;
            btnPlay1.MouseUp += btnPlay1_MouseUp;
            // 
            // pictureBoxWave
            // 
            pictureBoxWave.Location = new Point(86, 464);
            pictureBoxWave.Name = "pictureBoxWave";
            pictureBoxWave.Size = new Size(388, 79);
            pictureBoxWave.TabIndex = 5;
            pictureBoxWave.TabStop = false;
            pictureBoxWave.Paint += pictureBoxWave_Paint;
            // 
            // pnlControls
            // 
            pnlControls.BackColor = Color.FromArgb(22, 27, 34);
            pnlControls.Controls.Add(volBar);
            pnlControls.Controls.Add(btnRepeat);
            pnlControls.Controls.Add(btnNext);
            pnlControls.Controls.Add(btnPlay);
            pnlControls.Controls.Add(btnShuffle);
            pnlControls.Controls.Add(btnPrev);
            pnlControls.Location = new Point(93, 414);
            pnlControls.Name = "pnlControls";
            pnlControls.Size = new Size(380, 44);
            pnlControls.TabIndex = 4;
            // 
            // volBar
            // 
            volBar.Location = new Point(230, 13);
            volBar.Maximum = 100;
            volBar.Name = "volBar";
            volBar.Size = new Size(120, 45);
            volBar.TabIndex = 5;
            volBar.TickStyle = TickStyle.None;
            volBar.Value = 80;
            // 
            // btnRepeat
            // 
            btnRepeat.FlatAppearance.BorderSize = 0;
            btnRepeat.FlatStyle = FlatStyle.Flat;
            btnRepeat.Font = new Font("Segoe UI Emoji", 13F);
            btnRepeat.ForeColor = Color.FromArgb(139, 148, 158);
            btnRepeat.Location = new Point(174, 6);
            btnRepeat.Name = "btnRepeat";
            btnRepeat.Size = new Size(32, 32);
            btnRepeat.TabIndex = 4;
            btnRepeat.Text = "↻";
            btnRepeat.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Segoe UI Emoji", 13F);
            btnNext.ForeColor = Color.FromArgb(230, 237, 243);
            btnNext.Location = new Point(134, 6);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(32, 32);
            btnNext.TabIndex = 3;
            btnNext.Text = "⏭";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Paint += btnNext_Paint;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.FromArgb(46, 117, 182);
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.Font = new Font("Segoe UI Emoji", 13F);
            btnPlay.ForeColor = Color.White;
            btnPlay.Location = new Point(84, 0);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(44, 44);
            btnPlay.TabIndex = 2;
            btnPlay.Text = "▶";
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Paint += btnPlay_Paint;
            // 
            // btnShuffle
            // 
            btnShuffle.FlatAppearance.BorderSize = 0;
            btnShuffle.FlatStyle = FlatStyle.Flat;
            btnShuffle.Font = new Font("Segoe UI Emoji", 13F);
            btnShuffle.ForeColor = Color.FromArgb(139, 148, 158);
            btnShuffle.Location = new Point(0, 6);
            btnShuffle.Name = "btnShuffle";
            btnShuffle.Size = new Size(32, 32);
            btnShuffle.TabIndex = 0;
            btnShuffle.Text = "⇄";
            btnShuffle.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.FlatAppearance.BorderSize = 0;
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.Font = new Font("Segoe UI Emoji", 13F);
            btnPrev.ForeColor = Color.FromArgb(230, 237, 243);
            btnPrev.Location = new Point(40, 6);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(32, 32);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "⏮";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Paint += btnPrev_Paint;
            // 
            // progressBar1
            // 
            progressBar1.BackColor = Color.FromArgb(22, 27, 34);
            progressBar1.Location = new Point(130, 369);
            progressBar1.Maximum = 100;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(350, 45);
            progressBar1.TabIndex = 1;
            progressBar1.TickStyle = TickStyle.None;
            // 
            // lblArtist
            // 
            lblArtist.AutoSize = true;
            lblArtist.ForeColor = Color.FromArgb(139, 148, 158);
            lblArtist.Location = new Point(227, 369);
            lblArtist.Name = "lblArtist";
            lblArtist.Size = new Size(101, 15);
            lblArtist.TabIndex = 2;
            lblArtist.Text = "Queen · 1975 · 🇬🇧";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(230, 237, 243);
            lblTitle.Location = new Point(143, 346);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(188, 25);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Bohemian Rhapsody";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tabMain);
            tabPage1.Controls.Add(pnlFooter);
            tabPage1.Controls.Add(pnlYearCtrl);
            tabPage1.Controls.Add(pnlHeader);
            tabPage1.Location = new Point(4, 40);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1389, 667);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "Chart & News";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabMain
            // 
            tabMain.Appearance = TabAppearance.FlatButtons;
            tabMain.Controls.Add(tpNews);
            tabMain.Controls.Add(tpChart);
            tabMain.Dock = DockStyle.Fill;
            tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabMain.ItemSize = new Size(160, 34);
            tabMain.Location = new Point(3, 137);
            tabMain.Name = "tabMain";
            tabMain.Padding = new Point(12, 6);
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1383, 497);
            tabMain.SizeMode = TabSizeMode.Fixed;
            tabMain.TabIndex = 4;
            tabMain.DrawItem += tabMain_DrawItem;
            // 
            // tpNews
            // 
            tpNews.BackColor = Color.FromArgb(13, 17, 23);
            tpNews.Controls.Add(lstNews);
            tpNews.Controls.Add(lblNewsTitle);
            tpNews.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            tpNews.Location = new Point(4, 38);
            tpNews.Name = "tpNews";
            tpNews.Size = new Size(1375, 455);
            tpNews.TabIndex = 0;
            tpNews.Text = "  📰  가요·연예 뉴스";
            // 
            // lstNews
            // 
            lstNews.BackColor = Color.FromArgb(13, 17, 23);
            lstNews.BorderStyle = BorderStyle.None;
            lstNews.Cursor = Cursors.Hand;
            lstNews.Dock = DockStyle.Fill;
            lstNews.DrawMode = DrawMode.OwnerDrawFixed;
            lstNews.Font = new Font("맑은 고딕", 11F);
            lstNews.ForeColor = Color.FromArgb(200, 210, 220);
            lstNews.ItemHeight = 48;
            lstNews.Location = new Point(0, 42);
            lstNews.Name = "lstNews";
            lstNews.Size = new Size(1375, 413);
            lstNews.TabIndex = 0;
            lstNews.MouseClick += lstNews_MouseClick;
            lstNews.DrawItem += lstNews_DrawItem;
            // 
            // lblNewsTitle
            // 
            lblNewsTitle.BackColor = Color.FromArgb(22, 27, 34);
            lblNewsTitle.Dock = DockStyle.Top;
            lblNewsTitle.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            lblNewsTitle.ForeColor = Color.FromArgb(230, 237, 243);
            lblNewsTitle.Location = new Point(0, 0);
            lblNewsTitle.Name = "lblNewsTitle";
            lblNewsTitle.Padding = new Padding(16, 0, 0, 0);
            lblNewsTitle.Size = new Size(1375, 42);
            lblNewsTitle.TabIndex = 1;
            lblNewsTitle.Text = "📰  가요·연예 뉴스  (클릭하면 상세 내용을 볼 수 있어요)";
            lblNewsTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tpChart
            // 
            tpChart.BackColor = Color.FromArgb(13, 17, 23);
            tpChart.Controls.Add(dgvChart);
            tpChart.Controls.Add(lblChartTitle);
            tpChart.Location = new Point(4, 38);
            tpChart.Name = "tpChart";
            tpChart.Size = new Size(1375, 455);
            tpChart.TabIndex = 1;
            tpChart.Text = "  🏆  HOT 차트";
            // 
            // dgvChart
            // 
            dgvChart.AllowUserToAddRows = false;
            dgvChart.AllowUserToDeleteRows = false;
            dgvChart.BackgroundColor = Color.FromArgb(13, 17, 23);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(22, 27, 34);
            dataGridViewCellStyle1.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(139, 148, 158);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(22, 27, 34);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(139, 148, 158);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvChart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvChart.ColumnHeadersHeight = 36;
            dgvChart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChart.Columns.AddRange(new DataGridViewColumn[] { colRank, colTitle, colArtist, colGenre, colNote });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(13, 17, 23);
            dataGridViewCellStyle2.Font = new Font("맑은 고딕", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(230, 237, 243);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(28, 46, 74);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(230, 237, 243);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvChart.DefaultCellStyle = dataGridViewCellStyle2;
            dgvChart.Dock = DockStyle.Fill;
            dgvChart.EnableHeadersVisualStyles = false;
            dgvChart.GridColor = Color.FromArgb(33, 38, 45);
            dgvChart.Location = new Point(0, 42);
            dgvChart.Margin = new Padding(2);
            dgvChart.MultiSelect = false;
            dgvChart.Name = "dgvChart";
            dgvChart.ReadOnly = true;
            dgvChart.RowHeadersVisible = false;
            dgvChart.RowHeadersWidth = 51;
            dgvChart.RowTemplate.Height = 46;
            dgvChart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChart.Size = new Size(1375, 413);
            dgvChart.TabIndex = 1;
            // 
            // colRank
            // 
            colRank.HeaderText = "순위";
            colRank.MinimumWidth = 6;
            colRank.Name = "colRank";
            colRank.ReadOnly = true;
            colRank.Width = 56;
            // 
            // colTitle
            // 
            colTitle.HeaderText = "곡명";
            colTitle.MinimumWidth = 6;
            colTitle.Name = "colTitle";
            colTitle.ReadOnly = true;
            colTitle.Width = 200;
            // 
            // colArtist
            // 
            colArtist.HeaderText = "가수";
            colArtist.MinimumWidth = 6;
            colArtist.Name = "colArtist";
            colArtist.ReadOnly = true;
            colArtist.Width = 150;
            // 
            // colGenre
            // 
            colGenre.HeaderText = "장르";
            colGenre.MinimumWidth = 6;
            colGenre.Name = "colGenre";
            colGenre.ReadOnly = true;
            colGenre.Width = 90;
            // 
            // colNote
            // 
            colNote.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNote.HeaderText = "비고";
            colNote.MinimumWidth = 6;
            colNote.Name = "colNote";
            colNote.ReadOnly = true;
            // 
            // lblChartTitle
            // 
            lblChartTitle.BackColor = Color.FromArgb(22, 27, 34);
            lblChartTitle.Dock = DockStyle.Top;
            lblChartTitle.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            lblChartTitle.ForeColor = Color.FromArgb(230, 237, 243);
            lblChartTitle.Location = new Point(0, 0);
            lblChartTitle.Name = "lblChartTitle";
            lblChartTitle.Padding = new Padding(16, 0, 0, 0);
            lblChartTitle.Size = new Size(1375, 42);
            lblChartTitle.TabIndex = 0;
            lblChartTitle.Text = "🏆  한국 가요 HOT CHART";
            lblChartTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(22, 27, 34);
            pnlFooter.Controls.Add(lblFooter);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(3, 634);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1383, 30);
            pnlFooter.TabIndex = 5;
            pnlFooter.Paint += pnlFooter_Paint;
            // 
            // lblFooter
            // 
            lblFooter.AutoSize = true;
            lblFooter.Font = new Font("맑은 고딕", 7.5F);
            lblFooter.ForeColor = Color.FromArgb(139, 148, 158);
            lblFooter.Location = new Point(12, 7);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(186, 12);
            lblFooter.TabIndex = 0;
            lblFooter.Text = "WorldBeat  ·  한국 가요 HOT 차트 기준";
            // 
            // pnlYearCtrl
            // 
            pnlYearCtrl.BackColor = Color.FromArgb(22, 27, 34);
            pnlYearCtrl.Controls.Add(lblYearNum);
            pnlYearCtrl.Controls.Add(lblYearSub);
            pnlYearCtrl.Controls.Add(btnpreView);
            pnlYearCtrl.Controls.Add(trkYear);
            pnlYearCtrl.Controls.Add(btnNexts);
            pnlYearCtrl.Controls.Add(lblMin);
            pnlYearCtrl.Controls.Add(lblMax);
            pnlYearCtrl.Dock = DockStyle.Top;
            pnlYearCtrl.Location = new Point(3, 59);
            pnlYearCtrl.Name = "pnlYearCtrl";
            pnlYearCtrl.Size = new Size(1383, 78);
            pnlYearCtrl.TabIndex = 6;
            pnlYearCtrl.Paint += pnlYearCtrl_Paint;
            pnlYearCtrl.Resize += pnlYearCtrl_Resize;
            // 
            // lblYearNum
            // 
            lblYearNum.Font = new Font("Consolas", 28F, FontStyle.Bold);
            lblYearNum.ForeColor = Color.FromArgb(88, 166, 255);
            lblYearNum.Location = new Point(4, 12);
            lblYearNum.Name = "lblYearNum";
            lblYearNum.Size = new Size(128, 52);
            lblYearNum.TabIndex = 0;
            lblYearNum.Text = "2000";
            lblYearNum.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblYearSub
            // 
            lblYearSub.AutoSize = true;
            lblYearSub.Font = new Font("맑은 고딕", 7.5F);
            lblYearSub.ForeColor = Color.FromArgb(139, 148, 158);
            lblYearSub.Location = new Point(30, 64);
            lblYearSub.Name = "lblYearSub";
            lblYearSub.Size = new Size(49, 12);
            lblYearSub.TabIndex = 1;
            lblYearSub.Text = "년도 선택";
            // 
            // btnpreView
            // 
            btnpreView.BackColor = Color.FromArgb(33, 38, 45);
            btnpreView.Cursor = Cursors.Hand;
            btnpreView.FlatAppearance.BorderColor = Color.FromArgb(46, 117, 182);
            btnpreView.FlatStyle = FlatStyle.Flat;
            btnpreView.Font = new Font("Segoe UI", 10F);
            btnpreView.ForeColor = Color.FromArgb(88, 166, 255);
            btnpreView.Location = new Point(134, 8);
            btnpreView.Name = "btnpreView";
            btnpreView.Size = new Size(34, 34);
            btnpreView.TabIndex = 2;
            btnpreView.Text = "◀";
            btnpreView.UseVisualStyleBackColor = false;
            btnpreView.Click += btnpreView_Click;
            // 
            // trkYear
            // 
            trkYear.BackColor = Color.FromArgb(22, 27, 34);
            trkYear.Location = new Point(174, 0);
            trkYear.Maximum = 2026;
            trkYear.Minimum = 1992;
            trkYear.Name = "trkYear";
            trkYear.Size = new Size(680, 45);
            trkYear.TabIndex = 3;
            trkYear.TickFrequency = 2;
            trkYear.Value = 2000;
            trkYear.ValueChanged += trkYear_ValueChanged;
            // 
            // btnNexts
            // 
            btnNexts.BackColor = Color.FromArgb(33, 38, 45);
            btnNexts.Cursor = Cursors.Hand;
            btnNexts.FlatAppearance.BorderColor = Color.FromArgb(46, 117, 182);
            btnNexts.FlatStyle = FlatStyle.Flat;
            btnNexts.Font = new Font("Segoe UI", 10F);
            btnNexts.ForeColor = Color.FromArgb(88, 166, 255);
            btnNexts.Location = new Point(860, 8);
            btnNexts.Name = "btnNexts";
            btnNexts.Size = new Size(34, 34);
            btnNexts.TabIndex = 4;
            btnNexts.Text = "▶";
            btnNexts.UseVisualStyleBackColor = false;
            btnNexts.Click += btnNexts_Click;
            // 
            // lblMin
            // 
            lblMin.AutoSize = true;
            lblMin.Font = new Font("Consolas", 7.5F);
            lblMin.ForeColor = Color.FromArgb(139, 148, 158);
            lblMin.Location = new Point(174, 60);
            lblMin.Name = "lblMin";
            lblMin.Size = new Size(25, 12);
            lblMin.TabIndex = 5;
            lblMin.Text = "1992";
            // 
            // lblMax
            // 
            lblMax.AutoSize = true;
            lblMax.Font = new Font("Consolas", 7.5F);
            lblMax.ForeColor = Color.FromArgb(139, 148, 158);
            lblMax.Location = new Point(840, 60);
            lblMax.Name = "lblMax";
            lblMax.Size = new Size(25, 12);
            lblMax.TabIndex = 6;
            lblMax.Text = "2026";
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(22, 27, 34);
            pnlHeader.Controls.Add(lblMainTitle);
            pnlHeader.Controls.Add(lblDesc);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(3, 3);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1383, 56);
            pnlHeader.TabIndex = 7;
            pnlHeader.Paint += pnlHeader_Paint;
            // 
            // lblMainTitle
            // 
            lblMainTitle.AutoSize = true;
            lblMainTitle.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
            lblMainTitle.ForeColor = Color.FromArgb(230, 237, 243);
            lblMainTitle.Location = new Point(18, 4);
            lblMainTitle.Name = "lblMainTitle";
            lblMainTitle.Size = new Size(206, 25);
            lblMainTitle.TabIndex = 0;
            lblMainTitle.Text = "🎵  한국 가요 타임라인";
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Font = new Font("맑은 고딕", 8F);
            lblDesc.ForeColor = Color.FromArgb(139, 148, 158);
            lblDesc.Location = new Point(18, 34);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(614, 13);
            lblDesc.TabIndex = 1;
            lblDesc.Text = "연도를 선택하면 그 해의 가요·연예 뉴스와 HOT 차트를 확인할 수 있습니다.  |  뉴스를 클릭하면 상세 내용을 볼 수 있어요.";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(flowCards);
            tabPage3.Controls.Add(panelTop);
            tabPage3.Location = new Point(4, 40);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1389, 667);
            tabPage3.TabIndex = 3;
            tabPage3.Text = "뮤직 타임캡슐";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // flowCards
            // 
            flowCards.AutoScroll = true;
            flowCards.BackColor = Color.FromArgb(22, 27, 34);
            flowCards.Dock = DockStyle.Fill;
            flowCards.Location = new Point(3, 47);
            flowCards.Name = "flowCards";
            flowCards.Size = new Size(1383, 617);
            flowCards.TabIndex = 3;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(22, 27, 34);
            panelTop.Controls.Add(btnCreateTop);
            panelTop.Controls.Add(label1);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(3, 3);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1383, 44);
            panelTop.TabIndex = 2;
            // 
            // btnCreateTop
            // 
            btnCreateTop.BackColor = Color.FromArgb(192, 255, 255);
            btnCreateTop.FlatStyle = FlatStyle.Flat;
            btnCreateTop.ForeColor = SystemColors.Desktop;
            btnCreateTop.Location = new Point(1258, 10);
            btnCreateTop.Name = "btnCreateTop";
            btnCreateTop.Size = new Size(109, 23);
            btnCreateTop.TabIndex = 2;
            btnCreateTop.Text = "+ 새 캡슐 만들기";
            btnCreateTop.UseVisualStyleBackColor = false;
            btnCreateTop.Click += btnCreateTop_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(22, 27, 34);
            label1.Font = new Font("맑은 고딕", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(219, 28);
            label1.TabIndex = 1;
            label1.Text = "타임캡슐 플레이리스트";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // timerWave
            // 
            timerWave.Interval = 50;
            timerWave.Tick += timerWave_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 27, 34);
            ClientSize = new Size(1397, 711);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(2);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Beat";
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabControl2.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tprecent.ResumeLayout(false);
            pnlrecent.ResumeLayout(false);
            tpmost.ResumeLayout(false);
            pnlmost.ResumeLayout(false);
            pnlVisualizer.ResumeLayout(false);
            pnlVisualizer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)albumArt).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)song_DownLoad).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxWave).EndInit();
            pnlControls.ResumeLayout(false);
            pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)volBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)progressBar1).EndInit();
            tabPage1.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tpNews.ResumeLayout(false);
            tpChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvChart).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            pnlYearCtrl.ResumeLayout(false);
            pnlYearCtrl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkYear).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            tabPage3.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage3;
        private FlowLayoutPanel flowCards;
        private Panel panelTop;
        private Button btnCreateTop;
        private Label label1;
        private TabControl tabMain;
        private TabPage tpNews;
        private ListBox lstNews;
        private Label lblNewsTitle;
        private TabPage tpChart;
        private DataGridView dgvChart;
        private DataGridViewTextBoxColumn colRank;
        private DataGridViewTextBoxColumn colTitle;
        private DataGridViewTextBoxColumn colArtist;
        private DataGridViewTextBoxColumn colGenre;
        private DataGridViewTextBoxColumn colNote;
        private Label lblChartTitle;
        private Panel pnlFooter;
        private Label lblFooter;
        private Panel pnlYearCtrl;
        private Label lblYearNum;
        private Label lblYearSub;
        private Button btnpreView;
        private TrackBar trkYear;
        private Button btnNexts;
        private Label lblMin;
        private Label lblMax;
        private Panel pnlHeader;
        private Label lblMainTitle;
        private Label lblDesc;
        private System.Windows.Forms.Timer timerWave;
        private System.Windows.Forms.Timer timer1;
        private TabPage tabPage2;
        private Panel pnlVisualizer;
        private TabControl tabControl2;
        private TabPage tabPage4;
        private TabPage tprecent;
        private Button btnPlay1;
        private PictureBox pictureBoxWave;
        private Panel pnlControls;
        private TrackBar volBar;
        private Button btnRepeat;
        private Label lblTotal;
        private Button btnNext;
        private Label lblCurrent;
        private Button btnPlay;
        private Button btnShuffle;
        private Button btnPrev;
        private PictureBox albumArt;
        private Label lblArtist;
        private Label lblTitle;
        private Label lblPlaylistHdr;
        private Panel pnlPlaylist;
        private PictureBox pictureBox2;
        private PictureBox song_DownLoad;
        private PictureBox pictureBox3;
        private TabPage tpmost;
        private Panel pnlrecent;
        private Panel pnlmost;
        private ListBox lstrecent;
        private ListBox lstmost;
        private SeekBar progressBar1;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}
