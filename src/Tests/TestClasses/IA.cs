namespace NDependencyInjection.Tests.TestClasses
{
    public interface IA
    {
        int Property { get; set; }
        int DoSomething ( int x, int y );
    }

    public interface IDoSomething
    {
        int DoSomething ( int x, int y );
    }

    public interface IX<T>
    {
        T DoSomething ( );
    }

    public interface IX2
    {
        T DoSomething<T> ( );
    }
}