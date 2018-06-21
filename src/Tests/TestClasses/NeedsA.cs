namespace NDependencyInjection.Tests.TestClasses
{
    public class NeedsA : IB
    {
        public NeedsA ( IA a )
        {
            A = a;
        }

        public IA A { get; }
    }
}