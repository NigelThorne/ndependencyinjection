#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#endregion

namespace TaskTimer.Domain
{
    [SuppressMessage ( "ReSharper", "ClassNeverInstantiated.Global" )]
    public class TasksDomainController : ITasksDomainController
    {
        private readonly IList<TimerTask> _list;
        private readonly ITaskRepository _repository;

        public TasksDomainController ( ITaskRepository repository )
        {
            _repository = repository;
            _list = _repository.LoadTasks ();
            if ( _list.Count == 0 ) _list.Add ( TimerTask.CreateDefaultTask () );
        }

        public TimerTask CurrentTask => _list.LastOrDefault ();

        public void UpdateCurrentTask (
            string taskName,
            string comment,
            DateTime startAt,
            DateTime endAt )
        {
            if ( CurrentTask.Name != taskName )
            {
                // TODO: Add rules to make sure this task starts at the same time...?
                _list.Remove ( CurrentTask );
                _list.Add ( new TimerTask ( taskName, CurrentTask.Allocations ) );
                _repository.SaveTasks ( _list );
            }

            CurrentTask.AddAllocation ( comment, startAt, endAt );
        }

        public void AddNewTask (
            string taskName,
            string comment,
            DateTime startAt,
            DateTime endAt )
        {
            _list.Add ( new TimerTask ( taskName, new[]
                {
                    new TimerTask.Allocation ()
                    {
                        Comment = comment,
                        StartTime = startAt,
                        EndTime = endAt
                    },
                } )
            );

            _repository.SaveTasks ( _list );
        }

        public void AddNewBreakTask(string taskName, string comment, DateTime startAt, DateTime endAt)
        {
            var currentTaskName = CurrentTask.Name;
            _list.Add ( new TimerTask ( taskName, new[]
                {
                    new TimerTask.Allocation ()
                    {
                        Comment = comment,
                        StartTime = startAt,
                        EndTime = endAt
                    },
                } )
            );
            _list.Add ( new TimerTask ( currentTaskName, new[]
                {
                    new TimerTask.Allocation ()
                    {
                        Comment = "",
                        StartTime = endAt,
                        EndTime = endAt
                    },
                } )
            );
            _repository.SaveTasks ( _list );
            
        }
    }
}