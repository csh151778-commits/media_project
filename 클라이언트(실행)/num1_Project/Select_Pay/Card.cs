using System;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
//timer는 비밀번호 *로 가리기 위해 추가.

namespace num1_Project.Select_Pay
{
    public partial class Card : Form
    {
        // Pay.cs에서 전달받은 요금제 종류 ("일반" 또는 "VIP")
        private readonly string _planType;

        public Card(string planType)
        {
            InitializeComponent();
            _planType = planType;
        }

        private void Card_Load(object sender, EventArgs e)
        {
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox3.TextAlign = HorizontalAlignment.Center;
            textBox4.TextAlign = HorizontalAlignment.Center;
            textBox5.TextAlign = HorizontalAlignment.Center;
            textBox6.TextAlign = HorizontalAlignment.Center;
            textBox7.TextAlign = HorizontalAlignment.Center;

            // ── 비밀번호 초기 표시: 전부 * 로 시작
            textBox7.Text = "****";

            textBox1.MaxLength = 4;
            textBox2.MaxLength = 4;
            textBox3.MaxLength = 4;
            textBox4.MaxLength = 4;
            textBox5.MaxLength = 5;
            textBox6.MaxLength = 3;
            textBox7.MaxLength = 4;

            textBox1.KeyPress += TextBoxNumberOnly_KeyPress;
            textBox2.KeyPress += TextBoxNumberOnly_KeyPress;
            textBox3.KeyPress += TextBoxNumberOnly_KeyPress;
            textBox4.KeyPress += TextBoxNumberOnly_KeyPress;
            textBox5.KeyPress += TextBoxNumberOnly_KeyPress;
            textBox6.KeyPress += TextBoxNumberOnly_KeyPress;

            textBox1.TextChanged += textBox1_TextChanged;
            textBox2.TextChanged += textBox2_TextChanged;
            textBox3.TextChanged += textBox3_TextChanged;
            textBox5.TextChanged += textBox5_TextChanged;

            textBox7.KeyPress += textBox7_KeyPress;
            button1.Click += button1_Click;
        }

        private void TextBoxNumberOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 4) textBox2.Focus();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 4) textBox3.Focus();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == 4) textBox4.Focus();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string raw = textBox5.Text.Replace("/", "");
            if (raw.Length >= 3)
            {
                textBox5.TextChanged -= textBox5_TextChanged;
                textBox5.Text = raw.Substring(0, 2) + "/" + raw.Substring(2);
                textBox5.SelectionStart = textBox5.Text.Length;
                textBox5.TextChanged += textBox5_TextChanged;
            }
        }

        // ── 실제 비밀번호 앞 2자리 저장 변수
        private string realPassword = "";

        // ── 마스킹 타이머: 숫자 입력 후 잠깐 보였다가 * 로 변환
        private Timer _maskTimer;

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            if (e.KeyChar == (char)Keys.Back)
            {
                if (realPassword.Length > 0)
                    realPassword = realPassword.Substring(0, realPassword.Length - 1);

                // 백스페이스 시 타이머 불필요 → 즉시 마스킹 표시
                UpdatePasswordDisplay(maskAll: true);
                return;
            }

            if (!char.IsDigit(e.KeyChar) || realPassword.Length >= 2)
                return;

            realPassword += e.KeyChar;

            // 방금 입력한 숫자는 잠깐 보이도록 표시 (마스킹 전)
            UpdatePasswordDisplay(maskAll: false);

            // 기존 타이머 제거 후 새로 시작 (500ms 후 마스킹)
            _maskTimer?.Stop();
            _maskTimer?.Dispose();
            _maskTimer = new Timer();
            _maskTimer.Interval = 500; // 0.5초 후 * 로 변환
            _maskTimer.Tick += (s, args) =>
            {
                _maskTimer.Stop();
                _maskTimer.Dispose();
                _maskTimer = null;
                UpdatePasswordDisplay(maskAll: true);
            };
            _maskTimer.Start();
        }

        private void UpdatePasswordDisplay(bool maskAll)
        {
            // maskAll = true  → 입력된 자리 모두 * 로 표시  예: "****"
            // maskAll = false → 마지막 입력 숫자는 보이고 나머지 * 예: "3***", "3*" 입력 중
            string display = "";
            for (int i = 0; i < 2; i++)
            {
                if (i < realPassword.Length)
                    display += maskAll ? "*" : (i == realPassword.Length - 1 ? realPassword[i].ToString() : "*");
                else
                    display += "*";
            }
            display += "**"; // 뒤 2자리 고정 마스킹

            textBox7.Text = display;
            textBox7.SelectionStart = textBox7.Text.Length;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string selectedCard = "";
            if      (radioButton1.Checked) selectedCard = "신한카드";
            else if (radioButton2.Checked) selectedCard = "농협카드";
            else if (radioButton3.Checked) selectedCard = "국민카드";
            else if (radioButton4.Checked) selectedCard = "현대카드";

            if (selectedCard == "")
            {
                MessageBox.Show("카드사를 선택해주세요.", "알림");
                return;
            }

            if (textBox1.Text.Length != 4 || textBox2.Text.Length != 4 ||
                textBox3.Text.Length != 4 || textBox4.Text.Length != 4)
            {
                MessageBox.Show("카드 번호를 올바르게 입력해주세요. (각 4자리)", "알림");
                return;
            }

            if (textBox5.Text.Length < 5)
            {
                MessageBox.Show("카드 유효기간을 입력해주세요. (예: 12/26)", "알림");
                return;
            }

            if (textBox6.Text.Length != 3)
            {
                MessageBox.Show("CVC를 올바르게 입력해주세요. (3자리)", "알림");
                return;
            }

            if (realPassword.Length != 2)
            {
                MessageBox.Show("카드 비밀번호 앞 2자리를 입력해주세요.", "알림");
                return;
            }

            button1.Enabled = false;
            var result = await DatabaseHelper.UpdatePaymentAsync(_planType);

            if (!result.Success)
            {
                MessageBox.Show($"결제 처리 중 오류가 발생했습니다.\n{result.Message}", "오류");
                button1.Enabled = true;
                return;
            }

            MessageBox.Show($"[{selectedCard}] 결제가 완료되었습니다. 회원가입을 축하합니다!", "결제 완료");
            this.Close();
        }
    }
}
