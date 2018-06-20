using System;
using TaskTimer.Domain;
using TaskTimer.UI;

namespace TaskTimer
{
    public class Application : IRunnable
    {
        private readonly UI.IUIFactory _factory;
        private readonly ITaskHistory _history;

        public Application(
            UI.IUIFactory factory, 
            ITaskHistory history)
        {
            _factory = factory;
            _history = history;
        }

        public void Run()
        {
            var task = _history.CurrentTask();
            var timerUI = _factory.CreateUI();
            timerUI.StartTimerUI(task.Name, task.StartTime, task.EndTime);

        }


    }
}