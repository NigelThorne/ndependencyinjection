namespace NDependencyInjection.interfaces
{
    public interface ISystemDefinition
    {
        ISystemComponent HasInstance<S>(S instance);
        ISystemComponent HasFactory<S>();
        ISystemComponent HasSingleton<S>();
        ISystemComponent HasCollection(params ISubsystemBuilder[] subsystems);
        ISystemComponent HasSubsystem(ISubsystemBuilder subsystemBuilder);

        Service Get<Service>();
    }
}