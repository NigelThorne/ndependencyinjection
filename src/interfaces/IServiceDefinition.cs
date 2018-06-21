namespace NDependencyInjection.interfaces
{
    public interface IServiceDefinition
    {
        IServiceDefinition Provides<Interface> ( );
        IServiceDefinition HandlesCallsTo<EventsListener> ( );
        IServiceDefinition Decorates<Interface> ( );
        IServiceDefinition AddsToComposite<T> ( );
    }
}