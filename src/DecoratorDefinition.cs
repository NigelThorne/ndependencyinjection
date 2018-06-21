#region usings

using NDependencyInjection.DSL;
using NDependencyInjection.interfaces;
using NDependencyInjection.Providers;

#endregion

namespace NDependencyInjection
{
    public class DecoratorDefinition<Interface> : IDecoratingContext
    {
        private readonly IScope scope;

        public DecoratorDefinition ( IScope scope )
        {
            this.scope = scope;
        }

        public void With<DecoratingType> ( )
        {
            scope.DecorateService<Interface> ( new FactoryServiceProvider<DecoratingType> () );
        }
    }
}