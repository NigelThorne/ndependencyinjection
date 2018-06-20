//Copyright (c) 2008 Nigel Thorne

using System.Collections.Generic;
using System.Diagnostics;
using NDependencyInjection.DSL;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;

namespace NDependencyInjection
{
    //[DebuggerStepThrough]
    public class SystemDefinition : ISystemDefinition
    {
        private readonly IScope _scope;

        public SystemDefinition() : this(new Scope())
        {
        }

        private SystemDefinition(IScope scope)
        {
            this._scope = scope;
        }

        public void RelaysCallsTo<TMessage>()
        {
            _scope.RegisterBroadcaster<TMessage>();
        }

        public TService Get<TService>()
        {
            return (TService) _scope.GetService(typeof(TService));
        }

        public IServiceDefinition HasCollection(params ISubsystemBuilder[] subsystems)
        {
            var list = new List<IServiceProvider>();
            foreach (var subsystem in subsystems) list.Add(CreateSubsystemProvider(subsystem));
            return NewComponent(new CollectionProvider(list.ToArray()));
        }


        public IServiceDefinition HasFactory<TService>()
        {
            return NewComponent(new FactoryServiceProvider<TService>());
        }

        public IServiceDefinition HasInstance<TService>(TService instance)
        {
            return NewComponent(new InstanceServiceProvider(instance));
        }

        public IServiceDefinition HasSingleton<TService>()
        {
            return
                NewComponent(
                    new SingletonServiceProviderDecorator(
                        new FactoryServiceProvider<TService>()));
        }

        public IServiceDefinition HasSubsystem(ISubsystemBuilder subsystemBuilder)
        {
            return NewComponent(CreateSubsystemProvider(subsystemBuilder));
        }

        public IDecoratingContext Decorate<TService>()
        {
            return new DecoratorDefinition<TService>(_scope);
        }

        public IServiceDefinition HasComposite<TInterface>()
        {
            IServiceProvider provider = new CompositeProvider<TInterface>();
            _scope.RegisterServiceProvider(typeof(IComposite<TInterface>), provider);
            return NewComponent(provider);
        }

        /// <summary>
        /// CreateSubsystem returns a new _system_ definition with the previous 
        /// system definition as the parent.
        /// 
        ///     In most cases you are probably wanting "HasSubsystem" which returns a _service_ definition.
        /// </summary>
        /// <returns>A new system definition for a subsystem</returns>
        public ISystemDefinition CreateSubsystem(ISubsystemBuilder subsystem)
        {
            ISystemDefinition system = new SystemDefinition(_scope.CreateInnerScope());
            subsystem.Build(system);
            return system;
        }

        private IServiceProvider CreateSubsystemProvider(ISubsystemBuilder subsystemBuilder)
        {
            return new SubsystemProvider(CreateSubsystemWiring(subsystemBuilder));
        }

        private IServiceLocator CreateSubsystemWiring(ISubsystemBuilder subsystem)
        {
            var child = _scope.CreateInnerScope();
            subsystem.Build(new SystemDefinition(child));
            return child;
        }

        private IServiceDefinition NewComponent(IServiceProvider provider)
        {
            return new ServiceDefinition(_scope, provider);
        }
    }
}