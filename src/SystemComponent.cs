using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    internal class SystemComponent : ISystemComponent
    {
        private readonly IServiceProvider provider;
        private readonly ISystemWiring wiring;

        public SystemComponent(ISystemWiring wiring, IServiceProvider provider)
        {
            this.wiring = wiring;
            this.provider = provider;
        }

        public ISystemComponent Provides<Interface>()
        {
            wiring.RegisterServiceProvider<Interface>(provider);
            return this;
        }

        public ISystemComponent ListensTo<EventsListener>()
        {
            wiring.RegisterServiceListener<EventsListener>(provider);
            return this;
        }
    }
}