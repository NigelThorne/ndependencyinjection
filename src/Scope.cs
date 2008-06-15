//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;

namespace NDependencyInjection
{
    /// <summary>
    /// </summary>
    public class Scope : IScope
    {
        private readonly Dictionary<Type, IServiceProvider> dictionary = new Dictionary<Type, IServiceProvider>();
        private readonly IServiceLocator outerScope;

        public Scope()
            : this(new NullServiceLocator())
        {
        }

        public Scope(IServiceLocator outerScope)
        {
            this.outerScope = outerScope;
        }

        public void RegisterServiceProvider<T>(IServiceProvider provider)
        {
            if (dictionary.ContainsKey(typeof (T)))
                throw new InvalidOperationException(String.Format("Type {0} is already registered", typeof (T)));

            dictionary[typeof (T)] = provider;
            provider.AddMapping(typeof (T));
        }

        public void RegisterServiceListener<EventsInterface>(IServiceProvider provider)
        {
            GetService<IBroadcaster<EventsInterface>>().AddListeners(new TypeResolvingConduit<EventsInterface>(provider, this).Proxy);
        }

        public void RegisterBroadcaster<EventsInterface>()
        {
            BroadcasterProvider<EventsInterface> provider = new BroadcasterProvider<EventsInterface>();
            RegisterServiceProvider<IBroadcaster<EventsInterface>>(provider);
            RegisterServiceProvider<EventsInterface>(provider);
        }

        public bool HasService(Type serviceType)
        {
            if (dictionary.ContainsKey(serviceType)) return true;
            return outerScope.HasService(serviceType);
        }

        public object GetService(Type serviceType)
        {
            if (dictionary.ContainsKey(serviceType))
            {
                return dictionary[serviceType].GetService(serviceType, serviceType, this);
            }
            return outerScope.GetService(serviceType);
        }

        public IServiceLocator Parent
        {
            get { return outerScope; }
        }

        public IScope CreateInnerScope()
        {
            return new Scope(this);
        }

        public void DecorateService<InterfaceType, DecoratingType>()
        {
            if (!HasService(typeof(InterfaceType)))
                throw new InvalidOperationException(String.Format("Type {0} not defined", typeof(InterfaceType)));

            IScope subsystem = CreateInnerScope();
            subsystem.RegisterServiceProvider<InterfaceType>(dictionary[typeof(InterfaceType)]);
         
            dictionary[typeof(InterfaceType)] = new DecoratingServiceProvider<DecoratingType>();
        }


        public void ReplaceServiceProvider<T1>(IServiceProvider provider)
        {
            if (!dictionary.ContainsKey(typeof(T1)))
                throw new InvalidOperationException(String.Format("Type {0} not defined so you can't replace it", typeof(T1)));
            dictionary[typeof(T1)] = provider;
        }

        private T GetService<T>()
        {
            return (T) GetService(typeof (T));
        }
    }
}