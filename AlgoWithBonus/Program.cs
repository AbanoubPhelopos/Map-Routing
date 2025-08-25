using System.Diagnostics;

namespace MapRouting
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static void runTest(string mapPath, string queryPath, string? outputPath, bool useEnhanced = false)
        {
            Stopwatch IOincluded = new Stopwatch();
            IOincluded.Start();

            if (outputPath == null)
            {
                outputPath = @"D:\college\third year\6th term\algorism\Project\AlgoWithBonus\outputs\output.txt";
            }

            var (coords, adj, M, Q, I, N) = mapAndQueryReader.ReadMapFile(mapPath);
            var queries = mapAndQueryReader.ReadQueriesFromFile(queryPath);

            string s = "";
            Stopwatch IOexcluded = new Stopwatch();
            IOexcluded.Start();

            foreach (var query in queries)
            {
                if (useEnhanced)
                {
                    // Use enhanced algorithm
                    EnhancedSSSP.GetStartAndEndNodes(coords, adj,
                        query.Value.R,
                        query.Value.SourceX, query.Value.SourceY,
                        query.Value.DestinationX, query.Value.DestinationY, N);

                    var enhancedSolver = new EnhancedSSSP();
                    s += enhancedSolver.EnhancedDijkstra(adj, N); // Fixed: Added missing parameter N
                }
                else
                {
                    // Use original Dijkstra
                    solveProblem.getStartAndEndNodes(coords, adj,
                        query.Value.R,
                        query.Value.SourceX, query.Value.SourceY,
                        query.Value.DestinationX, query.Value.DestinationY, N);

                    s += solveProblem.dijkstra(adj);
                }
            }

            IOexcluded.Stop();
            IOincluded.Stop();

            s += $"{IOexcluded.ElapsedMilliseconds} ms ({(useEnhanced ? "Enhanced O(m log^(2/3) n)" : "Original O(m + n log n)")} Algorithm)\n\n";
            s += $"{IOincluded.ElapsedMilliseconds} ms (Total with I/O)";

            File.WriteAllText(outputPath, s);
            MessageBox.Show($"Test completed using {(useEnhanced ? "Enhanced" : "Original")} Algorithm!\nResults saved to: {outputPath}");
        }

        public static void runComparison(string mapPath, string queryPath, string? outputPath)
        {
            if (outputPath == null)
            {
                outputPath = @"D:\college\third year\6th term\algorism\Project\AlgoWithBonus\outputs\comparison.txt";
            }

            var (coords, adj, M, Q, I, N) = mapAndQueryReader.ReadMapFile(mapPath);
            var queries = mapAndQueryReader.ReadQueriesFromFile(queryPath);

            string results = "ALGORITHM COMPARISON RESULTS\n";
            results += "============================\n\n";

            foreach (var query in queries)
            {
                results += $"Query {query.Key}:\n";
                results += "----------------\n";

                // Test original algorithm
                var adjCopy1 = DeepCopyAdjacencyList(adj);
                solveProblem.getStartAndEndNodes(coords, adjCopy1,
                    query.Value.R,
                    query.Value.SourceX, query.Value.SourceY,
                    query.Value.DestinationX, query.Value.DestinationY, N);

                Stopwatch sw1 = Stopwatch.StartNew();
                string originalResult = solveProblem.dijkstra(adjCopy1);
                sw1.Stop();

                results += "Original Dijkstra O(m + n log n):\n";
                results += originalResult;
                results += $"Time: {sw1.ElapsedMilliseconds} ms\n\n";

                // Test enhanced algorithm
                var adjCopy2 = DeepCopyAdjacencyList(adj);
                EnhancedSSSP.GetStartAndEndNodes(coords, adjCopy2,
                    query.Value.R,
                    query.Value.SourceX, query.Value.SourceY,
                    query.Value.DestinationX, query.Value.DestinationY, N);

                Stopwatch sw2 = Stopwatch.StartNew();
                var enhancedSolver = new EnhancedSSSP();
                string enhancedResult = enhancedSolver.EnhancedDijkstra(adjCopy2, N); // Fixed: Added missing parameter N
                sw2.Stop();

                results += "Enhanced Algorithm O(m log^(2/3) n):\n";
                results += enhancedResult;
                results += $"Time: {sw2.ElapsedMilliseconds} ms\n";

                if (sw2.ElapsedMilliseconds > 0)
                {
                    double speedup = (double)sw1.ElapsedMilliseconds / sw2.ElapsedMilliseconds;
                    results += $"Speedup: {speedup:F2}x\n";
                }
                else
                {
                    results += "Speedup: N/A (too fast to measure)\n";
                }

                results += "----------------------------------------\n\n";
            }

            File.WriteAllText(outputPath, results);
            MessageBox.Show($"Algorithm comparison completed!\nResults saved to: {outputPath}");
        }

        private static Dictionary<int, List<(int, double, double)>> DeepCopyAdjacencyList(
            Dictionary<int, List<(int, double, double)>> original)
        {
            var copy = new Dictionary<int, List<(int, double, double)>>();
            foreach (var kvp in original)
            {
                copy[kvp.Key] = new List<(int, double, double)>(kvp.Value);
            }
            return copy;
        }

        public static (Dictionary<int, (double x, double y)> coordinates,
                      Dictionary<int, List<(int, double, double)>> adjacencyList,
                      int M, int N, double R, Dictionary<int, Query> queries)
        visualize(string mapPath, string queryPath)
        {
            var (coords, adj, M, Q, R, N) = mapAndQueryReader.ReadMapFile(mapPath);
            var queries = mapAndQueryReader.ReadQueriesFromFile(queryPath);
            return (coords, adj, M, N, R, queries);
        }

        // Fixed the commented method as well
        public static List<int> queryVisualize(Dictionary<int, (double x, double y)> coordinates,
            Dictionary<int, List<(int, double, double)>> adjacencyList,
            Dictionary<int, Query> queries,
            int querycnt, int N, bool useEnhanced = false)
        {
            if (useEnhanced)
            {
                EnhancedSSSP.GetStartAndEndNodes(coordinates, adjacencyList,
                    queries[querycnt].R,
                    queries[querycnt].SourceX, queries[querycnt].SourceY,
                    queries[querycnt].DestinationX, queries[querycnt].DestinationY,
                    N);

                var enhancedSolver = new EnhancedSSSP();
                return enhancedSolver.VisualizeEnhancedDijkstra(adjacencyList, N); // Fixed: Added missing parameter N
            }
            else
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
}
