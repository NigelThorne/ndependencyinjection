using Microsoft.Win32;
using NDependencyInjection.DSL;
using NDependencyInjection.Tests.ExampleUsage.UI;

namespace NDependencyInjection.Tests.ExampleUsage
{
    public class ExampleApp
    {
        public ExampleApp()
        {
            ISystemDefinition sys = new SystemDefinition();

            sys.HasSingleton<MainProcess>().Provides<IEntryPoint>();
            sys.HasSingleton<TaskRepository>().Provides<ITaskHistory>();
            sys.HasSingleton<ApplicationState>().Provides<ApplicationState>();
            sys.HasFactory<UIFactory>().Provides<IUIFactory>();


            sys.Get<IEntryPoint>().Run();
        }
    }
}