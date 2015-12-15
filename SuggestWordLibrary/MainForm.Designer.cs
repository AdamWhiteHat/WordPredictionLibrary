namespace SuggestWordLibrary
{
	partial class MainForm
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
			this.btnOpen = new System.Windows.Forms.Button();
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.btnTrain = new System.Windows.Forms.Button();
			this.labelTotalWords = new System.Windows.Forms.Label();
			this.labelUniqueWords = new System.Windows.Forms.Label();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.btnNew = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.flowLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOpen
			// 
			this.btnOpen.AutoSize = true;
			this.btnOpen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnOpen.Location = new System.Drawing.Point(4, 4);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(100, 23);
			this.btnOpen.TabIndex = 3;
			this.btnOpen.Text = "Open dictionary...";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.AutoScroll = true;
			this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel.Controls.Add(this.btnNew);
			this.flowLayoutPanel.Controls.Add(this.btnOpen);
			this.flowLayoutPanel.Controls.Add(this.btnSave);
			this.flowLayoutPanel.Controls.Add(this.btnTrain);
			this.flowLayoutPanel.Controls.Add(this.labelTotalWords);
			this.flowLayoutPanel.Controls.Add(this.labelUniqueWords);	
			this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Padding = new System.Windows.Forms.Padding(1);
			this.flowLayoutPanel.Size = new System.Drawing.Size(142, 283);
			this.flowLayoutPanel.TabIndex = 0;
			// 
			// btnTrain
			// 
			this.btnTrain.AutoSize = true;
			this.btnTrain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnTrain.Location = new System.Drawing.Point(4, 33);
			this.btnTrain.Name = "btnTrain";
			this.btnTrain.Size = new System.Drawing.Size(85, 23);
			this.btnTrain.TabIndex = 4;
			this.btnTrain.Text = "Train on text...";
			this.btnTrain.UseVisualStyleBackColor = true;
			this.btnTrain.Visible = false;
			this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
			// 
			// labelTotalWords
			// 
			this.labelTotalWords.AutoSize = true;
			this.labelTotalWords.Location = new System.Drawing.Point(4, 62);
			this.labelTotalWords.Margin = new System.Windows.Forms.Padding(3);
			this.labelTotalWords.Name = "labelTotalWords";
			this.labelTotalWords.Size = new System.Drawing.Size(82, 13);
			this.labelTotalWords.TabIndex = 5;
			this.labelTotalWords.Text = "{0} Total Words";
			this.labelTotalWords.Visible = false;
			// 
			// labelUniqueWords
			// 
			this.labelUniqueWords.AutoSize = true;
			this.labelUniqueWords.Location = new System.Drawing.Point(4, 81);
			this.labelUniqueWords.Margin = new System.Windows.Forms.Padding(3);
			this.labelUniqueWords.Name = "labelUniqueWords";
			this.labelUniqueWords.Size = new System.Drawing.Size(92, 13);
			this.labelUniqueWords.TabIndex = 6;
			this.labelUniqueWords.Text = "{0} Unique Words";
			this.labelUniqueWords.Visible = false;
			// 
			// openFileDialog
			// 
			this.openFileDialog.AddExtension = false;
			this.openFileDialog.CheckFileExists = false;
			this.openFileDialog.Title = "Open suggestion dictionary...";
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Right;
			this.treeView1.Location = new System.Drawing.Point(148, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(305, 283);
			this.treeView1.TabIndex = 7;
			// 
			// btnNew
			// 
			this.btnNew.AutoSize = true;
			this.btnNew.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnNew.Location = new System.Drawing.Point(4, 100);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(96, 23);
			this.btnNew.TabIndex = 1;
			this.btnNew.Text = "New dictionary...";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnSave
			// 
			this.btnSave.AutoSize = true;
			this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnSave.Location = new System.Drawing.Point(4, 129);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(99, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save dictionary...";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(453, 283);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.flowLayoutPanel);
			this.Name = "MainForm";
			this.Text = "Train Suggestion Dictionary";
			this.flowLayoutPanel.ResumeLayout(false);
			this.flowLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button btnTrain;
		private System.Windows.Forms.Label labelTotalWords;
		private System.Windows.Forms.Label labelUniqueWords;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
	}
}

