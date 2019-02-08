using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    //https://www.youtube.com/watch?v=JE0JE8ce1V0
    class TSPRecursiveBitMask
    {
        //int[][] adjmatrix = new int[][]{
        //    new int[]{0,20,42,25},
        //     new int[]{20,0,30,34},
        //      new int[]{42,30,0,10},
        //       new int[]{25,34,10,0}
        //};

        int[][] adjmatrix = new int[][]{
            new int[]{0,20,42},
             new int[]{20,0,30},
              new int[]{42,30,0 }
        };

        int n;
        int VISITED;
        int final_dist = int.MaxValue;
        public TSPRecursiveBitMask()
        {
           n  = adjmatrix.Length;
            VISITED = (1 << n) - 1;
        }
        
        public int tsp(int mask,int pos)
        {
            Console.WriteLine("Enter mask=" + mask + " pos=" + pos);
            int final_dist = int.MaxValue;

            if (mask == VISITED)
            {
                Console.WriteLine("Exit adjmatrix[pos= " + pos + "][0] = " + adjmatrix[pos][0]);
                return adjmatrix[pos][0];
                
            }

            
            for(int city = 0; city < n; city++)
            {
                //check if the city has been visited
                if( (mask & (1 << city)) == 0)
                {
                    //City 0, mask  = 0001
                    //City 1, mask =  0010 = 0001 OR 0010
                    int m = (mask | (1 << city));

                    Console.WriteLine("Enter Recursion adjmatrix[pos=" + pos + "][city=" + city + "]" + " + tsp(mask=" + m + ",city=" + city + ")");
                    int newdist = adjmatrix[pos][city] + tsp((mask | (1 << city)), city);
                    final_dist = Math.Min(final_dist, newdist);
                    Console.WriteLine("Exit Recursion final_dist=" + final_dist);

                }
            }

            return final_dist;
        }
    }
}
