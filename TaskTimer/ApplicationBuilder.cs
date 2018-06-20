using NDependencyInjection.DSL;
using TaskTimer.Domain;

namespace TaskTimer
{
    public class ApplicationBuilder : ISubsystemBuilder
    {
        public void Build(ISystemDefinition sys)
        {
            sys.HasSingleton<Application>()
                .Provides<IRunnable>();

            sys.HasSingleton<TaskRepository>()
                .Provides<ITaskRepository>();

            sys.HasSingleton<TasksDomainController>()
                .Provides<ITasksDomainController>();

            sys.HasFactory<UI.UIFactory>()
                .Provides<UI.IUIFactory>();
        }

    }
}