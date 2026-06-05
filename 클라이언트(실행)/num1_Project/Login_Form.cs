#nullable disable
using System;
using System.Windows.Forms;

namespace num1_Project
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
            textBox2.PasswordChar = '●';
            DatabaseHelper.InitApi();
        }

        private void Btn_Membership_Click(object sender, EventArgs e)
        {
            var memberForm = new Membership();
            memberForm.ShowDialog(this);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.PlaceholderText = "아이디";
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.PlaceholderText = "비밀번호";
        }

        private async void Btn_Login_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("아이디와 비밀번호를 모두 입력하세요.", "입력 오류");
                return;
            }

            try
            {
                // 서버에 로그인 요청을 보내고 결과를 받아옴
                // result.Data 안에 UserDto가 들어있고, 여기에 AdminRole 값도 포함돼 있어
                var result = await DatabaseHelper.LoginAsync(username, password);

                if (result.Success)
                {
                    // ──────────────────────────────────────────────────────
                    // [추가] 관리자 체크박스(checkBox1) 검증 로직
                    //
                    // checkBox1.Checked  → 사용자가 로그인 화면에서 "관리자" 체크박스를 체크했는지 여부
                    // isActuallyAdmin    → 서버 DB에서 받아온 AdminRole 값이 실제로 1인지 여부
                    //
                    // 케이스별 동작:
                    //   체크O + 실제관리자O → 관리자로 로그인 성공
                    //   체크O + 실제관리자X → 차단! "관리자 권한이 없습니다" 메시지
                    //   체크X + 실제관리자O → 일반 고객으로 로그인 (관리자 버튼 안 보임)
                    //   체크X + 실제관리자X → 일반 고객으로 로그인 정상 처리
                    // ──────────────────────────────────────────────────────
                    bool isAdminChecked = checkBox1.Checked;
                    bool isActuallyAdmin = result.Data?.AdminRole == 1;

                    // 관리자 체크박스를 눌렀는데 실제로 관리자가 아닌 경우 → 로그인 차단
                    if (isAdminChecked && !isActuallyAdmin)
                    {
                        MessageBox.Show("관리자 권한이 없는 계정입니다.", "로그인 실패");

                        // 보안을 위해 CurrentUser를 다시 null로 초기화해
                        // (LoginAsync 내부에서 이미 CurrentUser에 저장됐을 수 있으니까)
                        DatabaseHelper.Logout();

                        textBox2.Clear();
                        textBox2.Focus();
                        return;
                    }

                    // [추가] 관리자 체크박스 체크 여부를 DatabaseHelper 에 저장
                    // MainForm 에서 이 값을 읽어서 관리자 버튼 표시 여부를 결정해
                    // 조건: 실제 관리자(AdminRole == 1) 이면서 체크박스도 체크한 경우만 true
                    DatabaseHelper.IsAdminLogin = isAdminChecked && isActuallyAdmin;

                    // 타임캡슐 오픈 알림 체크 (기존 로직 유지)
                    await DatabaseHelper.CheckOpenedCapsulesAndNotifyAsync(this);

                    // 메인 폼으로 이동
                    var mainForm = new MainForm();
                    mainForm.Show();

                    this.Hide();
                    mainForm.FormClosed += (s, args) => Application.Exit();
                }
                else
                {
                    MessageBox.Show(result.Message, "로그인 실패");
                    textBox2.Clear();
                    textBox2.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류가 발생했습니다.\n{ex.Message}", "오류");
            }
        }

        private void Btn_findid_Click(object sender, EventArgs e)
        {
            MessageBox.Show("현재 서비스 준비중입니다. 아이디 찾기 기능은 아직 구현되지 않았습니다.", "안내");
        }

        private string ShowInput(string prompt, string title)
        {
            Form inputForm = new Form
            {
                Text = title,
                Width = 360,
                Height = 150,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lbl = new Label
            {
                Text = prompt,
                Left = 16,
                Top = 16,
                Width = 310,
                AutoSize = false
            };

            TextBox tb = new TextBox
            {
                Left = 16,
                Top = 44,
                Width = 310
            };

            Button ok = new Button
            {
                Text = "확인",
                DialogResult = DialogResult.OK,
                Left = 170,
                Top = 74,
                Width = 80
            };

            Button cancel = new Button
            {
                Text = "취소",
                DialogResult = DialogResult.Cancel,
                Left = 260,
                Top = 74,
                Width = 80
            };

            inputForm.Controls.AddRange(new System.Windows.Forms.Control[] { lbl, tb, ok, cancel });
            inputForm.AcceptButton = ok;
            inputForm.CancelButton = cancel;

            return inputForm.ShowDialog() == DialogResult.OK ? tb.Text.Trim() : "";
        }

        private void Btn_findpass_Click(object sender, EventArgs e)
        {
            MessageBox.Show("현재 서비스 준비중입니다. 비밀번호 찾기 기능은 아직 구현되지 않았습니다.", "안내");
        }
    }
}
