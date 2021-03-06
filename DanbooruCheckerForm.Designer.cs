﻿namespace Danbooru_Checker
{
    partial class DanbooruCheckerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dialogFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.labelDirectory = new System.Windows.Forms.Label();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.buttonAuthenticate = new System.Windows.Forms.Button();
            this.dataImage = new System.Windows.Forms.DataGridView();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.labelOutput = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataImage)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dialogFolderBrowser
            // 
            this.dialogFolderBrowser.ShowNewFolderButton = false;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(12, 12);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // labelDirectory
            // 
            this.labelDirectory.AutoSize = true;
            this.labelDirectory.Location = new System.Drawing.Point(93, 17);
            this.labelDirectory.Name = "labelDirectory";
            this.labelDirectory.Size = new System.Drawing.Size(0, 13);
            this.labelDirectory.TabIndex = 3;
            // 
            // buttonCheck
            // 
            this.buttonCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCheck.Location = new System.Drawing.Point(347, 363);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(75, 23);
            this.buttonCheck.TabIndex = 4;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // buttonAuthenticate
            // 
            this.buttonAuthenticate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAuthenticate.Location = new System.Drawing.Point(12, 363);
            this.buttonAuthenticate.Name = "buttonAuthenticate";
            this.buttonAuthenticate.Size = new System.Drawing.Size(75, 23);
            this.buttonAuthenticate.TabIndex = 5;
            this.buttonAuthenticate.Text = "Authenticate";
            this.buttonAuthenticate.UseVisualStyleBackColor = true;
            this.buttonAuthenticate.Click += new System.EventHandler(this.buttonAuthenticate_Click);
            // 
            // dataImage
            // 
            this.dataImage.AllowUserToAddRows = false;
            this.dataImage.AllowUserToDeleteRows = false;
            this.dataImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataImage.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataImage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataImage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.File,
            this.URL});
            this.dataImage.Location = new System.Drawing.Point(12, 41);
            this.dataImage.MultiSelect = false;
            this.dataImage.Name = "dataImage";
            this.dataImage.ReadOnly = true;
            this.dataImage.RowHeadersVisible = false;
            this.dataImage.RowTemplate.ReadOnly = true;
            this.dataImage.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataImage.Size = new System.Drawing.Size(410, 316);
            this.dataImage.TabIndex = 6;
            this.dataImage.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataImage_CellContentDoubleClick);
            // 
            // File
            // 
            this.File.HeaderText = "File";
            this.File.Name = "File";
            this.File.ReadOnly = true;
            // 
            // URL
            // 
            this.URL.HeaderText = "URL";
            this.URL.Name = "URL";
            this.URL.ReadOnly = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelOutput});
            this.statusStrip1.Location = new System.Drawing.Point(0, 389);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(434, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // labelOutput
            // 
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(0, 17);
            // 
            // DanbooruCheckerForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(434, 411);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataImage);
            this.Controls.Add(this.buttonAuthenticate);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.labelDirectory);
            this.Controls.Add(this.buttonOpen);
            this.MinimumSize = new System.Drawing.Size(350, 350);
            this.Name = "DanbooruCheckerForm";
            this.ShowIcon = false;
            this.Text = "Danbooru Checker";
            ((System.ComponentModel.ISupportInitialize)(this.dataImage)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog dialogFolderBrowser;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Label labelDirectory;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.Button buttonAuthenticate;
        private System.Windows.Forms.DataGridView dataImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn URL;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel labelOutput;
    }
}

