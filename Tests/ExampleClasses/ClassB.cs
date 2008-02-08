namespace NDependencyInjection.Tests.ExampleClasses
{
    public class ClassB : IB
    {
        private readonly IA a;

        public ClassB(IA a)
        {
            this.a = a;
        }

        public IA A
        {
            get { return a; }
        }
    }
}