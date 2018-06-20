using System;
using AmmySidekick;
using NDependencyInjection;
using NDependencyInjection.DSL;

namespace TaskTimer
{
    public static class MainEntryPoint
    {
        [STAThread]
        public static void Main()
        {
            App app = new App();

            RuntimeUpdateHandler.Register(app, "/" + Ammy.GetAssemblyName(app) + ";component/App.g.xaml");

            ISystemDefinition sys = new SystemDefinition();
            sys.HasSubsystem(new ApplicationBuilder()).Provides<IRunnable>();
            var runnable = sys.Get<IRunnable>();
            runnable.Run();
        }

    }
}