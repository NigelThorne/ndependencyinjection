#region usings

using System;
using TaskTimer.Domain;

#endregion

namespace TaskTimer.UI
{
    public class TimerController : ITimerUI, ITickListener, ITimeDialogEventListener
    {
        private readonly IClock _clock;
        private readonly ITasksDomainController _tasksDomainController;
        private readonly IViewClosedHandler _closeHandler;
        private readonly ITimerView _view;
        private readonly TimerViewModel _viewModel;

        public TimerController(
            TimerViewModel viewModel,
            ITimerView view,
            IClock clock,
            ITasksDomainController tasksDomainController,
            IViewClosedHandler closeHandler )
        {
            _view = view;
            _viewModel = viewModel;
            _clock = clock;
            _tasksDomainController = tasksDomainController;
            _closeHandler = closeHandler;
        }

        void ITimerUI.ShowUI()
        {
            var task = _tasksDomainController.CurrentTask;
            UpdateViewModel(task.Name, task.StartTime, task.EndTime);

            _view.ShowDialog();
        }

        void ITickListener.OnTick(DateTime now)
        {
            UpdateUnAllocatedTime();
        }

        void ITimeDialogEventListener.OnUpdateClicked()
        {
            var endtime = CalculateNewEndTime();

            _tasksDomainController.UpdateCurrentTask(
                _viewModel.TaskName,
                _viewModel.Comment,
                _viewModel.TimeAllocatedUpTo,
                endtime);

            UpdateViewModel(_viewModel.TaskName, endtime, endtime);
        }

        void ITimeDialogEventListener.OnNewTaskClicked()
        {
            var endtime = CalculateNewEndTime();

            _tasksDomainController.AddNewTask(
                _viewModel.TaskName,
                _viewModel.TimeAllocatedUpTo,
                endtime, _viewModel.Comment);

            UpdateViewModel(_viewModel.TaskName, _viewModel.TimeAllocatedUpTo, endtime);
        }

        void ITimeDialogEventListener.OnViewClosed()
        {
            _closeHandler.OnViewClosed();
        }

        private DateTime CalculateNewEndTime()
        {
            return _viewModel.TimeAllocatedUpTo.AddMinutes(_viewModel.MinutesToAllocate);
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
            return (int) _viewModel.UnAllocatedTime.TotalMinutes;
        }
    }
}