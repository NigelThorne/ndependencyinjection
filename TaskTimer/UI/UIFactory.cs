﻿#region usings

using NDependencyInjection;
using NDependencyInjection.DSL;
using TaskTimer.Domain;

#endregion

namespace TaskTimer.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly IClock _clock;
        private readonly IViewClosedHandler _closedHander;
        private readonly IScheduler _scheduler;
        private readonly ITasksDomainController _tasksDomainController;

        public UIFactory (
            ITasksDomainController tasksDomainController,
            IViewClosedHandler closedHander,
            IClock clock,
            IScheduler scheduler )
        {
            _tasksDomainController = tasksDomainController;
            _closedHander = closedHander;
            _clock = clock;
            _scheduler = scheduler;
        }

        public ITimerUI CreateUI ( )
        {
            ISystemDefinition sys = new SystemDefinition ();
            sys.RelaysCallsTo<ITickListener> ();
            sys.HasInstance ( _tasksDomainController ).Provides<ITasksDomainController> ();
            sys.HasInstance ( _closedHander ).Provides<IViewClosedHandler> ();
            sys.HasInstance ( _scheduler ).Provides<IScheduler> ();
            sys.HasSingleton<TickingClock> ().Provides<IClock> ();
            sys.HasSubsystem ( new UIBuilder () ).Provides<ITimerUI> ();
            return sys.Get<ITimerUI> ();
        }
    }
}