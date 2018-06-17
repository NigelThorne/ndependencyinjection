using NDependencyInjection.DSL;
using NDependencyInjection.interfaces;
using NDependencyInjection.Tests.ExampleUsage.UI;

namespace NDependencyInjection.Tests.ExampleUsage
{
    public class UIFactory : IUIFactory
    {
        private readonly ApplicationState _state;

        public UIFactory(ApplicationState state)
        {
            _state = state;
        }

        public ITimerUI CreateUI()
        {
            ISystemDefinition sys = new SystemDefinition();
            sys.BroadcastsTo<IStartListener>(); 
            sys.BroadcastsTo<ITickListener>(); 
            sys.HasSingleton<TickingClock>().Provides<IClock>().ListensFor<IStartListener>(); // this is ticking
            sys.HasSingleton<TimerDialog>().Provides<ITimerDialog>().ListensFor<IStartListener>();
            sys.HasSingleton<TimerViewModel>().Provides<TimerViewModel>();
            sys.HasSingleton<TimerController>().Provides<ITimerUI>().ListensFor<ITickListener>();
            return sys.Get<ITimerUI>();
        }
    }
}