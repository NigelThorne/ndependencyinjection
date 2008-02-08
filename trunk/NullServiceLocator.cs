using NDependencyInjection.interfaces;


namespace NDependencyInjection
{
    internal class NullServiceLocator : IServiceLocator
    {
        public T1 Get<T1>()
        {
            throw new UnknownTypeException(typeof (T1));
        }

        public bool Has<T>()
        {
            return false;
        }
    }
}