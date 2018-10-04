namespace EngMyanDict.Controls
{
    partial class FavoritesView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStripFavorites = new System.Windows.Forms.ToolStrip();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblTitle = new System.Windows.Forms.ToolStripLabel();
            this.toolStripEdit = new System.Windows.Forms.ToolStrip();
            this.btnDone = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblEditTitle = new System.Windows.Forms.ToolStripLabel();
            this.btnSelectAll = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.listFavorites = new DictControls.Controls.ListView(this.components);
            this.toolStripFavorites.SuspendLayout();
            this.toolStripEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripFavorites
            // 
            this.toolStripFavorites.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripFavorites.CanOverflow = false;
            this.toolStripFavorites.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripFavorites.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripFavorites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEdit,
            this.btnClose,
            this.toolStripSeparator1,
            this.lblTitle});
            this.toolStripFavorites.Location = new System.Drawing.Point(0, 0);
            this.toolStripFavorites.Name = "toolStripFavorites";
            this.toolStripFavorites.Padding = new System.Windows.Forms.Padding(8);
            this.toolStripFavorites.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripFavorites.Size = new System.Drawing.Size(374, 64);
            this.toolStripFavorites.TabIndex = 2;
            this.toolStripFavorites.Text = "Favorites";
            // 
            // btnEdit
            // 
            this.btnEdit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnEdit.AutoSize = false;
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdit.Image = global::EngMyanDict.Properties.Resources.mode_edit;
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(48, 48);
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = false;
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClose.Image = global::EngMyanDict.Properties.Resources.clear;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(48, 48);
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 48);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(81, 45);
            this.lblTitle.Text = "Favorites";
            // 
            // toolStripEdit
            // 
            this.toolStripEdit.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripEdit.CanOverflow = false;
            this.toolStripEdit.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEdit.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDone,
            this.toolStripSeparator2,
            this.lblEditTitle,
            this.btnSelectAll,
            this.btnDelete});
            this.toolStripEdit.Location = new System.Drawing.Point(0, 64);
            this.toolStripEdit.Name = "toolStripEdit";
            this.toolStripEdit.Padding = new System.Windows.Forms.Padding(8);
            this.toolStripEdit.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripEdit.Size = new System.Drawing.Size(374, 64);
            this.toolStripEdit.TabIndex = 3;
            this.toolStripEdit.Text = "Edit Favorites";
            // 
            // btnDone
            // 
            this.btnDone.AutoSize = false;
            this.btnDone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDone.Image = global::EngMyanDict.Properties.Resources.done;
            this.btnDone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDone.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(48, 48);
            this.btnDone.Text = "Done";
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 48);
            // 
            // lblEditTitle
            // 
            this.lblEditTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEditTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblEditTitle.Name = "lblEditTitle";
            this.lblEditTitle.Size = new System.Drawing.Size(118, 45);
            this.lblEditTitle.Text = "Edit Favorites";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnSelectAll.AutoSize = false;
            this.btnSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelectAll.Image = global::EngMyanDict.Properties.Resources.select_all;
            this.btnSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(48, 48);
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnDelete.AutoSize = false;
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = global::EngMyanDict.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(48, 48);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // listFavorites
            // 
            this.listFavorites.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listFavorites.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listFavorites.FormattingEnabled = true;
            this.listFavorites.IntegralHeight = false;
            this.listFavorites.Location = new System.Drawing.Point(8, 136);
            this.listFavorites.Margin = new System.Windows.Forms.Padding(8);
            this.listFavorites.Name = "listFavorites";
            this.listFavorites.Size = new System.Drawing.Size(356, 146);
            this.listFavorites.TabIndex = 4;
            this.listFavorites.SelectedIndexChanged += new System.EventHandler(this.listFavorites_SelectedIndexChanged);
            // 
            // FavoritesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripEdit);
            this.Controls.Add(this.toolStripFavorites);
            this.Controls.Add(this.listFavorites);
            this.Name = "FavoritesView";
            this.Size = new System.Drawing.Size(374, 294);
            this.toolStripFavorites.ResumeLayout(false);
            this.toolStripFavorites.PerformLayout();
            this.toolStripEdit.ResumeLayout(false);
            this.toolStripEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripFavorites;
        private System.Windows.Forms.ToolStripLabel lblTitle;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolStripEdit;
        private System.Windows.Forms.ToolStripButton btnDone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblEditTitle;
        private System.Windows.Forms.ToolStripButton btnSelectAll;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private DictControls.Controls.ListView listFavorites;
    }
}
