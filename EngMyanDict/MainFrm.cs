using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EngMyanDict
{
    public partial class MainFrm : Form
    {
        #region Variables
        private static string APP_NAME = "Eng-Myan Dictionary";
        private bool mIsClosed = false;
        private DictionaryDataAccess mDictionary = null;
        private UserDataAccess mUserData = null;
        private PictureLoader mPictureLoader = null;
        private int mSelectedIndex = 0;
        #endregion

        #region Constructor
        public MainFrm()
        {
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            Point location = Properties.Settings.Default.WindowLocation;
            if (!Point.Empty.Equals(location))
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = location;
            }
            Size size = Properties.Settings.Default.WindowSize;
            if (!Size.Empty.Equals(size))
            {
                this.Size = size;
            }

            this.toolStripMain.BackColor = Color.FromArgb(96, 99, 143);

            this.splitContainerMain.Dock = DockStyle.Fill;
            this.panelSearchList.Dock = DockStyle.Fill;
            this.favoritesView.Dock = DockStyle.Fill;
            this.favoritesView.Visible = false;
            this.recentsView.Dock = DockStyle.Fill;
            this.recentsView.Visible = false;

            this.pictureView.Top = this.toolStripMain.Height;
            this.pictureView.Visible = false;

            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            FileInfo sourceFile = new FileInfo(Path.Combine(appPath, "Data\\EMDictionary.db"));
            if (sourceFile.Exists)
            {
                this.mDictionary = new DictionaryDataAccess(sourceFile.FullName);
            }

            string appDataPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APP_NAME);
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            this.mUserData = new UserDataAccess(Path.Combine(appDataPath, "UserData.db"));
            if (this.mUserData.CreateTables())
            {
                this.mUserData.CreateTriggers();
            }

            this.mPictureLoader = new PictureLoader(appPath);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (this.mPictureLoader != null)
            {
                this.mPictureLoader.Dispose();
            }

            base.Dispose(disposing);
        }

        private void EnabledControls(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(this.EnabledControls), enabled);
                return;
            }

            this.txtSearch.Enabled = enabled;
        }

        private void ChangeFavorited(bool isFavorited)
        {
            if (isFavorited)
            {
                this.btnAddToFavorites.Text = "Remove from Favorite";
                this.btnAddToFavorites.Image = Properties.Resources.favorite_on;
                this.btnAddToFavorites.Tag = Boolean.FalseString;
            }
            else
            {
                this.btnAddToFavorites.Text = "Add to Favorite";
                this.btnAddToFavorites.Image = Properties.Resources.favorite_off;
                this.btnAddToFavorites.Tag = Boolean.TrueString;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string searchText = Properties.Settings.Default.SearchText;
            if (!string.IsNullOrEmpty(searchText))
            {
                this.txtSearch.Text = searchText;
                this.mSelectedIndex = Properties.Settings.Default.SelectedIndex;
                if (this.mSelectedIndex < 0)
                    this.mSelectedIndex = 0;
                this.txtSearch.PerformSearch();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!this.mIsClosed)
            {
                this.Hide();
                e.Cancel = true;
            }
            else
            {
                Properties.Settings.Default.SearchText = this.txtSearch.Text;
                Properties.Settings.Default.SelectedIndex = this.listResult.SelectedIndex;
                if (this.WindowState == FormWindowState.Normal)
                {
                    Properties.Settings.Default.WindowLocation = this.Location;
                    Properties.Settings.Default.WindowSize = this.Size;
                }
                Properties.Settings.Default.Save();
            }
            

            base.OnClosing(e);
        }
        #endregion

        #region Events
        private void workerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            string searchText = e.Argument as string;
            List<DictionaryItem> table = this.mDictionary.QueryWord(searchText);
            if (table != null)
            {
                e.Result = table;
            }
        }

        private void workerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.listResult.Items.Clear();
            if (e.Result != null)
            {
                List<DictionaryItem> result = (List<DictionaryItem>)e.Result;
                if (result != null && result.Count > 0)
                {
                    this.listResult.Items.AddRange(result.ToArray());
                    if (this.mSelectedIndex >= result.Count)
                        this.mSelectedIndex = 0;
                    this.listResult.SetSelected(this.mSelectedIndex, true);
                }
                else
                {
                    this.listResult.Items.Clear();
                }
                
            }
            this.EnabledControls(true);
        }

        private void workerDefinition_DoWork(object sender, DoWorkEventArgs e)
        {
            DictionaryItem item = e.Argument as DictionaryItem;
            if (this.mDictionary != null)
            {
                List<DictionaryItem> table = this.mDictionary.QueryDefinition(item.Id);
                if (table != null && table.Count > 0)
                {
                    DictionaryItem result = table[0];
                    Definition definition = new Definition(result);
                    if (this.mUserData != null)
                    {
                        definition.IsFavorited = this.mUserData.IsFavorited(item.Id);
                    }

                    if (this.mPictureLoader != null)
                    {
                        definition.Image = this.mPictureLoader.LoadPicture(result);
                    }

                    e.Result = definition;
                }
            }
        }

        private void workerDefinition_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Definition result = e.Result as Definition;
                if (result != null)
                {
                    string html = result.Html;
                    if (!string.IsNullOrEmpty(html))
                    {
                        this.htmlDetails.Text = html;
                    }
                    if (result.Image != null && result.Item.Picture)
                    {
                        this.pictureView.Image = result.Image;
                        this.btnPicture.Visible = true;
                    }
                    else
                    {
                        this.btnPicture.Visible = false;
                    }
                    
                    this.ChangeFavorited(result.IsFavorited);

                    if (!this.workerRecents.IsBusy)
                        this.workerRecents.RunWorkerAsync(result.Item);
                }
            }
        }

        private void workerFavorited_DoWork(object sender, DoWorkEventArgs e)
        {
            Pair<DictionaryItem, bool> args = e.Argument as Pair<DictionaryItem, bool>;
            if (args != null && this.mUserData != null)
            {
                int result = 0;
                if (args.Second)
                    result = this.mUserData.CreateFavorite(args.First.Word, args.First.Id);
                else
                    result = this.mUserData.RemoveFavoriteByRef(args.First.Id);

                if (result != 0)
                    e.Result = Pair<int, bool>.Create(result, args.Second);
            }
        }

        private void workerFavorited_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Pair<int, bool> result = e.Result as Pair<int, bool>;
                if (result != null && result.First > 0)
                {
                    this.ChangeFavorited(result.Second);
                }
            }
        }

        private void workerRecents_DoWork(object sender, DoWorkEventArgs e)
        {
            DictionaryItem item = e.Argument as DictionaryItem;
            if (this.mUserData != null && item != null)
            {
                this.mUserData.CreateHistory(item.Word, item.Id);
            }
        }

        private void workerLoadFavorites_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.mUserData != null)
            {
                e.Result = this.mUserData.GetAllFavorites();
            }
        }

        private void workerLoadFavorites_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                this.favoritesView.Items.Clear();
                List<DictionaryItem> items = e.Result as List<DictionaryItem>;
                if (items != null)
                {
                    this.favoritesView.Items.AddRange(items.ToArray());
                }
            }
        }

        private void workerLoadRecents_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.mUserData != null)
            {
                e.Result = this.mUserData.GetAllHistories();
            }
        }

        private void workerLoadRecents_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                this.recentsView.Items.Clear();
                List<DictionaryItem> items = e.Result as List<DictionaryItem>;
                if (items != null)
                {
                    this.recentsView.Items.AddRange(items.ToArray());
                }
            }
        }

        private void workerDeleteFavorites_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null && this.mUserData != null)
            {
                long[] ids = (long[])e.Argument;
                e.Result = this.mUserData.RemoveFavoriteByRef(ids);
            }
        }

        private void workerDeleteFavorites_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null && !this.workerLoadFavorites.IsBusy)
            {
                this.workerLoadFavorites.RunWorkerAsync();
            }
        }

        private void workerDeleteRecents_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.mUserData != null)
            {
                e.Result = this.mUserData.RemoveAllHistory();
            }
        }

        private void workerDeleteRecents_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null && !this.workerLoadRecents.IsBusy)
            {
                this.recentsView.Items.Clear();
                this.workerLoadRecents.RunWorkerAsync();
            }
        }

        private void txtSearch_Search(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtSearch.Text) && this.mDictionary != null && !this.workerSearch.IsBusy)
            {
                string searchText = this.txtSearch.Text;
                this.EnabledControls(false);
                this.workerSearch.RunWorkerAsync(searchText);
                //this.workerSearch_DoWork(this, new DoWorkEventArgs(searchText));
            }
        }

        private void listResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            DictionaryItem item = this.listResult.SelectedItem as DictionaryItem;
            if (item != null && !this.workerDefinition.IsBusy)
            {
                this.workerDefinition.RunWorkerAsync(item);
                //this.workerDefinition_DoWork(this, new DoWorkEventArgs(item));
            }
        }

        private void htmlDetails_StylesheetLoad(object sender, HtmlRenderer.Entities.HtmlStylesheetLoadEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("body,div,h1,h2,h3,input,textarea { margin: 0px; padding: 0px;  line-height:24px; -webkit-font-smoothing: antialiased!important; text-rendering: optimizeLegibility; }");
            builder.Append("p { margin:0pt; font-size:12.0pt; }");
            builder.Append("p.desc , p.synonym { margin-left:10pt; }");
            builder.Append("h2 { color: red; font-weight: bold; font-size: 16pt; }");
            builder.Append("h3 { color: #8080C0; font-weight: bold; font-size: 12pt; }");
            builder.Append("a { color: #000; text-decoration: none; border-bottom: 1px dotted #808080; }");

            e.SetStyleSheet = builder.ToString();
        }
        #endregion

        #region Menu Items Events
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mIsClosed = true;
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutFrm form = new AboutFrm();
            form.ShowDialog(this);
        }

        private void recentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.workerLoadRecents.IsBusy)
            {
                this.workerLoadRecents.RunWorkerAsync();
            }
            this.panelSearchList.Visible = false;
            this.recentsView.Visible = true;

            this.manageFavoriusToolStripMenuItem.Enabled = false;
            this.recentsToolStripMenuItem.Enabled = false;
        }

        private void manageFavoriusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.workerLoadFavorites.IsBusy)
            {
                this.workerLoadFavorites.RunWorkerAsync();
            }
            this.panelSearchList.Visible = false;
            this.favoritesView.Visible = true;

            this.manageFavoriusToolStripMenuItem.Enabled = false;
            this.recentsToolStripMenuItem.Enabled = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void btnAddToFavorites_Click(object sender, EventArgs e)
        {
            DictionaryItem item = this.listResult.SelectedItem as DictionaryItem;
            if (item != null && !this.workerFavorited.IsBusy)
            {
                bool add = (this.btnAddToFavorites.Tag as string) == Boolean.TrueString;
                Pair<DictionaryItem, bool> args = Pair<DictionaryItem, bool>.Create(item, add);
                this.workerFavorited.RunWorkerAsync(args);
                //this.workerFavorited_DoWork(this, new DoWorkEventArgs(args));
            }
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            this.pictureView.Visible = !this.pictureView.Visible;
        }

        private void favoritesView_Close(object sender, EventArgs e)
        {
            this.favoritesView.Visible = false;
            this.panelSearchList.Visible = true;
            this.manageFavoriusToolStripMenuItem.Enabled = true;
            this.recentsToolStripMenuItem.Enabled = true;

            if (this.listResult.SelectedIndex > -1)
                this.listResult.SetSelected(this.listResult.SelectedIndex, true);
        }

        private void favoritesView_Delete(object sender, EventArgs e)
        {
            if (this.favoritesView.SelectedItems != null && !this.workerDeleteFavorites.IsBusy)
            {
                int length = this.favoritesView.SelectedItems.Count;
                long[] ids = new long[length];
                for(int i = 0; i < length; i++)
                {
                    DictionaryItem item = this.favoritesView.SelectedItems[i] as DictionaryItem;
                    ids[i] = item.RefrenceId;
                }
                this.workerDeleteFavorites.RunWorkerAsync(ids);
            }
        }

        private void favoritesView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.favoritesView.SelectedItem != null)
            {
                DictionaryItem selected = this.favoritesView.SelectedItem as DictionaryItem;
                if (selected != null && !this.workerDefinition.IsBusy)
                {
                    DictionaryItem item = new DictionaryItem();
                    item.Id = selected.RefrenceId;
                    item.Word = selected.Word;
                    item.RefrenceId = selected.Id;
                    this.workerDefinition.RunWorkerAsync(item);
                }
            }
        }

        private void recentsView_Close(object sender, EventArgs e)
        {
            this.recentsView.Visible = false;
            this.panelSearchList.Visible = true;
            this.manageFavoriusToolStripMenuItem.Enabled = true;
            this.recentsToolStripMenuItem.Enabled = true;

            if (this.listResult.SelectedIndex > -1)
                this.listResult.SetSelected(this.listResult.SelectedIndex, true);
        }

        private void recentsView_Delete(object sender, EventArgs e)
        {
            if (!this.workerDeleteRecents.IsBusy)
            {
                this.workerDeleteRecents.RunWorkerAsync();
            }
        }

        private void recentsView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.recentsView.SelectedItem != null)
            {
                DictionaryItem selected = this.recentsView.SelectedItem as DictionaryItem;
                if (selected != null && !this.workerDefinition.IsBusy)
                {
                    DictionaryItem item = new DictionaryItem();
                    item.Id = selected.RefrenceId;
                    item.Word = selected.Word;
                    item.RefrenceId = selected.Id;
                    this.workerDefinition.RunWorkerAsync(item);
                }
            }
        }
        #endregion
    }
}
