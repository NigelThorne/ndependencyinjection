using System;
using System.Collections.Generic;
using System.Reflection;
using NDependencyInjection.interfaces;
using IServiceProvider=NDependencyInjection.interfaces.IServiceProvider;


namespace NDependencyInjection
{
    public class DecoratorDependencyResolvingServiceProvider<ConcreteType, InterfaceType> : IServiceProvider
    {
        private readonly IServiceLocator locator;

        private readonly IDictionary<object, ConcreteType> history =
            new Dictionary<object, ConcreteType>();

        public DecoratorDependencyResolvingServiceProvider(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public object GetService(Type serviceType, Type interfaceType)
        {
            object service = locator.GetService(serviceType, interfaceType);
            if (history.ContainsKey(service)) return history[service];

            ConstructorInfo constructor = GetCallableConstructor();
            ConcreteType s = Reflection.CallConstructor<ConcreteType>(constructor, GetParameters(constructor, service));
            history[service] = s;
            return s;
        }

        private object[] GetParameters(ConstructorInfo constructor, object service)
        {
            return GetServices(Reflection.GetParameterTypes(constructor), service);
        }

        private object[] GetServices(IEnumerable<Type> types, object service)
        {
            List<object> list = new List<object>();
            foreach (Type type in types)
            {
                if (type == typeof(InterfaceType)) list.Add(service);
                else list.Add(locator.GetService(type, type));
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