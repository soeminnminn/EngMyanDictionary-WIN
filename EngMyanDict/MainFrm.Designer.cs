namespace EngMyanDict
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.lblTitle = new System.Windows.Forms.ToolStripLabel();
            this.btnMoreMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.manageFavoriusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPicture = new System.Windows.Forms.ToolStripButton();
            this.btnAddToFavorites = new System.Windows.Forms.ToolStripButton();
            this.workerSearch = new System.ComponentModel.BackgroundWorker();
            this.workerDefinition = new System.ComponentModel.BackgroundWorker();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openNotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitNotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workerFavorited = new System.ComponentModel.BackgroundWorker();
            this.workerRecents = new System.ComponentModel.BackgroundWorker();
            this.workerLoadFavorites = new System.ComponentModel.BackgroundWorker();
            this.workerLoadRecents = new System.ComponentModel.BackgroundWorker();
            this.workerDeleteFavorites = new System.ComponentModel.BackgroundWorker();
            this.workerDeleteRecents = new System.ComponentModel.BackgroundWorker();
            this.pictureView = new EngMyanDict.Controls.PictureView();
            this.splitContainerMain = new DictControls.Controls.SplitView(this.components);
            this.recentsView = new EngMyanDict.Controls.RecentsView();
            this.favoritesView = new EngMyanDict.Controls.FavoritesView();
            this.panelSearchList = new System.Windows.Forms.Panel();
            this.txtSearch = new DictControls.Controls.SearchView();
            this.listResult = new DictControls.Controls.ListView(this.components);
            this.htmlDetails = new HtmlRenderer.HtmlPanel();
            this.toolStripMain.SuspendLayout();
            this.contextMenuNotify.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.panelSearchList.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.SystemColors.Highlight;
            this.toolStripMain.CanOverflow = false;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTitle,
            this.btnMoreMenu,
            this.btnPicture,
            this.btnAddToFavorites});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(16, 8, 8, 8);
            this.toolStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripMain.Size = new System.Drawing.Size(982, 64);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 45);
            this.lblTitle.Text = "Eng-Myan Dictionary";
            // 
            // btnMoreMenu
            // 
            this.btnMoreMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnMoreMenu.AutoSize = false;
            this.btnMoreMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoreMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageFavoriusToolStripMenuItem,
            this.recentsToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.btnMoreMenu.Image = global::EngMyanDict.Properties.Resources.more_vert;
            this.btnMoreMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoreMenu.Margin = new System.Windows.Forms.Padding(0);
            this.btnMoreMenu.Name = "btnMoreMenu";
            this.btnMoreMenu.ShowDropDownArrow = false;
            this.btnMoreMenu.Size = new System.Drawing.Size(48, 48);
            this.btnMoreMenu.Text = "More";
            // 
            // manageFavoriusToolStripMenuItem
            // 
            this.manageFavoriusToolStripMenuItem.Name = "manageFavoriusToolStripMenuItem";
            this.manageFavoriusToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.manageFavoriusToolStripMenuItem.Text = "Manage favorites";
            this.manageFavoriusToolStripMenuItem.Click += new System.EventHandler(this.manageFavoriusToolStripMenuItem_Click);
            // 
            // recentsToolStripMenuItem
            // 
            this.recentsToolStripMenuItem.Name = "recentsToolStripMenuItem";
            this.recentsToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.recentsToolStripMenuItem.Text = "Recents";
            this.recentsToolStripMenuItem.Click += new System.EventHandler(this.recentsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // btnPicture
            // 
            this.btnPicture.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPicture.AutoSize = false;
            this.btnPicture.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPicture.Image = global::EngMyanDict.Properties.Resources.photo;
            this.btnPicture.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPicture.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnPicture.Name = "btnPicture";
            this.btnPicture.Size = new System.Drawing.Size(48, 48);
            this.btnPicture.Text = "Picture";
            this.btnPicture.Click += new System.EventHandler(this.btnPicture_Click);
            // 
            // btnAddToFavorites
            // 
            this.btnAddToFavorites.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnAddToFavorites.AutoSize = false;
            this.btnAddToFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddToFavorites.Image = global::EngMyanDict.Properties.Resources.favorite_off;
            this.btnAddToFavorites.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddToFavorites.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnAddToFavorites.Name = "btnAddToFavorites";
            this.btnAddToFavorites.Size = new System.Drawing.Size(48, 48);
            this.btnAddToFavorites.Text = "Add to favorites";
            this.btnAddToFavorites.Click += new System.EventHandler(this.btnAddToFavorites_Click);
            // 
            // workerSearch
            // 
            this.workerSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerSearch_DoWork);
            this.workerSearch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerSearch_RunWorkerCompleted);
            // 
            // workerDefinition
            // 
            this.workerDefinition.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerDefinition_DoWork);
            this.workerDefinition.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerDefinition_RunWorkerCompleted);
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenuStrip = this.contextMenuNotify;
            this.notifyIconMain.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMain.Icon")));
            this.notifyIconMain.Text = "English-Myanmar Dictionary";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.DoubleClick += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // contextMenuNotify
            // 
            this.contextMenuNotify.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openNotifyToolStripMenuItem,
            this.exitNotifyToolStripMenuItem});
            this.contextMenuNotify.Name = "contextMenuNotify";
            this.contextMenuNotify.Size = new System.Drawing.Size(227, 52);
            // 
            // openNotifyToolStripMenuItem
            // 
            this.openNotifyToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.openNotifyToolStripMenuItem.Name = "openNotifyToolStripMenuItem";
            this.openNotifyToolStripMenuItem.Size = new System.Drawing.Size(226, 24);
            this.openNotifyToolStripMenuItem.Text = "Eng-Myan Dictionary";
            this.openNotifyToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitNotifyToolStripMenuItem
            // 
            this.exitNotifyToolStripMenuItem.Name = "exitNotifyToolStripMenuItem";
            this.exitNotifyToolStripMenuItem.Size = new System.Drawing.Size(226, 24);
            this.exitNotifyToolStripMenuItem.Text = "Exit";
            this.exitNotifyToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // workerFavorited
            // 
            this.workerFavorited.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerFavorited_DoWork);
            this.workerFavorited.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerFavorited_RunWorkerCompleted);
            // 
            // workerRecents
            // 
            this.workerRecents.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerRecents_DoWork);
            // 
            // workerLoadFavorites
            // 
            this.workerLoadFavorites.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerLoadFavorites_DoWork);
            this.workerLoadFavorites.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerLoadFavorites_RunWorkerCompleted);
            // 
            // workerLoadRecents
            // 
            this.workerLoadRecents.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerLoadRecents_DoWork);
            this.workerLoadRecents.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerLoadRecents_RunWorkerCompleted);
            // 
            // workerDeleteFavorites
            // 
            this.workerDeleteFavorites.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerDeleteFavorites_DoWork);
            this.workerDeleteFavorites.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerDeleteFavorites_RunWorkerCompleted);
            // 
            // workerDeleteRecents
            // 
            this.workerDeleteRecents.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerDeleteRecents_DoWork);
            this.workerDeleteRecents.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerDeleteRecents_RunWorkerCompleted);
            // 
            // pictureView
            // 
            this.pictureView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureView.AutoScroll = true;
            this.pictureView.BackColor = System.Drawing.Color.PaleGreen;
            this.pictureView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureView.Image = null;
            this.pictureView.Location = new System.Drawing.Point(478, 64);
            this.pictureView.Name = "pictureView";
            this.pictureView.Padding = new System.Windows.Forms.Padding(4);
            this.pictureView.Size = new System.Drawing.Size(500, 500);
            this.pictureView.TabIndex = 2;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 64);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.recentsView);
            this.splitContainerMain.Panel1.Controls.Add(this.favoritesView);
            this.splitContainerMain.Panel1.Controls.Add(this.panelSearchList);
            this.splitContainerMain.Panel1MinSize = 300;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.htmlDetails);
            this.splitContainerMain.Panel2.Padding = new System.Windows.Forms.Padding(4, 4, 8, 8);
            this.splitContainerMain.Size = new System.Drawing.Size(606, 496);
            this.splitContainerMain.SplitterDistance = 300;
            this.splitContainerMain.TabIndex = 0;
            // 
            // recentsView
            // 
            this.recentsView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.recentsView.Location = new System.Drawing.Point(6, 328);
            this.recentsView.Name = "recentsView";
            this.recentsView.Size = new System.Drawing.Size(288, 158);
            this.recentsView.TabIndex = 2;
            this.recentsView.SelectedIndexChanged += new EngMyanDict.Controls.RecentsView.SelectedIndexChangedEventHandler(this.recentsView_SelectedIndexChanged);
            this.recentsView.Delete += new EngMyanDict.Controls.RecentsView.DeleteEventHandler(this.recentsView_Delete);
            this.recentsView.Close += new EngMyanDict.Controls.RecentsView.CloseEventHandler(this.recentsView_Close);
            // 
            // favoritesView
            // 
            this.favoritesView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.favoritesView.Location = new System.Drawing.Point(5, 141);
            this.favoritesView.Name = "favoritesView";
            this.favoritesView.Size = new System.Drawing.Size(291, 178);
            this.favoritesView.TabIndex = 1;
            this.favoritesView.SelectedIndexChanged += new EngMyanDict.Controls.FavoritesView.SelectedIndexChangedEventHandler(this.favoritesView_SelectedIndexChanged);
            this.favoritesView.Delete += new EngMyanDict.Controls.FavoritesView.DeleteEventHandler(this.favoritesView_Delete);
            this.favoritesView.Close += new EngMyanDict.Controls.FavoritesView.CloseEventHandler(this.favoritesView_Close);
            // 
            // panelSearchList
            // 
            this.panelSearchList.Controls.Add(this.txtSearch);
            this.panelSearchList.Controls.Add(this.listResult);
            this.panelSearchList.Location = new System.Drawing.Point(0, 0);
            this.panelSearchList.Name = "panelSearchList";
            this.panelSearchList.Size = new System.Drawing.Size(298, 135);
            this.panelSearchList.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(5, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(289, 49);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Search += new DictControls.Controls.SearchView.SearchEventHandler(this.txtSearch_Search);
            // 
            // listResult
            // 
            this.listResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listResult.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listResult.FormattingEnabled = true;
            this.listResult.IntegralHeight = false;
            this.listResult.Location = new System.Drawing.Point(10, 59);
            this.listResult.Name = "listResult";
            this.listResult.Size = new System.Drawing.Size(280, 68);
            this.listResult.TabIndex = 1;
            this.listResult.SelectedIndexChanged += new System.EventHandler(this.listResult_SelectedIndexChanged);
            // 
            // htmlDetails
            // 
            this.htmlDetails.AutoScroll = true;
            this.htmlDetails.BackColor = System.Drawing.SystemColors.Window;
            this.htmlDetails.BaseStylesheet = null;
            this.htmlDetails.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.htmlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlDetails.Location = new System.Drawing.Point(4, 4);
            this.htmlDetails.Name = "htmlDetails";
            this.htmlDetails.Size = new System.Drawing.Size(290, 484);
            this.htmlDetails.TabIndex = 0;
            this.htmlDetails.Text = null;
            this.htmlDetails.UseGdiPlusTextRendering = true;
            this.htmlDetails.StylesheetLoad += new System.EventHandler<HtmlRenderer.Entities.HtmlStylesheetLoadEventArgs>(this.htmlDetails_StylesheetLoad);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 653);
            this.Controls.Add(this.pictureView);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "MainFrm";
            this.Text = "English-Myanmar Dictionary";
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.contextMenuNotify.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.panelSearchList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripLabel lblTitle;
        private System.Windows.Forms.ToolStripDropDownButton btnMoreMenu;
        private System.Windows.Forms.ToolStripMenuItem manageFavoriusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnPicture;
        private System.Windows.Forms.ToolStripButton btnAddToFavorites;
        private DictControls.Controls.SplitView splitContainerMain;
        private DictControls.Controls.SearchView txtSearch;
        private DictControls.Controls.ListView listResult;
        private HtmlRenderer.HtmlPanel htmlDetails;
        private System.ComponentModel.BackgroundWorker workerSearch;
        private System.ComponentModel.BackgroundWorker workerDefinition;
        private System.Windows.Forms.Panel panelSearchList;
        private Controls.FavoritesView favoritesView;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuNotify;
        private System.Windows.Forms.ToolStripMenuItem openNotifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitNotifyToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker workerFavorited;
        private System.ComponentModel.BackgroundWorker workerRecents;
        private System.ComponentModel.BackgroundWorker workerLoadFavorites;
        private System.ComponentModel.BackgroundWorker workerLoadRecents;
        private System.ComponentModel.BackgroundWorker workerDeleteFavorites;
        private System.ComponentModel.BackgroundWorker workerDeleteRecents;
        private Controls.RecentsView recentsView;
        private Controls.PictureView pictureView;
    }
}

