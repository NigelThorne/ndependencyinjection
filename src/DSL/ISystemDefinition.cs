//Copyright (c) 2008 Nigel Thorne

using NDependencyInjection.interfaces;
#pragma warning disable 1570

namespace NDependencyInjection.DSL
{
    public interface ISystemDefinition 
    {
        IServiceDefinition HasInstance<TService>(TService instance);
        IServiceDefinition HasFactory<TService>();
        IServiceDefinition HasSingleton<TService>();
        IServiceDefinition HasCollection(params ISubsystemBuilder[] subsystems);
        IServiceDefinition HasSubsystem(ISubsystemBuilder subsystemBuilder);

        /// <summary>
        /// Uses a type to decorate another type. 
        /// 
        /// Example: 
        /// _definition.HasSingleton<AddSomething>().Provides<IDoSomething>();
        /// _definition.Decorate<IDoSomething>().With<DoublingDecorator>();
        /// var addThenDouble = _definition.Get<IDoSomething>();
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        IDecoratingContext Decorate<TService>();
        
        /// <summary>
        /// HasComposite tells your scope that you have several implementations of this interface
        /// You can later retreaved as an array of services.
        /// 
        /// Example:
        /// _definition.HasComposite<IDoSomething>().Provides<IDoSomething[]>();
        /// _definition.HasSingleton<DoesSomething>().AddsToComposite<IDoSomething>();
        /// var composite = _definition.Get<IDoSomething[]>();
        /// 
        /// or you can get the composite directly and add to it.
        /// 
        /// _definition.HasComposite<IDoSomething>().Provides<IDoSomething[]>();
        /// var compositeProvider = _definition.Get<IComposite<IDoSomething>>();
        /// compositeProvider.Add(new DoesSomething());
        /// 
        /// var composite = _definition.Get<IDoSomething[]>();
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        IServiceDefinition HasComposite<TService>();

        TService Get<TService>();

        void RelaysCallsTo<TMessage>();

        /// <summary>
        /// CreateSubsystem returns a new _system_ definition with the previous 
        /// system definition as the parent.
        /// 
        ///     In most cases you are probably wanting "HasSubsystem" which returns a _service_ definition.
        /// </summary>
        /// <returns>A new system definition for a subsystem</returns>
        ISystemDefinition CreateSubsystem(ISubsystemBuilder subsystem);
    }
}