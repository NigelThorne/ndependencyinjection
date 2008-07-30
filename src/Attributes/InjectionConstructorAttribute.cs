//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    public class InjectionConstructorAttribute : Attribute
    {
    }
}