using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskTimer.Domain
{
    public class TaskHistory : ITaskHistory, ITimerCommandsHandler
    {
        private readonly ITaskRepository _repository;
        private readonly List<TimerTask> _list;

        public TaskHistory(ITaskRepository repository)
        {
            _repository = repository;
            _list = _repository.LoadList();
//            _list = new List<TimerTask>();
            if (_list.Count == 0) _list.Add(TimerTask.CreateDefaultTask());
        }

        public TimerTask CurrentTask()
        {
            return _list.LastOrDefault();
        }

        public void ReplaceCurrentTask(TimerTask timerTask)
        {
            // TODO: Add rules to make sure this task starts at the same time...?
            
            var task = CurrentTask();
            _list.Remove(task);
            _list.Add(timerTask);
            _repository.SaveHistory(_list);
        }

        public void RenameCurrentTask(string name)
        {
            // TODO: make Tasks Imutable
            CurrentTask().Name = name;
            _repository.SaveHistory(_list);
        }
        void ITimerCommandsHandler.UpdateCurrentTask(
            string taskName,
            string comment,
            DateTime startAt,
            DateTime endAt)
        {
            if (CurrentTask().Name != taskName)
                RenameCurrentTask(taskName);
            CurrentTask().AddAllocation( comment, startAt, endAt);
        }

        void ITimerCommandsHandler.AddNewTask(string taskName,
            DateTime startAt,
            DateTime endAt,
            string comment)
        {
            _list.Add(new TimerTask()
            {
                Name = taskName,
                Allocations = new List<TimerTask.Allocation>(new []
                {
                    new TimerTask.Allocation()
                    {
                        Comment = comment,
                        StartTime = startAt,
                        EndTime = endAt
                    }, 
                })
            });

            _repository.SaveHistory(_list);
        }
    }
}