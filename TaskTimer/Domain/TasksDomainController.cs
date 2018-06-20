using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskTimer.Domain
{
    public class TasksDomainController : ITasksDomainController
    {
        private readonly ITaskRepository _repository;
        private readonly IList<TimerTask> _list;

        public TasksDomainController(ITaskRepository repository)
        {
            _repository = repository;
            _list = _repository.LoadTasks();
            if (_list.Count == 0) _list.Add(TimerTask.CreateDefaultTask());
        }

        public TimerTask CurrentTask => _list.LastOrDefault();

        public void ReplaceCurrentTask(TimerTask timerTask)
        {
            // TODO: Add rules to make sure this task starts at the same time...?
            
            var task = CurrentTask;
            _list.Remove(task);
            _list.Add(timerTask);
            _repository.SaveTasks(_list);
        }

        public void RenameCurrentTask(string name)
        {
            // TODO: make Tasks Imutable
            CurrentTask.Name = name;
            _repository.SaveTasks(_list);
        }

        public void UpdateCurrentTask(
            string taskName,
            string comment,
            DateTime startAt,
            DateTime endAt)
        {
            if (CurrentTask.Name != taskName)
                RenameCurrentTask(taskName);
            CurrentTask.AddAllocation( comment, startAt, endAt);
        }

        public void AddNewTask(string taskName,
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

            _repository.SaveTasks(_list);
        }
    }
}