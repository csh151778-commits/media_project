using System;
using System.Drawing;
using System.Windows.Forms;

namespace num1_Project
{
    public partial class NewsDetailForm : Form
    {
        public NewsDetailForm()
        {
            InitializeComponent();
        }

        public NewsDetailForm(NewsItem item)
        {
            InitializeComponent();

            if (item != null)
            {
                lblHeadline.Text = item.Headline;
                lblDate.Text     = item.Date;
                lblBody.Text     = item.Body;

                if (!string.IsNullOrEmpty(item.ImageCaption))
                    lblImgCaption.Text = item.ImageCaption;

                // [추가] 이미지 URL이 있으면 PictureBox에 비동기 로딩x
                if (!string.IsNullOrWhiteSpace(item.ImageUrl))
                {
                    picMain.Visible = true;
                    picMain.SizeMode = PictureBoxSizeMode.Zoom;

                    // 로딩 실패 시 숨김 처리
                    picMain.LoadCompleted += (s, e) =>
                    {
                        if (e.Error != null || picMain.Image == null)
                            picMain.Visible = false;
                    };

                    try
                    {
                        picMain.LoadAsync(item.ImageUrl);
                    }
                    catch
                    {
                        picMain.Visible = false;
                    }
                }
                else
                {
                    picMain.Visible = false;
                }
            }

            btnClose.Click += (s, e) => this.Close();
        }

        private void NewsDetailForm_Load(object sender, EventArgs e)
        {
        }
    }
}
