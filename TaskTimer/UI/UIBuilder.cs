#region usings

using NDependencyInjection.DSL;

#endregion

namespace TaskTimer.UI
{
    public class UIBuilder : ISubsystemBuilder
    {
        public void Build ( ISystemDefinition sys )
        {
            sys.RelaysCallsTo<ITimeDialogEventListener> ();

            sys.HasSingleton<TimerDialog> ()
                .Provides<ITimerView> ();

            sys.HasSingleton<TimerViewModel> ()
                .Provides<TimerViewModel> ();

            sys.HasSingleton<TimerController> ()
                .Provides<ITimerUI> ()
                .HandlesCallsTo<ITickListener> ()
                .HandlesCallsTo<ITimeDialogEventListener> ();
        }
    }
}