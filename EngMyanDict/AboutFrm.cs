using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngMyanDict
{
    public partial class AboutFrm : Form
    {
        #region Variables
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int HTCAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Constructor
        public AboutFrm()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void DrawRoundedRectangle(Graphics g, Pen p, Rectangle rc, Size size)
        {
            // 1 pixel indent in all sides = Size(4, 4)
            // To make pixel indentation larger, change by a factor of 4,
            // i. e., 2 pixels indent = Size(8, 8);

            SmoothingMode oldSmoothingMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.DrawLine(p, rc.Left + size.Width / 2, rc.Top, rc.Right - size.Width / 2, rc.Top);
            g.DrawArc(p, rc.Right - size.Width, rc.Top, size.Width, size.Height, 270, 90);

            g.DrawLine(p, rc.Right, rc.Top + size.Height / 2, rc.Right, rc.Bottom - size.Height / 2);
            g.DrawArc(p, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height, 0, 90);

            g.DrawLine(p, rc.Right - size.Width / 2, rc.Bottom, rc.Left + size.Width / 2, rc.Bottom);
            g.DrawArc(p, rc.Left, rc.Bottom - size.Height, size.Width, size.Height, 90, 90);

            g.DrawLine(p, rc.Left, rc.Bottom - size.Height / 2, rc.Left, rc.Top + size.Height / 2);
            g.DrawArc(p, rc.Left, rc.Top, size.Width, size.Height, 180, 90);

            g.SmoothingMode = oldSmoothingMode;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Rectangle paintRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            Size radius = new Size(8, 8);
            this.DrawRoundedRectangle(e.Graphics, SystemPens.ActiveBorder, paintRect, radius);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN)
            {
                ReleaseCapture();
                SendMessage(m.HWnd, (int)WM_NCLBUTTONDOWN, new IntPtr((int)HTCAPTION), IntPtr.Zero);
                return;
            }
            base.WndProc(ref m);
        }
        #endregion
    }
}
