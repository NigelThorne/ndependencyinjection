#region usings

using System;
using NDependencyInjection;
using NDependencyInjection.DSL;

#endregion

namespace TaskTimer
{
    public static class MainEntryPoint
    {
        [STAThread]
        public static void Main ( )
        {
            ISystemDefinition sys = new SystemDefinition ();
            sys.HasSubsystem ( new ApplicationBuilder () ).Provides<IRunnable> ();
            var runnable = sys.Get<IRunnable> ();
            runnable.Run ();
        }
    }
}