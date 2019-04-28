using System;
using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

namespace SuggestWordLibrary
{
	/// <summary>
	/// Performs value equality comparison for Microsoft.Msagl.Drawing.Edge. Two edges are equal if their source and target are the same.
	/// </summary>
	public class GraphEdgeComparer : IEqualityComparer<Edge>
	{
		public bool Equals(Edge x, Edge y)
		{
			return (x.Source == y.Source && x.Target == y.Target);
		}

		public int GetHashCode(Edge obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return new Tuple<string, string, string>(obj.Source, obj.LabelText, obj.Target).GetHashCode();
		}
	}
}
