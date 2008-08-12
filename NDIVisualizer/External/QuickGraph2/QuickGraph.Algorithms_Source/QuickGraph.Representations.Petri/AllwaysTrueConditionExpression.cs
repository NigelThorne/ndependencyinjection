namespace QuickGraph.Representations.Petri
{
    using QuickGraph.Concepts.Petri;
    using QuickGraph.Concepts.Petri.Collections;
    using System;

    public class AllwaysTrueConditionExpression : IConditionExpression
    {
        public bool IsEnabled(ITokenCollection tokens)
        {
            return true;
        }
    }
}

