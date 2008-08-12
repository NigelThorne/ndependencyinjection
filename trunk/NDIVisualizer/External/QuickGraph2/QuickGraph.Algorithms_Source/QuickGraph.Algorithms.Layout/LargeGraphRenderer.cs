namespace QuickGraph.Algorithms.Layout
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    public class LargeGraphRenderer
    {
        private IEdgeRenderer edgeRenderer;
        private bool edgeVisible;
        private Size layoutSize;
        private VertexPointFDictionary positions;
        private IVertexRenderer vertexRenderer;
        private bool vertexVisible;
        private IVertexAndEdgeListGraph visitedGraph;

        public LargeGraphRenderer()
        {
            this.visitedGraph = null;
            this.positions = new VertexPointFDictionary();
            this.edgeVisible = true;
            this.edgeRenderer = new QuickGraph.Algorithms.Layout.EdgeRenderer();
            this.vertexVisible = true;
            this.vertexRenderer = new QuickGraph.Algorithms.Layout.VertexRenderer();
            this.layoutSize = new Size(800, 600);
        }

        public LargeGraphRenderer(IVertexAndEdgeListGraph visitedGraph)
        {
            this.visitedGraph = null;
            this.positions = new VertexPointFDictionary();
            this.edgeVisible = true;
            this.edgeRenderer = new QuickGraph.Algorithms.Layout.EdgeRenderer();
            this.vertexVisible = true;
            this.vertexRenderer = new QuickGraph.Algorithms.Layout.VertexRenderer();
            this.layoutSize = new Size(800, 600);
            if (visitedGraph == null)
            {
                throw new ArgumentNullException("visitedGraph");
            }
            this.visitedGraph = visitedGraph;
        }

        public RectangleF GetBoundingBox()
        {
            ICollection values = this.Positions.Values;
            int num = 0;
            float num2 = 0f;
            float num3 = 1f;
            float num4 = 0f;
            float num5 = 1f;
            foreach (PointF tf in values)
            {
                if (num == 0)
                {
                    num2 = Math.Min(0f, tf.X);
                    num3 = Math.Max((float) this.LayoutSize.Width, tf.X);
                    num4 = Math.Min(0f, tf.Y);
                    num5 = Math.Max((float) this.LayoutSize.Height, tf.Y);
                }
                else
                {
                    num2 = Math.Min(tf.X, num2);
                    num3 = Math.Max(tf.X, num3);
                    num4 = Math.Max(tf.Y, num4);
                    num5 = Math.Min(tf.Y, num5);
                }
                num++;
            }
            return new RectangleF(num2, num5, num3 - num2, num4 - num5);
        }

        public RectangleF GetOriginalBox()
        {
            return new RectangleF(0f, 0f, (float) this.LayoutSize.Width, (float) this.LayoutSize.Height);
        }

        public void Render(Graphics g)
        {
            if ((this.VisitedGraph != null) && (this.positions.Count != 0))
            {
                GraphicsContainer container = null;
                RectangleF boundingBox = this.GetBoundingBox();
                g.FillRectangle(Brushes.White, g.ClipBounds);
                g.BeginContainer(this.GetOriginalBox(), boundingBox, GraphicsUnit.Pixel);
                if (this.edgeVisible)
                {
                    int num = 0;
                    IEdgeEnumerator enumerator = this.VisitedGraph.get_Edges().GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        IEdge e = enumerator.get_Current();
                        this.edgeRenderer.Render(g, e, this.Positions.get_Item(e.get_Source()), this.Positions.get_Item(e.get_Target()));
                        num++;
                    }
                }
                if (this.vertexVisible)
                {
                    this.vertexRenderer.PreRender();
                    IDictionaryEnumerator enumerator2 = this.Positions.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        DictionaryEntry current = (DictionaryEntry) enumerator2.Current;
                        this.vertexRenderer.Render(g, (IVertex) current.Key, (PointF) current.Value);
                    }
                }
                if (container != null)
                {
                    g.EndContainer(container);
                }
            }
        }

        public void Render(Image img)
        {
            using (Graphics graphics = Graphics.FromImage(img))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                this.Render(graphics);
            }
        }

        public void Render(string fileName, ImageFormat imageFormat)
        {
            using (Bitmap bitmap = new Bitmap(this.LayoutSize.Width, this.LayoutSize.Height))
            {
                this.Render(bitmap);
                bitmap.Save(fileName, imageFormat);
            }
        }

        public void SetPositions(IVertexPointFDictionary positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException("positions");
            }
            IDictionaryEnumerator enumerator = positions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                this.positions.set_Item((IVertex) current.Key, (PointF) current.Value);
            }
        }

        public IEdgeRenderer EdgeRenderer
        {
            get
            {
                return this.edgeRenderer;
            }
            set
            {
                this.edgeRenderer = value;
            }
        }

        public bool EdgeVisible
        {
            get
            {
                return this.edgeVisible;
            }
            set
            {
                this.edgeVisible = value;
            }
        }

        public Size LayoutSize
        {
            get
            {
                return this.layoutSize;
            }
            set
            {
                this.layoutSize = value;
            }
        }

        public IVertexPointFDictionary Positions
        {
            get
            {
                return this.positions;
            }
        }

        public IVertexRenderer VertexRenderer
        {
            get
            {
                return this.vertexRenderer;
            }
            set
            {
                this.vertexRenderer = value;
            }
        }

        public bool VertexVisible
        {
            get
            {
                return this.vertexVisible;
            }
            set
            {
                this.vertexVisible = value;
            }
        }

        public IVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
    }
}

