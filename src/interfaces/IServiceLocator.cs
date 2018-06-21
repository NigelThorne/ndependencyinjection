#region usings

using System;

#endregion

namespace NDependencyInjection.interfaces
{
    /// <summary>
    ///     The interface that lets you have a tree of scopes.
    /// </summary>
    public interface IServiceLocator
    {
        // SMELL
        IServiceLocator Parent { get; }
        object GetService ( Type serviceType );
        bool HasService ( Type serviceType );
    }
}