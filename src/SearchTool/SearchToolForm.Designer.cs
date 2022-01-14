namespace Gears
{
	partial class SearchToolForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchToolForm));
			this.chkCaseInsensitive = new System.Windows.Forms.CheckBox();
			this.lblSearchFor = new System.Windows.Forms.Label();
			this.lblPath = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.lblCurrentFile = new System.Windows.Forms.Label();
			this.chkSubDirectories = new System.Windows.Forms.CheckBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.clearSearchPathHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.comboSearchFor = new System.Windows.Forms.ComboBox();
			this.comboPath = new System.Windows.Forms.ComboBox();
			this.lblFileMask = new System.Windows.Forms.Label();
			this.txtFileMask = new System.Windows.Forms.TextBox();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkCaseInsensitive
			// 
			this.chkCaseInsensitive.AutoSize = true;
			this.chkCaseInsensitive.Checked = true;
			this.chkCaseInsensitive.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCaseInsensitive.Location = new System.Drawing.Point(105, 14);
			this.chkCaseInsensitive.Name = "chkCaseInsensitive";
			this.chkCaseInsensitive.Size = new System.Drawing.Size(109, 19);
			this.chkCaseInsensitive.TabIndex = 0;
			this.chkCaseInsensitive.Text = "Case Insensitive";
			this.chkCaseInsensitive.UseVisualStyleBackColor = true;
			// 
			// lblSearchFor
			// 
			this.lblSearchFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSearchFor.AutoSize = true;
			this.lblSearchFor.Location = new System.Drawing.Point(29, 52);
			this.lblSearchFor.Name = "lblSearchFor";
			this.lblSearchFor.Size = new System.Drawing.Size(62, 15);
			this.lblSearchFor.TabIndex = 2;
			this.lblSearchFor.Text = "Search For";
			// 
			// lblPath
			// 
			this.lblPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPath.AutoSize = true;
			this.lblPath.Location = new System.Drawing.Point(64, 82);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(31, 15);
			this.lblPath.TabIndex = 4;
			this.lblPath.Text = "Path";
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.BackColor = System.Drawing.Color.ForestGreen;
			this.btnSearch.ForeColor = System.Drawing.SystemColors.Control;
			this.btnSearch.Location = new System.Drawing.Point(588, 107);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(87, 27);
			this.btnSearch.TabIndex = 8;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = false;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// lblCurrentFile
			// 
			this.lblCurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lblCurrentFile.AutoEllipsis = true;
			this.lblCurrentFile.AutoSize = true;
			this.lblCurrentFile.Location = new System.Drawing.Point(14, 148);
			this.lblCurrentFile.Name = "lblCurrentFile";
			this.lblCurrentFile.Size = new System.Drawing.Size(20, 15);
			this.lblCurrentFile.TabIndex = 10;
			this.lblCurrentFile.Text = "lbl";
			// 
			// chkSubDirectories
			// 
			this.chkSubDirectories.AutoSize = true;
			this.chkSubDirectories.Checked = true;
			this.chkSubDirectories.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSubDirectories.Location = new System.Drawing.Point(220, 14);
			this.chkSubDirectories.Name = "chkSubDirectories";
			this.chkSubDirectories.Size = new System.Drawing.Size(152, 19);
			this.chkSubDirectories.TabIndex = 1;
			this.chkSubDirectories.Text = "Search In Subdirectories";
			this.chkSubDirectories.UseVisualStyleBackColor = true;
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStop.BackColor = System.Drawing.Color.Crimson;
			this.btnStop.Enabled = false;
			this.btnStop.ForeColor = System.Drawing.SystemColors.Control;
			this.btnStop.Location = new System.Drawing.Point(682, 107);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(87, 27);
			this.btnStop.TabIndex = 10;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.BackColor = System.Drawing.Color.White;
			this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
			this.treeView1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.treeView1.FullRowSelect = true;
			this.treeView1.Location = new System.Drawing.Point(14, 166);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(754, 383);
			this.treeView1.TabIndex = 11;
			this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
			this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseAllToolStripMenuItem,
            this.expandAllToolStripMenuItem,
            this.toolStripMenuItem1,
            this.clearSearchPathHistoryToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(181, 76);
			// 
			// collapseAllToolStripMenuItem
			// 
			this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
			this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.collapseAllToolStripMenuItem.Text = "Collapse Results";
			this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
			// 
			// expandAllToolStripMenuItem
			// 
			this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
			this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.expandAllToolStripMenuItem.Text = "Expand Results";
			this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
			// 
			// clearSearchPathHistoryToolStripMenuItem
			// 
			this.clearSearchPathHistoryToolStripMenuItem.Name = "clearSearchPathHistoryToolStripMenuItem";
			this.clearSearchPathHistoryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.clearSearchPathHistoryToolStripMenuItem.Text = "Clear Search History";
			this.clearSearchPathHistoryToolStripMenuItem.Click += new System.EventHandler(this.clearSearchHistoryToolStripMenuItem_Click);
			// 
			// comboSearchFor
			// 
			this.comboSearchFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboSearchFor.FormattingEnabled = true;
			this.comboSearchFor.Location = new System.Drawing.Point(105, 48);
			this.comboSearchFor.Name = "comboSearchFor";
			this.comboSearchFor.Size = new System.Drawing.Size(663, 23);
			this.comboSearchFor.TabIndex = 12;
			// 
			// comboPath
			// 
			this.comboPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.comboPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
			this.comboPath.FormattingEnabled = true;
			this.comboPath.Location = new System.Drawing.Point(105, 78);
			this.comboPath.Name = "comboPath";
			this.comboPath.Size = new System.Drawing.Size(663, 23);
			this.comboPath.TabIndex = 13;
			// 
			// lblFileMask
			// 
			this.lblFileMask.AutoSize = true;
			this.lblFileMask.Location = new System.Drawing.Point(37, 115);
			this.lblFileMask.Name = "lblFileMask";
			this.lblFileMask.Size = new System.Drawing.Size(56, 15);
			this.lblFileMask.TabIndex = 14;
			this.lblFileMask.Text = "File Mask";
			// 
			// txtFileMask
			// 
			this.txtFileMask.Location = new System.Drawing.Point(105, 110);
			this.txtFileMask.Name = "txtFileMask";
			this.txtFileMask.Size = new System.Drawing.Size(201, 23);
			this.txtFileMask.TabIndex = 15;
			this.txtFileMask.Text = "*.*";
			// 
			// SearchToolForm
			// 
			this.AcceptButton = this.btnSearch;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.txtFileMask);
			this.Controls.Add(this.lblFileMask);
			this.Controls.Add(this.lblCurrentFile);
			this.Controls.Add(this.comboPath);
			this.Controls.Add(this.comboSearchFor);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.chkSubDirectories);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.lblPath);
			this.Controls.Add(this.lblSearchFor);
			this.Controls.Add(this.chkCaseInsensitive);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(697, 456);
			this.Name = "SearchToolForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Search Tool";
			this.Load += new System.EventHandler(this.SearchToolUI_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkCaseInsensitive;
		private System.Windows.Forms.Label lblSearchFor;
		private System.Windows.Forms.Label lblPath;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Label lblCurrentFile;
		private System.Windows.Forms.CheckBox chkSubDirectories;
		private System.Windows.Forms.Button btnStop;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
		private System.Windows.Forms.ComboBox comboSearchFor;
		private System.Windows.Forms.ComboBox comboPath;
		private System.Windows.Forms.Label lblFileMask;
		private System.Windows.Forms.TextBox txtFileMask;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem clearSearchPathHistoryToolStripMenuItem;
	}
}