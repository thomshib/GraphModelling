using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GraphModelling
{
    class LoadGraph
    {
        public LoadGraph()
        {

        }

        public void LoadAdjacencyListGraph(Graph<int> g, string filename)
        {

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {


                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(' ');
                    GraphNode<int> nodeFrom = new GraphNode<int>(int.Parse(values[0]));
                    GraphNode<int> nodeTo = new GraphNode<int>(int.Parse(values[1]));
                    double distance = double.Parse(values[2]);
                    GraphEdge<int> edge = new GraphEdge<int>(nodeFrom, nodeTo, distance);
                    g.AddVertex(nodeFrom);
                    g.AddVertex(nodeTo);
                    g.AddEdge(nodeFrom, nodeTo, distance);



                }
            }
        }

        public double[,] LoadAdjacencyMatrixGraph(string filename)
        {

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {


                string line;
                line = reader.ReadLine();
                int count = int.Parse(line);
                double[,] adjMatrix = new double[count, count];


                int row = 0;
                int col = 0;

                List<Tuple<double, double>> list = new List<Tuple<double, double>>(); 
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(' ');
                    double nodeFrom = double.Parse(values[0]);
                    double nodeTo = double.Parse(values[1]);
                    list.Add(new Tuple<double, double>(nodeFrom, nodeTo));              
                 
                }

                foreach (Tuple<double, double> xx in list)
                {
                    col = 0;
                    foreach (Tuple<double, double> yy in list)
                    {
                        if (xx.Item1.Equals(yy.Item1) && xx.Item2.Equals(yy.Item2))
                        {
                            //diagonal make the distance zero
                            adjMatrix[row, col] = 0;

                        }
                        else
                        {
                            adjMatrix[row, col] = distance(xx.Item1, xx.Item2, yy.Item1, yy.Item2);
                        }
                        col++;
                    }
                    row++;
                }

                return adjMatrix;

                
            }
        }

        public double distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }

    }
}
