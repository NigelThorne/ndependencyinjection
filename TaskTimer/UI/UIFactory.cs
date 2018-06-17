using NDependencyInjection;
using NDependencyInjection.DSL;

namespace TaskTimer.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly ITimerUpdateHandler _listener;

        public UIFactory(ITimerUpdateHandler listener)
        {
            _listener = listener;
        }

        public ITimerUI CreateUI()
        {
            ISystemDefinition sys = new SystemDefinition();
            sys.BroadcastsTo<IStartListener>();
            sys.BroadcastsTo<ITickListener>();
            sys.BroadcastsTo<ITimeDialogEventListener>();

            sys.HasInstance(_listener)
                .Provides<ITimerUpdateHandler>(); // the way events get out of this subsystem

            sys.HasSingleton<TickingClock>()
                .Provides<IClock>()
                .ListensFor<IStartListener>(); // this is ticking

            sys.HasSingleton<TimerDialog>()
                .Provides<ITimerDialog>();

            sys.HasSingleton<TimerViewModel>()
                .Provides<TimerViewModel>();

            sys.HasSingleton<TimerController>()
                .Provides<ITimerUI>()
                .ListensFor<ITickListener>()
                .ListensFor<ITimeDialogEventListener>();

            return sys.Get<ITimerUI>();
        }
    }
}