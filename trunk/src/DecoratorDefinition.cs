using NDependencyInjection.interfaces;

namespace NDependencyInjection
{
    public class DecoratorDefinition<Interface> : IDecoratingContext
    {
        private readonly IScope wiring;

        public DecoratorDefinition(IScope wiring)
        {
            this.wiring = wiring;
        }

        public void With<DecoratingType>()
        {
            wiring.DecorateService<Interface, DecoratingType>();
        }
    }
}