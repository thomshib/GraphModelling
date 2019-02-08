using System;

namespace GraphModelling
{
    class GraphEdge<T>
    {
        GraphNode<T> from;
        GraphNode<T> to;
        double distance;

        public GraphEdge(GraphNode<T> from, GraphNode<T> to,double distance)
        {
            this.from = from;
            this.to = to;
            this.distance = distance;
        }

        public double Distance { get => distance; set => distance = value; }
        internal GraphNode<T> From { get => from; set => from = value; }
        internal GraphNode<T> To { get => to; set => to = value; }

        public GraphNode<T> OtherNode(GraphNode<T> node)
        {
            if (node.Equals(From))
            {
                return To;
            }else if (node.Equals(To))
            {
                return From;
            }
            else
            {
                throw new ArgumentException("Node Not Found");
            }
        }
    }
}