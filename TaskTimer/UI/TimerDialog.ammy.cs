﻿#region usings

using System.Windows;

#endregion

namespace TaskTimer.UI
{
    public partial class TimerDialog : ITimerView
    {
        private readonly ITimeDialogEventListener _listener;

        public TimerDialog ( TimerViewModel model, ITimeDialogEventListener listener )
        {
            _listener = listener;
            DataContext = model;
            InitializeComponent ();
            Closed += ( sender, args ) => listener.OnViewClosed ();
            Activated += ( sender, args ) => Topmost = false;
            ShowActivated = false;
        }

        void ITimerView.ShowDialog ( )
        {
            if ( !IsVisible ) Show ();
            Topmost = true;
        }

        bool ITimerView.IsVisible()
        {
            return IsVisible;
        }

        private void OnRenameButtonClicked ( object sender, RoutedEventArgs e )
        {
            _listener.OnUpdateClicked ();
        }

        private void OnAddButtonClicked ( object sender, RoutedEventArgs e )
        {
            _listener.OnNewAllocationClicked ();
        }
        private void OnAddBreakClicked ( object sender, RoutedEventArgs e )
        {
            _listener.OnNewBreakAllocationClicked ();
        }

        private void OnTaskNameFocus ( object sender, RoutedEventArgs e )
        {
            TaskNameBox.SelectAll ();
        }

        private void OnCommentFocus ( object sender, RoutedEventArgs e )
        {
            Comment.SelectAll ();
        }
    }
}