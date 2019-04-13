using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Msagl;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;

namespace SuggestWordLibrary
{
	public partial class GraphVisualizer : Form
	{
		public GraphVisualizer(Graph graph)
		{
			InitializeComponent();
			gViewer.Graph = graph;
		}
	}
}
