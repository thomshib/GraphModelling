using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    class GraphProblems
    {

        public Dictionary<GraphNode<int>, Boolean> VertexCover(Graph<int> g)
        {
            #region pseudocode
            /*
            VERTEX_COVER(G: Graph)

            c ← { }
            E` ← E[G]

            while E` is not empty do
              Let(u, v) be an arbitrary edge of E`
              c ← c U { u, v}
              Remove from E` every edge incident on either u or v

            return c
            */
            #endregion


            Dictionary<GraphNode<int> , Boolean > visited = new Dictionary<GraphNode<int>, bool>();

            foreach(GraphNode<int> n in g.GetVertices())
            {
                if (visited[n] == false)
                {
                    foreach(GraphEdge<int> e in n.GetEdges())
                    {
                        if (visited[e.To] == false)
                        {
                            visited[n] = true;
                            visited[e.To] = true;

                            //ignore the rest of the edges of n
                            break;
                        }
                    }
                }

            }

            return visited;
        }

      
    }
}
