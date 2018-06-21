namespace NDependencyInjection.Tests.TestClasses
{
    public class ClassA : IA
    {
        public int Property { get; set; }

        public int DoSomething ( int x, int y )
        {
            return Property;
        }
    }

    public class DecoratorA : IA
    {
        public DecoratorA ( IA parent )
        {
            Parent = parent;
        }

        public IA Parent { get; set; }

        public int Property
        {
            get => Parent.Property;
            set => Parent.Property = value;
        }

        public int DoSomething ( int x, int y )
        {
            return Parent.DoSomething ( x + 1, y + 1 );
        }
    }
}