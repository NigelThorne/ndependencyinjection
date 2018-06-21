#region usings

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskTimer.Annotations;

#endregion

namespace TaskTimer
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private string _comment;
        private int _minutesToAllocate;
        private string _taskName;
        private DateTime _timeAllocatedUpTo;
        private TimeSpan _unAllocatedTime;

        public TimeSpan UnAllocatedTime
        {
            get => _unAllocatedTime;
            set
            {
                _unAllocatedTime = value;
                OnPropertyChanged ();
            }
        }

        public int MinutesToAllocate
        {
            get => _minutesToAllocate;
            set
            {
                _minutesToAllocate = value;
                OnPropertyChanged ();
            }
        }

        public DateTime TimeAllocatedUpTo
        {
            get => _timeAllocatedUpTo;
            set
            {
                _timeAllocatedUpTo = value;
                OnPropertyChanged ();
            }
        }

        public string TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                OnPropertyChanged ();
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged ();
            }
        }

        public DateTime CurrentTaskStartedAt { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this, new PropertyChangedEventArgs ( propertyName ) );
        }
    }
}