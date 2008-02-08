using System;
using System.Collections.Generic;
using System.Reflection;
using NDependencyInjection.interfaces;

namespace NDependencyInjection
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly Dictionary<Type, object> dictionary = new Dictionary<Type, object>();

        public void RegisterService<T>(IServiceProvider<T> provider)
        {
            if (dictionary.ContainsKey(typeof (T)))
                throw new InvalidOperationException(String.Format("Type {0} is already registered", typeof (T)));

            dictionary[typeof (T)] = provider;
        }

        public T Get<T>()
        {
            Type type = typeof (T);
            if (!dictionary.ContainsKey(type))
                throw new InvalidOperationException(String.Format("Type {0} is not registered", type));

            object obj = dictionary[type];
            return Reflection.CallMethod<T>(obj, "GetService", new Type[0], new object[0]);
        }

        public bool Has<T>()
        {
            return dictionary.ContainsKey(typeof(T));
        }
    }
}