﻿namespace SuggestWordLibrary
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
			this.btnNew = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnTrain = new System.Windows.Forms.Button();
			this.labelTotalWords = new System.Windows.Forms.Label();
			this.labelUniqueWords = new System.Windows.Forms.Label();
			this.btnVisualizeDict = new System.Windows.Forms.Button();
			this.btnDumpAll = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tbDepth = new System.Windows.Forms.TextBox();
			this.tbBreath = new System.Windows.Forms.TextBox();
			this.btnViewGraph = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.btnSuggest = new System.Windows.Forms.Button();
			this.btnForensics = new System.Windows.Forms.Button();
			this.btnViewSelectedWordGraph = new System.Windows.Forms.Button();
			this.btnPopulateList = new System.Windows.Forms.Button();
			this.listWords = new System.Windows.Forms.ListBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.cbShowSubsequentWords = new System.Windows.Forms.CheckBox();
			this.cbShowPriorWords = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOpen
			// 
			this.btnOpen.AutoSize = true;
			this.btnOpen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnOpen.Location = new System.Drawing.Point(4, 33);
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
			this.flowLayoutPanel.Controls.Add(this.btnVisualizeDict);
			this.flowLayoutPanel.Controls.Add(this.btnDumpAll);
			this.flowLayoutPanel.Controls.Add(this.panel2);
			this.flowLayoutPanel.Controls.Add(this.panel1);
			this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Padding = new System.Windows.Forms.Padding(1);
			this.flowLayoutPanel.Size = new System.Drawing.Size(142, 351);
			this.flowLayoutPanel.TabIndex = 0;
			// 
			// btnNew
			// 
			this.btnNew.AutoSize = true;
			this.btnNew.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnNew.Location = new System.Drawing.Point(4, 4);
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
			this.btnSave.Location = new System.Drawing.Point(4, 62);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(99, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save dictionary...";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnTrain
			// 
			this.btnTrain.AutoSize = true;
			this.btnTrain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnTrain.Location = new System.Drawing.Point(4, 91);
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
			this.labelTotalWords.Location = new System.Drawing.Point(4, 120);
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
			this.labelUniqueWords.Location = new System.Drawing.Point(4, 139);
			this.labelUniqueWords.Margin = new System.Windows.Forms.Padding(3);
			this.labelUniqueWords.Name = "labelUniqueWords";
			this.labelUniqueWords.Size = new System.Drawing.Size(92, 13);
			this.labelUniqueWords.TabIndex = 6;
			this.labelUniqueWords.Text = "{0} Unique Words";
			this.labelUniqueWords.Visible = false;
			// 
			// btnVisualizeDict
			// 
			this.btnVisualizeDict.Location = new System.Drawing.Point(4, 158);
			this.btnVisualizeDict.Name = "btnVisualizeDict";
			this.btnVisualizeDict.Size = new System.Drawing.Size(121, 23);
			this.btnVisualizeDict.TabIndex = 6;
			this.btnVisualizeDict.Text = "Visualize dictionary...";
			this.btnVisualizeDict.UseVisualStyleBackColor = true;
			this.btnVisualizeDict.Click += new System.EventHandler(this.btnVisualizeDict_Click);
			// 
			// btnDumpAll
			// 
			this.btnDumpAll.Location = new System.Drawing.Point(4, 187);
			this.btnDumpAll.Name = "btnDumpAll";
			this.btnDumpAll.Size = new System.Drawing.Size(121, 38);
			this.btnDumpAll.TabIndex = 7;
			this.btnDumpAll.Text = "Dump All Words and Following Words";
			this.btnDumpAll.UseVisualStyleBackColor = true;
			this.btnDumpAll.Click += new System.EventHandler(this.btnDumpAll_Click);
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel2.Location = new System.Drawing.Point(4, 231);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(0, 0);
			this.panel2.TabIndex = 7;
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.tbDepth);
			this.panel1.Controls.Add(this.tbBreath);
			this.panel1.Controls.Add(this.btnViewGraph);
			this.panel1.Location = new System.Drawing.Point(4, 237);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(125, 70);
			this.panel1.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Depth:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "Breadth:";
			// 
			// tbDepth
			// 
			this.tbDepth.Location = new System.Drawing.Point(60, 47);
			this.tbDepth.Name = "tbDepth";
			this.tbDepth.Size = new System.Drawing.Size(61, 20);
			this.tbDepth.TabIndex = 11;
			this.tbDepth.Text = "5";
			// 
			// tbBreath
			// 
			this.tbBreath.Location = new System.Drawing.Point(60, 26);
			this.tbBreath.Name = "tbBreath";
			this.tbBreath.Size = new System.Drawing.Size(61, 20);
			this.tbBreath.TabIndex = 9;
			this.tbBreath.Text = "5";
			// 
			// btnViewGraph
			// 
			this.btnViewGraph.Location = new System.Drawing.Point(1, 3);
			this.btnViewGraph.Name = "btnViewGraph";
			this.btnViewGraph.Size = new System.Drawing.Size(121, 23);
			this.btnViewGraph.TabIndex = 8;
			this.btnViewGraph.Text = "View Graph...";
			this.btnViewGraph.UseVisualStyleBackColor = true;
			this.btnViewGraph.Click += new System.EventHandler(this.btnViewGraph_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.AddExtension = false;
			this.openFileDialog.CheckFileExists = false;
			this.openFileDialog.Title = "Open suggestion dictionary...";
			// 
			// tbOutput
			// 
			this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbOutput.Location = new System.Drawing.Point(147, 29);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(458, 317);
			this.tbOutput.TabIndex = 2;
			this.tbOutput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbOutput_KeyUp);
			// 
			// btnSuggest
			// 
			this.btnSuggest.Location = new System.Drawing.Point(148, 3);
			this.btnSuggest.Name = "btnSuggest";
			this.btnSuggest.Size = new System.Drawing.Size(75, 23);
			this.btnSuggest.TabIndex = 3;
			this.btnSuggest.Text = "Suggest Next Word";
			this.btnSuggest.UseVisualStyleBackColor = true;
			this.btnSuggest.Click += new System.EventHandler(this.btnSuggest_Click);
			// 
			// btnForensics
			// 
			this.btnForensics.Location = new System.Drawing.Point(229, 3);
			this.btnForensics.Name = "btnForensics";
			this.btnForensics.Size = new System.Drawing.Size(75, 23);
			this.btnForensics.TabIndex = 5;
			this.btnForensics.Text = "Forensics";
			this.btnForensics.UseVisualStyleBackColor = true;
			this.btnForensics.Click += new System.EventHandler(this.btnForensics_Click);
			// 
			// btnViewSelectedWordGraph
			// 
			this.btnViewSelectedWordGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnViewSelectedWordGraph.Location = new System.Drawing.Point(2, 247);
			this.btnViewSelectedWordGraph.Name = "btnViewSelectedWordGraph";
			this.btnViewSelectedWordGraph.Size = new System.Drawing.Size(167, 23);
			this.btnViewSelectedWordGraph.TabIndex = 8;
			this.btnViewSelectedWordGraph.Text = "View selected word(s)↑ graph";
			this.btnViewSelectedWordGraph.UseVisualStyleBackColor = true;
			this.btnViewSelectedWordGraph.Click += new System.EventHandler(this.btnViewSelectedWordGraph_Click);
			// 
			// btnPopulateList
			// 
			this.btnPopulateList.Location = new System.Drawing.Point(3, 3);
			this.btnPopulateList.Name = "btnPopulateList";
			this.btnPopulateList.Size = new System.Drawing.Size(166, 23);
			this.btnPopulateList.TabIndex = 9;
			this.btnPopulateList.Text = "Populate Words List ↓";
			this.btnPopulateList.UseVisualStyleBackColor = true;
			this.btnPopulateList.Click += new System.EventHandler(this.btnPopulateList_Click);
			// 
			// listWords
			// 
			this.listWords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listWords.FormattingEnabled = true;
			this.listWords.Location = new System.Drawing.Point(3, 29);
			this.listWords.Name = "listWords";
			this.listWords.ScrollAlwaysVisible = true;
			this.listWords.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listWords.Size = new System.Drawing.Size(166, 212);
			this.listWords.TabIndex = 10;
			// 
			// panel3
			// 
			this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel3.Controls.Add(this.cbShowSubsequentWords);
			this.panel3.Controls.Add(this.cbShowPriorWords);
			this.panel3.Controls.Add(this.btnPopulateList);
			this.panel3.Controls.Add(this.listWords);
			this.panel3.Controls.Add(this.btnViewSelectedWordGraph);
			this.panel3.Location = new System.Drawing.Point(609, 29);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(173, 317);
			this.panel3.TabIndex = 11;
			// 
			// cbShowSubsequentWords
			// 
			this.cbShowSubsequentWords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbShowSubsequentWords.AutoSize = true;
			this.cbShowSubsequentWords.Checked = true;
			this.cbShowSubsequentWords.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbShowSubsequentWords.Location = new System.Drawing.Point(9, 291);
			this.cbShowSubsequentWords.Name = "cbShowSubsequentWords";
			this.cbShowSubsequentWords.Size = new System.Drawing.Size(142, 17);
			this.cbShowSubsequentWords.TabIndex = 12;
			this.cbShowSubsequentWords.Text = "Show subsequent words";
			this.cbShowSubsequentWords.UseVisualStyleBackColor = true;
			// 
			// cbShowPriorWords
			// 
			this.cbShowPriorWords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbShowPriorWords.AutoSize = true;
			this.cbShowPriorWords.Checked = true;
			this.cbShowPriorWords.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbShowPriorWords.Location = new System.Drawing.Point(9, 274);
			this.cbShowPriorWords.Name = "cbShowPriorWords";
			this.cbShowPriorWords.Size = new System.Drawing.Size(107, 17);
			this.cbShowPriorWords.TabIndex = 11;
			this.cbShowPriorWords.Text = "Show prior words";
			this.cbShowPriorWords.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(783, 351);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.btnForensics);
			this.Controls.Add(this.btnSuggest);
			this.Controls.Add(this.tbOutput);
			this.Controls.Add(this.flowLayoutPanel);
			this.MinimumSize = new System.Drawing.Size(16, 350);
			this.Name = "MainForm";
			this.Text = "Word Prediction & Forensics Library";
			this.flowLayoutPanel.ResumeLayout(false);
			this.flowLayoutPanel.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button btnTrain;
		private System.Windows.Forms.Label labelTotalWords;
		private System.Windows.Forms.Label labelUniqueWords;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.Button btnSuggest;
		private System.Windows.Forms.Button btnForensics;
		private System.Windows.Forms.Button btnVisualizeDict;
		private System.Windows.Forms.Button btnDumpAll;
		private System.Windows.Forms.Button btnViewGraph;
		private System.Windows.Forms.TextBox tbBreath;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbDepth;
		private System.Windows.Forms.Button btnViewSelectedWordGraph;
		private System.Windows.Forms.Button btnPopulateList;
		private System.Windows.Forms.ListBox listWords;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.CheckBox cbShowSubsequentWords;
		private System.Windows.Forms.CheckBox cbShowPriorWords;
	}
}

