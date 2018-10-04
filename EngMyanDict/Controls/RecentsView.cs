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
    public partial class RecentsView : UserControl
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
        public RecentsView()
        {
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            this.listRecents.Location = new Point(8, 66);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.listRecents.Size = new Size(this.Width - 16, this.Height - 76);
        }
        #endregion

        #region Propeties
        [Browsable(false)]
        public ListBox.ObjectCollection Items
        {
            get { return this.listRecents.Items; }
        }

        [Browsable(false)]
        public int SelectedIndex
        {
            get { return this.listRecents.SelectedIndex; }
        }

        [Browsable(false)]
        public object SelectedItem
        {
            get { return this.listRecents.SelectedItem; }
        }

        [Browsable(false)]
        public ListBox.SelectedIndexCollection SelectedIndices
        {
            get { return this.listRecents.SelectedIndices; }
        }

        [Browsable(false)]
        public ListBox.SelectedObjectCollection SelectedItems
        {
            get { return this.listRecents.SelectedItems; }
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

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (this.Delete != null)
            {
                this.Delete(this, e);
            }
        }

        private void listRecents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listRecents.SelectionMode == SelectionMode.One && this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, e);
            }
        }
        #endregion
    }
}
