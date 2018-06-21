#region usings

using System;
using System.Collections.Generic;
using NDependencyInjection.interfaces;
using IServiceProvider = NDependencyInjection.interfaces.IServiceProvider;

#endregion

namespace NDependencyInjection.Providers
{
    /// <summary>
    ///     Calls the constructor for the ConcreteType when GetService is called. Any Parameters are resolved first
    /// </summary>
    /// <typeparam name="ConcreteType"></typeparam>
    //[DebuggerStepThrough]
    public class FactoryServiceProvider<ConcreteType> : IServiceProvider
    {
        private readonly Type _concreteType;
        private readonly IList<Type> _myTypes = new List<Type> ();

        public FactoryServiceProvider ( )
        {
            _concreteType = typeof (ConcreteType);
        }

        //[DebuggerStepThrough]
        public object GetService ( Type serviceType, Type interfaceType, IServiceLocator context )
        {
            var constructor = ConstructorHelper.FindInjectionConstructor ( _concreteType );
            var types = Reflection.GetParameterTypes ( constructor );
            ConstructorHelper.EnsureAllServicesArePresent ( context, types, _concreteType );
            return Reflection.CallConstructor ( constructor, GetServicesForConstructorParameters ( context, types ) );
        }

        public void AddMapping ( Type serviceType )
        {
            if ( !serviceType.IsAssignableFrom ( _concreteType ) )
                throw new InvalidWiringException ( "Service of type {0} does not implement type {1}",
                    _concreteType, serviceType );
            _myTypes.Add ( serviceType );
        }

        private object[] GetServicesForConstructorParameters ( IServiceLocator context, IEnumerable<Type> types )
        {
            var list = new List<object> ();
            foreach ( var type in types )
                // You are the one that would supply these types in your context.
                // You can't for your own constructor.
                if ( _myTypes.Contains ( type ) )
                    list.Add ( context.Parent.GetService ( type ) );
                else
                    list.Add ( context.GetService ( type ) );
            return list.ToArray ();
        }
    }
}