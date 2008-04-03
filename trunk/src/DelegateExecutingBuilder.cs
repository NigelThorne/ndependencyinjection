using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public delegate void CreateSubsystem(ISystemDefinition scope);

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