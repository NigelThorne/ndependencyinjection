#region usings

using System;
using System.Collections.Generic;
using System.Reflection;
using NDependencyInjection.interfaces;

#endregion

namespace NDependencyInjection.Providers
{
    public class ConstructorHelper
    {
        //[DebuggerStepThrough]
        public static ConstructorInfo FindInjectionConstructor ( Type concreteType )
        {
            var constructors = concreteType.GetConstructors ( BindingFlags.Public | BindingFlags.Instance );
            if ( constructors.Length == 1 ) return constructors[0];

            foreach ( var info in constructors )
                if ( Reflection.HasAttribute<InjectionConstructorAttribute> ( info ) )
                    return info;
            throw new ApplicationException (
                $"Type {concreteType} has more or less than one public constructor. Indicate which constructor to use with a [InjectionConstructor] attribute" );
        }

        //[DebuggerStepThrough]
        public static void EnsureAllServicesArePresent ( IServiceLocator context, IEnumerable<Type> types,
            Type concreteType )
        {
            var unknownTypes = GetUnknownTypes ( context, types );
            if ( unknownTypes.Count > 0 )
                throw new ApplicationException (
                    $"Constructor reference to unknown type: \n\t{concreteType} referenced \n\t{TypesToString ( unknownTypes )}" );
        }

        //[DebuggerStepThrough]
        private static List<Type> GetUnknownTypes ( IServiceLocator context, IEnumerable<Type> types )
        {
            var unknownTypes = new List<Type> ();
            foreach ( var type in types )
                if ( !context.HasService ( type ) )
                    unknownTypes.Add ( type );
            return unknownTypes;
        }

        //[DebuggerStepThrough]
        public static string TypesToString ( IEnumerable<Type> types )
        {
            var message = "";
            foreach ( var type in types ) message += $"{type.FullName} \n";
            return message;
        }
    }
}