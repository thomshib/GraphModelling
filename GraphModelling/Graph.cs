using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    class Graph<T>
    {
        HashSet<GraphNode<T>> vertices;
        
        public Graph()
        {
            vertices = new HashSet<GraphNode<T>>();
        }
        public void AddVertex(GraphNode<T> node)
        {
            if (vertices.Where(n => n.Value.Equals(node.Value)).Count() == 0)
            {
                this.vertices.Add(node);
            }
        }
        public void DeleteVertex(GraphNode<T> node)
        {
            if (vertices.Where(n => n.Value.Equals(node.Value)).Count() > 0)
            {                

                this.vertices.Remove(node);
            }
        }

        public void AddEdge(GraphNode<T> from,GraphNode<T> to,double distance)
        {
            GraphNode<T> fromNode = vertices.Single(n => n.Value.Equals(from.Value));
            GraphNode<T> toNode = vertices.Single(n => n.Value.Equals(to.Value));


            if (fromNode == null)
            {
                this.vertices.Add(from);
                fromNode = from;


            }
            if (toNode == null)
                {
                this.vertices.Add(to);
                toNode = to;
                
            }

            GraphEdge<T> edge = new GraphEdge<T>(fromNode, toNode, distance);
            fromNode.AddEdge(edge);
            
        }

        public int GetVerticesCount()
        {
            return this.vertices.Count;
        }
        public int GetEdgesCount()
        {
            return this.GetEdges().Count;
        }

        public  HashSet<GraphNode<T>> GetVertices()
        {
            return vertices;
        }

        public GraphNode<T> GetVertex(GraphNode<T> node)
        {
            return vertices.Single(n => n.Value.Equals(node.Value));

        }
        public  HashSet<GraphEdge<T>> GetEdges()
        {
            HashSet<GraphEdge<T>> set = new HashSet<GraphEdge<T>>();
            foreach(GraphNode<T> node in vertices)
            {
               foreach(GraphEdge<T> edge in node.GetEdges())
                {
                    set.Add(edge);
                }
            }
            return set;
        }

    }
}
