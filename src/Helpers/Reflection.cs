#region usings

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace NDependencyInjection
{
    public class Reflection
    {
        //        public static T CallMethod<T>(object subject, string methodName, Type[] parameterTypes, object[] parameters)
        //        {
        //            try
        //            {
        //                return (T) subject.GetType().GetMethod(methodName, parameterTypes).Invoke(subject, parameters);
        //            }
        //            catch (TargetInvocationException ex)
        //            {
        //                throw ex.InnerException;
        //            }
        //        }

        //[DebuggerStepThrough]
        public static object CallConstructor ( ConstructorInfo constructor, object[] parameters )
        {
            try
            {
                return constructor.Invoke ( parameters );
            }
            catch ( TargetInvocationException ex )
            {
                throw ex.InnerException;
            }
        }

        //        public static T CallGenericMethod<T>(string methodName, Type subjectType, object subject, Type[] types,
        //                                             object[] parameters)
        //        {
        //            MethodInfo genericMethod = subjectType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
        //            MethodInfo resolvedMethod = genericMethod.MakeGenericMethod(types);
        //            try
        //            {
        //                return (T) resolvedMethod.Invoke(subject, parameters);
        //            }
        //            catch (TargetInvocationException ex)
        //            {
        //                throw ex.InnerException;
        //            }
        //        }

        //[DebuggerStepThrough]
        public static IEnumerable<Type> GetParameterTypes ( ConstructorInfo constructor )
        {
            var parameterInfos = constructor.GetParameters ();
            var list = new List<Type> ();
            foreach ( var parameterInfo in parameterInfos ) list.Add ( parameterInfo.ParameterType );
            return list;
        }

        //[DebuggerStepThrough]
        public static bool HasAttribute<TAttributeType> ( ConstructorInfo info )
        {
            var attributes = info.GetCustomAttributes ( false );
            foreach ( var attribute in attributes )
                if ( attribute is TAttributeType )
                    return true;
            return false;
        }
    }
}