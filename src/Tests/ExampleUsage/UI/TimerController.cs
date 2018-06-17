using System;

namespace NDependencyInjection.Tests.ExampleUsage.UI
{
        public class  TimerController: ITimerUI, ITickListener
        {
            private readonly ITimerDialog _view;
            private readonly TimerViewModel _viewModel;
            private readonly IClock _clock;
            private readonly IStartListener _startListener;

            public TimerController(
                TimerViewModel viewModel, 
                ITimerDialog view, 
                IClock clock, 
                IStartListener startListener)
            {
                _view = view;
                _viewModel = viewModel;
                _clock = clock;
                _startListener = startListener;
            }

            public void StartTimerUI(string currentTask, DateTime startTime)
            {
                _viewModel.TimeAllocatedUpTo = _clock.CurrentTime();
                _viewModel.TaskName = currentTask;
                _viewModel.CurrentTaskStartedAt = startTime;

                _startListener.OnStart();
                _view.ShowDialog();
            }

            void ITickListener.OnMinuteTick()
            {
                _viewModel.UnAllocatedTime = _clock.CurrentTime() - _viewModel.TimeAllocatedUpTo;
            }

            public void Dispose()
            {

            }
        }
}