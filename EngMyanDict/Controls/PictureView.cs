using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngMyanDict.Controls
{
    public partial class PictureView : UserControl
    {
        #region Variables
        private const double ZOOMFACTOR = 1.25;   // = 25% smaller or larger
        private const int MINMAX = 5;				// 5 times bigger or smaller than the ctrl
        #endregion

        #region Constructor
        public PictureView()
        {
            InitializeComponent();

            this.AutoScroll = true;
            this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            this.pictureBox.MouseWheel += this.PictureBox_MouseWheel;
            this.pictureBox.MouseEnter += this.PictureBox_MouseEnter;
        }

        #endregion

        #region Methods
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.Visible = false;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.PictureBox_MouseEnter(this.pictureBox, e);
        }

        private void ZoomIn()
        {
            if ((this.pictureBox.Width < (MINMAX * this.Width)) &&
                (this.pictureBox.Height < (MINMAX * this.Height)))
            {
                int width = Convert.ToInt32(this.pictureBox.Width * ZOOMFACTOR);
                int height = Convert.ToInt32(this.pictureBox.Height * ZOOMFACTOR);
                this.pictureBox.Size = new Size(width, height);
                this.pictureBox.Location = new Point((this.Width - width) / 2, (this.Height - height) / 2);
                this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                
            }
        }

        private void ZoomOut()
        {
            if ((this.pictureBox.Width > (this.Width / MINMAX)) &&
                (this.pictureBox.Height > (this.Height / MINMAX)))
            {
                this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                int width = Convert.ToInt32(this.pictureBox.Width / ZOOMFACTOR);
                int height = Convert.ToInt32(this.pictureBox.Height / ZOOMFACTOR);
                this.pictureBox.Size = new Size(width, height);
                this.pictureBox.Location = new Point((this.Width - width) / 2, (this.Height - height) / 2);
            }
        }
        #endregion

        #region Events
        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (!this.pictureBox.Focused)
            {
                this.pictureBox.Focus();
            }
        }

        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.ZoomOut();
            }
            else
            {
                this.ZoomIn();
            }
        }
        #endregion

        #region Properties
        public Image Image
        {
            get
            {
                return this.pictureBox.Image;
            }
            set
            {
                if (value != null)
                {
                    this.pictureBox.Size = new Size(value.Width, value.Height);
                    this.pictureBox.Location = new Point((this.Width - value.Width) / 2, (this.Height - value.Height) / 2);
                    this.pictureBox.Image = value;
                }
            }
        }
        #endregion
    }
}
