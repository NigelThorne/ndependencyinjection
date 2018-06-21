#region usings

using System;
using System.Collections;
using NDependencyInjection.interfaces;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection.Providers
{
    internal class CollectionProvider : IServiceProvider
    {
        private readonly IServiceProvider[] subsystems;

        public CollectionProvider ( IServiceProvider[] subsystems )
        {
            this.subsystems = subsystems;
        }

        public object GetService ( Type serviceType, Type interfaceType, IServiceLocator context )
        {
            var list = new ArrayList ();
            foreach ( var subsystem in subsystems )
                list.Add ( subsystem.GetService ( serviceType.GetElementType (), interfaceType.GetElementType (),
                    context ) );
            return list.ToArray ( interfaceType.GetElementType () );
        }

        // SMELL: Not used
        public void AddMapping ( Type serviceType )
        {
        }
    }
}