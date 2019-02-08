using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    class GraphTraversal
    {
        const double LargeValue = 99999.0;
        public GraphTraversal()
        {

        }
        public Tuple<bool, Dictionary<GraphNode<int>, GraphNode<int>>, Dictionary<GraphNode<int>, double>> BellmanFordFunction(Graph<int> g, GraphNode<int> start,bool isModifiedGraph = false)
        {
            #region pseudocode
            /*
             * function bellmanFord(G, S)
	                for each vertex V in G
			                distance[V] <- infinite
			                previous[V] <- NULL
	                distance[S] <- 0
	                for each vertex V in G				
		                for each edge (U,V) in G
			                tempDistance <- distance[U] + edge_weight(U, V)
			                if tempDistance < distance[V]
			                   distance[V] <- tempDistance
			                   previous[V] <- U
                for each edge (U,V) in G
		                If distance[U] + edge_weight(U, V) < distance[V}
			                Error: Negative Cycle Exists
                return distance[], previous[]

             * 
             */
            #endregion pseudocode

            int numVertices = g.GetVerticesCount();
            int numEdges = g.GetEdgesCount();
            bool isNegativeCycle = false;
            Dictionary<GraphNode<int>, double> distance = new Dictionary<GraphNode<int>, double>();
            Dictionary<GraphNode<int>, GraphNode<int>> parent = new Dictionary<GraphNode<int>, GraphNode<int>>();

            GraphNode<int> startNode = g.GetVertex(start);

            if (startNode == null)
            {
                return null;
            }


            //initialize all distances to infinity
            foreach (GraphNode<int> item in g.GetVertices())
            {
                distance[item] = LargeValue;
            }

            //initialize source distance to zero
            distance[startNode] = 0;

            //for each vertex V in G
            foreach (GraphNode<int> currentNode in g.GetVertices())
            {

                //for each edge (U,V)
                foreach (GraphEdge<int> edge in currentNode.GetEdges())
                {
                    //tempDistance <- distance[U] + edge_weight(U, V)
                    GraphNode<int> toNode = edge.OtherNode(currentNode);
                    double tempDistance = distance[currentNode] + edge.Distance;
                    if (tempDistance.LT(distance[toNode]))
                    {
                        distance[toNode] = tempDistance;
                        parent[toNode] = currentNode;
                    }
                }
            }

            /*
            Check for negative cycle
            for each edge (U,V) in G
            If distance[U] +edge_weight(U, V) < distance[V]
            
            //If the Graph is called from Johnson algo with additional edge,distance(u,v) has been reweighted/changed
            //Hence, this logic needs to run on the actual distance and not on the reweighted weights
            // actual distance has been changed by exactly to distance`(u, v) + h(v) - h(u)
            */
            foreach (GraphEdge<int> edge in g.GetEdges())
            {
                GraphNode<int> fromNode = edge.From;
                GraphNode<int> toNode = edge.To;
                double eDistance = edge.Distance;

                if (isModifiedGraph)
                {
                    //calculate the actual distance
                     eDistance = edge.Distance + distance[edge.From] - distance[edge.To];
                }
                           
                if (distance[toNode] != double.MaxValue && distance[toNode].GT(distance[fromNode] + eDistance))
                 {
                        isNegativeCycle = true;
                        break;
                 }
               

            }



            return new Tuple<bool, Dictionary<GraphNode<int>, GraphNode<int>>, Dictionary<GraphNode<int>, double>>(isNegativeCycle, parent,distance);

        }

        public Tuple<bool, Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double>> FloydWarshallFunction(Graph<int> g)
        {
            #region pseudocode

            /*
             * 
             * Create a |V| x |V| matrix, M, that will describe the distances between vertices
                For each cell (i, j) in M:
                    if i == j:
                        M[i][j] = 0
                    if (i, j) is an edge in E:
                        M[i][j] = weight(i, j)
                    else:
                        M[i][j] = infinity
                for k from 1 to |V|:
                    for i from 1 to |V|:
                        for j from 1 to |V|:
                            if M[i][j] > M[i][k] + M[k][j]:
                                M[i][j] = M[i][k] + M[k][j]
             * 
             * 
             */
            #endregion
            bool isNegativeCycle = false;
            Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double> distance = new Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double>();


           

            foreach (GraphNode<int> node1 in g.GetVertices())
            {
                foreach (GraphNode<int> node2 in g.GetVertices())
                {
                    Tuple<GraphNode<int>, GraphNode<int>> keyset = new Tuple<GraphNode<int>, GraphNode<int>>(node1, node2);
                    if (node1.Value.Equals(node2.Value))
                    {
                        distance.Add(keyset, 0);
                    }
                    else
                    {
                        distance.Add(keyset, double.MaxValue);
                    }

                }
            }

            foreach (GraphEdge<int> edge in g.GetEdges())
            {
                GraphNode<int> fromNode = edge.From;
                GraphNode<int> toNode = edge.To;
                Tuple<GraphNode<int>, GraphNode<int>> keyset = new Tuple<GraphNode<int>, GraphNode<int>>(fromNode, toNode);
                distance[keyset] = edge.Distance;
            }



            foreach (GraphNode<int> knode in g.GetVertices())
            {
                foreach (GraphNode<int> inode in g.GetVertices())
                {
                    foreach (GraphNode<int> jnode in g.GetVertices())
                    {
                        Tuple<GraphNode<int>, GraphNode<int>> ijkeyset = new Tuple<GraphNode<int>, GraphNode<int>>(inode, jnode);
                        Tuple<GraphNode<int>, GraphNode<int>> ikkeyset = new Tuple<GraphNode<int>, GraphNode<int>>(inode, knode);
                        Tuple<GraphNode<int>, GraphNode<int>> kjkeyset = new Tuple<GraphNode<int>, GraphNode<int>>(knode, jnode);

                        if (distance[ijkeyset] > distance[ikkeyset] + distance[kjkeyset])
                        {
                            distance[ijkeyset] = distance[ikkeyset] + distance[kjkeyset];
                        }
                    }
                }
            }

            //detect negative cycle
            //if the distacne of node i to itself is < 0 then there is a negative cycle
            foreach (GraphNode<int> node in g.GetVertices())
            {
               
                if (distance[new Tuple<GraphNode<int>, GraphNode<int>>(node,node)] < 0.0)
                {
                    isNegativeCycle = true;

                }

            }

                return new Tuple<bool, Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double>>(isNegativeCycle,distance);
        }

        public Tuple<Boolean, Dictionary<GraphNode<int>, float>, Dictionary<GraphNode<int>, GraphNode<int>>> DijkstraFunction(Graph<int> g, GraphNode<int>startNode,GraphNode<int> goalNode)
        {
            #region pseudocode
            /*

            function dijkstra(G, S)
                for each vertex V in G
                    distance[V] < -infinite
                    previous[V] < -NULL
                    If V != S, add V to Priority Queue Q
                distance[S] < -0


                while Q IS NOT EMPTY
                    U < -Extract MIN from Q
                    for each unvisited neighbour V of U
                        tempDistance < -distance[U] + edge_weight(U, V)
                        if tempDistance < distance[V]
                            distance[V] < -tempDistance
                            previous[V] < -U
                return distance[], previous[]

    */
            #endregion
            Dictionary<GraphNode<int>, float> distance = new Dictionary<GraphNode<int>, float>();
            Dictionary<GraphNode<int>, GraphNode<int>> parent = new Dictionary<GraphNode<int>, GraphNode<int>>();
            SimplePriorityQueue<GraphNode<int>> queue = new SimplePriorityQueue<GraphNode<int>>();
            HashSet<GraphNode<int>> visited = new HashSet<GraphNode<int>>();
            Tuple<Boolean, Dictionary<GraphNode<int>, float>, Dictionary<GraphNode<int>, GraphNode<int>>> returnVal; 


            foreach (GraphNode<int> node in g.GetVertices())
            {
                distance[node] = float.PositiveInfinity;
            }
            distance[startNode] = 0;
            queue.Enqueue(startNode, distance[startNode]);

            GraphNode<int> currentNode = null;

            while(queue.Count != 0)
            {
                currentNode = queue.Dequeue();

                if (!visited.Contains(currentNode))
                {
                    visited.Add(currentNode);

                    if(currentNode == goalNode)
                    {
                        break;
                    }
                    foreach(GraphEdge<int> edge in currentNode.GetEdges())
                    {
                        GraphNode<int> nextNode = edge.OtherNode(currentNode);
                        float newDistance = distance[currentNode] + (float) edge.Distance;
                        if(newDistance < distance[nextNode])
                        {
                            distance[nextNode] = newDistance;
                            queue.Enqueue(nextNode, distance[nextNode]);
                            parent[nextNode] = currentNode;
                        }


                    }

                }

            }

            

            if (currentNode == goalNode)
            {
               returnVal = new Tuple<bool, Dictionary<GraphNode<int>, float>, Dictionary<GraphNode<int>, GraphNode<int>>>(true, distance, parent);
            }
            else
            {
                returnVal = new Tuple<bool, Dictionary<GraphNode<int>, float>, Dictionary<GraphNode<int>, GraphNode<int>>>(false, null, null);
            }

            return returnVal;



        }


        public Tuple<Dictionary<GraphNode<int>, float>, Dictionary<GraphNode<int>, GraphNode<int>>> DijkstraFunction(Graph<int> g, GraphNode<int> startNode)
        {
            #region pseudocode
            /*

            function dijkstra(G, S)
                for each vertex V in G
                    distance[V] < -infinite
                    previous[V] < -NULL
                    If V != S, add V to Priority Queue Q
                distance[S] < -0


                while Q IS NOT EMPTY
                    U < -Extract MIN from Q
                    for each unvisited neighbour V of U
                        tempDistance < -distance[U] + edge_weight(U, V)
                        if tempDistance < distance[V]
                            distance[V] < -tempDistance
                            previous[V] < -U
                return distance[], previous[]

            */
            #endregion
            Dictionary<GraphNode<int>, float> distance = new Dictionary<GraphNode<int>, float>();
            Dictionary<GraphNode<int>, GraphNode<int>> parent = new Dictionary<GraphNode<int>, GraphNode<int>>();
            SimplePriorityQueue<GraphNode<int>> queue = new SimplePriorityQueue<GraphNode<int>>();
            HashSet<GraphNode<int>> visited = new HashSet<GraphNode<int>>();
            Tuple<Boolean, Dictionary<GraphNode<int>, float>, Dictionary<GraphNode<int>, GraphNode<int>>> returnVal;


            foreach (GraphNode<int> node in g.GetVertices())
            {
                distance[node] = float.PositiveInfinity;
            }
            distance[startNode] = 0;
            queue.Enqueue(startNode, distance[startNode]);

            GraphNode<int> currentNode = null;

            while (queue.Count != 0)
            {
                currentNode = queue.Dequeue();

                if (!visited.Contains(currentNode))
                {
                    visited.Add(currentNode);

                   
                    foreach (GraphEdge<int> edge in currentNode.GetEdges())
                    {
                        GraphNode<int> nextNode = edge.OtherNode(currentNode);
                        float newDistance = distance[currentNode] + (float)edge.Distance;
                        if (newDistance < distance[nextNode])
                        {
                            distance[nextNode] = newDistance;
                            queue.Enqueue(nextNode, distance[nextNode]);
                            parent[nextNode] = currentNode;
                        }


                    }

                }

            }



            
            return new Tuple<Dictionary<GraphNode<int>, float>, Dictionary<GraphNode<int>, GraphNode<int>>>(distance,parent);
            
           



        }
        public Tuple<bool,Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double>>  JohnsonFunction(Graph<int> g)
        {
            #region pseudocode
            /*
            Johnson(G)
                1.
                create G` where G`.V = G.V + { s},
                    G`.E = G.E + ((s, u) for u in G.V), and
                    weight(s, u) = 0 for u in G.V
                2.
                if Bellman - Ford(s) == False
                    return "The input graph has a negative weight cycle"
                else:
                    for vertex v in G`.V:
                        h(v) = distance(s, v) computed by Bellman - Ford
                    for edge(u, v) in G`.E:
                        weight`(u, v) = weight(u, v) + h(u) - h(v)
                3.
                    D = new matrix of distances initialized to infinity
                    for vertex u in G.V:
                        run Dijkstra(G, weight`, u) to compute distance`(u, v) for all v in G.V
                        for each vertex v in G.V:
                            D_(u, v) = distance`(u, v) + h(v) - h(u)
                    return D

           
            */
            #endregion

            Dictionary<GraphNode<int>, double> distance =  new Dictionary<GraphNode<int>, double>();
            Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double> distMatrix = new Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double>();
            bool isNegativeCycle = false;

            //Step1 - Add a additional node

            Tuple<Graph<int>, GraphNode<int>> result = AddAdditionalNode(g);

            Graph<int> gprime = result.Item1;
            GraphNode<int> newNode = result.Item2;

            //Step2 - Bellman Ford

            Tuple<bool, Dictionary<GraphNode<int>, GraphNode<int>>, Dictionary<GraphNode<int>, double>> returnVal = BellmanFordFunction(gprime, newNode,true);

            
            if(returnVal.Item1 == true)
            {
                //The input graph has a negative weight cycle
                isNegativeCycle = true;
                return new Tuple<bool, Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double>>(isNegativeCycle,null);
            }

            //h(v) = distance(s, v) computed by Bellman - Ford
            distance = returnVal.Item3;

            //remove the additional vertex
            gprime.DeleteVertex(newNode);

            // weight`(u, v) = weight(u, v) + h(u) - h(v)
            foreach (GraphEdge<int> edge in gprime.GetEdges())
            {
                edge.Distance = edge.Distance + distance[edge.From] - distance[edge.To];
            }


           
            //Step 3
            //initialize all distances to infinity
            foreach (GraphNode<int> node1 in gprime.GetVertices())
            {
                foreach (GraphNode<int> node2 in gprime.GetVertices())
                {
                    Tuple<GraphNode<int>, GraphNode<int>> keyset = new Tuple<GraphNode<int>, GraphNode<int>>(node1, node2);                   
                        distMatrix.Add(keyset, double.MaxValue);                  
                }
            }

            foreach(GraphNode<int> u in gprime.GetVertices())
            {
                //distance from u
                var dist = DijkstraFunction(gprime, u).Item1;
                foreach(GraphNode<int> v in dist.Keys)
                {
                    //D_(u, v) = distance`(u, v) + h(v) - h(u)
                    double d = (double)dist[v] + distance[v] - distance[u];
                    distMatrix[new Tuple<GraphNode<int>, GraphNode<int>>(u, v)] = d;

                }

            }

           
            return new Tuple<bool, Dictionary<Tuple<GraphNode<int>, GraphNode<int>>, double>>(isNegativeCycle, distMatrix);
        }

        private Tuple<Graph<int>, GraphNode<int>> AddAdditionalNode(Graph<int> g)
        {
            Graph<int> gprime = new Graph<int>();

            //Add a new node
            GraphNode<int> n = new GraphNode<int>(int.MaxValue);
            gprime.AddVertex(n);

            foreach (GraphNode<int> node in g.GetVertices())
            {
                gprime.AddVertex(new GraphNode<int>(node.Value));
            }
            foreach (GraphEdge<int> edge in g.GetEdges())
            {
                gprime.AddEdge(new GraphNode<int>(edge.From.Value),new GraphNode<int>(edge.To.Value),edge.Distance);
            }

           
           

            //Add an edge from the new node to all nodes with distance zero;

            foreach (GraphNode<int> node in g.GetVertices())
            {
                gprime.AddEdge(n, node, 0);
            }

            return new Tuple<Graph<int>, GraphNode<int>>(gprime,n);
        }
    }
}

