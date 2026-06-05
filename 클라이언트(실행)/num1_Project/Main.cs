#nullable disable
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace num1_Project
{
    public partial class MainForm : Form
    {
        private float[] _fftBuffer = new float[512];
        private NAudio.Dsp.Complex[] _complexBuffer = new NAudio.Dsp.Complex[512];
        private int _sampleCount = 0;
        private WaveOutEvent _waveOut;
        private AudioFileReader _audioReader;
        private SongInfo _currentSong;
        private List<SongInfo> _playlist = new List<SongInfo>();
        private int _currentIndex = -1;
        private bool _isDragging = false;

        private readonly Dictionary<int, YearRecord> _db = KoreanMusicDb.Build();
        private int _currentYear = 2000;

        private System.Windows.Forms.Timer _progressTimer;

        private List<CapsuleInfo> capsuleList = new List<CapsuleInfo>();

        // ── 최근 재생 / 많이 들은 음악 ─────────────────────
        private List<SongInfo> _recentList = new List<SongInfo>();
        private List<SongInfo> _mostList = new List<SongInfo>();
        private bool _recentLoaded = false;
        private bool _mostLoaded = false;

        // [추가] 미가입 유저는 1분 미리듣기만 허용
        private const int FREE_PREVIEW_SECONDS = 60;
        private bool _previewStopRequested = false;


        // ── 검색 ──────────────────────────────────────────
        private TextBox _txtSearch;
        private Button _btnSearch;
        private List<SongInfo> _searchResults = new List<SongInfo>();
        private bool _isSearchMode = false;
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            DatabaseHelper.InitApi();
            InitSearchBar();
            ApplyButtonDesign();
            InitProgressTimer();
            WirePlayerEvents();

            this.DoubleBuffered = true;

            if (pnlPlaylist != null)
            {
                pnlPlaylist.AutoScroll = true;
                pnlPlaylist.Resize += pnlPlaylist_Resize;
            }

            // ── 최근 재생 / 많이 들은 음악 ListBox 초기화 ──
            if (lstrecent != null)
            {
                lstrecent.DrawItem += lstrecent_DrawItem;
                lstrecent.DoubleClick += lstrecent_DoubleClick;
            }
            if (lstmost != null)
            {
                lstmost.DrawItem += lstmost_DrawItem;
                lstmost.DoubleClick += lstmost_DoubleClick;
            }
            if (tabControl2 != null)
                tabControl2.SelectedIndexChanged += tabControl2_SelectedIndexChanged;

            if (dgvChart != null)
            {
                typeof(DataGridView).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.SetProperty,
                    null, dgvChart, new object[] { true });

                dgvChart.AutoGenerateColumns = false;
                dgvChart.CellDoubleClick += dgvChart_CellDoubleClick;
            }

            if (trkYear != null)
            {
                trkYear.Minimum = 1992;
                trkYear.Maximum = 2026;
                trkYear.Value = 2000;
            }

            this.Shown += MainForm_Shown;
            this.Activated += MainForm_Activated;
            LoadYear(2000);
           

        }

        private async void dgvChart_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvChart == null)
                return;

            if (_playlist == null || _playlist.Count == 0)
                await LoadPlaylistAsync();

            if (_playlist == null || _playlist.Count == 0)
            {
                MessageBox.Show("플레이리스트가 비어 있습니다.", "알림");
                return;
            }

            Song chartSong = dgvChart.Rows[e.RowIndex].Tag as Song;
            if (chartSong == null)
                return;

            int idx = FindSongIndexInPlaylist(chartSong.Title, chartSong.Artist);
            if (idx < 0)
            {
                MessageBox.Show("플레이리스트에 해당 곡이 없습니다.", "알림");
                return;
            }

            PlaySong(idx);
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "로그아웃 하시겠습니까?",
                "로그아웃",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            _waveOut?.Stop();
            _waveOut?.Dispose();
            _waveOut = null;
            _audioReader?.Dispose();
            _audioReader = null;

            DatabaseHelper.Logout();

            var loginForm = new Login_Form();
            loginForm.Show();

            this.FormClosed -= (s, args) => Application.Exit();
            loginForm.FormClosed += (s, args) => Application.Exit();

            this.Close();
        }


        private int FindSongIndexInPlaylist(string title, string artist)
        {
            string targetTitle = NormalizeSongKey(title);
            string targetArtist = NormalizeSongKey(artist);

            if (string.IsNullOrWhiteSpace(targetTitle))
                return -1;

            int idx = _playlist.FindIndex(s =>
                NormalizeSongKey(s.Title) == targetTitle &&
                (
                    string.IsNullOrWhiteSpace(targetArtist) ||
                    NormalizeSongKey(s.Artist) == targetArtist ||
                    NormalizeSongKey(s.Artist).Contains(targetArtist) ||
                    targetArtist.Contains(NormalizeSongKey(s.Artist))
                ));

            if (idx >= 0)
                return idx;

            idx = _playlist.FindIndex(s =>
                NormalizeSongKey(s.Title) == targetTitle);

            if (idx >= 0)
                return idx;

            idx = _playlist.FindIndex(s =>
            {
                string playlistTitle = NormalizeSongKey(s.Title);
                return playlistTitle.Contains(targetTitle) || targetTitle.Contains(playlistTitle);
            });

            return idx;
        }

        private string NormalizeSongKey(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            var chars = new List<char>();

            foreach (char ch in text.Trim().ToUpperInvariant())
            {
                if (char.IsLetterOrDigit(ch))
                    chars.Add(ch);
            }

            return new string(chars.ToArray());
        }

        public class VisualizerSampleProvider : NAudio.Wave.ISampleProvider
        {
            private readonly NAudio.Wave.ISampleProvider _source;
            private readonly Action<float> _onSample;
            public VisualizerSampleProvider(NAudio.Wave.ISampleProvider source, Action<float> onSample)
            {
                _source = source;
                _onSample = onSample;
            }
            public NAudio.Wave.WaveFormat WaveFormat => _source.WaveFormat;
            public int Read(float[] buffer, int offset, int count)
            {
                int read = _source.Read(buffer, offset, count);
                for (int i = 0; i < read; i++) _onSample(buffer[offset + i]);
                return read;
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await RefreshCapsulesAsync();
            // ──────────────────────────────────────────────────────
            // [수정] 관리자 버튼(button2) 표시 조건
            //
            // 두 조건이 모두 충족될 때만 버튼이 보여:
            //   1. DB에서 AdminRole == 1 인 실제 관리자
            //   2. 로그인할 때 관리자 체크박스를 체크하고 로그인함
            //
            // 즉, 관리자 계정이어도 체크박스 없이 로그인하면 버튼 안 보임
            // ──────────────────────────────────────────────────────
            button2.Visible = DatabaseHelper.CurrentUser?.AdminRole == 1
                              && DatabaseHelper.IsAdminLogin;
        }

        private async void MainForm_Activated(object sender, EventArgs e)
        {
            await LoadPlaylistAsync();
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            await LoadPlaylistAsync();

            // 현재 선택된 탭이 최근/많이 들은 탭이면 바로 로드
            if (tabControl2?.SelectedTab == tprecent && !_recentLoaded)
            {
                _recentLoaded = true;
                await LoadRecentAsync();
            }
            else if (tabControl2?.SelectedTab == tpmost && !_mostLoaded)
            {
                _mostLoaded = true;
                await LoadMostPlayedAsync();
            }
        }

        private async System.Threading.Tasks.Task LoadPlaylistAsync()
        {
            try
            {
                _playlist = await DatabaseHelper.GetSongsByGenreAsync("전체");
                RenderPlaylist();
            }
            catch (Exception ex)
            {
                pnlPlaylist.Controls.Clear();

                Label lbl = new Label();
                lbl.Text = "서버에서 음악 목록을 불러오지 못했습니다.\n" + ex.Message;
                lbl.ForeColor = Color.FromArgb(220, 120, 120);
                lbl.Font = new Font("맑은 고딕", 10f);
                lbl.AutoSize = false;
                lbl.Dock = DockStyle.Fill;
                lbl.TextAlign = ContentAlignment.MiddleCenter;

                pnlPlaylist.Controls.Add(lbl);
            }
        }

        private void RenderPlaylist()
        {
            pnlPlaylist.SuspendLayout();
            pnlPlaylist.Controls.Clear();

            if (_playlist.Count == 0)
            {
                Label lbl = new Label();
                lbl.Text = "서버에 등록된 음악이 없습니다.";
                lbl.ForeColor = Color.FromArgb(139, 148, 158);
                lbl.Font = new Font("맑은 고딕", 10f);
                lbl.AutoSize = false;
                lbl.Dock = DockStyle.Fill;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                pnlPlaylist.Controls.Add(lbl);
                pnlPlaylist.ResumeLayout();
                return;
            }

            for (int i = 0; i < _playlist.Count; i++)
            {
                var song = _playlist[i];
                Panel row = CreateSongRow(song, i);
                pnlPlaylist.Controls.Add(row);
            }

            RelayoutPlaylistRows();
            HighlightCurrentRow();

            pnlPlaylist.ResumeLayout();
        }

        private Panel CreateSongRow(SongInfo song, int idx)
        {
            Panel row = new Panel();
            int finalWidth = GetPlaylistRowWidth() - 35;
            row.Size = new Size(finalWidth, 60);
            row.Location = new Point(0, idx * 62);
            row.BackColor = (idx == _currentIndex)
                            ? Color.FromArgb(30, 40, 65)
                            : (idx % 2 == 0 ? Color.FromArgb(22, 27, 34) : Color.FromArgb(17, 22, 30));
            row.Cursor = Cursors.Hand;
            row.Tag = idx;
            row.Padding = new Padding(8, 6, 8, 6);
            row.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // --- 내부 요소 생성 ---
            PictureBox pic = new PictureBox();
            pic.Size = new Size(46, 46);
            pic.Location = new Point(8, 7);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Tag = idx; // ★ 태그 추가
            SetAlbumArtImage(pic, song.AlbumArtUrl);

            Label lblTitle = new Label();
            lblTitle.Text = song.Title;
            lblTitle.Font = new Font("맑은 고딕", 10f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(230, 237, 243);
            lblTitle.Location = new Point(64, 8);
            lblTitle.Size = new Size(finalWidth - 140, 22);
            lblTitle.AutoEllipsis = true;
            lblTitle.Tag = idx; // ★ 태그 추가

            Label lblArtist = new Label();
            lblArtist.Text = song.Artist;
            lblArtist.Font = new Font("맑은 고딕", 8.5f);
            lblArtist.ForeColor = Color.FromArgb(139, 148, 158);
            lblArtist.Location = new Point(64, 32);
            lblArtist.Size = new Size(finalWidth - 140, 18);
            lblArtist.AutoEllipsis = true;
            lblArtist.Tag = idx; // ★ 태그 추가

            Label lblDur = new Label();
            lblDur.Text = song.DurationText;
            lblDur.Font = new Font("Consolas", 9f);
            lblDur.ForeColor = Color.FromArgb(139, 148, 158);
            lblDur.TextAlign = ContentAlignment.MiddleRight;
            lblDur.Size = new Size(60, 46);
            lblDur.Location = new Point(finalWidth - 70, 7);
            lblDur.Tag = idx; // ★ 태그 추가

            row.Controls.AddRange(new Control[] { pic, lblTitle, lblArtist, lblDur });

            // --- ★ 핵심: 모든 요소를 눌러도 재생되게 기능 복구 ★ ---
            EventHandler dblClick = (s, e) =>
            {
                int i = (int)((Control)s).Tag;
                PlaySong(i); // 노래 재생 함수 호출
            };

            row.DoubleClick += dblClick;
            pic.DoubleClick += dblClick;
            lblTitle.DoubleClick += dblClick;
            lblArtist.DoubleClick += dblClick;
            lblDur.DoubleClick += dblClick;

            // 마우스 효과 복구
            EventHandler mouseEnter = (s, e) => row.BackColor = Color.FromArgb(40, 50, 85);
            EventHandler mouseLeave = (s, e) => row.BackColor = (idx == _currentIndex)
                                        ? Color.FromArgb(30, 40, 65)
                                        : (idx % 2 == 0 ? Color.FromArgb(22, 27, 34) : Color.FromArgb(17, 22, 30));

            row.MouseEnter += mouseEnter;
            pic.MouseEnter += mouseEnter;
            lblTitle.MouseEnter += mouseEnter;
            lblArtist.MouseEnter += mouseEnter;
            lblDur.MouseEnter += mouseEnter;

            row.MouseLeave += mouseLeave;
            pic.MouseLeave += mouseLeave;
            lblTitle.MouseLeave += mouseLeave;
            lblArtist.MouseLeave += mouseLeave;
            lblDur.MouseLeave += mouseLeave;

            // 디자인 선 긋기 (남색)
            row.Paint += (s, e) =>
            {
                Control ctrl = (Control)s;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Color lineNavy = Color.FromArgb(40, 60, 110);
                using (Pen p = new Pen(lineNavy, 1.0f))
                {
                    e.Graphics.DrawLine(p, 15, ctrl.Height - 1, ctrl.Width - 15, ctrl.Height - 1);
                }
            };

            return row;
        }

        private void ResetNowPlaying()
        {
            lblTitle.Text = "재생 중인 곡 없음";
            lblArtist.Text = "";
            albumArt.Image = GetDefaultAlbumArt();
            lblCurrent.Text = "0:00";
            lblTotal.Text = "0:00";
            progressBar1.Value = 0;
            btnPlay.Text = "▶";
        }

        private int GetPlaylistRowWidth()
        {
            int width = pnlPlaylist.ClientSize.Width - (pnlPlaylist.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0) - 4;
            return Math.Max(260, width);
        }

        private void RelayoutPlaylistRows()
        {
            if (pnlPlaylist == null)
                return;

            int y = 0;
            int rowWidth = GetPlaylistRowWidth();

            foreach (Control control in pnlPlaylist.Controls)
            {
                if (control is not Panel row || row.Tag is not int)
                    continue;

                row.SuspendLayout();
                row.Location = new Point(0, y);
                row.Size = new Size(rowWidth, 60);

                foreach (Control child in row.Controls)
                {
                    if (child.Name == "picAlbum")
                    {
                        child.Location = new Point(8, 7);
                        child.Size = new Size(46, 46);
                    }
                    else if (child.Name == "lblTitle")
                    {
                        child.Location = new Point(64, 8);
                        child.Size = new Size(Math.Max(120, row.Width - 230), 22);
                    }
                    else if (child.Name == "lblArtist")
                    {
                        child.Location = new Point(64, 32);
                        child.Size = new Size(Math.Max(120, row.Width - 230), 18);
                    }
                    else if (child.Name == "btnDelete")
                    {
                        child.Location = new Point(row.Width - 132, 16);
                        child.Size = new Size(52, 28);
                    }
                    else if (child.Name == "lblDur")
                    {
                        child.Location = new Point(row.Width - 72, 7);
                        child.Size = new Size(60, 46);
                    }
                }

                row.ResumeLayout();
                y += 62;
            }

            pnlPlaylist.AutoScrollMinSize = new Size(0, y + 4);
        }

        private void pnlPlaylist_Resize(object sender, EventArgs e)
        {
            RelayoutPlaylistRows();
        }

        private void SetAlbumArtImage(PictureBox pictureBox, string imageUrl)
        {
            pictureBox.Image = GetDefaultAlbumArt();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            if (string.IsNullOrWhiteSpace(imageUrl))
                return;

            pictureBox.LoadCompleted += PictureBox_LoadCompleted;

            try
            {
                pictureBox.LoadAsync(imageUrl);
            }
            catch
            {
                pictureBox.Image = GetDefaultAlbumArt();
            }
        }

        private void PictureBox_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (sender is not PictureBox pictureBox)
                return;

            if (e.Error != null || pictureBox.Image == null)
                pictureBox.Image = GetDefaultAlbumArt();

            pictureBox.LoadCompleted -= PictureBox_LoadCompleted;
        }

        private Image GetDefaultAlbumArt()
        {
            Bitmap bmp = new Bitmap(46, 46);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(46, 117, 182));
            using var font = new Font("Segoe UI Emoji", 18f);
            g.DrawString("🎵", font, Brushes.White,
                new RectangleF(0, 0, 46, 46),
                new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
            return bmp;
        }

        // [추가] 재생 리소스 정리 전용 메서드
        private void DisposePlaybackObjects()
        {
            if (_waveOut != null)
            {
                try
                {
                    _waveOut.PlaybackStopped -= OnPlaybackStopped;
                }
                catch
                {
                }

                try
                {
                    _waveOut.Dispose();
                }
                catch
                {
                }

                _waveOut = null;
            }

            if (_audioReader != null)
            {
                try
                {
                    _audioReader.Dispose();
                }
                catch
                {
                }

                _audioReader = null;
            }
        }

        // [추가] 미가입 유저 여부 확인
        private bool IsPreviewOnlyPlayback()
        {
            return DatabaseHelper.IsPreviewOnlyUser();
        }

        private async void PlaySong(int index)
        {
            if (index < 0 || index >= _playlist.Count)
                return;

            try
            {
                StopPlayback();

                _previewStopRequested = false;
                _currentIndex = index;
                _currentSong = _playlist[index];

                string localPath = await DatabaseHelper.DownloadSongToCacheAsync(_currentSong);

                if (string.IsNullOrWhiteSpace(localPath) || !File.Exists(localPath))
                {
                    MessageBox.Show("파일을 찾을 수 없습니다.", "재생 오류");
                    return;
                }

                _audioReader = new AudioFileReader(localPath);
                _waveOut = new WaveOutEvent();
                var sampleProvider = _audioReader.ToSampleProvider();

                //_waveOut.Init(_audioReader);
                _waveOut.Init(new VisualizerSampleProvider(sampleProvider, sample =>
                {
                    _complexBuffer[_sampleCount].X = (float)(sample * NAudio.Dsp.FastFourierTransform.HammingWindow(_sampleCount, 512));
                    _complexBuffer[_sampleCount].Y = 0;
                    _sampleCount++;

                    if (_sampleCount >= 512)
                    {
                        _sampleCount = 0;
                        NAudio.Dsp.FastFourierTransform.FFT(true, 9, _complexBuffer);
                        for (int i = 0; i < 256; i++)
                        {
                            _fftBuffer[i] = (float)Math.Sqrt(_complexBuffer[i].X * _complexBuffer[i].X + _complexBuffer[i].Y * _complexBuffer[i].Y);
                        }
                    }
                }));

                _waveOut.Volume = volBar.Value / 100f;
                _waveOut.PlaybackStopped += OnPlaybackStopped;
                _waveOut.Play();

                UpdateNowPlaying(_currentSong);

                progressBar1.Maximum = Math.Max(1, (int)_audioReader.TotalTime.TotalSeconds);
                progressBar1.Value = 0;
                lblTotal.Text = _currentSong.DurationText;
                lblCurrent.Text = "0:00";

                btnPlay.Text = "⏸";

                await DatabaseHelper.IncrementPlayCountAsync(_currentSong.SongId);

                // 최근 재생 / 많이 들은 음악 탭 갱신
                RefreshRecentAndTop();

                HighlightCurrentRow();
                _progressTimer.Start();
                timerWave.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"재생 오류: {ex.Message}");
            }
        }

        private void UpdateNowPlaying(SongInfo song)
        {
            lblTitle.Text = song.Title;
            lblArtist.Text = $"{song.Artist}  ·  {(song.Year > 0 ? song.Year.ToString() : "")}";
            SetAlbumArtImage(albumArt, song.AlbumArtUrl);
        }

        private void HighlightCurrentRow()
        {
            foreach (Control c in pnlPlaylist.Controls)
            {
                if (c is Panel row && row.Tag is int idx)
                {
                    row.BackColor = idx == _currentIndex
                        ? Color.FromArgb(28, 46, 74)
                        : idx % 2 == 0
                            ? Color.FromArgb(22, 27, 34)
                            : Color.FromArgb(17, 22, 30);
                }
            }
        }

        private void StopPlayback()
        {
            _progressTimer?.Stop();
            timerWave?.Stop();

            if (_waveOut != null)
            {
                try
                {
                    _waveOut.PlaybackStopped -= OnPlaybackStopped;
                    _waveOut.Stop();
                }
                catch
                {
                }
            }

            DisposePlaybackObjects();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (!this.IsHandleCreated || this.IsDisposed)
                return;

            this.BeginInvoke((Action)(() =>
            {
                btnPlay.Text = "▶";
                _progressTimer.Stop();
                timerWave.Stop();

                // [추가] 미가입 유저 1분 미리듣기 종료 처리
                if (_previewStopRequested)
                {
                    _previewStopRequested = false;

                    DisposePlaybackObjects();

                    if (progressBar1.Maximum > 0)
                        progressBar1.Value = 0;

                    lblCurrent.Text = "0:00";

                    MessageBox.Show(
                        "요금제 미가입 유저는 모든 곡을 1분만 무료로 들을 수 있습니다.\n일반 또는 VIP 가입 후 전체 재생이 가능합니다.",
                        "1분 무료듣기 종료");

                    return;
                }

                if (_currentSong == null || _playlist.Count == 0)
                {
                    ResetNowPlaying();
                    return;
                }

                if (_currentIndex + 1 < _playlist.Count)
                    PlaySong(_currentIndex + 1);
                else
                {
                    if (progressBar1.Maximum > 0)
                        progressBar1.Value = 0;
                    lblCurrent.Text = "0:00";
                }
            }));
        }

        private void InitProgressTimer()
        {
            _progressTimer = new System.Windows.Forms.Timer();
            _progressTimer.Interval = 500;
            _progressTimer.Tick += (s, e) =>
            {
                if (_audioReader == null || _isDragging)
                    return;

                int pos = (int)_audioReader.CurrentTime.TotalSeconds;
                if (progressBar1.Maximum > 0)
                    progressBar1.Value = Math.Min(pos, progressBar1.Maximum);

                lblCurrent.Text = $"{pos / 60}:{pos % 60:D2}";

                // [추가] 미가입 유저는 1분이 지나면 자동 정지
                if (IsPreviewOnlyPlayback() && pos >= FREE_PREVIEW_SECONDS)
                {
                    _previewStopRequested = true;

                    if (_waveOut != null)
                        _waveOut.Stop();
                }
            };
        }

        private void WirePlayerEvents()
        {
            btnPlay.Click += (s, e) =>
            {
                if (_waveOut == null)
                {
                    if (_playlist.Count > 0)
                    {
                        int playIndex = _currentIndex >= 0 ? _currentIndex : 0;
                        PlaySong(playIndex);
                    }
                    return;
                }

                if (_waveOut.PlaybackState == PlaybackState.Playing)
                {
                    _waveOut.Pause();
                    btnPlay.Text = "▶";
                    _progressTimer.Stop();
                    timerWave.Stop();
                }
                else
                {
                    _waveOut.Play();
                    btnPlay.Text = "⏸";
                    _progressTimer.Start();
                    timerWave.Start();
                }
            };

            btnPrev.Click += (s, e) =>
            {
                if (_currentIndex > 0)
                    PlaySong(_currentIndex - 1);
            };

            btnNext.Click += (s, e) =>
            {
                if (_currentIndex < _playlist.Count - 1)
                    PlaySong(_currentIndex + 1);
            };

            volBar.ValueChanged += (s, e) =>
            {
                if (_waveOut != null)
                    _waveOut.Volume = volBar.Value / 100f;
            };
            progressBar1.MouseDown += (s, e) => { _isDragging = true; };

            progressBar1.MouseUp += (s, e) =>
            {
                _isDragging = false;
                if (_audioReader != null)
                {
                    int newPos = progressBar1.Value;

                    // [추가] 미가입 유저는 1분 이상 시크 금지
                    if (IsPreviewOnlyPlayback() && newPos > FREE_PREVIEW_SECONDS)
                        newPos = FREE_PREVIEW_SECONDS;

                    _audioReader.CurrentTime = TimeSpan.FromSeconds(newPos);

                    if (progressBar1.Value != newPos)
                        progressBar1.Value = newPos;

                    lblCurrent.Text = $"{newPos / 60}:{newPos % 60:D2}";
                }
            };

            progressBar1.ValueChanged += (s, e) =>
            {
                if (_isDragging)
                {
                    int pos = progressBar1.Value;

                    // [추가] 미가입 유저는 1분 이상 시크 금지
                    if (IsPreviewOnlyPlayback() && pos > FREE_PREVIEW_SECONDS)
                        pos = FREE_PREVIEW_SECONDS;

                    lblCurrent.Text = $"{pos / 60}:{pos % 60:D2}";
                }
            };

            this.FormClosing += (s, e) => StopPlayback();
        }

        private void MakeControlRound(Control control)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, control.Width, control.Height);
            control.Region = new Region(path);
        }

        private void ApplyButtonDesign()
        {
            Button[] buttons = { btnShuffle, btnPrev, btnPlay, btnNext, btnRepeat };
            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
            }

            btnPlay.Size = new Size(45, 45);
            btnPlay.BackColor = Color.FromArgb(52, 152, 219);
            btnPlay.ForeColor = Color.White;
            btnPlay.Text = "▶";
            MakeControlRound(btnPlay);

            btnShuffle.Text = "⇄";
            btnPrev.Text = "⏮";
            btnNext.Text = "⏭";
            btnRepeat.Text = "↻";

            btnShuffle.ForeColor = btnRepeat.ForeColor = Color.Gray;
            btnPrev.ForeColor = btnNext.ForeColor = Color.White;
        }

        private void RenderCapsules()
        {
            flowCards.Controls.Clear();

            foreach (CapsuleInfo capsule in capsuleList)
                flowCards.Controls.Add(CreateCapsuleCard(capsule));

            flowCards.Controls.Add(CreateAddCard());
        }

        private Panel CreateAddCard()
        {
            Panel card = new Panel();
            card.Size = new Size(240, 200);
            card.BackColor = Color.FromArgb(13, 23, 38);
            card.Margin = new Padding(10);
            card.Cursor = Cursors.Hand;

            Label lblPlus = new Label();
            lblPlus.Text = "+";
            lblPlus.ForeColor = Color.FromArgb(150, 170, 200);
            lblPlus.Font = new Font("맑은 고딕", 28, FontStyle.Regular);
            lblPlus.AutoSize = false;
            lblPlus.Size = new Size(240, 60);
            lblPlus.Location = new Point(0, 60);
            lblPlus.TextAlign = ContentAlignment.MiddleCenter;
            lblPlus.Cursor = Cursors.Hand;

            Label lblText = new Label();
            lblText.Text = "새 캡슐 만들기";
            lblText.ForeColor = Color.FromArgb(150, 170, 200);
            lblText.Font = new Font("맑은 고딕", 11, FontStyle.Regular);
            lblText.AutoSize = false;
            lblText.Size = new Size(240, 30);
            lblText.Location = new Point(0, 130);
            lblText.TextAlign = ContentAlignment.MiddleCenter;
            lblText.Cursor = Cursors.Hand;

            card.Controls.Add(lblPlus);
            card.Controls.Add(lblText);
            card.Click += OpenCreateCapsuleForm;
            lblPlus.Click += OpenCreateCapsuleForm;
            lblText.Click += OpenCreateCapsuleForm;

            return card;
        }

        private Panel CreateCapsuleCard(CapsuleInfo capsule)
        {
            Panel card = new Panel();
            card.Size = new Size(240, 200);
            card.BackColor = Color.FromArgb(13, 23, 38);
            card.Margin = new Padding(10);

            Panel topPanel = new Panel();
            topPanel.Size = new Size(240, 60);
            topPanel.Location = new Point(0, 0);
            topPanel.BackColor = capsule.IsOpenable
                ? Color.FromArgb(24, 61, 52)
                : Color.FromArgb(31, 53, 77);
            card.Controls.Add(topPanel);

            Label lblIcon = new Label();
            lblIcon.AutoSize = false;
            lblIcon.Size = new Size(240, 30);
            lblIcon.Location = new Point(0, 18);
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblIcon.ForeColor = Color.White;
            lblIcon.Font = new Font("맑은 고딕", 16, FontStyle.Bold);
            lblIcon.Text = capsule.IsOpenable ? "✉" : "🔒";
            card.Controls.Add(lblIcon);

            Label lblCapsuleTitle = new Label();
            lblCapsuleTitle.Text = capsule.Title;
            lblCapsuleTitle.ForeColor = Color.White;
            lblCapsuleTitle.Font = new Font("맑은 고딕", 12, FontStyle.Bold);
            lblCapsuleTitle.AutoSize = false;
            lblCapsuleTitle.Size = new Size(240, 30);
            lblCapsuleTitle.Location = new Point(0, 75);
            lblCapsuleTitle.TextAlign = ContentAlignment.MiddleCenter;
            card.Controls.Add(lblCapsuleTitle);

            Label lblSongCount = new Label();
            lblSongCount.Text = $"{capsule.Songs.Count}곡";
            lblSongCount.ForeColor = Color.FromArgb(160, 180, 200);
            lblSongCount.Font = new Font("맑은 고딕", 9);
            lblSongCount.AutoSize = false;
            lblSongCount.Size = new Size(240, 20);
            lblSongCount.Location = new Point(0, 105);
            lblSongCount.TextAlign = ContentAlignment.MiddleCenter;
            card.Controls.Add(lblSongCount);

            if (capsule.IsOpenable)
            {
                Button btnOpen = new Button();
                btnOpen.Text = "열기";
                btnOpen.Size = new Size(64, 36);
                btnOpen.Location = new Point(18, 150);
                btnOpen.BackColor = Color.FromArgb(46, 160, 97);
                btnOpen.ForeColor = Color.White;
                btnOpen.FlatStyle = FlatStyle.Flat;
                btnOpen.FlatAppearance.BorderSize = 0;
                btnOpen.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                btnOpen.Click += (s, e) =>
                {
                    if (capsule.Songs == null || capsule.Songs.Count == 0)
                    {
                        MessageBox.Show("캡슐 안에 재생할 곡이 없습니다.", "캡슐 열기");
                        return;
                    }

                    _playlist = new List<SongInfo>(capsule.Songs);
                    _currentIndex = -1;
                    RenderPlaylist();
                    PlaySong(0);
                };
                card.Controls.Add(btnOpen);

                Button btnEdit = new Button();
                btnEdit.Text = "수정";
                btnEdit.Size = new Size(64, 36);
                btnEdit.Location = new Point(88, 150);
                btnEdit.BackColor = Color.FromArgb(52, 152, 219);
                btnEdit.ForeColor = Color.White;
                btnEdit.FlatStyle = FlatStyle.Flat;
                btnEdit.FlatAppearance.BorderSize = 0;
                btnEdit.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                btnEdit.Click += async (s, e) => await EditCapsuleAsync(capsule);
                card.Controls.Add(btnEdit);

                Button btnDelete = new Button();
                btnDelete.Text = "삭제";
                btnDelete.Size = new Size(64, 36);
                btnDelete.Location = new Point(158, 150);
                btnDelete.BackColor = Color.FromArgb(231, 76, 60);
                btnDelete.ForeColor = Color.White;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                btnDelete.Click += async (s, e) => await DeleteCapsuleAsync(capsule);
                card.Controls.Add(btnDelete);
            }
            else
            {
                Panel bottomPanel = new Panel();
                bottomPanel.Size = new Size(170, 36);
                bottomPanel.Location = new Point(35, 126);
                bottomPanel.BackColor = Color.FromArgb(24, 41, 63);

                Label lblBottom = new Label();
                lblBottom.Text = $"D-{capsule.DDay}";
                lblBottom.Dock = DockStyle.Fill;
                lblBottom.TextAlign = ContentAlignment.MiddleCenter;
                lblBottom.ForeColor = Color.DeepSkyBlue;
                lblBottom.Font = new Font("맑은 고딕", 10, FontStyle.Bold);

                bottomPanel.Controls.Add(lblBottom);
                card.Controls.Add(bottomPanel);

                Button btnEdit = new Button();
                btnEdit.Text = "수정";
                btnEdit.Size = new Size(80, 28);
                btnEdit.Location = new Point(25, 166);
                btnEdit.BackColor = Color.FromArgb(52, 152, 219);
                btnEdit.ForeColor = Color.White;
                btnEdit.FlatStyle = FlatStyle.Flat;
                btnEdit.FlatAppearance.BorderSize = 0;
                btnEdit.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                btnEdit.Click += async (s, e) => await EditCapsuleAsync(capsule);
                card.Controls.Add(btnEdit);

                Button btnDelete = new Button();
                btnDelete.Text = "삭제";
                btnDelete.Size = new Size(80, 28);
                btnDelete.Location = new Point(135, 166);
                btnDelete.BackColor = Color.FromArgb(231, 76, 60);
                btnDelete.ForeColor = Color.White;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                btnDelete.Click += async (s, e) => await DeleteCapsuleAsync(capsule);
                card.Controls.Add(btnDelete);
            }

            return card;
        }

        private async Task EditCapsuleAsync(CapsuleInfo capsule)
        {
            using (var form = new CreateCapsuleForm(capsule))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await RefreshCapsulesAsync();
                }
            }
        }

        private async Task DeleteCapsuleAsync(CapsuleInfo capsule)
        {
            if (capsule == null)
                return;

            var dr = MessageBox.Show(
                $"'{capsule.Title}' 캡슐을 삭제할까요?",
                "캡슐 삭제",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr != DialogResult.Yes)
                return;

            bool ok = await DatabaseHelper.DeleteCapsuleAsync(capsule.CapsuleId);
            if (!ok)
            {
                MessageBox.Show("캡슐 삭제에 실패했습니다.");
                return;
            }

            await RefreshCapsulesAsync();
        }

        private async void OpenCreateCapsuleForm(object sender, EventArgs e)
        {
            using (var form = new CreateCapsuleForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    capsuleList = await DatabaseHelper.GetMyCapsulesAsync();
                    RenderCapsules();
                }
            }
        }

        private void btnCreateTop_Click(object sender, EventArgs e)
        {
            OpenCreateCapsuleForm(sender, e);
        }

        // 송현우: LordYear에 async 구문 추가. -> 뉴스 데이터베이스 입력 적용 후 윈폼과 연결 과정.
        private async void LoadYear(int year)
        {
            _currentYear = year;
            lblYearNum.Text = year.ToString();
            lblFooter.Text = $"WorldBeat · {year}년 한국 가요 HOT 차트 기준";

            lstNews.Items.Clear();
            dgvChart?.Rows.Clear();

            var newsList = await DatabaseHelper.GetNewsByYearAsync(year);
            foreach (var n in newsList)
                lstNews.Items.Add(n);

            YearRecord rec = null;
            int best = int.MaxValue;

            foreach (var kv in _db)
            {
                int d = Math.Abs(kv.Key - year);
                if (d < best) { best = d; rec = kv.Value; }
            }

            if (rec != null && dgvChart != null)
            {
                dgvChart.SuspendLayout();
                foreach (var s in rec.Songs)
                {
                    int rowIndex = dgvChart.Rows.Add(
                        s.Rank.ToString("D2"),
                        s.Title,
                        s.Artist,
                        s.Genre,
                        s.Note);

                    dgvChart.Rows[rowIndex].Tag = s;
                }
                dgvChart.ResumeLayout();
            }

            tabMain.Invalidate(true);
        }

        private void pnlYearCtrl_Resize(object sender, EventArgs e)
        {
            int slW = pnlYearCtrl.Width - 310;
            trkYear.Width = slW;
            btnNext.Left = trkYear.Right + 6;
            lblMax.Left = trkYear.Right - 4;
        }

        private void trkYear_ValueChanged(object sender, EventArgs e)
        {
            LoadYear(trkYear.Value);
        }

        private void btnpreView_Click(object sender, EventArgs e)
        {
            if (trkYear.Value > trkYear.Minimum)
                trkYear.Value--;
        }

        private void btnNexts_Click(object sender, EventArgs e)
        {
            if (trkYear.Value < trkYear.Maximum)
                trkYear.Value++;
        }

        /* 송현우 코드 교체. -> 윈폼 실행 시 뉴스 화면에서 같은 부분 여러 개 나옴. */
        private void lstNews_MouseClick(object sender, MouseEventArgs e)
        {
            int idx = lstNews.IndexFromPoint(e.Location);
            if (idx < 0 || idx >= lstNews.Items.Count)
                return;

            var item = lstNews.Items[idx] as NewsItem;
            if (item == null)
                return;

            new NewsDetailForm(item).Show(this);
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(33, 38, 45)), 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private void pnlYearCtrl_Paint(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(33, 38, 45)), 0, 0, p.Width, 0);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(33, 38, 45)), 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private void pnlFooter_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(33, 38, 45)), 0, 0, ((Panel)sender).Width, 0);
        }

        private void tabMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = (TabControl)sender;
            var page = tab.TabPages[e.Index];
            bool sel = e.Index == tab.SelectedIndex;

            using (var bg = new SolidBrush(sel ? Color.FromArgb(22, 27, 34) : Color.FromArgb(13, 17, 23)))
                e.Graphics.FillRectangle(bg, e.Bounds);

            if (sel)
                e.Graphics.FillRectangle(
                    new SolidBrush(Color.FromArgb(88, 166, 255)),
                    new Rectangle(e.Bounds.X, e.Bounds.Bottom - 2, e.Bounds.Width, 2));

            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.DrawString(
                page.Text,
                new Font("맑은 고딕", 9f, sel ? FontStyle.Bold : FontStyle.Regular),
                new SolidBrush(sel ? Color.FromArgb(88, 166, 255) : Color.FromArgb(139, 148, 158)),
                e.Bounds, sf);
        }

        private void lstNews_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            var g = e.Graphics;

            bool hov = (e.State & DrawItemState.Selected) != 0;
            Color bg = hov
                ? Color.FromArgb(28, 46, 74)
                : (e.Index % 2 == 0 ? Color.FromArgb(17, 22, 30) : Color.FromArgb(13, 17, 23));

            g.FillRectangle(new SolidBrush(bg), e.Bounds);

            var badge = new Rectangle(e.Bounds.X + 14, e.Bounds.Y + 13, 22, 22);
            g.FillEllipse(new SolidBrush(Color.FromArgb(46, 117, 182)), badge);

            var sfC = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString((e.Index + 1).ToString(),
                new Font("Consolas", 8f, FontStyle.Bold),
                Brushes.White,
                badge,
                sfC);

            var item = lstNews.Items[e.Index] as NewsItem;
            string headline = item?.Headline ?? lstNews.Items[e.Index].ToString();

            var sfL = new StringFormat
            {
                LineAlignment = StringAlignment.Center
            };

            g.DrawString(headline,
                new Font("맑은 고딕", 10.5f),
                new SolidBrush(hov ? Color.FromArgb(230, 237, 243) : Color.FromArgb(200, 210, 220)),
                new RectangleF(e.Bounds.X + 46, e.Bounds.Y, e.Bounds.Width - 60, e.Bounds.Height),
                sfL);

            g.DrawString("›",
                new Font("맑은 고딕", 16f),
                new SolidBrush(hov ? Color.FromArgb(88, 166, 255) : Color.FromArgb(50, 88, 166, 255)),
                new RectangleF(e.Bounds.Right - 28, e.Bounds.Y, 22, e.Bounds.Height),
                sfL);

            g.DrawLine(new Pen(Color.FromArgb(25, 255, 255, 255)),
                e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }

        private void timerWave_Tick(object sender, EventArgs e)
        {
            pictureBoxWave.Invalidate();
        }

        private void pictureBoxWave_Paint(object sender, PaintEventArgs e)
        {
            //pictureBoxWave_Paint함수에 넣어주기 !
            if (_waveOut == null || _waveOut.PlaybackState != NAudio.Wave.PlaybackState.Playing)
            {
                e.Graphics.DrawLine(Pens.DimGray, 0, pictureBoxWave.Height / 2, pictureBoxWave.Width, pictureBoxWave.Height / 2);
                return;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 막대 두께 12, 간격 2 유지
            int barWidth = 12;
            int gap = 2;
            int maxBars = pictureBoxWave.Width / (barWidth + gap);

            for (int i = 0; i < maxBars; i++)
            {
                float ratio = (float)i / maxBars;

                int dataIndex = (int)(Math.Pow(ratio, 1.2) * 255);

                float rawVal = _fftBuffer[Math.Clamp(dataIndex, 0, 255)];
                float boostedVal = (float)Math.Sqrt(rawVal) * 1200; // 작은 신호를 위로 끌어올림

                float finalVal = boostedVal * (1.0f + ratio * 2.0f); // 고음역대 추가 보정

                int barHeight = (int)Math.Clamp(finalVal, 5, pictureBoxWave.Height - 5);

                int x = i * (barWidth + gap);
                int y = (pictureBoxWave.Height - barHeight) / 2;

                //그라데이션 색상 적용
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(x, y, barWidth, barHeight),
                    Color.HotPink,
                    Color.DeepSkyBlue,
                    System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, x, y, barWidth, barHeight);
                }
            }
        }

        private async Task RefreshCapsulesAsync()
        {
            capsuleList = await DatabaseHelper.GetMyCapsulesAsync();
            RenderCapsules();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            // 테두리 두께를 2로 키우고 핫핑크색으로 변경
            btnPlay1.FlatAppearance.BorderSize = 2;
            btnPlay1.FlatAppearance.BorderColor = Color.HotPink;
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            // 다시 테두리를 없애서 네온을 끔
            btnPlay1.FlatAppearance.BorderSize = 0;
        }

        private void pnlNowPlaying_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnPlay1_MouseDown(object sender, MouseEventArgs e)
        {
            btnPlay1.FlatAppearance.BorderSize = 2;
            btnPlay1.FlatAppearance.BorderColor = Color.HotPink;
        }

        private void btnPlay1_MouseUp(object sender, MouseEventArgs e)
        {
            btnPlay1.FlatAppearance.BorderSize = 0;
        }

        private void btnPlay1_Click(object sender, EventArgs e)
        {
            string url = "https://www.youtube.com/watch?v=fJ9rUzIMcZQ";
            try
            {
                // 시스템 기본 브라우저를 통해 링크를 엽니다.
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("링크를 열 수 없습니다: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════
        //  탭 전환 → 최초 1회만 서버에서 데이터 로드
        // ══════════════════════════════════════════════════
        private async void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tc = (TabControl)sender;
            if (tc.SelectedTab == tprecent && !_recentLoaded)
            {
                _recentLoaded = true;
                await LoadRecentAsync();
            }
            else if (tc.SelectedTab == tpmost && !_mostLoaded)
            {
                _mostLoaded = true;
                await LoadMostPlayedAsync();
            }
        }

        // ── 최근 재생 목록 로드 ──────────────────────────
        private async System.Threading.Tasks.Task LoadRecentAsync()
        {
            lstrecent.Items.Clear();
            lstrecent.Items.Add("⏳ 불러오는 중...");

            _recentList = await DatabaseHelper.GetRecentPlayedAsync(50);

            lstrecent.Items.Clear();
            if (_recentList.Count == 0)
            {
                lstrecent.Items.Add("아직 재생된 곡이 없습니다.");
                return;
            }
            foreach (var s in _recentList)
                lstrecent.Items.Add(s);
        }

        // ── 가장 많이 들은 음악 로드 ─────────────────────
        private async System.Threading.Tasks.Task LoadMostPlayedAsync()
        {
            lstmost.Items.Clear();
            lstmost.Items.Add("⏳ 불러오는 중...");

            _mostList = await DatabaseHelper.GetMostPlayedAsync(50);

            lstmost.Items.Clear();
            if (_mostList.Count == 0)
            {
                lstmost.Items.Add("아직 재생된 곡이 없습니다.");
                return;
            }
            foreach (var s in _mostList)
                lstmost.Items.Add(s);
        }

        // ── 재생 후 두 탭 모두 즉시 갱신 ─────────────────
        private async void RefreshRecentAndTop()
        {
            _recentList = await DatabaseHelper.GetRecentPlayedAsync(50);
            _mostList = await DatabaseHelper.GetMostPlayedAsync(50);

            // 현재 탭이 열려있으면 즉시 반영
            if (tabControl2?.SelectedTab == tprecent)
            {
                lstrecent.Items.Clear();
                foreach (var s in _recentList) lstrecent.Items.Add(s);
                if (_recentList.Count == 0) lstrecent.Items.Add("아직 재생된 곡이 없습니다.");
            }
            else if (tabControl2?.SelectedTab == tpmost)
            {
                lstmost.Items.Clear();
                foreach (var s in _mostList) lstmost.Items.Add(s);
                if (_mostList.Count == 0) lstmost.Items.Add("아직 재생된 곡이 없습니다.");
            }

            // 다음 탭 전환 시 재로드되도록 플래그 리셋
            _recentLoaded = false;
            _mostLoaded = false;
        }

        // ── 더블클릭 → 플레이리스트에서 찾아 재생 ───────
        private void lstrecent_DoubleClick(object sender, EventArgs e)
        {
            if (lstrecent.SelectedItem is not SongInfo song) return;
            int idx = _playlist.FindIndex(s => s.SongId == song.SongId);
            if (idx >= 0) PlaySong(idx);
        }

        private void lstmost_DoubleClick(object sender, EventArgs e)
        {
            if (lstmost.SelectedItem is not SongInfo song) return;
            int idx = _playlist.FindIndex(s => s.SongId == song.SongId);
            if (idx >= 0) PlaySong(idx);
        }

        // ══════════════════════════════════════════════════
        //  lstrecent DrawItem  ── 🕐 최근 재생 스타일
        // ══════════════════════════════════════════════════
        private void lstrecent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            var g = e.Graphics;

            // 문자열(로딩/빈 메시지) 처리
            if (lstrecent.Items[e.Index] is not SongInfo song)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(13, 17, 23)), e.Bounds);
                g.DrawString(lstrecent.Items[e.Index].ToString(),
                    new Font("맑은 고딕", 10f),
                    new SolidBrush(Color.FromArgb(139, 148, 158)),
                    new RectangleF(e.Bounds.X + 14, e.Bounds.Y, e.Bounds.Width - 14, e.Bounds.Height),
                    new StringFormat { LineAlignment = StringAlignment.Center });
                return;
            }

            bool sel = (e.State & DrawItemState.Selected) != 0;
            Color bg = sel
                ? Color.FromArgb(28, 46, 74)
                : (e.Index % 2 == 0 ? Color.FromArgb(17, 22, 30) : Color.FromArgb(13, 17, 23));
            g.FillRectangle(new SolidBrush(bg), e.Bounds);

            // 순번 배지 (파란 원)
            var badge = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 12, 24, 24);
            g.FillEllipse(new SolidBrush(Color.FromArgb(30, 90, 160)), badge);
            var sfC = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString((e.Index + 1).ToString(),
                new Font("Consolas", 8f, FontStyle.Bold), Brushes.White, badge, sfC);

            var sfL = new StringFormat { LineAlignment = StringAlignment.Center };
            int tx = e.Bounds.X + 44;
            int tw = e.Bounds.Width - 120;

            // 제목 (상단)
            g.DrawString(song.Title,
                new Font("맑은 고딕", 10.5f, FontStyle.Bold),
                new SolidBrush(sel ? Color.FromArgb(230, 237, 243) : Color.FromArgb(210, 220, 230)),
                new RectangleF(tx, e.Bounds.Y + 2, tw, e.Bounds.Height / 2f), sfL);

            // 아티스트 (하단)
            g.DrawString(song.Artist,
                new Font("맑은 고딕", 8.5f),
                new SolidBrush(Color.FromArgb(100, 140, 180)),
                new RectangleF(tx, e.Bounds.Y + e.Bounds.Height / 2f, tw, e.Bounds.Height / 2f), sfL);

            // 재생 시간 (우측)
            g.DrawString(song.DurationText,
                new Font("Consolas", 8.5f),
                new SolidBrush(Color.FromArgb(88, 100, 120)),
                new RectangleF(e.Bounds.Right - 62, e.Bounds.Y, 56, e.Bounds.Height),
                new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });

            // 구분선
            g.DrawLine(new Pen(Color.FromArgb(20, 255, 255, 255)),
                e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }

        // ══════════════════════════════════════════════════
        //  lstmost DrawItem  ── 🏆 많이 들은 음악 스타일
        // ══════════════════════════════════════════════════
        private void lstmost_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            var g = e.Graphics;

            if (lstmost.Items[e.Index] is not SongInfo song)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(13, 17, 23)), e.Bounds);
                g.DrawString(lstmost.Items[e.Index].ToString(),
                    new Font("맑은 고딕", 10f),
                    new SolidBrush(Color.FromArgb(139, 148, 158)),
                    new RectangleF(e.Bounds.X + 14, e.Bounds.Y, e.Bounds.Width - 14, e.Bounds.Height),
                    new StringFormat { LineAlignment = StringAlignment.Center });
                return;
            }

            int rank = e.Index + 1;
            bool sel = (e.State & DrawItemState.Selected) != 0;

            // 1~3위 배경 틴트
            Color tint = rank == 1 ? Color.FromArgb(20, 254, 188, 46)
                       : rank == 2 ? Color.FromArgb(14, 180, 180, 180)
                       : rank == 3 ? Color.FromArgb(14, 185, 125, 60)
                       : Color.Empty;

            Color bg = sel
                ? Color.FromArgb(28, 46, 74)
                : (e.Index % 2 == 0 ? Color.FromArgb(17, 22, 30) : Color.FromArgb(13, 17, 23));
            g.FillRectangle(new SolidBrush(bg), e.Bounds);
            if (tint != Color.Empty)
                g.FillRectangle(new SolidBrush(tint), e.Bounds);

            // 순위 배지 (금/은/동/기본)
            Color badgeClr = rank == 1 ? Color.FromArgb(180, 140, 20)
                           : rank == 2 ? Color.FromArgb(120, 130, 140)
                           : rank == 3 ? Color.FromArgb(140, 90, 40)
                           : Color.FromArgb(30, 60, 100);

            var badge = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 12, 24, 24);
            g.FillEllipse(new SolidBrush(badgeClr), badge);
            var sfC = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(rank.ToString(),
                new Font("Consolas", 7.5f, FontStyle.Bold),
                new SolidBrush(rank <= 3 ? Color.White : Color.FromArgb(139, 148, 158)),
                badge, sfC);

            var sfL = new StringFormat { LineAlignment = StringAlignment.Center };
            int tx = e.Bounds.X + 44;
            int tw = e.Bounds.Width - 160;

            // 제목
            Color titleClr = rank == 1 ? Color.FromArgb(254, 220, 100)
                           : rank == 2 ? Color.FromArgb(210, 215, 220)
                           : rank == 3 ? Color.FromArgb(210, 170, 110)
                           : (sel ? Color.FromArgb(230, 237, 243) : Color.FromArgb(210, 220, 230));
            g.DrawString(song.Title,
                new Font("맑은 고딕", 10.5f, rank <= 3 ? FontStyle.Bold : FontStyle.Regular),
                new SolidBrush(titleClr),
                new RectangleF(tx, e.Bounds.Y + 2, tw, e.Bounds.Height / 2f), sfL);

            // 아티스트
            g.DrawString(song.Artist,
                new Font("맑은 고딕", 8.5f),
                new SolidBrush(Color.FromArgb(100, 140, 180)),
                new RectangleF(tx, e.Bounds.Y + e.Bounds.Height / 2f, tw, e.Bounds.Height / 2f), sfL);

            // 재생 횟수 (우측)
            string playTxt = song.PlayCount > 0 ? $"▶ {song.PlayCount:N0}회" : "--";
            g.DrawString(playTxt,
                new Font("맑은 고딕", 8.5f),
                new SolidBrush(rank <= 3 ? Color.FromArgb(180, 210, 120) : Color.FromArgb(80, 110, 80)),
                new RectangleF(e.Bounds.Right - 100, e.Bounds.Y, 94, e.Bounds.Height),
                new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });

            // 구분선
            g.DrawLine(new Pen(Color.FromArgb(20, 255, 255, 255)),
                e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e) //탭컨트롤　이미지　변경　추가　
        {
            TabControl tc = (TabControl)sender;
            Graphics g = e.Graphics;
            Rectangle tabRect = tc.GetTabRect(e.Index); // 현재 그리는 탭의 영역

            // 1. 그라데이션 브러시 생성 (네온 핑크 -> 네온 블루)

            using (System.Drawing.Drawing2D.LinearGradientBrush brush =
                new System.Drawing.Drawing2D.LinearGradientBrush(
                    tabRect,
                    Color.FromArgb(255, 105, 180), // 핫핑크
                    Color.FromArgb(0, 191, 255),   // 딥 스카이블루
                    System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
            {
                // 2. 탭 배경 채우기
                g.FillRectangle(brush, tabRect);
            }

            string tabText = tc.TabPages[e.Index].Text;
            TextRenderer.DrawText(g, tabText, tc.Font, tabRect, Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);

            if (e.State == DrawItemState.Selected)
            {
                using (Pen p = new Pen(Color.FromArgb(150, Color.White), 2)) // 반투명 흰색 테두리
                {
                    g.DrawRectangle(p, tabRect.X + 2, tabRect.Y + 2, tabRect.Width - 5, tabRect.Height - 5);
                }
            }
        }

        private void btnPlay_Paint(object sender, PaintEventArgs e) // 버튼　이미지　변경　추가
        {
            Button btn = (Button)sender;
            Graphics g = e.Graphics;

            // 선을 아주 매끄럽게 그리는 설정
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 버튼 영역 설정
            Rectangle rect = btn.ClientRectangle;
            rect.Inflate(-3, -3);

            using (var neonBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                rect,
                Color.FromArgb(255, 105, 180), // 네온 핑크
                Color.FromArgb(0, 191, 255),   // 네온 블루
                45f)) // 대각선 방향으로 색이 섞이게
            {
                using (Pen neonPen = new Pen(neonBrush, 3))
                {
                    g.DrawEllipse(neonPen, rect);
                }
            }
        }

        private void btnPrev_Paint(object sender, PaintEventArgs e) // 버튼　이미지　변경　코드　추가　
        {
            Button btn = (Button)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(30, 30, 30));
            Rectangle rect = btn.ClientRectangle;
            rect.Inflate(-3, -3);

            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                rect, Color.HotPink, Color.DeepSkyBlue, 45f))
            {
                using (Pen p = new Pen(brush, 3)) { g.DrawEllipse(p, rect); }

                int w = rect.Width;
                int h = rect.Height;
                int size = w / 4;
                int midX = w / 2;
                int midY = h / 2;

                Point[] triangle1 = { new Point(midX - 2, midY - size), new Point(midX - 2, midY + size), new Point(midX - size - 2, midY) };
                Point[] triangle2 = { new Point(midX + size - 2, midY - size), new Point(midX + size - 2, midY + size), new Point(midX - 2, midY) };

                g.FillPolygon(brush, triangle1);
                g.FillPolygon(brush, triangle2);
                g.FillRectangle(brush, midX - size - 5, midY - size, 3, size * 2); // 맨 왼쪽 막대
            }
        }

        private void btnNext_Paint(object sender, PaintEventArgs e) //버튼　이미지　변경　코드　추가
        {
            Button btn = (Button)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(30, 30, 30));
            Rectangle rect = btn.ClientRectangle;
            rect.Inflate(-3, -3);

            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                rect, Color.HotPink, Color.DeepSkyBlue, 45f))
            {
                // 1. 네온 테두리 원
                using (Pen p = new Pen(brush, 3)) { g.DrawEllipse(p, rect); }

                // 2. 다음 곡 모양 (>>|) 직접 그리기
                int w = rect.Width;
                int h = rect.Height;
                int size = w / 4;
                int midX = w / 2;
                int midY = h / 2;

                Point[] triangle1 = { new Point(midX + 2, midY - size), new Point(midX + 2, midY + size), new Point(midX + size + 2, midY) };
                Point[] triangle2 = { new Point(midX - size + 2, midY - size), new Point(midX - size + 2, midY + size), new Point(midX + 2, midY) };

                g.FillPolygon(brush, triangle1);
                g.FillPolygon(brush, triangle2);
                g.FillRectangle(brush, midX + size + 2, midY - size, 3, size * 2); // 맨 오른쪽 막대
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DatabaseHelper.CurrentUser == null)
            {
                MessageBox.Show("로그인이 필요합니다.", "알림");
                return;
            }

            var payForm = new Pay();
            payForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ──────────────────────────────────────────────────────
            // [추가] 이중 보안 체크
            //
            // button2.Visible = false 로 버튼을 숨겼더라도,
            // 혹시 모를 우회 접근에 대비해서 클릭 시에도 한 번 더 권한을 확인해.
            // AdminRole이 1이 아니면 관리자 폼을 열지 않아.
            // ──────────────────────────────────────────────────────
            if (DatabaseHelper.CurrentUser?.AdminRole != 1)
            {
                MessageBox.Show("관리자만 접근할 수 있습니다.", "접근 거부");
                return;
            }

            // 관리자 확인됐으면 Admin 폼 열기
            Admin adminForm = new Admin();
            adminForm.Show();
        }

        private void tabControl2_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tc = (TabControl)sender;
            Graphics g = e.Graphics;
            Rectangle tabRect = tc.GetTabRect(e.Index);

            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                tabRect, Color.HotPink, Color.DeepSkyBlue, 0f))
            {
                g.FillRectangle(brush, tabRect);
            }

            TextRenderer.DrawText(g, tc.TabPages[e.Index].Text, tc.Font, tabRect,
                Color.White, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // 재생 중인 곡이 없으면 안내 메시지
            if (_currentSong == null)
            {
                MessageBox.Show("재생 중인 곡이 없습니다.", "가사");
                return;
            }

            // Lyrics 폼 열기
            // _currentSong.Lyrics → DB에서 가져온 .lrc 형식 가사
            // _audioReader        → 현재 재생 위치를 Lyrics 폼에서 참조하기 위해 전달
            var lyricsForm = new Lyrics(_currentSong.Lyrics, _audioReader);
            lyricsForm.Show();
        }

        private async void song_DownLoad_Click(object sender, EventArgs e)
        {
            //노래 다운로드 버튼

            if (_currentSong == null)
            {
                MessageBox.Show("현재 재생 중인 곡이 없습니다.", "다운로드");
                return;
            }

            if (DatabaseHelper.CurrentUser == null)
            {
                MessageBox.Show("로그인 후 다운로드할 수 있습니다.", "다운로드");
                return;
            }

            string planType = DatabaseHelper.GetCurrentPlanType();

            // 요금제 미가입 유저는 다운로드 불가
            if (planType != "일반" && planType != "VIP")
            {
                MessageBox.Show("요금제 미가입 유저는 다운로드할 수 없습니다.", "다운로드");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "현재 재생 중인 곡 다운로드";
                sfd.FileName = DatabaseHelper.GetSuggestedSongFileName(_currentSong);
                sfd.Filter = "Audio Files|*.mp3;*.wav;*.flac;*.aac;*.m4a|All Files|*.*";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                Control btn = sender as Control;
                if (btn != null)
                    btn.Enabled = false;

                try
                {
                    var result = await DatabaseHelper.DownloadSongForOfflineAsync(_currentSong, sfd.FileName);
                    MessageBox.Show(
                        result.Message,
                        result.Success ? "다운로드 완료" : "다운로드 실패");
                }
                finally
                {
                    if (btn != null)
                        btn.Enabled = true;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
    "로그아웃 하시겠습니까?",
    "로그아웃",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            _waveOut?.Stop();
            _waveOut?.Dispose();
            _waveOut = null;
            _audioReader?.Dispose();
            _audioReader = null;

            DatabaseHelper.Logout();

            var loginForm = new Login_Form();
            loginForm.Show();

            this.FormClosed -= (s, args) => Application.Exit();
            loginForm.FormClosed += (s, args) => Application.Exit();

            this.Close();
        }

        // ══════════════════════════════════════════════════
        //  검색창 초기화 (가장 많이 들은 음악 탭 옆에 배치)
        // ══════════════════════════════════════════════════
        private void InitSearchBar()
        {
            // tabControl2가 담긴 부모 패널을 찾아서 검색창을 그 위에 올립니다.
            // tabControl2의 위치/부모를 기준으로 동적으로 배치합니다.
            if (tabControl2 == null) return;

            var parent = tabControl2.Parent;
            if (parent == null) return;

            // 검색 텍스트박스
            _txtSearch = new TextBox();
            _txtSearch.PlaceholderText = "🔍 노래 제목 검색...";
            _txtSearch.Font = new Font("맑은 고딕", 9.5f);
            _txtSearch.BackColor = Color.FromArgb(30, 36, 46);
            _txtSearch.ForeColor = Color.FromArgb(210, 220, 230);
            _txtSearch.BorderStyle = BorderStyle.FixedSingle;
            _txtSearch.Size = new Size(160, 24);
            // tabControl2 오른쪽 상단에 배치
            _txtSearch.Location = new Point(
                tabControl2.Right - 220,
                tabControl2.Top - 1
            );
            _txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _txtSearch.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    await PerformSearchAsync();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    ClearSearch();
                }
            };
            parent.Controls.Add(_txtSearch);
            _txtSearch.BringToFront();

            // 검색 버튼
            _btnSearch = new Button();
            _btnSearch.Text = "검색";
            _btnSearch.Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold);
            _btnSearch.Size = new Size(52, 24);
            _btnSearch.Location = new Point(_txtSearch.Right + 4, _txtSearch.Top);
            _btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnSearch.BackColor = Color.FromArgb(88, 130, 220);
            _btnSearch.ForeColor = Color.White;
            _btnSearch.FlatStyle = FlatStyle.Flat;
            _btnSearch.FlatAppearance.BorderSize = 0;
            _btnSearch.Cursor = Cursors.Hand;
            _btnSearch.Click += async (s, e) => await PerformSearchAsync();
            parent.Controls.Add(_btnSearch);
            _btnSearch.BringToFront();
        }

        // ── 검색 실행 ─────────────────────────────────────
        private async Task PerformSearchAsync()
        {
            string query = _txtSearch?.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(query))
            {
                ClearSearch();
                return;
            }

            // 로딩 표시
            _btnSearch.Text = "...";
            _btnSearch.Enabled = false;

            try
            {
                // 서버 검색 (클라이언트 필터링 방식으로 폴백 지원)
                var allSongs = await DatabaseHelper.GetSongsByGenreAsync("전체");
                _searchResults = allSongs
                    .Where(s => s.Title != null &&
                                s.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                _isSearchMode = true;

                // 결과를 플레이리스트 패널에 렌더링
                RenderSearchResults(query);

                // 플레이리스트 탭으로 이동해서 결과 표시
                if (tabControl2 != null)
                    tabControl2.SelectedTab = tabPage4; // 플레이리스트 탭 이름에 맞게 조정
            }
            catch (Exception ex)
            {
                MessageBox.Show("검색 오류: " + ex.Message);
            }
            finally
            {
                _btnSearch.Text = "검색";
                _btnSearch.Enabled = true;
            }
        }

        // ── 검색 결과 렌더링 ──────────────────────────────
        private void RenderSearchResults(string query)
        {
            pnlPlaylist.SuspendLayout();
            pnlPlaylist.Controls.Clear();

            if (_searchResults.Count == 0)
            {
                Label lblNone = new Label();
                lblNone.Text = "검색 결과가 없습니다.";
                lblNone.ForeColor = Color.FromArgb(139, 148, 158);
                lblNone.Font = new Font("맑은 고딕", 10f);
                lblNone.AutoSize = false;
                lblNone.Size = new Size(GetPlaylistRowWidth(), 60);
                lblNone.Location = new Point(0, 0);
                lblNone.TextAlign = ContentAlignment.MiddleCenter;
                pnlPlaylist.Controls.Add(lblNone);
                pnlPlaylist.ResumeLayout();
                return;
            }

            var savedPlaylist = _playlist;
            _playlist = _searchResults;

            for (int i = 0; i < _searchResults.Count; i++)
            {
                Panel row = CreateSongRow(_searchResults[i], i);
                row.Location = new Point(0, i * 62);
                pnlPlaylist.Controls.Add(row);
            }

            _playlist = savedPlaylist;
            pnlPlaylist.AutoScrollMinSize = new Size(0, _searchResults.Count * 62 + 4);
            pnlPlaylist.ResumeLayout();

            foreach (Control c in pnlPlaylist.Controls)
            {
                if (c is Panel row && row.Tag is int idx && idx < _searchResults.Count)
                {
                    var foundSong = _searchResults[idx];
                    EventHandler dblClick = (s, e) =>
                    {
                        int pIdx = _playlist.FindIndex(p => p.SongId == foundSong.SongId);
                        if (pIdx >= 0) PlaySong(pIdx);
                        else
                        {
                            _playlist = new List<SongInfo>(_searchResults);
                            _currentIndex = -1;
                            int sIdx = _playlist.FindIndex(p => p.SongId == foundSong.SongId);
                            if (sIdx >= 0) PlaySong(sIdx);
                        }
                    };
                    foreach (Control child in row.Controls)
                        child.DoubleClick += dblClick;
                    row.DoubleClick += dblClick;
                }
            }
        }

        // ── 검색 초기화 (원래 플레이리스트로 복원) ─────────
        private void ClearSearch()
        {
            _isSearchMode = false;
            _searchResults.Clear();
            if (_txtSearch != null) _txtSearch.Text = "";
            RenderPlaylist(); // 원본 플레이리스트 복원
        }
    }
}
