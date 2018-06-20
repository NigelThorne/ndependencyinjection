//Copyright (c) 2008 Nigel Thorne
using System;
using System.Diagnostics;
using NDependencyInjection.Exceptions;
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    //[DebuggerStepThrough]
    internal class NullServiceLocator : IServiceLocator
    {

        public object GetService(Type serviceType)
        {
            throw new UnknownTypeException(serviceType);
        }

        public bool HasService(Type serviceType)
        {
            return false;
        }

        public IServiceLocator Parent
        {
            get { return this; }
        }
    }
}