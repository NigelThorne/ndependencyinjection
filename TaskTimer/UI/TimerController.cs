#region usings

using System;
using TaskTimer.Domain;

#endregion

namespace TaskTimer.UI
{
    public class TimerController : ITimerUI, ITickListener, ITimeDialogEventListener
    {
        private readonly IClock _clock;
        private readonly IViewClosedHandler _closeHandler;
        private readonly ITasksDomainController _tasksDomainController;
        private readonly ITimerView _view;
        private readonly TimerViewModel _viewModel;

        public TimerController (
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

        void ITimeDialogEventListener.OnUpdateClicked ( )
        {
            CalculateAndTriggerChange ( _tasksDomainController.UpdateCurrentTask );
        }

        void ITimeDialogEventListener.OnNewAllocationClicked ( )
        {
            CalculateAndTriggerChange ( _tasksDomainController.AddNewTask );
        }

        void ITimeDialogEventListener.OnNewBreakAllocationClicked()
        {
            CalculateAndTriggerChange ( _tasksDomainController.AddNewBreakTask );
        }

        void ITimeDialogEventListener.OnViewClosed ( )
        {
            _closeHandler.OnViewClosed ();
        }

        void ITimerUI.ShowUI ( )
        {
            if (!_view.IsVisible())
            {
                var task = _tasksDomainController.CurrentTask;
                UpdateViewModel(task.Name, task.StartTime, task.EndTime);
            }

            _view.ShowDialog ();
        }

        void ITickListener.OnTick ( DateTime now )
        {
            UpdateUnAllocatedTime ();
        }

        private void CalculateAndTriggerChange ( Action<string, string, DateTime, DateTime> controllerAction )
        {
            var endtime = CalculateNewEndTime ();

            controllerAction (
                _viewModel.TaskName,
                _viewModel.Comment,
                _viewModel.TimeAllocatedUpTo,
                endtime );

            UpdateViewModel ( _viewModel.TaskName, endtime, endtime );
            if ( UnallocatedTimeInMinutes () == 0 ) _view.Hide ();
        }

        private DateTime CalculateNewEndTime ( )
        {
            var endTime = _viewModel.TimeAllocatedUpTo.AddMinutes ( _viewModel.MinutesToAllocate );
            if ( endTime > _clock.CurrentTime () ) endTime = _clock.CurrentTime ();
            return endTime;
        }

        private void UpdateViewModel ( string currentTask, DateTime startTime, DateTime taskEndTime )
        {
            _viewModel.TimeAllocatedUpTo = taskEndTime;
            _viewModel.TaskName = currentTask;
            _viewModel.CurrentTaskStartedAt = startTime;
            _viewModel.Comment = "";
            UpdateUnAllocatedTime ();
            _viewModel.MinutesToAllocate = UnallocatedTimeInMinutes ();
        }

        private void UpdateUnAllocatedTime ( )
        {
            var allocateAllUnallocatedTime = _viewModel.MinutesToAllocate == UnallocatedTimeInMinutes ();
            _viewModel.UnAllocatedTime = _clock.CurrentTime () - _viewModel.TimeAllocatedUpTo;

            // follow unallocatedTime if we are set to max
            if ( allocateAllUnallocatedTime )
                _viewModel.MinutesToAllocate = UnallocatedTimeInMinutes ();
        }

        public int UnallocatedTimeInMinutes ( )
        {
            return (int) _viewModel.UnAllocatedTime.TotalMinutes;
        }
    }
}