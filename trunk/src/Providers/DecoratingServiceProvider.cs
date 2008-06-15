//Copyright (c) 2008 Nigel Thorne
using System;
using System.Collections.Generic;
using System.Reflection;
using NDependencyInjection.Attributes;
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
            ConstructorInfo constructor = GetCallableConstructor();
            EnsureConstructorIsCallable(constructor, context);
            return Reflection.CallConstructor<ConcreteType>(constructor, GetParameters(constructor, context));
        }

        public void AddMapping(Type serviceType)
        {
            myTypes.Add(serviceType);
        }

        private object[] GetParameters(ConstructorInfo constructor, IServiceLocator context)
        {
            return GetServices(Reflection.GetParameterTypes(constructor), context);
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

        private static ConstructorInfo GetCallableConstructor()
        {
            ConstructorInfo constructor =
                PickConstructor(typeof (ConcreteType).GetConstructors(BindingFlags.Public | BindingFlags.Instance));
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

        private static void EnsureConstructorIsCallable(ConstructorInfo info, IServiceLocator context)
        {
            List<Type> unknownTypes = GetUnknownTypes(info, context);
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

        private static List<Type> GetUnknownTypes(ConstructorInfo info, IServiceLocator context)
        {
            List<Type> unknownTypes = new List<Type>();
            foreach (ParameterInfo parameter in info.GetParameters())
            {
                if (!context.HasService(parameter.ParameterType)) unknownTypes.Add(parameter.ParameterType);
            }
            return unknownTypes;
        }
    }
}