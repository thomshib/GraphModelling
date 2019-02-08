using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    class GraphNode<T>
    {
        T value;
        HashSet<GraphEdge<T>> adjacentList;

        public GraphNode(T value):this(value, null)
        {

        }
        public GraphNode(T value, GraphEdge<T> edge)
        {
            this.value = value;
            this.adjacentList = new HashSet<GraphEdge<T>>();
            if (edge != null)
            {
                this.adjacentList.Add(edge);
            }
            

        }
        public T Value { get => value; set => this.value = value; }

        public HashSet<GraphNode<T>> NeighborNodes()
        {
            HashSet<GraphNode<T>> set = new HashSet<GraphNode<T>>();
           
            foreach (GraphEdge<T> edge in adjacentList)
            {
                set.Add(edge.OtherNode(this));
            }

            return set;

        }
        public HashSet<GraphEdge<T>> GetEdges()
        {
            return adjacentList;
        }

        public void AddEdge(GraphEdge<T> edge)
        {
            
                foreach (GraphEdge<T> e in adjacentList)
                {
                    if (e.To.Value.Equals(edge.To.Value))
                    {
                    return;
                    }
                }
           
                adjacentList.Add(edge);
            
          
        }
    }
}
