//Copyright (c) 2008 Nigel Thorne

using NDependencyInjection.DSL;
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    /// <summary>
    /// Buil
    /// </summary>
    public class DelegateExecutingBuilder : ISubsystemBuilder
    {
        private readonly CreateSubsystem method;

        public DelegateExecutingBuilder(CreateSubsystem method)
        {
            this.method = method;
        }

        public void Build(ISystemDefinition system)
        {
            method(system);
        }
    }
}