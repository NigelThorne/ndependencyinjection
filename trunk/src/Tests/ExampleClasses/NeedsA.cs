namespace NDependencyInjection.Tests.ExampleClasses
{
    public class NeedsA : IB
    {
        private readonly IA a;

        public NeedsA(IA a)
        {
            this.a = a;
        }

        public IA A
        {
            get { return a; }
        }
    }
}