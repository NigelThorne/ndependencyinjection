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
}