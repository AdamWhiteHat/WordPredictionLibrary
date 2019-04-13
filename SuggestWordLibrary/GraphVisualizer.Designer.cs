using Microsoft.Msagl;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;

namespace SuggestWordLibrary
{
	partial class GraphVisualizer
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
			Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation planeTransformation1 = new Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation();
			this.gViewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
			this.SuspendLayout();
			// 
			// gViewer
			// 
			this.gViewer.ArrowheadLength = 10D;
			this.gViewer.AsyncLayout = false;
			this.gViewer.AutoScroll = true;
			this.gViewer.BackwardEnabled = false;
			this.gViewer.BuildHitTree = true;
			this.gViewer.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.UseSettingsOfTheGraph;
			this.gViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gViewer.EdgeInsertButtonVisible = false;
			this.gViewer.FileName = "";
			this.gViewer.ForwardEnabled = false;
			this.gViewer.Graph = null;
			this.gViewer.InsertingEdge = false;
			this.gViewer.LayoutAlgorithmSettingsButtonVisible = true;
			this.gViewer.LayoutEditingEnabled = true;
			this.gViewer.Location = new System.Drawing.Point(0, 0);
			this.gViewer.LooseOffsetForRouting = 0.25D;
			this.gViewer.MouseHitDistance = 0.05D;
			this.gViewer.Name = "gViewer";
			this.gViewer.NavigationVisible = false;
			this.gViewer.NeedToCalculateLayout = true;
			this.gViewer.OffsetForRelaxingInRouting = 0.6D;
			this.gViewer.PaddingForEdgeRouting = 8D;
			this.gViewer.PanButtonPressed = false;
			this.gViewer.SaveAsImageEnabled = true;
			this.gViewer.SaveAsMsaglEnabled = true;
			this.gViewer.SaveButtonVisible = true;
			this.gViewer.SaveGraphButtonVisible = true;
			this.gViewer.SaveInVectorFormatEnabled = true;
			this.gViewer.Size = new System.Drawing.Size(503, 441);
			this.gViewer.TabIndex = 3;
			this.gViewer.TightOffsetForRouting = 0.125D;
			this.gViewer.ToolBarIsVisible = true;
			this.gViewer.Transform = planeTransformation1;
			this.gViewer.UndoRedoButtonsVisible = false;
			this.gViewer.WindowZoomButtonPressed = false;
			this.gViewer.ZoomF = 1D;
			this.gViewer.ZoomWindowThreshold = 0.05D;
			// 
			// GraphVisualizer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(503, 441);
			this.Controls.Add(this.gViewer);
			this.Name = "GraphVisualizer";
			this.Text = "GraphVisualizer";
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Msagl.GraphViewerGdi.GViewer gViewer;
	}
}