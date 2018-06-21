using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TaskTimer.UI
{
    public partial class TimerDialog : ITimerView
    {
        private readonly ITimeDialogEventListener _listener;

        public TimerDialog(TimerViewModel model, ITimeDialogEventListener listener)
        {
            _listener = listener;
            this.DataContext = model;
            InitializeComponent();
            this.Closed += (sender, args) => listener.OnViewClosed();
            this.Activated += (sender, args) => this.Topmost = false;
        }

        void ITimerView.ShowDialog()
        {
            if(!this.IsVisible) this.Show();
            this.Topmost = true;
        }

        private void OnRenameButtonClicked(object sender, RoutedEventArgs e)
        {
            _listener.OnUpdateClicked();
        }

        private void OnAddButtonClicked(object sender, RoutedEventArgs e)
        {
            _listener.OnNewAllocationClicked();
        }
        
        private void OnTaskNameFocus(object sender, RoutedEventArgs e)
        {
            this.TaskNameBox.SelectAll();
        }
        private void OnCommentFocus(object sender, RoutedEventArgs e)
        {
            this.Comment.SelectAll();
        }

    }
}
