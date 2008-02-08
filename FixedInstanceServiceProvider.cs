using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class FixedInstanceServiceProvider<InterfaceType> : IServiceProvider<InterfaceType>
    {
        private readonly InterfaceType instance;

        public FixedInstanceServiceProvider(InterfaceType instance)
        {
            this.instance = instance;
        }

        public InterfaceType GetService()
        {
            return instance;
        }
    }
}