using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace num1_Project
{
    /// <summary>
    /// TrackBar를 상속해서 OnPaint만 오버라이드한 커스텀 재생바.
    /// Value, Minimum, Maximum, ValueChanged 등 TrackBar 기능은 그대로 사용.
    /// </summary>
    public class SeekBar : TrackBar
    {
        public SeekBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            TickStyle = TickStyle.None;
            BackColor = Color.FromArgb(22, 27, 34);
            Height = 20;
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);

            int trackH = 4;
            int trackY = (Height - trackH) / 2;
            int trackX = 8;
            int trackW = Width - 16;

            // ── 배경 트랙 (회색) ──────────────────────
            using (var brush = new SolidBrush(Color.FromArgb(55, 60, 70)))
            using (var path = RoundedRect(new Rectangle(trackX, trackY, trackW, trackH), 2))
                g.FillPath(brush, path);

            // ── 진행된 부분 (파란색) ──────────────────
            float ratio = Maximum > Minimum
                ? (float)(Value - Minimum) / (Maximum - Minimum)
                : 0f;
            int fillW = (int)(trackW * ratio);

            if (fillW > 0)
            {
                using (var brush = new SolidBrush(Color.FromArgb(88, 166, 255)))
                using (var path = RoundedRect(new Rectangle(trackX, trackY, fillW, trackH), 2))
                    g.FillPath(brush, path);
            }

            // ── 핸들 (흰색 원) ────────────────────────
            int handleX = trackX + fillW;
            int r = 6;
            using (var brush = new SolidBrush(Color.White))
                g.FillEllipse(brush, handleX - r, Height / 2 - r, r * 2, r * 2);
        }

        // ── 값이 바뀔 때마다 다시 그리기 ─────────────
        protected override void OnValueChanged(System.EventArgs e)
        {
            base.OnValueChanged(e);
            Invalidate();
        }

        // ── 유틸: 둥근 사각형 경로 ───────────────────
        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            SetValueFromMouse(e.X);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
                SetValueFromMouse(e.X);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            SetValueFromMouse(e.X);
        }

        private void SetValueFromMouse(int mouseX)
        {
            int trackX = 8;
            int trackW = Width - 16;
            float ratio = Math.Clamp((float)(mouseX - trackX) / trackW, 0f, 1f);
            Value = Minimum + (int)(ratio * (Maximum - Minimum));
        }
    }
}
