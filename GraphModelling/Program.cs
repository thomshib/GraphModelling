using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    class Program
    {
        static void Main(string[] args)
        {

            BellmanFordTestMethod();
            ////FloydWarshallFunction();
            ////JohnsonFunction();
            ////GenerateSets();
            ////Bitmask();
            ////TSPRecursiveBitMaskTestMethod();
            ////TSPIterativeBitMaskTestMethod();
            TSPDynamicPrgTestMethod();











            Console.ReadLine();

        }

        private static void TSPDynamicPrgTestMethod()
        {
            //double[,] adjmatrix = new double[,]{
            //    {0,20,42},
            //     {20,0,30},
            //      {42,30,0}

            //};
            LoadGraph loader = new LoadGraph();
            double[,] adjmatrix = loader.LoadAdjacencyMatrixGraph(@"data\tsp.txt");

            TSPIterativeBitMask<double> tsp = new TSPIterativeBitMask<double>(adjmatrix);

            TSPDynamicPrg obj = new TSPDynamicPrg(adjmatrix);
            Console.WriteLine(obj.tsp());



        }

        private static void TSPIterativeBitMaskTestMethod()
        {
            int[,] adjmatrix = new int[,]{
                {0,20,42},
                 {20,0,30},
                  {42,30,0}

            };
            TSPIterativeBitMask<int> tsp = new TSPIterativeBitMask<int>(adjmatrix);
            

            //LoadGraph loader = new LoadGraph();
            //double[,] adjmatrix = loader.LoadAdjacencyMatrixGraph(@"data\tsp.txt");
           
            //TSPIterativeBitMask<double> tsp = new TSPIterativeBitMask<double>(adjmatrix);
            Console.WriteLine(tsp.tsp());
        }

        private static void TSPRecursiveBitMaskTestMethod()
        {
            TSPRecursiveBitMask tsp = new TSPRecursiveBitMask();
            Console.WriteLine(tsp.tsp(1,0));
        }

        private static void Bitmask()
        {
            int n = 5;
            Console.WriteLine(1 << n);

        }

        private static void JohnsonFunction()
        {
            string filename = @"data\SimpleGraph.txt";
            Graph<int> g = new Graph<int>();
            //LoadGraph loader = new LoadGraph(g, @"data\GraphWithNoNegativeCycle.txt");
            LoadGraph loader = new LoadGraph();
            loader.LoadAdjacencyListGraph(g, filename);
            var alg = new GraphTraversal();
            var result = alg.JohnsonFunction(g);
            if(result.Item1)
            {
                Console.WriteLine("Negative Path found");
                return;
            }
            if (result.Item2 != null)
            {
                foreach (var item in result.Item2)
                {
                    Tuple<GraphNode<int>, GraphNode<int>> keyset = item.Key;
                    GraphNode<int> nodeFrom = keyset.Item1;
                    GraphNode<int> nodeTo = keyset.Item2;
                    Console.WriteLine("Shortest distance from Node: " + nodeFrom.Value + " To: " + nodeTo.Value + " = " + item.Value);

                }

            }
            Console.WriteLine("Path found: ");
        }

        private static void FloydWarshallFunction()
        {
            string filename = @"data\GraphWithNegativeCycle.txt";
            Graph<int> g = new Graph<int>();
            //LoadGraph loader = new LoadGraph(g, @"data\SimpleGraphWithNegativeWeights.txt");
            LoadGraph loader = new LoadGraph();
            loader.LoadAdjacencyListGraph(g, filename);
            var alg = new GraphTraversal();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var result = alg.FloydWarshallFunction(g);
            sw.Stop();
            if (result.Item1)
            {
                Console.WriteLine("Negative cycle exists");
                return;
            }

            double minVal = double.MaxValue;
            if ( result.Item2 != null)
            {
                foreach(var item in result.Item2)
                {
                    Tuple<GraphNode<int>, GraphNode<int>> keyset = item.Key;
                    GraphNode<int> nodeFrom = keyset.Item1;
                    GraphNode<int> nodeTo = keyset.Item2;
                    Console.WriteLine("Shortest distance from Node: " + nodeFrom.Value + " To: " + nodeTo.Value + " = " + item.Value);
                    if(minVal > item.Value)
                    {
                        minVal = item.Value;
                    }
                    Console.WriteLine("The minimum cost is " + minVal);
                }

            }
            else
            {
                Console.WriteLine("This graph has a negative cycle");
            }
            Console.WriteLine("Path found: Elapsed time(ms) : " + sw.ElapsedMilliseconds);
        }

        

        static void BellmanFordTestMethod()
        {
            Graph<int> g = new Graph<int>();
            LoadGraph loader = new LoadGraph();
            loader.LoadAdjacencyListGraph(g, @"data\SimpleGraph.txt");
            GraphNode<int> start = g.GetVertex(new GraphNode<int>(1));
            var alg = new GraphTraversal();
            var result = alg.BellmanFordFunction(g, start);
            Console.WriteLine("Negative Cycle found: " + result.Item1);


            
        }


        static void GenerateSets()
        {
            int[] arr = new int[] { 1, 2, 3 };
            int r=0;
            int[] data = new int[arr.Length];

            List<SortedSet<int>> sets = new List<SortedSet<int>>(); 

            for (int i=1; i <= arr.Length; i++)
            {
                r = i;
                GenerateCombinationSets(arr, data,sets, 0, arr.Length - 1, 0, r);
            }

            Console.WriteLine();
            //GenerateCombination(arr,data,0,arr.Length-1,0,r);

        }

        private static void GenerateCombination(int[] arr,int[] data,int start,int end, int index, int r)
        {
            if(index == r)
            {
                for(int j=0; j < r; j++)
                {
                    Console.Write(data[j]);
                }
                Console.WriteLine();
                return;
            }

            for(int i = start; i <= end && (end - i + 1) >= (r - index) ; i++)
            {
                data[index] = arr[i];
                GenerateCombination(arr, data, i + 1, end, index + 1, r);
            }

        }

        private static void GenerateCombinationSets(int[] arr, int[] data, List<SortedSet<int>> sets , int start, int end, int index, int r)
        {
            if (index == r)
            {
                SortedSet<int> hs = new SortedSet<int>();
                for (int j = 0; j < r; j++)
                {
                    hs.Add(data[j]);
                    Console.Write(data[j]);
                }
                sets.Add(hs);
                Console.WriteLine();
                return;
            }

            for (int i = start; i <= end && (end - i + 1) >= (r - index); i++)
            {
                data[index] = arr[i];
                GenerateCombinationSets(arr, data,sets, i + 1, end, index + 1, r);
            }

        }

    }
}
