using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Formats.Tar;

namespace AlgoWithBonus
{


    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static void runTest(string mapPath, string queryPath, string outputPath)
        {

            Stopwatch IOincluded = new Stopwatch();
            IOincluded.Start();
            if (outputPath == null)
            { 
            outputPath = @"D:\college\third year\6th term\algorism\Project\AlgoWithBonus\outputs\output.txt";
            }

            var (coords, adj, M, Q, R, N) = mapAndQueryReader.ReadMapFile(mapPath);
            /*Console.WriteLine($"Intersections: {coords.Count}");
            Console.WriteLine($"Roads: {M}");*/

            var queries = mapAndQueryReader.ReadQueriesFromFile(queryPath);
            /*Console.WriteLine($"Loaded {queries.Count} queries from {queryPath}");
            foreach (var kv in queries)
                Console.WriteLine($"Query {kv.Key}: {kv.Value}");*/
            string s = "";
            Stopwatch IOexcluded = new Stopwatch();
            IOexcluded.Start();
            foreach (var query in queries)
            {
                //var localAdj = new Dictionary<int, List<(int neighbor, double time, double length)>>();

                /*foreach (var kv in adj)
                {
                    localAdj[kv.Key] = new List<(int, double, double)>(kv.Value);
                }*/

                solveProblem.getStartAndEndNodes(coords, adj,
                    query.Value.R,
                    query.Value.SourceX, query.Value.SourceY,
                    query.Value.DestinationX, query.Value.DestinationY,
                    N);

                s += solveProblem.dijkstra(adj);

            }
            IOexcluded.Stop();
            //Console.WriteLine(IOexcluded.ElapsedMilliseconds);


            IOincluded.Stop();
            //Console.WriteLine(IOincluded.ElapsedMilliseconds);

            s += $"{IOexcluded.ElapsedMilliseconds} ms\n\n";
            s += $"{IOincluded.ElapsedMilliseconds} ms";

            File.WriteAllText(outputPath, s);



            MessageBox.Show("Test Done");

            /*coords.Clear();
            adj.Clear();*/
        }


        public static (Dictionary<int, (double x, double y)> coordinates,
                   Dictionary<int, List<(int neighbor, double time, double length)>> adjacencyList,
                   int M, int N, double R, Dictionary<int, Query> queries) visualize(string mapPath, string queryPath)
        {

            

            var (coords, adj, M, Q, R, N) = mapAndQueryReader.ReadMapFile(mapPath);
            var queries = mapAndQueryReader.ReadQueriesFromFile(queryPath);


            return (coords, adj, M, N, R, queries);
        }

        public static List<int> queryVisualize(Dictionary<int, (double x, double y)> coordinates,
                   Dictionary<int, List<(int neighbor, double time, double length)>> adjacencyList, Dictionary<int, Query> queries, 
                   int querycnt, int N)
        {
            solveProblem.getStartAndEndNodes(coordinates, adjacencyList,
                    queries[querycnt].R,
                    queries[querycnt].SourceX, queries[querycnt].SourceY,
                    queries[querycnt].DestinationX, queries[querycnt].DestinationY,
                    N);

            return solveProblem.visDijkstra(adjacencyList);
        }
    }
}