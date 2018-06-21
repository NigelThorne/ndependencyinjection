#region usings

using System;
using System.Collections.Generic;
using System.Reflection;
using LinFu.DynamicProxy;
using NDependencyInjection.interfaces;

#endregion

namespace NDependencyInjection.Generics
{
    /// <summary>
    ///     This is the interceptor that implements the IBroadcaster interface.
    ///     This is used by MessageBroadcastProvider
    /// </summary>
    /// <typeparam name="ListenerType"></typeparam>
    public class TypeSafeBroadcaster<ListenerType> : IBroadcaster<ListenerType>, IInterceptor
    {
        private readonly List<ListenerType> listeners = new List<ListenerType> ();

        public TypeSafeBroadcaster ( )
        {
            var factory = new ProxyFactory ();
            Listener = factory.CreateProxy<ListenerType> ( this );
        }

        public void AddListeners ( params ListenerType[] newListerners )
        {
            listeners.AddRange ( newListerners );
        }

        public void RemoveListeners ( params ListenerType[] oldListeners )
        {
            foreach ( var item in oldListeners ) listeners.Remove ( item );
        }

        public ListenerType Listener { get; }

        //[DebuggerStepThrough]
        object IInterceptor.Intercept ( InvocationInfo info )
        {
            try
            {
                if ( info.TargetMethod.ReturnType != typeof (void) )
                    throw new InvalidOperationException ( "You can only broadcast void methods." );

                var copyOfListeners = listeners.ToArray ();
                foreach ( var childListeners in copyOfListeners )
                    info.TargetMethod.Invoke ( childListeners, info.Arguments );
                return null;
            }
            catch ( TargetInvocationException ex )
            {
                throw ex.InnerException;
            }
        }
    }
}