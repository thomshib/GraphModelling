using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    class TSPIterativeBitMask<T>
    {
        //http://www.cs.ucf.edu/~dmarino/progcontests/modules/dptsp/DP-TSP-Notes.pdf
        //https://www.hackerearth.com/practice/notes/codemonk-dynamic-programming-ii-1/
        //https://ideone.com/q9443k
        //https://www.hackerearth.com/practice/algorithms/graphs/hamiltonian-path/tutorial/
        //http://my-zhang.github.io/blog/2014/04/20/dynamic-programming-with-bitmask/

        //int[,] adjmatrix = new int[,]{
        //    {0,20,42,25},
        //     {20,0,30,34},
        //      {42,30,0,10},
        //       {25,34,10,0}
        //};

        //int[,] adjmatrix = new int[,]{
        //    {0,20,42},
        //     {20,0,30},
        //      {42,30,0}

        //};

        T[,] adjmatrix;

        int n;


        public TSPIterativeBitMask(T[,] adj)
        {
            this.adjmatrix = adj;
        }

        public T tsp()
        {
            int startNode = 0;
            n = adjmatrix.GetLength(0);

            int LIMIT_MASK = (1 << n);
            T maxValue = GetMaxValue();


            //first dimesion is the subset and the second dimension is the last vertex
            T[,] dp = new T[LIMIT_MASK, n];

            ////initialize everything to max value;

            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < LIMIT_MASK; k++)
                {
                    dp[k, i] = GetMaxValue();
                }
                //base case of visiting the city from itself does not work
                //dp[1 << i, i] = 0;

                //base case of visiting the city from a startnode
                dp[1 << i, i] = dist(startNode, i);
            }

            //for all subsets
            for (int mask = 0; mask < LIMIT_MASK; mask++)
            {
                for (int last = 0; last < n; last++)
                {
                   
                    if (!dp[mask, last].Equals(maxValue)) continue;

                    //the last vertex isn’t in the subset specified by mask, then continue
                    if (is_element(mask, last))
                    {
                        T min_cost = maxValue;
                        int prev = mask - (1 << last);
                        for (int k = 0; k < n; k++)
                        {
                            if (is_element(mask, k) && k != last)
                            {
                                T curCost = Add(dp[prev, k], dist(k, last));

                                if (min_cost.Equals(maxValue) )
                                {
                                    min_cost = curCost;
                                }
                                if(GreaterThan(min_cost, curCost))
                                {
                                    min_cost = curCost;
                                }
                            }
                        }

                        dp[mask, last] = min_cost;

                    }
                }
            }

            T m_cost =  maxValue; ;
            for (int i = 0; i < n; i++)
            {
                T cost = Add(dp[LIMIT_MASK - 1, i], dist(i, startNode));
                if (m_cost.Equals(maxValue)) 
                {
                    m_cost = cost;
                }

                if(GreaterThan(m_cost,cost)){
                        m_cost = cost;
                  }
            }

            return m_cost;

        }

        public bool tsp_path()
        {
            bool[,] adj = new bool[,]{
            {false,true,false},
             {true,false,true},
              {true,true,false}

        };

            n = adj.GetLength(0);

            int LIMIT_MASK = (1 << n);

            //first dimesion is the subset and the second dimension is the last vertex
            bool[,] dp = new bool[n, LIMIT_MASK];

            ////initialize 
            for (int i = 0; i < LIMIT_MASK; i++)
            {
                for (int j = 0; j < n; j++)
                {

                    dp[j, i] = false;

                }
            }

            //base case of city to itself is zero
            for (int i = 0; i < n; ++i)
            {
                dp[i, 1 << i] = true;
            }

            //for all subsets
            for (int mask = 0; mask < LIMIT_MASK; ++mask) //i
            {
                for (int last = 0; last < n; ++last) //j
                {
                    //check if last is set in mask
                    if (((mask >> last) & 1) == 0)
                    {

                        for (int prev = 0; prev < n; ++prev) //k
                        {
                            if ((last != prev) && (((mask >> prev) & 1) == 0) && (adj[prev, last] == true))
                            {
                                var maskXORj = mask ^ (1 << last);
                                if (dp[prev, maskXORj] == true)
                                {
                                    dp[last, mask] = true;
                                    //break;
                                }

                            }

                        }
                    }


                }
            }

            bool ans = false;
            for (int i = 0; i < n; i++)
            {
                if (dp[i, (LIMIT_MASK - 1)] == true)
                {
                    ans = true;
                    break;

                }
            }

            return ans;

        }


        bool is_element(int bitmask, int index)
        {
            return bitmask == (bitmask | (1 << index));
        }

        T dist(int i, int j)
        {
            return adjmatrix[i, j];
        }

        public T GetMaxValue()
        {
            decimal d = 9999999;
            object maxValue = default(T);
            TypeCode typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Decimal:
                    maxValue = d;
                    break;
                case TypeCode.Double:
                    maxValue = d;
                    break;
                case TypeCode.Int16:
                    maxValue = short.MaxValue;
                    break;
                case TypeCode.Int32:
                    maxValue = int.MaxValue;
                    break;
                case TypeCode.Int64:
                    maxValue = long.MaxValue;
                    break;
                default:
                    maxValue = default(T);//set default value
                    break;
            }

            return (T)Convert.ChangeType(maxValue, typeof(T)); ;
        }

        public T Add(T a , T b)
        {
            object retValue = default(T);
            object objA = a;
            object objB = b;
            TypeCode typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Decimal:
                    retValue = (decimal)objA + (decimal)objB;
                        break;
                case TypeCode.Double:
                    retValue = (Double)objA + (Double)objB;
                    break;
                case TypeCode.Int16:
                    retValue = (Int16)objA + (Int16)objB;
                    break;
                case TypeCode.Int32:
                    retValue = (Int32)objA + (Int32)objB;
                    break;
                case TypeCode.Int64:
                    retValue = (Int64)objA + (Int64)objB;
                    break;
                default:
                    retValue = default(T);//set default value
                    break;
            }

            return (T)retValue;


        }


        public bool GreaterThan(T a, T b)
        {
            bool retValue = false;
            object objA = a;
            object objB = b;
            TypeCode typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Decimal:
                    retValue = (decimal)objA > (decimal)objB;
                    break;
                case TypeCode.Double:
                    retValue = (Double)objA > (Double)objB;
                    break;
                case TypeCode.Int16:
                    retValue = (Int16)objA > (Int16)objB;
                    break;
                case TypeCode.Int32:
                    retValue = (Int32)objA > (Int32)objB;
                    break;
                case TypeCode.Int64:
                    retValue = (Int64)objA > (Int64)objB;
                    break;
                
            }

            return retValue;
        }
    }
}
