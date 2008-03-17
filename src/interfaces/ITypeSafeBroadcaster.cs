namespace NDependencyInjection.interfaces
{
    public interface ITypeSafeBroadcaster<ListenerType>
    {
        ListenerType Listener { get; }
        void AddListeners(params ListenerType[] newListerners);
    }
}