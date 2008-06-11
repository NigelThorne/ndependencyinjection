//Copyright (c) 2008 Nigel Thorne
using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    internal class SystemComponent : ISystemComponent
    {
        private readonly IServiceProvider provider;
        private readonly IServiceScope scope;

        public SystemComponent(IServiceScope scope, IServiceProvider provider)
        {
            this.scope = scope;
            this.provider = provider;
        }

        public ISystemComponent Provides<Interface>()
        {
            scope.RegisterServiceProvider<Interface>(provider);
            return this;
        }

        public ISystemComponent ListensTo<EventsListener>()
        {
            scope.RegisterServiceListener<EventsListener>(provider);
            return this;
        }
    }
}