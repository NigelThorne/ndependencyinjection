#region usings

using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection.Providers
{
    //[DebuggerStepThrough]
    public class SingletonServiceProviderDecorator : IServiceProvider
    {
        private readonly IList<IConduit> conduits = new List<IConduit> ();
        private readonly IServiceProvider serviceProvider;
        private bool buildingInstance = false;
        private object instance;

        public SingletonServiceProviderDecorator ( IServiceProvider serviceProvider )
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetService ( Type service, Type interfaceType, IServiceLocator context )
        {
            if ( instance != null ) return instance;

            if ( buildingInstance ) return NewProxy ( interfaceType );

            buildingInstance = true;
            instance = serviceProvider.GetService ( service, interfaceType, context );
            buildingInstance = false;

            ResolveProxies ();

            return instance;
        }

        public void AddMapping ( Type serviceType )
        {
            serviceProvider.AddMapping ( serviceType );
        }

        private object NewProxy ( Type interfaceType )
        {
            IConduit conduit = new Conduit ( interfaceType );
            conduits.Add ( conduit );
            return conduit.Proxy;
        }

        private void ResolveProxies ( )
        {
            foreach ( var conduit in conduits ) conduit.SetTarget ( instance );
        }
    }
}