//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.Tests.TestClasses
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