//Copyright (c) 2008 Nigel Thorne
using System;


namespace NDependencyInjection.Tests.TestClasses
{
    public class NeedsB : IA
    {
        private IB b;

        public NeedsB(IB b)
        {
            this.b = b;
        }

        public int Property
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int DoSomething(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}