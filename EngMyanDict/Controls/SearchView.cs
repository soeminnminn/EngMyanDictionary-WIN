using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace DictControls.Controls
{
    public partial class SearchView : UserControl
    {
        #region Variables
        public delegate void SearchEventHandler(object sender, EventArgs e);
        public event SearchEventHandler Search = null;
        #endregion

        #region Constructor
        public SearchView()
        {
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            this.txtSearch.BorderStyle = BorderStyle.None;
            this.txtSearch.BackColor = SystemColors.Window;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
        }

        private void FillRoundedRectangle(Graphics g, Brush b, Rectangle rc, Size size)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddLine(rc.Left + size.Width / 2, rc.Top, rc.Right - size.Width / 2, rc.Top);
                path.AddArc(rc.Right - size.Width, rc.Top, size.Width, size.Height, 270, 90);

                path.AddLine(rc.Right, rc.Top + size.Height / 2, rc.Right, rc.Bottom - size.Height / 2);
                path.AddArc(rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height, 0, 90);

                path.AddLine(rc.Right - size.Width / 2, rc.Bottom, rc.Left + size.Width / 2, rc.Bottom);
                path.AddArc(rc.Left, rc.Bottom - size.Height, size.Width, size.Height, 90, 90);

                path.AddLine(rc.Left, rc.Bottom - size.Height / 2, rc.Left, rc.Top + size.Height / 2);
                path.AddArc(rc.Left, rc.Top, size.Width, size.Height, 180, 90);

                SmoothingMode oldSmoothingMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(b, path);
                g.SmoothingMode = oldSmoothingMode;
            }
        }

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

        private void DrawGlass(Graphics graphics, Rectangle rectangle)
        {
            Point ptDraw = new Point(rectangle.X + ((rectangle.Width - 20) / 2), rectangle.Y + ((rectangle.Height - 18) / 2));
            GraphicsPath path = new GraphicsPath();

            SmoothingMode oldSmoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            path.StartFigure();
            path.AddEllipse(ptDraw.X + 1, ptDraw.Y + 1, 10, 10);
            path.AddLine(ptDraw.X + 10, ptDraw.Y + 10, ptDraw.X + 14, ptDraw.Y + 14);
            graphics.DrawPath(new Pen(Color.FromArgb(100, 0, 0, 0), 1), path);
            path.Dispose();

            ptDraw = new Point(rectangle.X + ((rectangle.Width - 20) / 2), rectangle.Y + ((rectangle.Height - 15) / 2));
            path = new GraphicsPath();

            path.StartFigure();
            path.AddEllipse(ptDraw.X + 1, ptDraw.Y + 1, 10, 10);
            path.AddLine(ptDraw.X + 10, ptDraw.Y + 10, ptDraw.X + 14, ptDraw.Y + 14);
            graphics.DrawPath(new Pen(Color.FromArgb(120, 0, 0, 0), 1), path);
            path.Dispose();

            graphics.SmoothingMode = oldSmoothingMode;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Rectangle paintRect = new Rectangle(2, 2, this.Width - 4, this.Height - 4);
            Size radius = new Size(8, 8);
            this.FillRoundedRectangle(e.Graphics, SystemBrushes.Window, paintRect, radius);
            this.DrawRoundedRectangle(e.Graphics, SystemPens.ActiveBorder, paintRect, radius);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle glassRect = new Rectangle(this.Width - 36, (this.Height - 38) / 2, 40, 40);
            this.DrawGlass(e.Graphics, glassRect);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.txtSearch.Location = new Point(12, (this.Height - this.txtSearch.Height) / 2);
            this.txtSearch.Width = this.Width - 42;
        }

        protected void OnSearch()
        {
            if (this.Search != null)
            {
                this.Search(this, EventArgs.Empty);
            }
        }
        
        public void PerformSearch()
        {
            this.OnSearch();
        }
        #endregion

        #region Events
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.OnSearch();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.txtSearch.Clear();
            }
        }
        #endregion

        #region Properties
        [Browsable(true)]
        public override string Text
        {
            get { return this.txtSearch.Text; }
            set { this.txtSearch.Text = value; }
        }
        #endregion
    }
}
