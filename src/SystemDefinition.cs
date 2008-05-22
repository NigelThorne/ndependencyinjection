//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class SystemDefinition : ISystemDefinition
    {
        private readonly ISystemWiring wiring;

        public SystemDefinition() : this(new SystemWiring())
        {
        }

        public SystemDefinition(ISystemWiring wiring)
        {
            this.wiring = wiring;
        }

        [Obsolete("use the HasSubsystem Syntax")]
        public SystemDefinition(SystemDefinition parent) : this(parent.wiring.CreateSubsystem())
        {
        }

        public void Broadcasts<S>()
        {
            wiring.RegisterBroadcaster<S>();
        }

        public Service Get<Service>()
        {
            return (Service) wiring.GetService(typeof (Service));
        }

        public ISystemComponent HasCollection(params ISubsystemBuilder[] subsystems)
        {
            List<IServiceLocator> list = new List<IServiceLocator>();
            foreach (ISubsystemBuilder subsystem in subsystems)
            {
                list.Add(CreateSubsystemWiring(subsystem));
            }
            return NewComponent(new CollectionProvider(list.ToArray()));
        }

        public ISystemComponent HasFactory<S>()
        {
            return NewComponent(new FactoryServiceProvider<S>(wiring));
        }

        public ISystemComponent HasInstance<S>(S instance)
        {
            return NewComponent(new FixedInstanceServiceProvider(instance));
        }

        public ISystemComponent HasSingleton<S>()
        {
            return
                NewComponent(
                    new SingletonServiceProviderDecorator(
                        new FactoryServiceProvider<S>(wiring)));
        }

        public ISystemComponent HasSubsystem(ISubsystemBuilder subsystemBuilder)
        {
            return NewComponent(new SubsystemProvider(CreateSubsystemWiring(subsystemBuilder)));
        }

        private IServiceLocator CreateSubsystemWiring(ISubsystemBuilder subsystem)
        {
            ISystemWiring child = wiring.CreateSubsystem();
            subsystem.Build(new SystemDefinition(child));
            return child;
        }

        private ISystemComponent NewComponent(IServiceProvider provider)
        {
            return new SystemComponent(wiring, provider);
        }
    }
}