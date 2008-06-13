using NDependencyInjection.interfaces;

namespace NDependencyInjection
{
    public class DecoratingContext<Interface> : IDecoratingContext
    {
        private readonly IScope wiring;

        public DecoratingContext(IScope wiring)
        {
            this.wiring = wiring;
        }

        public void With<DecoratingType>()
        {
            wiring.DecorateService<Interface, DecoratingType>();
        }
    }
}