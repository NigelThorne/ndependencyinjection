using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    public class TypeMappingServiceProvider<DerivedType, BaseType> : IServiceProvider<DerivedType> where BaseType : DerivedType
    {
        private readonly IServiceLocator locator;

        public TypeMappingServiceProvider(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public DerivedType GetService()
        {
            return locator.Get<BaseType>();
        }
    }
}