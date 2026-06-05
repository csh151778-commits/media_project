#nullable disable
using System;
using System.Windows.Forms;

namespace num1_Project
{
    public partial class Membership : Form
    {
        public Membership()
        {
            InitializeComponent();

            comboBox1.Items.Add("경기도");
            comboBox1.Items.Add("강원도");
            comboBox1.Items.Add("충청도");
            comboBox1.Items.Add("경상도");
            comboBox1.Items.Add("전라도");
            comboBox1.Items.Add("제주특별자치도");
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.ActiveControl = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            if (comboBox1.Text == "경기도")
            {
                listBox1.Items.Add("서울특별시");
                listBox1.Items.Add("인천광역시");
                listBox1.Items.Add("수원시");
                listBox1.Items.Add("오산시");
                listBox1.Items.Add("평택시");
            }

            if (comboBox1.Text == "강원도")
            {
                listBox1.Items.Add("춘천시");
                listBox1.Items.Add("강릉시");
                listBox1.Items.Add("원주시");
            }

            if (comboBox1.Text == "충청도")
            {
                listBox1.Items.Add("천안시");
                listBox1.Items.Add("충주시");
                listBox1.Items.Add("청주시");
            }

            if (comboBox1.Text == "경상도")
            {
                listBox1.Items.Add("부산시");
                listBox1.Items.Add("대구시");
                listBox1.Items.Add("구미시");
                listBox1.Items.Add("상주시");
            }

            if (comboBox1.Text == "전라도")
            {
                listBox1.Items.Add("광주광역시");
                listBox1.Items.Add("여수시");
                listBox1.Items.Add("목포시");
                listBox1.Items.Add("순천시");
            }

            if (comboBox1.Text == "제주특별자치도")
            {
                listBox1.Items.Add("-");
            }
        }

        private async void Btn_newmem_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string name = textBox3.Text.Trim();
            string phone = textBox4.Text.Trim();
            string region = comboBox1.Text;
            string city = listBox1.SelectedItem?.ToString() ?? "";
            string detailAddr = textBox5.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("아이디를 입력하세요.", "입력 오류");
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("비밀번호를 입력하세요.", "입력 오류");
                textBox2.Focus();
                return;
            }

            if (!radioButton1.Checked)
            {
                MessageBox.Show("이용 약관에 동의해야 가입할 수 있습니다.", "동의 필요");
                return;
            }

            try
            {
                var request = new RegisterRequest
                {
                    Username = username,
                    Password = password,
                    Name = name,
                    Phone = phone,
                    Region = region,
                    City = city,
                    DetailAddr = detailAddr
                };

                var result = await DatabaseHelper.RegisterAsync(request);

                if (result.Success)
                {
                    MessageBox.Show("가입 완료 되었습니다!", "회원가입 성공");
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "회원가입 실패");
                    textBox1.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류가 발생했습니다.\n{ex.Message}", "오류");
            }
        }
    }
}