//Copyright (c) 2008 Nigel Thorne
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;

namespace NDependencyInjection
{
    public class SystemDefinition : ISystemDefinition
    {
        private readonly IScope scope;

        public SystemDefinition() : this(new Scope())
        {
        }

        private SystemDefinition(IScope scope)
        {
            this.scope = scope;
        }

        public void Broadcasts<S>()
        {
            scope.RegisterBroadcaster<S>();
        }

        public Service Get<Service>()
        {
            return (Service) scope.GetService(typeof (Service));
        }

        public IServiceDefinition HasCollection(params ISubsystemBuilder[] subsystems)
        {
            List<IServiceLocator> list = new List<IServiceLocator>();
            foreach (ISubsystemBuilder subsystem in subsystems)
            {
                list.Add(CreateSubsystemWiring(subsystem));
            }
            return NewComponent(new CollectionProvider(list.ToArray()));
        }

        public IServiceDefinition HasFactory<S>()
        {
            return NewComponent(new FactoryServiceProvider<S>());
        }

        public IServiceDefinition HasInstance<S>(S instance)
        {
            return NewComponent(new InstanceServiceProvider(instance));
        }

        public IServiceDefinition HasSingleton<S>()
        {
            return
                NewComponent(
                    new SingletonServiceProviderDecorator(
                        new FactoryServiceProvider<S>()));
        }

        public IServiceDefinition HasSubsystem(ISubsystemBuilder subsystemBuilder)
        {
            return NewComponent(new SubsystemProvider(CreateSubsystemWiring(subsystemBuilder)));
        }

        public IServiceDefinition HasSubsystem(CreateSubsystem method)
        {
            return HasSubsystem(new DelegateExecutingBuilder(method));
        }

        public IDecoratingContext Decorate<S>()
        {
            return new DecoratorDefinition<S>(scope);
        }

        /// <summary>
        /// Are you sure you don't want to use "HasSubsystem"? 
        /// </summary>
        /// <returns></returns>
        public ISystemDefinition CreateSubsystem(ISubsystemBuilder subsystem)
        {
            ISystemDefinition system = new SystemDefinition(scope.CreateInnerScope());
            subsystem.Build(system);
            return system;
        }

        private IServiceLocator CreateSubsystemWiring(ISubsystemBuilder subsystem)
        {
            IScope child = scope.CreateInnerScope();
            subsystem.Build(new SystemDefinition(child));
            return child;
        }

        private IServiceDefinition NewComponent(IServiceProvider provider)
        {
            return new ServiceDefinition(scope, provider);
        }
    }

}