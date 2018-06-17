using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AmmySidekick;
using NDependencyInjection.DSL;

namespace NDependencyInjection.Tests.ExampleUsage.UI
{
    public partial class App : MediaTypeNames.Application
    {
        [STAThread]
        public static void Main()
        {
            ISystemDefinition sys = new SystemDefinition();

            sys.HasSingleton<MainProcess>().Provides<IEntryPoint>();
            sys.HasSingleton<TaskRepository>().Provides<ITaskHistory>();
            sys.HasSingleton<ApplicationState>().Provides<ApplicationState>();
            sys.HasFactory<UIFactory>().Provides<IUIFactory>();


            sys.Get<IEntryPoint>().Run();


            App app = new App();
            app.InitializeComponent();

            RuntimeUpdateHandler.Register(app, "/" + AmmySidekick.Ammy.GetAssemblyName(app) + ";component/App.g.xaml");

            app.Run();
        }
    }
}
