using System;
using System.Collections.Generic;
using System.Reflection;
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{

    /// <summary>
    /// Calls the constructor for the ServiceType when GetService is called. Any Parameters are resolved first
    /// </summary>
    public class ConstructorResolvingServiceProvider<ConcreteType> : IServiceProvider<ConcreteType> 
    {
        private readonly IServiceLocator locator;

        public ConstructorResolvingServiceProvider(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public ConcreteType GetService()
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
                list.Add(Reflection.CallGenericMethod<object>("Get",typeof(IServiceLocator), locator, new Type[]{type}, new object[0]));
            }
            return list.ToArray();
        }

        private ConstructorInfo GetCallableConstructor()
        {
            ConstructorInfo constructor = PickConstructor(typeof(ConcreteType).GetConstructors(BindingFlags.Public | BindingFlags.Instance));
            EnsureConstructorIsCallable(constructor);
            return constructor;
        }

        private static ConstructorInfo PickConstructor(ConstructorInfo[] constructors)
        {
            if (constructors.Length == 1) return constructors[0];

            foreach (ConstructorInfo info in constructors)
            {
                if (Reflection.HasAttribute< InjectionConstructor>(info)) return info;
            }
            throw new ApplicationException(string.Format("Type {0} has more or less than one constructor. Indicate the constructor to use with a [ConstructorToCall] attribute", typeof(ConcreteType)));
        }

        private void EnsureConstructorIsCallable(ConstructorInfo info)
        {
            List<Type> unknownTypes = GetUnknownTypes(info);
            if (unknownTypes.Count > 0)
            {
                throw new ApplicationException(
                    string.Format("Constructor for {0} referenced types unknown within this scope: \n{1}",
                                  typeof (ConcreteType),TypesToString(unknownTypes)));
            }
        }

        private static string TypesToString(IEnumerable<Type> types)
        {
            string message = "";
            foreach (Type type in types)
            {
                message += string.Format("{0} \n",type.FullName);
            }
            return message;
        }

        private List<Type> GetUnknownTypes(ConstructorInfo info)
        {
            List<Type> unknownTypes = new List<Type>();
            foreach (ParameterInfo parameter in info.GetParameters())
            {
                if (!HasType(locator, parameter)) unknownTypes.Add(parameter.ParameterType);
            }
            return unknownTypes;
        }

        private static bool HasType(IServiceLocator locator, ParameterInfo parameter)
        {
            return Reflection.CallGenericMethod<bool>("Has",typeof(IServiceLocator),locator,new Type[]{parameter.ParameterType}, new object[0]);
        }
    }
}