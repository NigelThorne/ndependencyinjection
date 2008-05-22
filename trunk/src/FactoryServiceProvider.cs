//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using System.Reflection;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    /// <summary>
    /// Calls the constructor for the ConcreteType when GetService is called. Any Parameters are resolved first
    /// </summary>
    /// <typeparam name="ConcreteType"></typeparam>
    public class FactoryServiceProvider<ConcreteType> : IServiceProvider
    {
        private readonly IServiceLocator locator;

        public FactoryServiceProvider(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            ConstructorInfo constructor = GetCallableConstructor();
            return Reflection.CallConstructor<ConcreteType>(constructor, GetParameters(constructor));
        }

        private object[] GetParameters(ConstructorInfo constructor)
        {
            return GetServices(Reflection.GetParameterTypes(constructor));
        }

        private object[] GetServices(IEnumerable<Type> types)
        {
            List<object> list = new List<object>();
            foreach (Type type in types)
            {
                list.Add(locator.GetService(type));
            }
            return list.ToArray();
        }

        private ConstructorInfo GetCallableConstructor()
        {
            ConstructorInfo constructor =
                PickConstructor(typeof (ConcreteType).GetConstructors(BindingFlags.Public | BindingFlags.Instance));
            EnsureConstructorIsCallable(constructor);
            return constructor;
        }

        private static ConstructorInfo PickConstructor(ConstructorInfo[] constructors)
        {
            if (constructors.Length == 1) return constructors[0];

            foreach (ConstructorInfo info in constructors)
            {
                if (Reflection.HasAttribute<InjectionConstructorAttribute>(info)) return info;
            }
            throw new ApplicationException(
                string.Format(
                    "Type {0} has more or less than one constructor. Indicate the constructor to use with a [InjectionConstructor] attribute",
                    typeof (ConcreteType)));
        }

        private void EnsureConstructorIsCallable(ConstructorInfo info)
        {
            List<Type> unknownTypes = GetUnknownTypes(info);
            if (unknownTypes.Count > 0)
            {
                throw new ApplicationException(
                    string.Format("Constructor for {0} referenced types unknown within this scope: \n{1}",
                                  typeof (ConcreteType), TypesToString(unknownTypes)));
            }
        }

        private static string TypesToString(IEnumerable<Type> types)
        {
            string message = "";
            foreach (Type type in types)
            {
                message += string.Format("{0} \n", type.FullName);
            }
            return message;
        }

        private List<Type> GetUnknownTypes(ConstructorInfo info)
        {
            List<Type> unknownTypes = new List<Type>();
            foreach (ParameterInfo parameter in info.GetParameters())
            {
                if (!locator.HasService(parameter.ParameterType)) unknownTypes.Add(parameter.ParameterType);
            }
            return unknownTypes;
        }
    }
}