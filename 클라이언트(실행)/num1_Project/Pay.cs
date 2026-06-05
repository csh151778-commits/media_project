using num1_Project.Select_Pay;
using System;
using System.Windows.Forms;

namespace num1_Project
{
    public partial class Pay : Form
    {
        // 요금제 가격
        private const int PRICE_NORMAL = 5000;
        private const int PRICE_VIP = 100000;

        public Pay()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                label3.Text = PRICE_NORMAL.ToString("N0") + "원";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                label3.Text = PRICE_VIP.ToString("N0") + "원";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 요금제 선택 확인
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("요금제를 선택해주세요.", "알림");
                return;
            }

            // 결제 수단 선택 확인
            if (!radioButton3.Checked && !radioButton4.Checked)
            {
                MessageBox.Show("결제 수단을 선택해주세요.", "알림");
                return;
            }

            // ── [추가] 선택한 요금제를 문자열로 결정
            // Card/DepoBank 생성자에 전달 → 결제 완료 시 DB에 저장될 값
            string planType = radioButton1.Checked ? "일반" : "VIP";

            // 선택한 결제 수단에 맞는 폼 열기
            if (radioButton3.Checked)
            {
                // ── [수정] Card 생성자에 planType 전달 (기존: new Card())
                var cardForm = new Card(planType);
                cardForm.ShowDialog();
            }
            else if (radioButton4.Checked)
            {
                // ── [수정] DepoBank 생성자에 planType 전달 (기존: new DepoBank())
                var depoBankForm = new DepoBank(planType);
                depoBankForm.ShowDialog();
            }
        }
    }
}
