//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.Tests.ExampleClasses
{
    public class ClassC : IC
    {
        private readonly IA a;
        private readonly IB b;

        public ClassC(IA a, IB b)
        {
            this.a = a;
            this.b = b;
        }

        public IA A
        {
            get { return a; }
        }

        public IB B
        {
            get { return b; }
        }
    }

    public class TwoConstructorsNoAttributes : IC
    {
        private readonly IA a;
        private readonly IB b;

        public TwoConstructorsNoAttributes(IA a, IB b)
        {
            this.a = a;
            this.b = b;
        }

        public TwoConstructorsNoAttributes(IA a)
        {
            this.a = a;
        }

        public IA A
        {
            get { return a; }
        }

        public IB B
        {
            get { return b; }
        }
    }

    public class TwoConstructorsAttributes : IC
    {
        private readonly IA a;
        private readonly IB b;

        [InjectionConstructor]
        public TwoConstructorsAttributes(IA a, IB b)
        {
            this.a = a;
            this.b = b;
        }

        public TwoConstructorsAttributes(IC a)
        {
        }

        public IA A
        {
            get { return a; }
        }

        public IB B
        {
            get { return b; }
        }
    }

    public class TwoConstructorsTwoAttributes : IC
    {
        private readonly IA a;
        private readonly IB b;

        [InjectionConstructor]
        public TwoConstructorsTwoAttributes(IA a, IB b)
        {
            this.a = a;
            this.b = b;
        }

        [InjectionConstructor]
        public TwoConstructorsTwoAttributes(IC a)
        {
        }

        public IA A
        {
            get { return a; }
        }

        public IB B
        {
            get { return b; }
        }
    }
}