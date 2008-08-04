using System;
using System.Collections.Generic;
using System.Reflection;
using NDependencyInjection.interfaces;


namespace NDependencyInjection.Providers
{
    public class ConstructorHelper
    {
        public static ConstructorInfo FindInjectionConstructor(Type concreteType)
        {
            ConstructorInfo[] constructors = concreteType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 1) return constructors[0];

            foreach (ConstructorInfo info in constructors)
            {
                if (Reflection.HasAttribute<InjectionConstructorAttribute>(info)) return info;
            }
            throw new ApplicationException(
                string.Format(
                    "Type {0} has more or less than one constructor. Indicate the constructor to use with a [InjectionConstructor] attribute",
                    concreteType));
        }

        public static void EnsureAllServicesArePresent<ConcreteType>(IServiceLocator context, IEnumerable<Type> types)
        {
            List<Type> unknownTypes = GetUnknownTypes(context, types);
            if (unknownTypes.Count > 0)
            {
                throw new ApplicationException(
                    string.Format("Constructor for {0} referenced types unknown within this scope: \n{1}",
                                  typeof (ConcreteType), TypesToString(unknownTypes)));
            }
        }

        private static List<Type> GetUnknownTypes(IServiceLocator context, IEnumerable<Type> types)
        {
            List<Type> unknownTypes = new List<Type>();
            foreach (Type type in types)
            {
                if (!context.HasService(type)) unknownTypes.Add(type);
            }
            return unknownTypes;
        }

        public static string TypesToString(IEnumerable<Type> types)
        {
            string message = "";
            foreach (Type type in types)
            {
                message += string.Format("{0} \n", type.FullName);
            }
            return message;
        }
    }
}