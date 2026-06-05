using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace num1_Project
{
    public partial class CreateCapsuleForm : Form
    {
        public CapsuleInfo CreatedCapsule { get; private set; }

        private List<SongInfo> _allSongs = new List<SongInfo>();
        private bool _isEditMode = false;
        private CapsuleInfo _editingCapsule;

        public CreateCapsuleForm()
        {
            InitializeComponent();
            this.Load += CreateCapsuleForm_Load;
        }

        public CreateCapsuleForm(CapsuleInfo capsule) : this()
        {
            if (capsule != null)
            {
                _isEditMode = true;
                _editingCapsule = new CapsuleInfo
                {
                    CapsuleId = capsule.CapsuleId,
                    UserId = capsule.UserId,
                    Title = capsule.Title,
                    OpenDate = capsule.OpenDate,
                    NoticeShown = capsule.NoticeShown,
                    Songs = capsule.Songs != null
                        ? new List<SongInfo>(capsule.Songs)
                        : new List<SongInfo>()
                };
            }
        }

        private async void CreateCapsuleForm_Load(object sender, EventArgs e)
        {
            dtpOpenDate.MinDate = DateTime.Now.Date.AddDays(1);
            dtpOpenDate.Value = DateTime.Now.Date.AddDays(1);

            lstSongs.DisplayMember = "DisplayText";

            await LoadSongsAsync();

            if (_isEditMode && _editingCapsule != null)
            {
                this.Text = "캡슐 수정";
                btnSave.Text = "수정";

                txtTitle.Text = _editingCapsule.Title ?? "";

                DateTime targetDate = _editingCapsule.OpenDate.Date;
                if (targetDate < dtpOpenDate.MinDate)
                    targetDate = dtpOpenDate.MinDate;

                dtpOpenDate.Value = targetDate;

                lstSongs.Items.Clear();
                foreach (var song in _editingCapsule.Songs)
                {
                    var matched = _allSongs.FirstOrDefault(x => x.SongId == song.SongId);
                    lstSongs.Items.Add(matched ?? song);
                }
            }
            else
            {
                this.Text = "캡슐 만들기";
                btnSave.Text = "저장";
            }
        }

        private async Task LoadSongsAsync()
        {
            try
            {
                _allSongs = await DatabaseHelper.GetSongsByGenreAsync("전체");

                if (songList is ComboBox combo)
                {
                    combo.DataSource = null;
                    combo.DisplayMember = "DisplayText";
                    combo.ValueMember = "SongId";
                    combo.DataSource = _allSongs;
                    combo.SelectedIndex = -1;
                    combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    combo.AutoCompleteSource = AutoCompleteSource.ListItems;
                }
                else if (songList is ComboBox textBox)
                {
                    var autoSource = new AutoCompleteStringCollection();
                    autoSource.AddRange(_allSongs.Select(x => x.DisplayText).ToArray());

                    textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBox.AutoCompleteCustomSource = autoSource;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("곡 목록을 불러오지 못했습니다.\n" + ex.Message);
            }
        }

        private SongInfo FindSelectedSong()
        {
            if (songList is ComboBox combo && combo.SelectedItem is SongInfo comboSong)
                return comboSong;

            string text = songList.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return _allSongs.FirstOrDefault(x =>
                string.Equals(x.Title, text, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x.DisplayText, text, StringComparison.OrdinalIgnoreCase));
        }

        private void btnAddSong_Click(object sender, EventArgs e)
        {
            SongInfo song = FindSelectedSong();

            if (song == null)
            {
                MessageBox.Show("플레이리스트에 있는 곡을 선택하세요.");
                return;
            }

            bool exists = lstSongs.Items
                .Cast<object>()
                .Any(x => x is SongInfo info && info.SongId == song.SongId);

            if (exists)
            {
                MessageBox.Show("이미 추가된 곡입니다.");
                return;
            }

            lstSongs.Items.Add(song);
            songList.Text = "";
            songList.Focus();
        }

        private void btnRemoveSong_Click(object sender, EventArgs e)
        {
            if (lstSongs.SelectedIndex == -1)
            {
                MessageBox.Show("삭제할 노래를 선택하세요.");
                return;
            }

            lstSongs.Items.RemoveAt(lstSongs.SelectedIndex);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("캡슐 이름을 입력하세요.");
                txtTitle.Focus();
                return;
            }

            if (lstSongs.Items.Count == 0)
            {
                MessageBox.Show("최소 1곡 이상 추가하세요.");
                songList.Focus();
                return;
            }

            var capsule = new CapsuleInfo
            {
                CapsuleId = _isEditMode && _editingCapsule != null ? _editingCapsule.CapsuleId : 0,
                UserId = _isEditMode && _editingCapsule != null ? _editingCapsule.UserId : 0,
                Title = txtTitle.Text.Trim(),
                OpenDate = dtpOpenDate.Value.Date,
                NoticeShown = _isEditMode && _editingCapsule != null && _editingCapsule.NoticeShown
            };

            foreach (var item in lstSongs.Items)
            {
                if (item is SongInfo song)
                    capsule.Songs.Add(song);
            }

            ApiResult<CapsuleInfo> result;

            if (_isEditMode)
                result = await DatabaseHelper.UpdateCapsuleAsync(capsule);
            else
                result = await DatabaseHelper.CreateCapsuleAsync(capsule);

            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }

            CreatedCapsule = result.Data ?? capsule;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}