namespace TaskTimer.UI
{
    public interface ITimeDialogEventListener
    {
        void OnUpdateClicked ( );
        void OnNewAllocationClicked ( );
        void OnViewClosed ( );
    }
}