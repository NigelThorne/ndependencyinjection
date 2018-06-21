#region usings

using System;

#endregion

namespace NDependencyInjection.Providers
{
    public class InvalidWiringException : Exception
    {
        public InvalidWiringException ( string message, params object[] args ) : base (
            string.Format ( message, args ) )
        {
        }
    }
}