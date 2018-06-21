#region usings

using System;
using NDependencyInjection.Exceptions;
using NDependencyInjection.interfaces;

#endregion

namespace NDependencyInjection
{
    //[DebuggerStepThrough]
    internal class NullServiceLocator : IServiceLocator
    {
        public object GetService ( Type serviceType )
        {
            throw new UnknownTypeException ( serviceType );
        }

        public bool HasService ( Type serviceType )
        {
            return false;
        }

        public IServiceLocator Parent => this;
    }
}