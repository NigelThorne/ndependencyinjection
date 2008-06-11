//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.Exceptions
{
    public class UnknownTypeException : Exception
    {
        public UnknownTypeException(Type type)
            : base(string.Format("Type {0} is not registered", type))
        {
        }
    }
}