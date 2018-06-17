using NDependencyInjection.Tests.ExampleUsage.UI;

namespace NDependencyInjection.Tests.ExampleUsage
{
    public class MainProcess : IEntryPoint
    {
        private readonly IUIFactory _factory;
        private readonly ITaskHistory _history;

        public MainProcess(IUIFactory factory, ITaskHistory history)
        {
            _factory = factory;
            _history = history;
        }

        public void Run()
        {
            var task = _history.LastAllocatedTask();
            var timerUI = _factory.CreateUI();
            timerUI.StartTimerUI(task.Name, task.StartTime);

        }
    }
}