namespace NDependencyInjection.Tests.TestClasses
{
    public class ClassC : IC
    {
        public ClassC ( IA a, IB b )
        {
            A = a;
            B = b;
        }

        public IA A { get; }

        public IB B { get; }
    }

    public class TwoConstructorsNoAttributes : IC
    {
        public TwoConstructorsNoAttributes ( IA a, IB b )
        {
            A = a;
            B = b;
        }

        public TwoConstructorsNoAttributes ( IA a )
        {
            A = a;
        }

        public IA A { get; }

        public IB B { get; }
    }

    public class TwoConstructorsAttributes : IC
    {
        [InjectionConstructor]
        public TwoConstructorsAttributes ( IA a, IB b )
        {
            A = a;
            B = b;
        }

        public TwoConstructorsAttributes ( IC a )
        {
        }

        public IA A { get; }

        public IB B { get; }
    }

    public class TwoConstructorsTwoAttributes : IC
    {
        [InjectionConstructor]
        public TwoConstructorsTwoAttributes ( IA a, IB b )
        {
            A = a;
            B = b;
        }

        [InjectionConstructor]
        public TwoConstructorsTwoAttributes ( IC a )
        {
        }

        public IA A { get; }

        public IB B { get; }
    }
}