#region usings

using System;

#endregion

namespace NDependencyInjection
{
    [AttributeUsage ( AttributeTargets.Constructor, AllowMultiple = false )]
    public class InjectionConstructorAttribute : Attribute
    {
    }
}