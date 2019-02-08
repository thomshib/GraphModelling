using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    class TSPDynamicPrg
    {

        double[,] adjmatrix;
        int[] setArray;
        double MAX_VALUE = 99999;
        public TSPDynamicPrg(double[,] adjmatrix)
        {
            this.adjmatrix = adjmatrix;   
        }

        public double tsp()
        {
            
            int n = adjmatrix.GetLength(0);
            int startNode = 0;

            Dictionary<Node, double> minCostNodeMap = new Dictionary<Node, double>();

            //contains {1},{2},{3},{1,2},{1,3},{2,3},{1,2,3} sets
            List<SortedSet<int>> sets = GenerateSets(n);

            //a two dim arrary indexed by subset S={1,2,3,..n} that contain startnode and 
            //destinations j={1,2,3,...n}
            int[,] dp = new int[sets.Count, n];

            //initialization
            //
            double tsp_min_cost = MAX_VALUE;
            foreach (SortedSet<int> s in sets)
            {
                for (int currNode = 0; currNode < n; currNode++)
                {
                    if (!set_contains(s, currNode))
                    {
                        double min_cost = MAX_VALUE;
                        SortedSet<int> copyset = new SortedSet<int>(s);
                        Node sprime = new Node(currNode, copyset);
                        

                        foreach (int prevNode in s)
                        {
                            double cost = adjmatrix[prevNode, currNode] + GetCost(copyset, prevNode, minCostNodeMap);

                            if(cost < min_cost)
                            {
                                min_cost = cost;
                            }
                            
                            


                        }
                        
                    // for empty subset
                        if(s.Count == 0){
                                min_cost = adjmatrix[startNode, currNode];
                            }

                        minCostNodeMap.Add(sprime, min_cost);



                    }
                    
                   

                }
            }


            double m_cost = MAX_VALUE;
            SortedSet<int> newSet = new SortedSet<int>();
            for (int i = 0; i < n; i++)
            {
                newSet.Add(i);
            }

            for (int i = 0; i < n; i++)
            {
                double cost = GetCost(newSet, i, minCostNodeMap) + adjmatrix[i, startNode];
               if(cost < m_cost)
                {
                    m_cost = cost;
                }
            }

            return m_cost;
        }

        private double GetCost(SortedSet<int> s, int prevNode, Dictionary<Node, double> minCostNodeMap)
        {
            double cost = 0;

            //s = {0,1} remove the prevNode to get only the cost for prevNode 
            s.Remove(prevNode);
            //s = {0,1} remove the prevNode to get only the cost for prevNode 
            SortedSet<int> ss = new SortedSet<int>(s);
            Node sprime = new Node(prevNode, ss);
            if (minCostNodeMap.ContainsKey(sprime))
            {
                cost = minCostNodeMap[sprime];
            }
            s.Add(prevNode);
            return cost;

        }

        private int set_extract_prevoius(SortedSet<int> s, int last)
        {
            int count = s.Count;

            return 0;
        }

        private bool set_contains(SortedSet<int> s, int last)
        {
            return s.Contains<int>(last);
        }

        #region Generate Sets

        private  List<SortedSet<int>> GenerateSets(int n)
        {
            setArray = new int[n] ;
            for(int i =0; i < n; i++)
            {
                setArray[i] = i;
            }

            int r = 0;
            int[] data = new int[n];
            

            List<SortedSet<int>> sets = new List<SortedSet<int>>();
            //add an empty set
            sets.Add(new SortedSet<int>());

            for (int i = 1; i <= setArray.Length; i++)
            {
                r = i;
                GenerateCombinationSets(setArray, data, sets, 0, setArray.Length - 1, 0, r);
            }

            return sets;
            

        }

        private  void GenerateCombinationSets(int[] arr, int[] data, List<SortedSet<int>> sets, int start, int end, int index, int r)
        {
            if (index == r)
            {
                SortedSet<int> hs = new SortedSet<int>();
                for (int j = 0; j < r; j++)
                {
                    hs.Add(data[j]);
                  
                }
                sets.Add(hs);
               
                return;
            }

            for (int i = start; i <= end && (end - i + 1) >= (r - index); i++)
            {
                data[index] = arr[i];
                GenerateCombinationSets(arr, data, sets, i + 1, end, index + 1, r);
            }

        }

        #endregion

        #region Set Class

        internal class Node
        {
            int node;
            SortedSet<int> nodeSet;
            public Node(int node, SortedSet<int> nodeSet)
            {
                this.node = node;
                this.nodeSet = nodeSet;
            }

            public override bool Equals(object obj)
            {

                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                Node s = (Node)obj;

                if (!this.node.Equals(s.node)) return false;

                if (nodeSet != null && s.nodeSet != null)
                {
                    return nodeSet.SetEquals(s.nodeSet);
                }

                if (nodeSet == null && s.nodeSet == null)
                {
                    return true;
                }

                if ((nodeSet == null && s.nodeSet != null) || (nodeSet != null && s.nodeSet == null) )
                {
                    return false;
                }

                return false;



            }

            public override int GetHashCode()
            {
                //int result = node;

                //result = 31 * result + (this.nodeSet == null ? 0 : nodeSet.GetHashCode()) ;

                //return result;

                int hash = 19;
                hash = hash * 31 + node;

                //this does not work
                //hash = hash * 31 + (this.nodeSet == null ? 0 : nodeSet.GetHashCode());
                
                foreach(var item in nodeSet)
                {
                    hash = hash * 31 + item.GetHashCode();
                }
                
                return hash;
            }
        }

        #endregion
    }


}
