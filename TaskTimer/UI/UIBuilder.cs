using NDependencyInjection.DSL;

namespace TaskTimer.UI
{
    public class UIBuilder : ISubsystemBuilder
    {

        public void Build(ISystemDefinition sys)
        {
            sys.BroadcastsTo<IStartListener>();
            sys.BroadcastsTo<ITickListener>();
            sys.BroadcastsTo<ITimeDialogEventListener>();

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
        }
    }
}