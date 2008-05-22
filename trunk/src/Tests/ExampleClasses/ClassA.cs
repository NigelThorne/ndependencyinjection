//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.Tests.ExampleClasses
{
    public class ClassA : IA
    {
        private int property;

        public int Property
        {
            get { return property; }
            set { property = value; }
        }

        public int DoSomething(int x, int y)
        {
            return property;
        }
    }

    public class DecoratorA: IA
    {
        private IA parent;

        public DecoratorA(IA parent)
        {
            this.parent = parent;
        }

        public IA Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public int Property
        {
            get { return parent.Property; }
            set { parent.Property = value; }
        }

        public int DoSomething(int x, int y)
        {
            return parent.DoSomething(x + 1, y + 1);
        }
    }
}