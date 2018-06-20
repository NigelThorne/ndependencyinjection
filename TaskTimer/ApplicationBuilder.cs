using NDependencyInjection.DSL;
using TaskTimer.Domain;

namespace TaskTimer
{
    public class ApplicationBuilder : ISubsystemBuilder
    {
        public void Build(ISystemDefinition sys)
        {
            sys.BroadcastsTo<ITimerCommandsHandler>();

            sys.HasSingleton<Application>()
                .Provides<IRunnable>();

            sys.HasSingleton<TaskRepository>()
                .Provides<ITaskRepository>()
                .ListensFor<ITimerCommandsHandler>();

            sys.HasSingleton<TaskHistory>()
                .Provides<ITaskHistory>();

            sys.HasFactory<UI.UIFactory>()
                .Provides<UI.IUIFactory>();
        }

    }
}