using System;
using System.Drawing;
using System.Windows.Forms;

namespace NoCloseWindow_1
{
    public class Form1 : Form
    {
        private Label label;
        private bool isExiting = false;

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

            this.Size = new Size(400, 200);
            this.BackColor = Color.White;

            label = new Label
            {
                Text = "This window cannot be closed.",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Black,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(label);

            this.FormClosing += (s, e) =>
            {
                if (!isExiting)
                {
                    e.Cancel = true;
                }
            };
        }
    }
}
