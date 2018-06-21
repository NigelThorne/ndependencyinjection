#region usings

using NDependencyInjection.DSL;
using TaskTimer.Domain;
using TaskTimer.UI;

#endregion

namespace TaskTimer
{
    public class ApplicationBuilder : ISubsystemBuilder
    {
        public void Build ( ISystemDefinition sys )
        {
            sys.RelaysCallsTo<ITickListener> ();
            sys.RelaysCallsTo<IViewClosedHandler> ();

            sys.HasSingleton<Application> ()
                .Provides<IRunnable> ()
                .HandlesCallsTo<IViewClosedHandler> ()
                .HandlesCallsTo<ITickListener> ();

            sys.HasSingleton<TaskRepository> ()
                .Provides<ITaskRepository> ();

            sys.HasSingleton<TasksDomainController> ()
                .Provides<ITasksDomainController> ();

            sys.HasSingleton<TickingClock> ()
                .Provides<IClock> ();

            sys.HasSingleton<Scheduler> ()
                .Provides<IScheduler> ()
                .HandlesCallsTo<ITickListener> ();

            sys.HasFactory<UIFactory> ()
                .Provides<IUIFactory> ();
        }
    }
}