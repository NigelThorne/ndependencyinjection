//Copyright (c) 2008 Nigel Thorne
using System;
using NDependencyInjection.Generics;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    /// <summary>
    /// A serviceProvider that returns either the IBroadcaster(TX) or the broadcaster as a TX 
    /// </summary>
    /// <typeparam name="TEventsInterface"></typeparam>
    public class BroadcasterProvider<TEventsInterface> : IServiceProvider
    {
        private IBroadcaster<TEventsInterface> _broadcaster;

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            if (!typeof (TEventsInterface).IsAssignableFrom(interfaceType) 
                && !typeof (IBroadcaster<TEventsInterface>).IsAssignableFrom(interfaceType))
                throw new InvalidProgramException(
                    $"Broadcaster supports type {typeof(TEventsInterface)} not {interfaceType}");

            if (_broadcaster == null)
            {
                _broadcaster = new TypeSafeBroadcaster<TEventsInterface>();
            }

            if (serviceType == typeof(IBroadcaster<TEventsInterface>))
            {
                return _broadcaster;
            }
            return _broadcaster.Listener;
        }

        public void AddMapping(Type serviceType)
        {
            
        }
    }
}