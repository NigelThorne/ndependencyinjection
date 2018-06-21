namespace NDependencyInjection.Tests.TestClasses
{
    public class ClassB : IB
    {
        public ClassB ( IA a )
        {
            A = a;
        }

        public IA A { get; }
    }
}