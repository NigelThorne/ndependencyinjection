using NDependencyInjection;
using NDependencyInjection.DSL;
using NDependencyInjection.interfaces;
using TaskTimer.Domain;

namespace TaskTimer.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly ITasksDomainController _tasksDomainController;

        public UIFactory(ITasksDomainController tasksDomainController)
        {
            _tasksDomainController = tasksDomainController;
        }

        public ITimerUI CreateUI()
        {
            ISystemDefinition sys = new SystemDefinition();
            sys.HasInstance(_tasksDomainController).Provides<ITasksDomainController>();
            sys.HasSubsystem(new UIBuilder()).Provides<ITimerUI>();
            return sys.Get<ITimerUI>();
        }
    }
}