namespace QuickGraph.Algorithms.Visitors
{
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Visitors;
    using System;

    public class TimeStamperVisitor : ITimeStamperVisitor
    {
        private VertexIntDictionary m_DiscoverTimes = new VertexIntDictionary();
        private VertexIntDictionary m_FinishTimes = new VertexIntDictionary();
        private int m_Time = 0;

        public void DiscoverVertex(object sender, VertexEventArgs args)
        {
            this.m_DiscoverTimes.set_Item(args.get_Vertex(), this.m_Time++);
        }

        public void FinishVertex(object sender, VertexEventArgs args)
        {
            this.m_FinishTimes.set_Item(args.get_Vertex(), this.m_Time++);
        }

        public VertexIntDictionary DiscoverTimes
        {
            get
            {
                return this.m_DiscoverTimes;
            }
        }

        public VertexIntDictionary FinishTimes
        {
            get
            {
                return this.m_FinishTimes;
            }
        }

        public int Time
        {
            get
            {
                return this.m_Time;
            }
            set
            {
                this.m_Time = value;
            }
        }
    }
}

