#region usings

using NDependencyInjection.DSL;

#endregion

namespace NDependencyInjection
{
    /// <summary>
    ///     Buil
    /// </summary>
    public class DelegateExecutingBuilder : ISubsystemBuilder
    {
        private readonly CreateSubsystem _method;

        public DelegateExecutingBuilder ( CreateSubsystem method )
        {
            _method = method;
        }

        public void Build ( ISystemDefinition system )
        {
            _method ( system );
        }
    }
}