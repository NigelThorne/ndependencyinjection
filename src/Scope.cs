#region usings

using System;
using System.Collections.Generic;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection
{
    //[DebuggerStepThrough]
    public class Scope : IScope
    {
        private readonly Dictionary<Type, IServiceProvider>
            _providersByType = new Dictionary<Type, IServiceProvider> ();

        private Type _pendingType = null;

        public Scope ( )
            : this ( new NullServiceLocator () )
        {
        }

        public Scope ( IServiceLocator outerScope )
        {
            Parent = outerScope;
        }

        public void RegisterServiceProvider ( Type serviceType, IServiceProvider provider )
        {
            if ( _providersByType.ContainsKey ( serviceType ) )
                throw new InvalidOperationException ( $"Type {serviceType} is already registered" );

            _providersByType[serviceType] = provider;

            provider.AddMapping ( serviceType );
        }

        public void RegisterServiceListener<TEventsInterface> ( IServiceProvider provider )
        {
            GetService<IBroadcaster<TEventsInterface>> ()
                .AddListeners ( new TypeResolvingConduit<TEventsInterface> ( provider, this ).Proxy );
        }

        public void RegisterBroadcaster<TEventsInterface> ( )
        {
            var provider = new BroadcasterProvider<TEventsInterface> ();
            RegisterServiceProvider ( typeof (IBroadcaster<TEventsInterface>), provider );
            RegisterServiceProvider ( typeof (TEventsInterface), provider );
        }

        public bool HasService ( Type serviceType )
        {
            return _providersByType.ContainsKey ( serviceType ) || Parent.HasService ( serviceType );
        }

        public object GetService ( Type serviceType )
        {
            if ( _pendingType == serviceType )
                throw new InvalidWiringException ( "You have a scope defined as providing type " + serviceType +
                                                   " but it doesn't" );
            if ( _providersByType.ContainsKey ( serviceType ) )
                return _providersByType[serviceType].GetService ( serviceType, serviceType, this );
            _pendingType = serviceType;
            var service = Parent.GetService ( serviceType );
            _pendingType = null;
            return service;
        }

        public IServiceLocator Parent { get; }

        public IScope CreateInnerScope ( )
        {
            return new Scope ( this );
        }

        public void DecorateService<TInterfaceType> ( IServiceProvider decorator )
        {
            if ( !HasService ( typeof (TInterfaceType) ) )
                throw new InvalidOperationException ( $"Type {typeof (TInterfaceType)} not defined" );

            if ( !_providersByType.ContainsKey ( typeof (TInterfaceType) ) )
                _providersByType[typeof (TInterfaceType)] = new ScopeQueryingProvider ( Parent );
            _providersByType[typeof (TInterfaceType)] =
                new DecoratingServiceProvider<TInterfaceType> ( _providersByType[typeof (TInterfaceType)], decorator );
        }

        public void RegisterCompositeItem<TInterface> ( IServiceProvider provider )
        {
            GetService<IComposite<TInterface>> ().Add ( new TypeResolvingConduit<TInterface> ( provider, this ).Proxy );
        }

        public void ReplaceServiceProvider<T1> ( IServiceProvider provider )
        {
            if ( !_providersByType.ContainsKey ( typeof (T1) ) )
                throw new InvalidOperationException ( $"Type {typeof (T1)} not defined so you can't replace it" );
            _providersByType[typeof (T1)] = provider;
        }

        private T GetService<T> ( )
        {
            return (T) GetService ( typeof (T) );
        }
    }
}