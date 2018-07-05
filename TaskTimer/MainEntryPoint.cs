#region usings

using System;
using System.Threading;
using NDependencyInjection;
using NDependencyInjection.DSL;

#endregion

namespace TaskTimer
{
    /// <summary>
    /// Holds a list of arguments given to an application at startup.
    /// </summary>
    public class ArgumentsReceivedEventArgs : EventArgs
    {
        public String[] Args { get; set; }
    }

    public static class MainEntryPoint
    {
        private static IRunnable _runnable;

        [STAThread]
        static void Main(String[] args)
        {
            Guid guid = new Guid("{6671835c-60a2-4091-a8a8-7597fee58d1a}");
            using (SingleInstance singleInstance = new SingleInstance(guid))
            {
                if (singleInstance.IsFirstInstance)
                {
                    singleInstance.ArgumentsReceived += singleInstance_ArgumentsReceived;
                    singleInstance.ListenForArgumentsFromSuccessiveInstances();

                    ISystemDefinition sys = new SystemDefinition ();
                    sys.HasSubsystem ( new ApplicationBuilder () ).Provides<IRunnable> ();
                    _runnable = sys.Get<IRunnable> ();
                    _runnable.Run ();
                }
                else
                    singleInstance.PassArgumentsToFirstInstance(args);
            }
        }

        private static void singleInstance_ArgumentsReceived ( object sender, ArgumentsReceivedEventArgs e )
        {
            System.Windows.Application.Current.Dispatcher.Invoke ( ( ) =>
            {
                _runnable.ShowRunningUI ();
            } );
        }
    }
}