using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoWithBonus
{

    public class Query
    {
        public double SourceX { get; set; }
        public double SourceY { get; set; }
        public double DestinationX { get; set; }
        public double DestinationY { get; set; }
        public double R { get; set; }
        public Query(double sourceX, double sourceY, double destinationX, double destinationY, double r)
        {
            SourceX = sourceX;
            SourceY = sourceY;
            DestinationX = destinationX;
            DestinationY = destinationY;
            R = r;
        }
    }
    class mapAndQueryReader
    {

        public static Dictionary<int, Query> ReadQueriesFromFile(string path)
        {
            var queries = new Dictionary<int, Query>();
            string[] lines = File.ReadAllLines(path);
            int q = int.Parse(lines[0]);

            for (int i = 1; i <= q; i++)
            {
                var parts = lines[i].Split();
                double sourceX = double.Parse(parts[0], CultureInfo.InvariantCulture);
                double sourceY = double.Parse(parts[1], CultureInfo.InvariantCulture);
                double destinationX = double.Parse(parts[2], CultureInfo.InvariantCulture);
                double destinationY = double.Parse(parts[3], CultureInfo.InvariantCulture);
                double r = double.Parse(parts[4], CultureInfo.InvariantCulture);
                queries.Add(i - 1, new Query(sourceX, sourceY, destinationX, destinationY, r));
            }

            return queries;
        }

        public static (Dictionary<int, (double x, double y)> coordinates,
                   Dictionary<int, List<(int neighbor, double time, double length)>> adjacencyList, int M, int Q, double I, int N)
        ReadMapFile(string filePath)
        {

            string[] lines = File.ReadAllLines(filePath);
            int lineIndex = 0;
            int N = int.Parse(lines[lineIndex++]);
            var coordinates = new Dictionary<int, (double, double)>();

            for (int i = 0; i < N; i++)
            {
                var parts = lines[lineIndex++].Split();
                int id = int.Parse(parts[0]);
                double x = double.Parse(parts[1], CultureInfo.InvariantCulture);
                double y = double.Parse(parts[2], CultureInfo.InvariantCulture);
                coordinates[id] = (x, y);
            }

            var adjacencyList = new Dictionary<int, List<(int, double, double)>>();


            var meta = lines[lineIndex++].Split();
            int M;
            int Q = -1;
            double I = -1;

            if (meta.Length == 1)
                M = int.Parse(meta[0]);

            else if (meta.Length == 3)
            {
                M = int.Parse(meta[0]);
                Q = int.Parse(meta[1]);
                I = double.Parse(meta[2], CultureInfo.InvariantCulture);
            }
            else
                throw new FormatException($"Unexpected metadata line: {string.Join(" ", meta)}");

            //MessageBox.Show(Q.ToString());

            if (Q == -1)
            {
                foreach (var id in coordinates.Keys)
                    adjacencyList[id] = new List<(int, double, double)>();

                for (int i = 0; i < M; i++)
                {
                    var parts = lines[lineIndex++].Split();
                    int u = int.Parse(parts[0]);
                    int v = int.Parse(parts[1]);

                    double length = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    double speed = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    double time = (length / speed) * 60;

                    adjacencyList[u].Add((v, time, length));
                    adjacencyList[v].Add((u, time, length));
                }
            }

            /*for (int i = 0; i < N; i++)
            {
                foreach (var item in adjacencyList[i])
                {
                    Console.WriteLine($"From Node: {i} to Node: {item.Item1}, Time: {item.Item2}, length: {item.Item3}");
                }
            }*/

            return (coordinates, adjacencyList, M, Q, I, N);
        }
    }
}
