//Copyright (c) 2008 Nigel Thorne
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;

namespace NDependencyInjection
{
    public class SystemDefinition : ISystemDefinition
    {
        private readonly IScope scope;

        public SystemDefinition() : this(new ServiceRepository())
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
            return NewComponent(new FactoryServiceProvider<S>(scope));
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
                        new FactoryServiceProvider<S>(scope)));
        }

        public IServiceDefinition HasSubsystem(ISubsystemBuilder subsystemBuilder)
        {
            return NewComponent(new SubsystemProvider(CreateSubsystemWiring(subsystemBuilder)));
        }

        /// <summary>
        /// Are you sure you don't want to use "HasSubsystem"? 
        /// </summary>
        /// <returns></returns>
        public ISystemDefinition CreateSubsystem()
        {
            return new SystemDefinition(scope.CreateChildScope());
        }

        private IServiceLocator CreateSubsystemWiring(ISubsystemBuilder subsystem)
        {
            IScope child = scope.CreateChildScope();
            subsystem.Build(new SystemDefinition(child));
            return child;
        }

        private IServiceDefinition NewComponent(IServiceProvider provider)
        {
            return new ServiceDefinition(scope, provider);
        }
    }
}