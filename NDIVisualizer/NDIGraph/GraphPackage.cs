using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using Reflector.NDIGraph.Controls;


namespace Reflector.NDIGraph
{
    public class GraphPackage : IPackage
    {
        private ICommandBarManager commandBarManager;
        private readonly ArrayList commands = new ArrayList();
        private IWindowManager windowManager;

        #region IPackage Members

        public void Load(IServiceProvider serviceProvider)
        {
            windowManager = (IWindowManager) serviceProvider.GetService(typeof (IWindowManager));
            commandBarManager = (ICommandBarManager) serviceProvider.GetService(typeof (ICommandBarManager));

            UserControl wiringDiagram = new WiringDiagramControl(serviceProvider);

            windowManager.Windows.Add("Graph.WiringDiagram", wiringDiagram, "NDI Wiring Diagram");
            AddCommand("Browser.TypeDeclaration", "NDI Wiring Diagram", WiringDiagram_Click);
        }

        public void Unload()
        {
            windowManager.Windows.Remove("Graph.WiringDiagram");

            foreach (Command command in commands)
            {
                ICommandBarItemCollection commandBarItems = commandBarManager.CommandBars[command.CommandBar].Items;
                commandBarItems.Remove(command.Button);
                commandBarItems.Remove(command.Separator);
            }
        }

        #endregion

        private void AddCommand(string identifier, string text, EventHandler eventHandler)
        {
            ICommandBar commandBar = commandBarManager.CommandBars[identifier];

            Command command = new Command();
            command.CommandBar = identifier;
            command.Separator = commandBar.Items.AddSeparator();
            command.Button = commandBar.Items.AddButton(text, eventHandler);
            commands.Add(command);
        }

        private void WiringDiagram_Click(object sender, EventArgs e)
        {
            windowManager.Windows["Graph.WiringDiagram"].Visible = true;
        }

        #region Nested type: Command

        private struct Command
        {
            public ICommandBarButton Button;
            public string CommandBar;
            public ICommandBarSeparator Separator;
        }

        #endregion
    }

    public sealed class FlowToCodeConverter
    {
        private readonly Hashtable codeFlows = new Hashtable();

        public FlowToCodeConverter()
        {
            foreach (FieldInfo fi in
                typeof (OpCodes).GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static
                    )
                )
            {
                OpCode code = (OpCode) fi.GetValue(null);
                codeFlows[(int) code.Value] = code.FlowControl;
            }
        }

        public FlowControl Convert(int code)
        {
            Object o = codeFlows[code];
            if (o == null)
                return FlowControl.Meta;
            //				throw new Exception(String.Format("code.Value {0} not found",code.Value));
            return (FlowControl) o;
        }
    }
}