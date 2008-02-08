using NDependencyInjection.interfaces;


namespace NDependencyInjection.Tests.ExampleClasses
{
    public class SP : IServiceProvider<IMyTestClassA>
    {
        private readonly IMyTestClassA service;
        public bool gotCalled = false;

        public SP(IMyTestClassA service)
        {
            this.service = service;
        }

        public IMyTestClassA GetService()
        {
            gotCalled = true;
            return service;
        }
    }
}