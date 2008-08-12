using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Glee.Drawing;
using QuickGraph;
using QuickGraph.Glee;
using Reflector.CodeModel;
using Reflector.CodeModel.Memory;

namespace Reflector.NDIGraph.Controls
{
    internal sealed class WiringDiagramControl : GraphControl
    {
        private readonly IAssemblyBrowser assemblyBrowser;
        private readonly StringCollection excludedTypes = new StringCollection();

        public WiringDiagramControl(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            assemblyBrowser = (IAssemblyBrowser) serviceProvider.GetService(typeof (IAssemblyBrowser));

            excludedTypes.Add("System.Object");
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

            AdjacencyGraph<string, IEdge<string>> g = new AdjacencyGraph<string, IEdge<string>>();
            List<string> services = new List<string>();
            string serviceNode = null;

            foreach (IInstruction instruction in method)
            {
                IMethodReference methodInstanceReference = instruction.Value as IMethodReference;
                if (methodInstanceReference == null) continue;

                switch (methodInstanceReference.Name)
                {
                    case "HasSingleton":
                        {
                            ITypeReference reference = methodInstanceReference.GenericArguments[0] as ITypeReference;
                            if (reference == null) break;

                            serviceNode = GetVertex(reference.Name, services, g);

                            IMethodDeclaration constructor = GetInjectionConstructor(reference);
                            if (constructor == null) break;

                            foreach (IParameterDeclaration param in constructor.Parameters)
                            {
                                g.AddEdge(new Edge<string>(GetVertex(GetName(param.ParameterType), services, g), serviceNode));
                            }
                        }
                        break;
                    case "HasInstance":
                    case "HasFactory":
                    case "HasCollection":
                    case "HasSubsystem":
                        {
                            serviceNode = GetVertex(methodInstanceReference.GenericArguments[0].ToString(),services, g);
                        }
                        break;
                    case "Provides":
                    case "ListensTo":
                        {
                            if (serviceNode != null)
                            {
                                string vertex =
                                    GetVertex(methodInstanceReference.GenericArguments[0].ToString(),
                                              services, g);
                                g.AddEdge(new Edge<string>(serviceNode, vertex));
                            }
                        }
                        break;
                    default:
                        serviceNode = null;
                        break;
                }
            }

            GleeGraphPopulator<string, IEdge<string>> populator = GleeGraphUtility.Create(g);
            populator.Compute();
            Graph graph = populator.GleeGraph;

            Viewer.Graph = graph; // we have the graph :)
        }

        private static string GetName(IType typeRef)
        {
            IArrayType arrayType = typeRef as IArrayType;
            if (arrayType != null)
            {
                ITypeReference type = arrayType.ElementType as ITypeReference;
                if (type != null)
                {
                    return type.Name + "[]";
                }
            }

            ITypeReference paramType = typeRef as ITypeReference;
            if (paramType != null)
            {
                return paramType.Name;
            }
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

        private static string GetVertex(string name, ICollection<string> services,
                                        IMutableVertexListGraph<string, IEdge<string>> g)
        {
            string vertex = name;
            if (!services.Contains(vertex))
            {
                services.Add(vertex);
                g.AddVertex(vertex);
            }
            return vertex;
        }
    }
}