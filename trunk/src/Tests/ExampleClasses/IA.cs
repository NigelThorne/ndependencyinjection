//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.Tests.ExampleClasses
{
    public interface IA
    {
        int Property { get; set; }
        int DoSomething(int x, int y);
    }

    public interface X<T>
    {
        T DoSomething();
    }
}