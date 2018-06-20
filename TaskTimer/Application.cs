using System;
using System.Threading.Tasks;
using AmmySidekick;
using TaskTimer.Domain;
using TaskTimer.UI;

namespace TaskTimer
{
    public class Application : IRunnable, IViewClosedHandler, ITickListener
    {
        private readonly UI.IUIFactory _factory;
        private readonly IClock _clock;
        private readonly IScheduler _scheduler;
        private ITimerUI _currentUI;
        private object _UILock = new Object();

        public Application(UI.IUIFactory factory, IClock clock, IScheduler scheduler)
        {
            _factory = factory;
            _clock = clock;
            _scheduler = scheduler;
        }

        public void Run()
        {
            App app = new App();
            RuntimeUpdateHandler.Register(app, "/" + Ammy.GetAssemblyName(app) + ";component/App.g.xaml");

            _scheduler.ScheduleCallback(ShowUI, 10 * 60);
            _clock.StartTicking();
//            while (true)
//            {
//                System.Threading.Thread.Yield();
//            }
            app.Run();
        }

        private void ShowUI(DateTime time)
        {
            ITimerUI currentUI;
            lock (_UILock)
            {
                if (_currentUI == null) _currentUI = _factory.CreateUI();
                currentUI = _currentUI;
            }

            currentUI.ShowUI();
        }

        public void OnViewClosed()
        {
            lock (_UILock)
            {
                _currentUI = null;
            }
        }

        public void OnTick(DateTime time)
        {
            ITimerUI currentUI;
            lock (_UILock)
            {
                currentUI = _currentUI;
            }
            if (currentUI != null) currentUI.OnTick(time);

        }
    }
}