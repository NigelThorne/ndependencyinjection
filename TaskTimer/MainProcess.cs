using System;
using AmmySidekick;
using NDependencyInjection;
using NDependencyInjection.DSL;
using TaskTimer.Domain;

namespace TaskTimer
{
    public class MainProcess : IRunnable, UI.ITimerUpdateHandler
    {
        private readonly UI.IUIFactory _factory;
        private readonly ITaskHistory _history;

        [STAThread]
        public static void Main()
        {
            App app = new App();
            var runnable = CreateApplication();

            RuntimeUpdateHandler.Register(app, "/" + Ammy.GetAssemblyName(app) + ";component/App.g.xaml");

            runnable.Run();
        }

        private static IRunnable CreateApplication()
        {
            ISystemDefinition sys = new SystemDefinition();
            sys.BroadcastsTo<UI.ITimerUpdateHandler>();

            sys.HasSingleton<MainProcess>()
                .Provides<IRunnable>()
                .ListensFor<UI.ITimerUpdateHandler>();

            sys.HasSingleton<TaskRepository>()
                .Provides<ITaskRepository>();

            sys.HasSingleton<TaskHistory>()
                .Provides<ITaskHistory>();

            sys.HasFactory<UI.UIFactory>()
                .Provides<UI.IUIFactory>();

            return sys.Get<IRunnable>();
        }


        public MainProcess(
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

        void UI.ITimerUpdateHandler.UpdateCurrentTask(
            string taskName, 
            string comment, 
            DateTime startAt, 
            DateTime endAt)
        {
            if(_history.CurrentTask().Name != taskName)
                _history.RenameCurrentTask(taskName);
            _history.AddAllocationToCurrentTask(startAt, endAt, comment);
        }

        void UI.ITimerUpdateHandler.AddNewTask(
            string taskName, 
            string comment, 
            DateTime startAt, 
            DateTime endAt)
        {
            _history.AddNewTask(taskName, startAt, endAt, comment);
        }
    }
}