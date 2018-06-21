namespace NDependencyInjection.interfaces
{
    public interface IBroadcaster
    {
        object Listener { get; }
    }

    public interface IBroadcaster<ListenerType>
    {
        ListenerType Listener { get; }
        void AddListeners ( params ListenerType[] newListerners );
        void RemoveListeners ( params ListenerType[] oldListeners );
    }
}