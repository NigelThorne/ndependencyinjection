#region usings

using System;

#endregion

namespace NDependencyInjection.interfaces
{
    /// <summary>
    /// </summary>
    public interface IServiceProvider
    {
        object GetService ( Type serviceType, Type interfaceType, IServiceLocator context );

        /// <summary>
        ///     Tells the provider that we are expecting it to provide the specified type.
        /// </summary>
        /// <param name="serviceType"></param>
        void AddMapping ( Type serviceType );
    }
}