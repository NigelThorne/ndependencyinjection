using Microsoft.Glee.GraphViewerGdi;


namespace Reflector.NDIGraph
{
	using System.Collections;
	using System.Collections.Specialized;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using System;
	using System.ComponentModel;
	using Reflector.CodeModel;
	using System.Reflection;
	using System.Xml;
	using Microsoft.Glee.Drawing;

	internal abstract class GraphControl : UserControl
    {
        private IAssemblyBrowser assemblyBrowser = null;
        private Microsoft.Glee.GraphViewerGdi.GViewer viewer;

		public GraphControl(IServiceProvider serviceProvider)
        {
			this.assemblyBrowser = (IAssemblyBrowser)serviceProvider.GetService(typeof(IAssemblyBrowser));

            this.viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();
            this.viewer.SelectionChanged += new EventHandler(OnSelectionChanged);
            DockControl(this.viewer);
        }

	    public void DockControl(Control control)
	    {
	        control.Dock = DockStyle.Fill;
	        this.Controls.Add(control);
	        this.Dock = DockStyle.Fill;
	    }

	    protected Microsoft.Glee.Drawing.Graph CreateGraph(string label)
        {
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph(label);
            graph.GraphAttr.NodeAttr.FontName = "Tahoma";
            graph.GraphAttr.NodeAttr.Fontsize = 8;
            graph.GraphAttr.NodeAttr.Shape = Shape.Box;
            graph.GraphAttr.NodeAttr.Fillcolor = Microsoft.Glee.Drawing.Color.WhiteSmoke;

            graph.GraphAttr.EdgeAttr.FontName = "Tahoma";
            graph.GraphAttr.EdgeAttr.Fontsize = 8;
            return graph;
        }

        protected virtual void OnSelectionChanged(object sender, EventArgs e)
        {
            if (this.viewer.SelectedObject == null)
                return;

            Node node = this.viewer.SelectedObject as Node;
            if (node != null)
            {
                object value = node.UserData;
                if (value != null)
					this.assemblyBrowser.ActiveItem = value;
                return;
            }

            Microsoft.Glee.Drawing.Edge edge = this.viewer.SelectedObject as Microsoft.Glee.Drawing.Edge;
            if (edge != null)
            {
                object value = edge.UserData;
                if (value != null)
					this.assemblyBrowser.ActiveItem = value;
                return;
            }
        }

        public Microsoft.Glee.GraphViewerGdi.GViewer Viewer
        {
            get { return this.viewer; }
        }
    }
}
