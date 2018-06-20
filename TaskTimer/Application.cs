using System;
using TaskTimer.Domain;
using TaskTimer.UI;

namespace TaskTimer
{
    public class Application : IRunnable
    {
        private readonly UI.IUIFactory _factory;

        public Application(UI.IUIFactory factory)
        {
            _factory = factory;
        }

        public void Run()
        {
            _factory.CreateUI().StartTimerUI();
        }
    }
}