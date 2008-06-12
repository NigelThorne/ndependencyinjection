//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.interfaces
{
    public interface ISystemDefinition
    {
        IServiceDefinition HasInstance<S>(S instance);
        IServiceDefinition HasFactory<S>();
        IServiceDefinition HasSingleton<S>();
        IServiceDefinition HasCollection(params ISubsystemBuilder[] subsystems);
        IServiceDefinition HasSubsystem(ISubsystemBuilder subsystemBuilder);
        IServiceDefinition HasSubsystem(CreateSubsystem method);

        Service Get<Service>();
        void Broadcasts<T1>();
    }
}