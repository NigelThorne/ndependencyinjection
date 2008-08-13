using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Glee;
using Microsoft.Glee.Drawing;
using QuickGraph;
using QuickGraph.Algorithms.Layout;
using QuickGraph.Glee;
using Reflector.CodeModel;
using Color=Microsoft.Glee.Drawing.Color;
using Rectangle=Microsoft.Glee.Splines.Rectangle;

namespace Reflector.NDIGraph.Controls
{
    public class Node
    {
        private readonly string name;
        private bool isProvided;
        private bool isServed;

        public Node(string name)
        {
            isProvided = false;
            this.name = name;
        }

        public bool IsProvided
        {
            get { return isProvided; }
            set { isProvided = value; }
        }

        public bool IsServed
        {
            get { return isServed; }
            set { isServed = value; }
        }

        public override string ToString()
        {
            return name;
        }
    }

    internal sealed class WiringDiagramControl : GraphControl
    {
        private readonly IAssemblyBrowser assemblyBrowser;
        private readonly StringCollection excludedTypes = new StringCollection();
        private AdjacencyGraph<Node, IEdge<Node>> g = null;
        private RandomLayoutAlgorithm<Node, IEdge<Node>, AdjacencyGraph<Node, IEdge<Node>>> layout;


        public WiringDiagramControl(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            assemblyBrowser = (IAssemblyBrowser) serviceProvider.GetService(typeof (IAssemblyBrowser));

            excludedTypes.Add("System.Object");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
//            if (g != null)
//            {
//                GdiGraphLayoutRenderer
//                    <Node, IEdge<Node>, AdjacencyGraph<Node, IEdge<Node>>,
//                        LayoutAlgorithmBase<Node, IEdge<Node>, AdjacencyGraph<Node, IEdge<Node>>>> renderer =
//                            new GdiGraphLayoutRenderer
//                                <Node, IEdge<Node>, AdjacencyGraph<Node, IEdge<Node>>,
//                                    LayoutAlgorithmBase<Node, IEdge<Node>, AdjacencyGraph<Node, IEdge<Node>>>>(
//                                this.layout, e.Graphics);
//                renderer.Render();
//            }
            base.OnPaint(e);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent != null)
            {
                Translate();
                assemblyBrowser.ActiveItemChanged += assemblyBrowser_ActiveItemChanged;
            }
            else
            {
                assemblyBrowser.ActiveItemChanged -= assemblyBrowser_ActiveItemChanged;
            }
        }

        private void assemblyBrowser_ActiveItemChanged(object sender, EventArgs e)
        {
            if (Parent == null)
                return;

            ITypeReference typeReference = assemblyBrowser.ActiveItem as ITypeReference;
            if (typeReference == null)
                return;

            Translate();
        }


