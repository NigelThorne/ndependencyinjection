//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;


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

        public ISystemComponent HasFactory<S>()
        {
            return new SystemComponent(wiring, new DependencyResolvingServiceProvider<S>(wiring));
        }

        public ISystemComponent HasSingleton<S>()
        {
            return
                new SystemComponent(wiring,
                                    new SingletonServiceProviderDecorator(
                                        new DependencyResolvingServiceProvider<S>(wiring)));
        }

        public Service Get<Service>()
        {
            return (Service) wiring.GetService(typeof (Service));
        }

        public void Broadcasts<S>()
        {
            wiring.RegisterBroadcaster<S>();
        }

        public ISystemComponent HasInstance<S>(S instance)
        {
            return new SystemComponent(wiring, new FixedInstanceServiceProvider(instance));
        }

        public ISystemComponent HasCollection(params ISubsystemBuilder[] subsystems)
        {
            List<IServiceLocator> list = new List<IServiceLocator>();
            foreach (ISubsystemBuilder subsystem in subsystems)
            {
                list.Add(CreateSubsystemWiring(subsystem));
            }
            return new SystemComponent(wiring, new CollectionProvider(list.ToArray()));
        }

        public ISystemComponent HasSubsystem(ISubsystemBuilder subsystemBuilder)
        {
            return new SystemComponent(wiring, new SubsystemProvider(CreateSubsystemWiring(subsystemBuilder)));
        }

        private IServiceLocator CreateSubsystemWiring(ISubsystemBuilder subsystem)
        {
            ISystemWiring child = wiring.CreateSubsystem();
            subsystem.Build(new SystemDefinition(child));
            return child;
        }
    }
}