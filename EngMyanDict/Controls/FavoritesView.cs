using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EngMyanDict.Controls
{
    public partial class FavoritesView : UserControl
    {
        #region Variables
        public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);
        public event SelectedIndexChangedEventHandler SelectedIndexChanged = null;

        public delegate void DeleteEventHandler(object sender, EventArgs e);
        public event DeleteEventHandler Delete = null;

        public delegate void CloseEventHandler(object sender, EventArgs e);
        public event CloseEventHandler Close = null;
        #endregion

        #region Constructor
        public FavoritesView()
        {
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            this.toolStripEdit.Visible = false;
            this.listFavorites.Location = new Point(8, 66);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.listFavorites.Size = new Size(this.Width - 16, this.Height - 76);
        }
        #endregion

        #region Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.Close != null)
            {
                this.Close(this, e);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.toolStripFavorites.Visible = false; ;
            this.toolStripEdit.Visible = true;
            this.listFavorites.SelectionMode = SelectionMode.MultiSimple;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.toolStripFavorites.Visible = true;
            this.toolStripEdit.Visible = false;
            this.listFavorites.SelectionMode = SelectionMode.One;
            if (this.listFavorites.SelectedIndex > -1)
                this.listFavorites.SetSelected(this.listFavorites.SelectedIndex, true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.Delete != null)
            {
                this.Delete(this, e);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.listFavorites.SelectAll();
        }

        private void listFavorites_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listFavorites.SelectionMode == SelectionMode.One && this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, e);
            }
        }
        #endregion

        #region Properties
        [Browsable(false)]
        public ListBox.ObjectCollection Items
        {
            get { return this.listFavorites.Items; }
        }

        [Browsable(false)]
        public int SelectedIndex
        {
            get { return this.listFavorites.SelectedIndex; }
        }

        [Browsable(false)]
        public object SelectedItem
        {
            get { return this.listFavorites.SelectedItem; }
        }

        [Browsable(false)]
        public ListBox.SelectedIndexCollection SelectedIndices
        {
            get { return this.listFavorites.SelectedIndices; }
        }

        [Browsable(false)]
        public ListBox.SelectedObjectCollection SelectedItems
        {
            get { return this.listFavorites.SelectedItems; }
        }
        #endregion
    }
}
