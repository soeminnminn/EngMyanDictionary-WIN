using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;

namespace DictControls.Controls
{
    public class ListView : ListBox
    {
        #region Variables
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        private object mSelectLock = new object();
        #endregion

        #region Constructor
        public ListView()
        {
            this.InitializeComponent();
            this.InitializeControl();
        }

        public ListView(IContainer container)
        {
            container.Add(this);

            this.InitializeComponent();
            this.InitializeControl();
        }

        protected void InitializeControl()
        {
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.IntegralHeight = false;
            this.ItemHeight = 28;

            //this.SetStyle(ControlStyles.ResizeRedraw, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.DoubleBuffer, true);
            //this.UpdateStyles();

            this.DoubleBuffered = true;
        }
        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        #region Methods
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

        private void DrawCheckBox(Graphics graphics, Rectangle rcPaint, ButtonState state, bool enabled)
        {
            VisualStyleElement element = enabled ? VisualStyleElement.Button.CheckBox.UncheckedNormal : VisualStyleElement.Button.CheckBox.UncheckedDisabled;
            if (state == ButtonState.Checked)
            {
                element = enabled ? VisualStyleElement.Button.CheckBox.CheckedNormal : VisualStyleElement.Button.CheckBox.CheckedDisabled;
            }
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(element))
            {
                VisualStyleRenderer renderer = new VisualStyleRenderer(element);
                renderer.DrawBackground(graphics, rcPaint);
            }
            else
            {
                ControlPaint.DrawCheckBox(graphics, rcPaint, state);
            }
        }

        private void RenderItem(Graphics graphics, Rectangle itemRect, string text, DrawItemState state, bool checkBox)
        {
            bool selected = (state & DrawItemState.Selected) == DrawItemState.Selected;
            bool focused = (state & DrawItemState.Focus) == DrawItemState.Focus;

            graphics.FillRectangle(new SolidBrush(this.BackColor), itemRect);
            Rectangle innerRect = new Rectangle(itemRect.X + 2, itemRect.Y, itemRect.Width - 4, itemRect.Height - 3);

            if (selected && !checkBox)
                this.FillRoundedRectangle(graphics, new SolidBrush(Color.FromArgb(60, SystemColors.Highlight)), innerRect, new System.Drawing.Size(4, 4));
            else
                graphics.FillRectangle(new SolidBrush(this.BackColor), innerRect);

            if (focused)
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Gray)), innerRect);

            if (checkBox)
            {
                int middle = (innerRect.Height - 16) / 2;
                Rectangle checkBoxRect = new Rectangle(innerRect.Right - 27, innerRect.Top + middle, 16, 16);
                this.DrawCheckBox(graphics, checkBoxRect, selected ? ButtonState.Checked : ButtonState.Normal, this.Enabled);
            }
            TextRenderer.DrawText(graphics, text, this.Font, innerRect, this.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            graphics.DrawLine(new Pen(SystemColors.ActiveBorder, 1f), 2, itemRect.Bottom - 2, itemRect.Right - 4, itemRect.Bottom - 2);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            string text = (this.Items.Count > 0 && e.Index >= 0) ? this.Items[e.Index].ToString() : string.Empty;
            this.RenderItem(e.Graphics, e.Bounds, text, e.State, this.SelectionMode == SelectionMode.MultiSimple);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            lock (this.mSelectLock)
            {
                base.OnSelectedIndexChanged(e);
            }
            this.Invalidate();
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            lock (this.mSelectLock)
            {
                base.OnSelectedValueChanged(e);
            }
        }

        public void SelectAll()
        {
            if (this.SelectionMode == SelectionMode.MultiSimple || this.SelectionMode == SelectionMode.MultiExtended)
            {
                if (this.Items.Count > 0)
                {
                    lock (this.mSelectLock)
                    {
                        this.ClearSelected();
                        for (int i = 0; i < this.Items.Count; i++)
                        {
                            this.SetSelected(i, true);
                        }
                    }
                    
                }
            }
            
        }
        #endregion

        #region Properties
        [Browsable(false)]
        public override DrawMode DrawMode
        {
            get { return base.DrawMode; }
            set { base.DrawMode = value; }
        }

        [Browsable(false)]
        public new bool IntegralHeight
        {
            get { return base.IntegralHeight; }
            set { base.IntegralHeight = value; }
        }

        [DefaultValue(28)]
        public override int ItemHeight
        {
            get { return base.ItemHeight; }
            set { base.ItemHeight = value; }
        }
        #endregion
    }
}
