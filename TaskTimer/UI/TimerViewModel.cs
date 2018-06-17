using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskTimer.Annotations;
using TaskTimer.UI;

namespace TaskTimer
{
    public class TimerViewModel: INotifyPropertyChanged
    {
        private TimeSpan _unAllocatedTime;
        private DateTime _timeAllocatedUpTo;
        private string _taskName;
        private string _comment;
        private int _minutesToAllocate;

        public TimeSpan UnAllocatedTime
        {
            get => _unAllocatedTime;
            set
            {
                _unAllocatedTime = value;
                OnPropertyChanged();
            }
        }

        public int MinutesToAllocate
        {
            get => _minutesToAllocate;
            set
            {
                _minutesToAllocate = value;
                OnPropertyChanged();
            }
        }

        public DateTime TimeAllocatedUpTo
        {
            get => _timeAllocatedUpTo;
            set
            {
                _timeAllocatedUpTo = value;
                OnPropertyChanged();
            }
        }

        public string TaskName
        {
            get => _taskName;
            set
            {
                _taskName = value;
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }

        public DateTime CurrentTaskStartedAt { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}