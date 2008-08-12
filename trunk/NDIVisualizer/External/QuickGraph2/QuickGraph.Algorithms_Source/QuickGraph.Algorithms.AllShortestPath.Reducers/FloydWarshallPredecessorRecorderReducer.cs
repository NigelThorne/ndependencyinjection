namespace QuickGraph.Algorithms.AllShortestPath.Reducers
{
    using QuickGraph.Algorithms.AllShortestPath;
    using QuickGraph.Concepts.Collections;
    using System;

    public class FloydWarshallPredecessorRecorderReducer
    {
        private IVertexPredecessorMatrix predecessors;

        public FloydWarshallPredecessorRecorderReducer(IVertexPredecessorMatrix predecessors)
        {
            if (predecessors == null)
            {
                throw new ArgumentNullException("predecessors");
            }
            this.predecessors = predecessors;
        }

        public void ReducePath(object sender, FloydWarshallEventArgs args)
        {
            this.Predecessors.SetPredecessor(args.Source, args.Target, this.Predecessors.Predecessor(args.Intermediate, args.Target));
        }

        public IVertexPredecessorMatrix Predecessors
        {
            get
            {
                return this.predecessors;
            }
        }
    }
}

