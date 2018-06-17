using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NDependencyInjection.interfaces;


namespace NDependencyInjection.Providers
{
    public class ConstructorHelper
    {
        [DebuggerStepThrough]
        public static ConstructorInfo FindInjectionConstructor(Type concreteType)
        {
            ConstructorInfo[] constructors = concreteType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 1) return constructors[0];

            foreach (ConstructorInfo info in constructors)
            {
                if (Reflection.HasAttribute<InjectionConstructorAttribute>(info)) return info;
            }
            throw new ApplicationException(
                $"Type {concreteType} has more or less than one public constructor. Indicate which constructor to use with a [InjectionConstructor] attribute");
        }

        [DebuggerStepThrough]
        public static void EnsureAllServicesArePresent(IServiceLocator context, IEnumerable<Type> types, Type concreteType)
        {
            List<Type> unknownTypes = GetUnknownTypes(context, types);
            if (unknownTypes.Count > 0)
            {
                throw new ApplicationException(
                    $"Constructor reference to unknown type: \n\t{concreteType} referenced \n\t{TypesToString(unknownTypes)}");
            }
        }

        [DebuggerStepThrough]
        private static List<Type> GetUnknownTypes(IServiceLocator context, IEnumerable<Type> types)
        {
            List<Type> unknownTypes = new List<Type>();
            foreach (Type type in types)
            {
                if (!context.HasService(type)) unknownTypes.Add(type);
            }
            return unknownTypes;
        }

        [DebuggerStepThrough]
        public static string TypesToString(IEnumerable<Type> types)
        {
            string message = "";
            foreach (Type type in types)
            {
                message += $"{type.FullName} \n";
            }
            return message;
        }
    }
}