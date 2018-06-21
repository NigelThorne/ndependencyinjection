namespace NDependencyInjection.interfaces
{
    public interface IComposite<InterfaceType>
    {
        void Add ( InterfaceType item );
        void Remove ( InterfaceType item );
    }
}