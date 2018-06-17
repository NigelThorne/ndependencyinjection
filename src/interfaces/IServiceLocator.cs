//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.interfaces
{
    /// <summary>
    /// The interface that lets you have a tree of scopes.
    /// </summary>
    public interface IServiceLocator 
    {
        object GetService(Type serviceType);
        bool HasService(Type serviceType);

        // SMELL
        IServiceLocator Parent { get; }
    }
}