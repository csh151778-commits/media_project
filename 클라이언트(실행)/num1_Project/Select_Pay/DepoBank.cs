using System;
using System.Windows.Forms;

namespace num1_Project.Select_Pay
{
    public partial class DepoBank : Form
    {
        // ── [추가] Pay.cs에서 전달받은 요금제 종류 ("일반" 또는 "VIP")
        // 확인 버튼 클릭 시 DB 업데이트에 사용
        private readonly string _planType;

        // ── [수정] 기존 DepoBank() → DepoBank(string planType) 으로 변경
        // Pay.cs에서 new DepoBank(planType) 형태로 호출
        public DepoBank(string planType)
        {
            InitializeComponent();
            _planType = planType;
        }

        // ── [수정] 확인 버튼: API 호출로 DB 업데이트 후 완료 메시지
        // 기존: MessageBox만 띄우고 닫음
        // 변경: DatabaseHelper.UpdatePaymentAsync(_planType) 호출 → IsPaid='결제', PlanType 저장
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false; // 중복 클릭 방지

            // ── [추가] API 호출: IsPaid = '결제', PlanType = '일반'/'VIP' 로 업데이트
            var result = await DatabaseHelper.UpdatePaymentAsync(_planType);

            if (!result.Success)
            {
                MessageBox.Show($"결제 처리 중 오류가 발생했습니다.\n{result.Message}", "오류");
                button1.Enabled = true;
                return;
            }

            MessageBox.Show("입금이 확인 되었습니다. 회원가입을 축하합니다!");
            this.Close();
        }
    }
}
