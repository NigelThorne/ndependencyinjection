//Copyright (c) 2008 Nigel Thorne
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;

namespace NDependencyInjection
{
    public class SystemDefinition : ISystemDefinition
    {
        private readonly IServiceScope scope;

        public SystemDefinition() : this(new ServiceRepository())
        {
        }

        private SystemDefinition(IServiceScope scope)
        {
            this.scope = scope;
        }

        #region ISystemDefinition Members

        public void Broadcasts<S>()
        {
            scope.RegisterBroadcaster<S>();
        }

        public Service Get<Service>()
        {
            return (Service) scope.GetService(typeof (Service));
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
            return NewComponent(new FactoryServiceProvider<S>(scope));
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
                        new FactoryServiceProvider<S>(scope)));
        }

        public ISystemComponent HasSubsystem(ISubsystemBuilder subsystemBuilder)
        {
            return NewComponent(new SubsystemProvider(CreateSubsystemWiring(subsystemBuilder)));
        }

        #endregion

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
            IServiceScope child = scope.CreateChildScope();
            subsystem.Build(new SystemDefinition(child));
            return child;
        }

        private ISystemComponent NewComponent(IServiceProvider provider)
        {
            return new SystemComponent(scope, provider);
        }
    }
}