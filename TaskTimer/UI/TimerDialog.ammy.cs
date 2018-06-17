using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskTimer.UI
{
    public partial class TimerDialog : ITimerDialog
    {
        private readonly ITimeDialogEventListener _listener;

        public TimerDialog(TimerViewModel model, ITimeDialogEventListener listener)
        {
            _listener = listener;
            this.DataContext = model;
            InitializeComponent();
        }

        void ITimerDialog.ShowDialog()
        {
            this.ShowDialog();
        }

        private void OnUpdateClicked(object sender, RoutedEventArgs e)
        {
            _listener.OnUpdateClicked();
        }

        private void OnNewTaskClicked(object sender, RoutedEventArgs e)
        {
            _listener.OnNewTaskClicked();
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
