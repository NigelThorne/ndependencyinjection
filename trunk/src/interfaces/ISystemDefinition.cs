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
        IDecoratingContext Decorate<S>();

        Service Get<Service>();
        void Broadcasts<T1>();

        /// <summary>
        /// Are you sure you don't want to use "HasSubsystem"? 
        /// </summary>
        /// <returns></returns>        
        ISystemDefinition CreateSubsystem(ISubsystemBuilder subsystem);
    }
}