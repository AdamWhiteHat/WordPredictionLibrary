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
			this.label2 = new System.Windows.Forms.Label();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.treeView1 = new System.Windows.Forms.TreeView();
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
			this.btnOpen.TabIndex = 1;
			this.btnOpen.Text = "Open dictionary...";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.AutoScroll = true;
			this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel.Controls.Add(this.btnOpen);
			this.flowLayoutPanel.Controls.Add(this.btnTrain);
			this.flowLayoutPanel.Controls.Add(this.labelTotalWords);
			this.flowLayoutPanel.Controls.Add(this.label2);
			this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Padding = new System.Windows.Forms.Padding(1);
			this.flowLayoutPanel.Size = new System.Drawing.Size(142, 283);
			this.flowLayoutPanel.TabIndex = 2;
			// 
			// btnTrain
			// 
			this.btnTrain.AutoSize = true;
			this.btnTrain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnTrain.Location = new System.Drawing.Point(4, 33);
			this.btnTrain.Name = "btnTrain";
			this.btnTrain.Size = new System.Drawing.Size(85, 23);
			this.btnTrain.TabIndex = 2;
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
			this.labelTotalWords.TabIndex = 3;
			this.labelTotalWords.Text = "{0} Total Words";
			this.labelTotalWords.Visible = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 81);
			this.label2.Margin = new System.Windows.Forms.Padding(3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "{0} Unique Words";
			this.label2.Visible = false;
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
			this.treeView1.TabIndex = 5;
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
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TreeView treeView1;
	}
}