        private void Translate()
        {
            ITypeDeclaration activeType = assemblyBrowser.ActiveItem as ITypeDeclaration;
            if (activeType == null) return;

            IInstructionCollection method = FindBuildMethod(activeType);
            if (method == null) return;

//            AdjacencyGraph<Node, IEdge<Node>> g = new AdjacencyGraph<Node, IEdge<Node>>();
            g = new AdjacencyGraph<Node, IEdge<Node>>();
            Dictionary<string, Node> services = new Dictionary<string, Node>();
            Node serviceNode = null;

            foreach (IInstruction instruction in method)
            {
                IMethodReference methodInstanceReference = instruction.Value as IMethodReference;
                if (methodInstanceReference == null) continue;

                switch (methodInstanceReference.Name)
                {
                    case "HasSingleton":
                    case "HasFactory":
                        {
                            ITypeReference reference = methodInstanceReference.GenericArguments[0] as ITypeReference;
                            if (reference == null) break;

                            serviceNode = GetVertex(reference.Name, services, g);
                            serviceNode.IsServed = true;

                            IMethodDeclaration constructor = GetInjectionConstructor(reference);
                            if (constructor == null) break;

                            foreach (IParameterDeclaration param in constructor.Parameters)
                            {
                                g.AddEdge(new Edge<Node>(
                                                         GetVertex(GetName(param.ParameterType), services, g), serviceNode));
                            }
                        }
                        break;
                    case "HasInstance":
                    case "HasSubsystem":
                        {
                            serviceNode = GetVertex(methodInstanceReference.Parameters[0].Name, services, g);
                            serviceNode.IsServed = true;
                        }
                        break;
                    case "HasCollection":
                        {
                            serviceNode = GetVertex(methodInstanceReference.GenericArguments[0].ToString(), services, g);
                            serviceNode.IsServed = true;
                        }
                        break;
                    case "Provides":
                    case "ListensTo":
                        {
                            if (serviceNode != null)
                            {
                                Node vertex =
                                    GetVertex(methodInstanceReference.GenericArguments[0].ToString(),
                                              services, g);
                                g.AddEdge(new Edge<Node>(serviceNode, vertex));
                                vertex.IsProvided = true;
                            }
                        }
                        break;
                    //default:
                    //    serviceNode = null;
                    //    break;
                }
            }

//            Dictionary<Node, PointF> positions = new Dictionary<Node, PointF>();
//            Graphics graphics = CreateGraphics();
//
            layout = new RandomLayoutAlgorithm<Node, IEdge<Node>, AdjacencyGraph<Node, IEdge<Node>>>(g, new Dictionary<Node, PointF>());
            this.layout.BoundingBox = this.ClientRectangle;
            this.layout.Compute();


            GleeGraphPopulator<Node, IEdge<Node>> populator = GleeGraphUtility.Create(g);
            populator.NodeAdded += OnGleeVertexNodeEvent;
            populator.Compute();
            Graph graph = populator.GleeGraph;
            Viewer.Graph = graph; // we have the graph :)      
//            Viewer.CalculateLayout(graph);

//            RandomLayoutAlgorithm<Node, IEdge<Node>, AdjacencyGraph<Node, IEdge<Node>>> layout = new RandomLayoutAlgorithm<Node, IEdge<Node>, AdjacencyGraph<Node, IEdge<Node>>>(g, positions);
//            layout.BoundingBox = new RectangleF(-500,500,1000,1000);
//            layout.Compute();
//
//            foreach (Microsoft.Glee.Drawing.Node node in graph.NodeMap.Values)
//            {
//                PointF p = positions[(Node)node.UserData];
//                node.Attr.GleeNode.Center = new Microsoft.Glee.Splines.Point(p.X, p.Y);
//            }
//            graph.NeedCalculateLayout = false;
//            graph.GleeGraph.BoundingBox = new Rectangle(-5000, 5000, 5000, -5000);
//            graph.GleeGraph.CalculateLayout();
        }

        

        private void OnGleeVertexNodeEvent(object sender, GleeVertexEventArgs<Node> args)
        {
            if (args.Vertex.IsServed)
            {
                args.Node.Attr.Fillcolor = Color.LightGreen;
            }
            else if (args.Vertex.IsProvided)
            {
                args.Node.Attr.Shape = Shape.Ellipse;
                args.Node.Attr.Fillcolor = Color.LightBlue;
            }
            else
            {
                args.Node.Attr.Fillcolor = Color.LightGray;
            }
        }


        private static string GetName(IType typeRef)
        {
            ITypeReference paramType = typeRef as ITypeReference;
            if (paramType != null) return paramType.Name;

            IArrayType arrayType = typeRef as IArrayType;
            if (arrayType != null) return GetName(arrayType.ElementType) + "[]";

            return "Unknown Name";
        }

        private static IMethodDeclaration GetInjectionConstructor(ITypeReference reference)
        {
            ITypeDeclaration resolve = reference.Resolve();
            List<IMethodDeclaration> constructors = new List<IMethodDeclaration>();
            foreach (IMethodDeclaration methodD in resolve.Methods)
            {
                if (methodD.Name == ".ctor" && methodD.Visibility == MethodVisibility.Public)
                    constructors.Add(methodD);
            }

            if (constructors.Count == 1) return constructors[0];

            foreach (IMethodDeclaration method2 in constructors)
            {
                foreach (ICustomAttribute attribute in method2.Attributes)
                {
                    ITypeReference type;
                    if (attribute.Constructor == null ||
                        ((type = attribute.Constructor.DeclaringType as ITypeReference) == null)) continue;
                    if (type.Name == "InjectionConstructorAttribute" &&
                        type.Namespace == "NDependencyInjection") return method2;
                }
            }
            return null;
        }

        private static IInstructionCollection FindBuildMethod(ITypeDeclaration activeType)
        {
            foreach (IMethodDeclaration method in activeType.Methods)
            {
                IMethodBody body;
                if (method.Name == "Build" && ((body = method.Body as IMethodBody) != null))
                {
                    return body.Instructions;
                }
            }
            return null;
        }

        private static Node GetVertex(string name, IDictionary<string, Node> services,
                                      IMutableVertexListGraph<Node, IEdge<Node>> g)
        {
            if (!services.ContainsKey(name))
            {
                Node v = new Node(name);
                services[name] = v;
                g.AddVertex(v);
                return v;
            }
            return services[name];
        }
    }
}