using System;
using System.Windows.Forms;
using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;

namespace Reflector.NDIGraph
{
    internal abstract class GraphControl : UserControl
    {
        private readonly IAssemblyBrowser assemblyBrowser;
        private readonly GViewer viewer;

        public GraphControl(IServiceProvider serviceProvider)
        {
            assemblyBrowser = (IAssemblyBrowser) serviceProvider.GetService(typeof (IAssemblyBrowser));

            viewer = new GViewer();
            viewer.SelectionChanged += OnSelectionChanged;
            DockControl(viewer);
        }

        public GViewer Viewer
        {
            get { return viewer; }
        }

        public void DockControl(Control control)
        {
            control.Dock = DockStyle.Fill;
            Controls.Add(control);
            Dock = DockStyle.Fill;
        }

        protected Graph CreateGraph(string label)
        {
            Graph graph = new Graph(label);
            graph.GraphAttr.NodeAttr.FontName = "Tahoma";
            graph.GraphAttr.NodeAttr.Fontsize = 8;
            graph.GraphAttr.NodeAttr.Shape = Shape.Box;
            graph.GraphAttr.NodeAttr.Fillcolor = Color.WhiteSmoke;

            graph.GraphAttr.EdgeAttr.FontName = "Tahoma";
            graph.GraphAttr.EdgeAttr.Fontsize = 8;
            return graph;
        }

        protected virtual void OnSelectionChanged(object sender, EventArgs e)
        {
            if (viewer.SelectedObject == null)
                return;

            Node node = viewer.SelectedObject as Node;
            if (node != null)
            {
                object value = node.UserData;
                if (value != null)
                    assemblyBrowser.ActiveItem = value;
                return;
            }

            Edge edge = viewer.SelectedObject as Edge;
            if (edge != null)
            {
                object value = edge.UserData;
                if (value != null)
                    assemblyBrowser.ActiveItem = value;
                return;
            }
        }
    }
}