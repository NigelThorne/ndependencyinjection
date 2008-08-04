//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using System.Reflection;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection.Providers
{
    /// <summary>
    /// Calls the constructor for the ConcreteType when GetService is called. Any Parameters are resolved first
    /// </summary>
    /// <typeparam name="ConcreteType"></typeparam>
    public class FactoryServiceProvider<ConcreteType> : IServiceProvider
    {
        private readonly List<Type> myTypes = new List<Type>();

        public object GetService(Type serviceType, Type interfaceType, IServiceLocator context)
        {
            ConstructorInfo constructor = ConstructorHelper.FindInjectionConstructor(typeof (ConcreteType));
            IEnumerable<Type> types = Reflection.GetParameterTypes(constructor);
            ConstructorHelper.EnsureAllServicesArePresent<ConcreteType>(context, types);
            return Reflection.CallConstructor<ConcreteType>(constructor, GetServicesForParameters(context, types));
        }

        public void AddMapping(Type serviceType)
        {
            myTypes.Add(serviceType);
            if (!serviceType.IsAssignableFrom(typeof(ConcreteType))) 
                throw new InvalidWiringException("Service of type {0} does not implement type {1}", typeof(ConcreteType), serviceType);
        }

        private object[] GetServicesForParameters(IServiceLocator context, IEnumerable<Type> types)
        {
            return GetServices(types, context);
        }

        private object[] GetServices(IEnumerable<Type> types, IServiceLocator context)
        {
            List<object> list = new List<object>();
            foreach (Type type in types)
            {
                if (myTypes.Contains(type))
                    list.Add(context.Parent.GetService(type));
                else
                    list.Add(context.GetService(type));
            }
            return list.ToArray();
        }
    }

    public class InvalidWiringException : Exception
    {
        public InvalidWiringException(string message, params object[] args) : base(string.Format(message, args))
        {
        }
    }
}