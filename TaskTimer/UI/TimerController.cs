using System;

namespace TaskTimer.UI
{
    public class TimerController : ITimerUI, ITickListener, ITimeDialogEventListener
    {
        private readonly IClock _clock;
        private readonly IStartListener _startListener;
        private readonly ITimerCommandsHandler _commandsHandlers;
        private readonly ITimerDialog _view;
        private readonly TimerViewModel _viewModel;

        public TimerController(
            TimerViewModel viewModel,
            ITimerDialog view,
            IClock clock,
            IStartListener startListener,
            ITimerCommandsHandler commandsHandlers)
        {
            _view = view;
            _viewModel = viewModel;
            _clock = clock;
            _startListener = startListener;
            _commandsHandlers = commandsHandlers;
        }

        void ITickListener.OnTick(DateTime now)
        {
            UpdateUnAllocatedTime();
        }

        void ITimeDialogEventListener.OnUpdateClicked()
        {
            var endtime = CalculateNewEndTime();

            _commandsHandlers.UpdateCurrentTask(
                _viewModel.TaskName, 
                _viewModel.Comment, 
                _viewModel.TimeAllocatedUpTo,
                endtime);

            UpdateViewModel(_viewModel.TaskName, endtime, endtime);
            
        }

        private DateTime CalculateNewEndTime()
        {
            return _viewModel.TimeAllocatedUpTo.AddMinutes(_viewModel.MinutesToAllocate);
        }

        void ITimeDialogEventListener.OnNewTaskClicked()
        {
            var endtime = CalculateNewEndTime();

            _commandsHandlers.AddNewTask(
                _viewModel.TaskName,
                _viewModel.TimeAllocatedUpTo,
                endtime, _viewModel.Comment);

            UpdateViewModel(_viewModel.TaskName, _viewModel.TimeAllocatedUpTo, endtime);
        }

        public void StartTimerUI(string currentTask, DateTime startTime, DateTime taskEndTime)
        {
            UpdateViewModel(currentTask, startTime, taskEndTime);

            _startListener.OnStart();
            _view.ShowDialog();
        }

        private void UpdateViewModel(string currentTask, DateTime startTime, DateTime taskEndTime)
        {
            _viewModel.TimeAllocatedUpTo = taskEndTime;
            _viewModel.TaskName = currentTask;
            _viewModel.CurrentTaskStartedAt = startTime;
            _viewModel.Comment = "";
            UpdateUnAllocatedTime();
            _viewModel.MinutesToAllocate = UnallocatedTimeInlMinutes();
        }

        private void UpdateUnAllocatedTime()
        {
            var allocateAllUnallocatedTime = _viewModel.MinutesToAllocate == UnallocatedTimeInlMinutes();
            _viewModel.UnAllocatedTime = _clock.CurrentTime() - _viewModel.TimeAllocatedUpTo;

            // follow unallocatedTime if we are set to max
            if (allocateAllUnallocatedTime)
                _viewModel.MinutesToAllocate = UnallocatedTimeInlMinutes();
        }

        private int UnallocatedTimeInlMinutes()
        {
            return (int)_viewModel.UnAllocatedTime.TotalMinutes;
        }
    }
}