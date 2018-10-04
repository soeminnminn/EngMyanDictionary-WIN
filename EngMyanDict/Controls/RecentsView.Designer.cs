namespace EngMyanDict.Controls
{
    partial class RecentsView
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
            this.toolStripRecents = new System.Windows.Forms.ToolStrip();
            this.btnClearAll = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblTitle = new System.Windows.Forms.ToolStripLabel();
            this.listRecents = new DictControls.Controls.ListView(this.components);
            this.toolStripRecents.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripRecents
            // 
            this.toolStripRecents.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripRecents.CanOverflow = false;
            this.toolStripRecents.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripRecents.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripRecents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClearAll,
            this.btnClose,
            this.toolStripSeparator1,
            this.lblTitle});
            this.toolStripRecents.Location = new System.Drawing.Point(0, 0);
            this.toolStripRecents.Name = "toolStripRecents";
            this.toolStripRecents.Padding = new System.Windows.Forms.Padding(8);
            this.toolStripRecents.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripRecents.Size = new System.Drawing.Size(394, 64);
            this.toolStripRecents.TabIndex = 0;
            this.toolStripRecents.Text = "Recents";
            // 
            // btnClearAll
            // 
            this.btnClearAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnClearAll.AutoSize = false;
            this.btnClearAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearAll.Image = global::EngMyanDict.Properties.Resources.delete;
            this.btnClearAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearAll.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(48, 48);
            this.btnClearAll.Text = "Clear Recents";
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
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
            this.lblTitle.Size = new System.Drawing.Size(71, 45);
            this.lblTitle.Text = "Recents";
            // 
            // listRecents
            // 
            this.listRecents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listRecents.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listRecents.FormattingEnabled = true;
            this.listRecents.IntegralHeight = false;
            this.listRecents.Location = new System.Drawing.Point(10, 74);
            this.listRecents.Margin = new System.Windows.Forms.Padding(8);
            this.listRecents.Name = "listRecents";
            this.listRecents.Size = new System.Drawing.Size(376, 146);
            this.listRecents.TabIndex = 1;
            this.listRecents.SelectedIndexChanged += new System.EventHandler(this.listRecents_SelectedIndexChanged);
            // 
            // RecentsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listRecents);
            this.Controls.Add(this.toolStripRecents);
            this.Name = "RecentsView";
            this.Size = new System.Drawing.Size(394, 230);
            this.toolStripRecents.ResumeLayout(false);
            this.toolStripRecents.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripRecents;
        private System.Windows.Forms.ToolStripButton btnClearAll;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblTitle;
        private DictControls.Controls.ListView listRecents;
    }
}
