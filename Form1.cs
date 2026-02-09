using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NoCloseWindow_1
{
    public partial class Form1 : Form
    {
        private Label label;
        private bool isExiting = false;
        private Button rebootButton;  // 추가: 수동 재부팅 버튼

        public Form1()
        {
            this.Text = "Non-closable Window";
            this.ControlBox = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.TopMost = true;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Location = new Point(100, 100);
            this.Move += (s, e) =>
            {
                this.Location = new Point(100, 100);
            };

            this.Size = new Size(450, 250);  // 버튼 공간 확보
            this.BackColor = Color.White;

            // 기존 레이블
            label = new Label
            {
                Location = new Point(20, 20),
                Size = new Size(400, 100),
                Text = "This window cannot be closed.\n이 창은 닫을 수 없습니다.",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(label);

            // 추가: 수동 재부팅 버튼 (UI 안전)
            rebootButton = new Button
            {
                Text = "재부팅 (5초 후)",
                Location = new Point(20, 140),
                Size = new Size(120, 35),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            rebootButton.Click += RebootButton_Click;
            this.Controls.Add(rebootButton);

            this.FormClosing += Form1_FormClosing;

            // SessionEnding 등록 (단순 확인만)
            SystemEvents.SessionEnding += OnSessionEnding;
        }

        // ===== 리팩토링: SessionEnding에서 단순 확인만 =====
        private void OnSessionEnding(object? sender, SessionEndingEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to restart?\n정말로 다시 시작하시겠습니까?",
                "재부팅 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Windows 자체 타이머 사용 (UI 불필요)
                Process.Start("shutdown", "/r /t 5 /f");
                isExiting = true;  // 정상 종료 허용
            }
            else
            {
                e.Cancel = true;  // 재부팅 취소
            }
        }

        // ===== 추가: 버튼 클릭으로 재부팅 (UI 안전) =====
        private void RebootButton_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "5초 후 재부팅됩니다. 계속하시겠습니까?",
                "재부팅 실행",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "shutdown",
                    Arguments = "/r /t 5 /f",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                isExiting = true;
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!isExiting)
            {
                e.Cancel = true;
            }
        }
    }
}
