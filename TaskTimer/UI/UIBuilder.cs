using NDependencyInjection.DSL;

namespace TaskTimer.UI
{
    public class UIBuilder : ISubsystemBuilder
    {

        public void Build(ISystemDefinition sys)
        {
            sys.RelaysCallsTo<IStartListener>();
            sys.RelaysCallsTo<ITickListener>();
            sys.RelaysCallsTo<ITimeDialogEventListener>();

            sys.HasSingleton<TickingClock>()
                .Provides<IClock>()
                .HandlesCallsTo<IStartListener>(); // this is ticking

            sys.HasSingleton<TimerDialog>()
                .Provides<ITimerDialog>();

            sys.HasSingleton<TimerViewModel>()
                .Provides<TimerViewModel>();

            sys.HasSingleton<TimerController>()
                .Provides<ITimerUI>()
                .HandlesCallsTo<ITickListener>()
                .HandlesCallsTo<ITimeDialogEventListener>();
        }
    }
}