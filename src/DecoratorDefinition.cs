using NDependencyInjection.interfaces;

namespace NDependencyInjection
{
    public class DecoratorDefinition<Interface> : IDecoratingContext
    {
        private readonly IScope scope;

        public DecoratorDefinition(IScope scope)
        {
            this.scope = scope;
        }

        public void With<DecoratingType>()
        {
            scope.DecorateService<Interface, DecoratingType>();
        }
    }
}