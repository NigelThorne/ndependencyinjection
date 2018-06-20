using NDependencyInjection;
using NDependencyInjection.DSL;
using NDependencyInjection.interfaces;

namespace TaskTimer.UI
{
    public class UIFactory : UIBuilder, IUIFactory
    {
        private readonly ITimerCommandsHandler _listener;
        public UIFactory(ITimerCommandsHandler listener)
        {
            _listener = listener;
        }

        public ITimerUI CreateUI()
        {
            ISystemDefinition sys = new SystemDefinition();
            sys.HasInstance<ITimerCommandsHandler>(_listener);
            sys.HasSubsystem(this);
            return sys.Get<ITimerUI>();
        }
    }
}