using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace num1_Project
{
    public partial class Lyrics : Form
    {
        private struct LrcLine
        {
            public double TimeSeconds;
            public string Text;
        }

        private List<LrcLine> _lines = new List<LrcLine>();
        private NAudio.Wave.AudioFileReader _audioReader;
        private System.Windows.Forms.Timer _syncTimer;
        private int _currentLineIndex = -1;

        private readonly Color COLOR_NORMAL  = Color.Gray;
        private readonly Color COLOR_CURRENT = Color.White;
        private readonly Font  FONT_NORMAL   = new Font("맑은 고딕", 13f, FontStyle.Regular);
        private readonly Font  FONT_CURRENT  = new Font("맑은 고딕", 13f, FontStyle.Bold);

        public Lyrics(string lyricsText, NAudio.Wave.AudioFileReader audioReader)
        {
            InitializeComponent();

            _audioReader = audioReader;

            richTextBox1.ReadOnly    = true;
            richTextBox1.BackColor   = Color.Black;
            richTextBox1.Font        = FONT_NORMAL;
            richTextBox1.ScrollBars  = RichTextBoxScrollBars.Vertical;
            richTextBox1.BorderStyle = BorderStyle.None;

            // [1] 텍스트 캐럿(세로선) 숨기기
            richTextBox1.GotFocus  += (s, e) => HideCaret(richTextBox1.Handle);
            richTextBox1.MouseDown += (s, e) => HideCaret(richTextBox1.Handle);
            richTextBox1.MouseUp   += (s, e) => HideCaret(richTextBox1.Handle);

            if (string.IsNullOrWhiteSpace(lyricsText))
            {
                richTextBox1.ForeColor = COLOR_NORMAL;
                richTextBox1.Text = "등록된 가사가 없습니다.";
                return;
            }

            _lines = ParseLrc(lyricsText);

            if (_lines.Count == 0)
            {
                richTextBox1.ForeColor = COLOR_NORMAL;
                richTextBox1.Text = lyricsText;
                return;
            }

            InitialRender();

            if (_audioReader == null)
                return;

            _syncTimer = new System.Windows.Forms.Timer();
            _syncTimer.Interval = 300;
            _syncTimer.Tick += SyncTimer_Tick;
            _syncTimer.Start();

            this.FormClosed += (s, e) =>
            {
                _syncTimer?.Stop();
                FONT_NORMAL?.Dispose();
                FONT_CURRENT?.Dispose();
            };
        }

        private void InitialRender()
        {
            richTextBox1.Clear();

            for (int i = 0; i < _lines.Count; i++)
            {
                string lineText = _lines[i].Text + "\n";
                int start = richTextBox1.TextLength;

                richTextBox1.AppendText(lineText);
                richTextBox1.Select(start, lineText.Length);
                richTextBox1.SelectionColor     = COLOR_NORMAL;
                richTextBox1.SelectionFont      = FONT_NORMAL;
                // [2] 가운데 정렬
                richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            }

            richTextBox1.DeselectAll();
        }

        private void SyncTimer_Tick(object sender, EventArgs e)
        {
            if (_audioReader == null || _lines.Count == 0)
                return;

            double currentSec = _audioReader.CurrentTime.TotalSeconds;

            int newIndex = -1;
            for (int i = 0; i < _lines.Count; i++)
            {
                if (_lines[i].TimeSeconds <= currentSec)
                    newIndex = i;
                else
                    break;
            }

            if (newIndex == _currentLineIndex)
                return;

            // 이전 줄 → 회색 + 일반
            if (_currentLineIndex >= 0)
                SetLineStyle(_currentLineIndex, COLOR_NORMAL, FONT_NORMAL);

            // 현재 줄 → 흰색 + 굵게
            if (newIndex >= 0)
                SetLineStyle(newIndex, COLOR_CURRENT, FONT_CURRENT);

            _currentLineIndex = newIndex;

            // [3] 현재 줄이 화면 가운데에 오도록 스크롤
            // ScrollToCaret 은 무조건 맨 위로 올려버리므로 직접 계산
            if (newIndex >= 0)
                ScrollToMiddle(newIndex);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
        private const int WM_SETREDRAW = 11;

        private void SetLineStyle(int lineIndex, Color color, Font font)
        {
            int charPos = 0;
            for (int i = 0; i < lineIndex; i++)
                charPos += _lines[i].Text.Length + 1;

            // 화면 업데이트 잠금 → 색상 변경 → 한 번에 그리기
            SendMessage(richTextBox1.Handle, WM_SETREDRAW, false, 0);

            richTextBox1.Select(charPos, _lines[lineIndex].Text.Length);
            richTextBox1.SelectionColor = color;
            richTextBox1.SelectionFont  = font;
            richTextBox1.DeselectAll();

            SendMessage(richTextBox1.Handle, WM_SETREDRAW, true, 0);
            richTextBox1.Invalidate();
            HideCaret(richTextBox1.Handle);
        }

        // 현재 줄이 RichTextBox 화면 가운데 오도록 스크롤
        private void ScrollToMiddle(int lineIndex)
        {
            // 한 줄 높이 추정 (폰트 높이 기준)
            int lineHeight = FONT_NORMAL.Height + 4;

            // 현재 줄의 Y 위치
            int targetY = lineIndex * lineHeight;

            // 화면 절반 높이만큼 위로 올려서 가운데 오게
            int scrollY = targetY - (richTextBox1.Height / 2) + (lineHeight / 2);
            scrollY = Math.Max(0, scrollY);

            // WinAPI 없이 FirstVisibleIndex 로 스크롤
            // RichTextBox 는 직접 스크롤 위치 설정이 어려우므로
            // Select + ScrollToCaret 으로 위치 맞춘 후 보정
            int charPos = 0;
            for (int i = 0; i < lineIndex; i++)
                charPos += _lines[i].Text.Length + 1;

            richTextBox1.Select(charPos, 0);
            richTextBox1.ScrollToCaret();

            // 현재 줄 위치에서 절반 높이만큼 위로 당겨서 가운데 정렬 효과
            int halfLines = richTextBox1.Height / 2 / lineHeight;
            int targetLine = Math.Max(0, lineIndex - halfLines);

            int targetCharPos = 0;
            for (int i = 0; i < targetLine; i++)
                targetCharPos += _lines[i].Text.Length + 1;

            richTextBox1.Select(targetCharPos, 0);
            richTextBox1.ScrollToCaret();
            richTextBox1.DeselectAll();
        }

        private static List<LrcLine> ParseLrc(string lrcText)
        {
            var result = new List<LrcLine>();
            var lines  = lrcText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                if (!trimmed.StartsWith("[")) continue;

                int closeBracket = trimmed.IndexOf(']');
                if (closeBracket < 0) continue;

                string timeStr = trimmed.Substring(1, closeBracket - 1);
                string text    = trimmed.Substring(closeBracket + 1).Trim();

                double seconds = ParseTimeTag(timeStr);
                if (seconds < 0) continue;

                result.Add(new LrcLine { TimeSeconds = seconds, Text = text });
            }

            result.Sort((a, b) => a.TimeSeconds.CompareTo(b.TimeSeconds));
            return result;
        }

        private static double ParseTimeTag(string timeStr)
        {
            try
            {
                int colon = timeStr.IndexOf(':');
                if (colon < 0) return -1;

                double minutes = double.Parse(timeStr.Substring(0, colon));
                double seconds = double.Parse(timeStr.Substring(colon + 1),
                    System.Globalization.CultureInfo.InvariantCulture);

                return minutes * 60 + seconds;
            }
            catch { return -1; }
        }
    }
}
