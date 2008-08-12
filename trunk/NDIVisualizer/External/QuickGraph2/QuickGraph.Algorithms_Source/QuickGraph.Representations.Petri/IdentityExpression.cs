namespace QuickGraph.Representations.Petri
{
    using QuickGraph.Concepts.Petri;
    using QuickGraph.Concepts.Petri.Collections;
    using System;

    public class IdentityExpression : IExpression
    {
        public ITokenCollection Eval(ITokenCollection marking)
        {
            return marking;
        }
    }
}

