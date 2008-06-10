using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class DecoratingContext<Interface> : IDecoratingContext
    {
        private readonly ISystemWiring wiring;

        public DecoratingContext(ISystemWiring wiring)
        {
            this.wiring = wiring;
        }


        public void With<DecoratingType>()
        {
            wiring.DecorateService<Interface, DecoratingType>();
        }
    }
}