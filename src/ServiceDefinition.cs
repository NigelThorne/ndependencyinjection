//Copyright (c) 2008 Nigel Thorne
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    internal class ServiceDefinition : IServiceDefinition
    {
        private readonly IServiceProvider provider;
        private readonly IScope scope;

        public ServiceDefinition(IScope scope, IServiceProvider provider)
        {
            this.scope = scope;
            this.provider = provider;
        }

        public IServiceDefinition Provides<Interface>()
        {
            scope.RegisterServiceProvider(typeof(Interface), provider);
            return this;
        }

        public IServiceDefinition ListensTo<EventsListener>()
        {
            scope.RegisterServiceListener<EventsListener>(provider);
            return this;
        }
    }
}