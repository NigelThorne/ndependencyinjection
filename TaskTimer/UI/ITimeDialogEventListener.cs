namespace TaskTimer.UI
{
    public interface ITimeDialogEventListener
    {
        void OnUpdateClicked();
        void OnNewTaskClicked();
        void OnViewClosed();
    }
}