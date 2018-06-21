#region usings

using System;

#endregion

namespace NDependencyInjection.Exceptions
{
    public class UnknownTypeException : Exception
    {
        public UnknownTypeException ( Type type )
            : base ( string.Format ( "Type {0} is not registered", type ) )
        {
        }
    }
}