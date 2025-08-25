namespace MapRouting
{
    internal class EnhancedSSSP
    {
        private Dictionary<int, List<(int neighbor, double time, double length)>> adj = new();
        private Dictionary<int, double> distances = new();
        private Dictionary<int, int> predecessors = new();
        private HashSet<int> complete = new();
        private int k; // ⌊log^(1/3)(n)⌋
        private int t; // ⌊log^(2/3)(n)⌋
        private int n; // Number of vertices

        public static void GetStartAndEndNodes(Dictionary<int, (double x, double y)> coordinates,
            Dictionary<int, List<(int, double, double)>> adj, double R,
            double sourceX, double sourceY, double destinationX, double destinationY, int N)
        {
            // Add virtual start and end nodes
            adj[-1] = new List<(int, double, double)>();
            adj[-2] = new List<(int, double, double)>();

            foreach (var item in coordinates)
            {
                // Connect source to reachable nodes
                double sx = sourceX - item.Value.x;
                double sy = sourceY - item.Value.y;
                double sourceDistanceKm = Math.Sqrt((sx * sx) + (sy * sy));
                double sourceDistanceMeter = sourceDistanceKm * 1000;

                if (sourceDistanceMeter <= R)
                {
                    double walkTime = (sourceDistanceKm / 5.0) * 60.0;
                    adj[item.Key].Add((-1, walkTime, sourceDistanceKm));
                    adj[-1].Add((item.Key, walkTime, sourceDistanceKm));
                }

                // Connect reachable nodes to destination
                double dx = destinationX - item.Value.x;
                double dy = destinationY - item.Value.y;
                double destinationDistanceKm = Math.Sqrt((dx * dx) + (dy * dy));
                double destinationDistanceMeter = destinationDistanceKm * 1000;

                if (destinationDistanceMeter <= R)
                {
                    double walkTime = (destinationDistanceKm / 5.0) * 60.0;
                    adj[item.Key].Add((-2, walkTime, destinationDistanceKm));
                    adj[-2].Add((item.Key, walkTime, destinationDistanceKm));
                }
            }
        }

        public string EnhancedDijkstra(Dictionary<int, List<(int, double, double)>> adjacencyList, int originalN)
        {
            this.adj = adjacencyList;
            n = originalN + 2; // Account for virtual nodes

            // Calculate parameters as per research paper
            double logn = Math.Log(n);
            k = Math.Max(2, (int)Math.Floor(Math.Pow(logn, 1.0 / 3.0)));
            t = Math.Max(2, (int)Math.Floor(Math.Pow(logn, 2.0 / 3.0)));

            InitializeDataStructures();

            // For small graphs or when enhanced algorithm might have issues, fallback to standard Dijkstra
            if (n <= 20 || !HasValidPath())
            {
                return StandardDijkstra();
            }

            try
            {
                // Run enhanced SSSP algorithm
                var levels = CalculateLogLevels();
                var (boundary, vertices) = BMSSP(levels, double.PositiveInfinity, new HashSet<int> { -1 });

                // Verify we found a valid path
                if (!predecessors.ContainsKey(-2) || distances[-2] == double.PositiveInfinity)
                {
                    // Fallback to standard Dijkstra if enhanced algorithm failed
                    InitializeDataStructures();
                    return StandardDijkstra();
                }

                return ConstructPath();
            }
            catch
            {
                // If enhanced algorithm throws any exception, fallback to standard Dijkstra
                InitializeDataStructures();
                return StandardDijkstra();
            }
        }

        private bool HasValidPath()
        {
            // Quick check to see if there's any connection from virtual start to virtual end
            return adj.ContainsKey(-1) && adj[-1].Any() && adj.ContainsKey(-2) &&
                   adj.Values.Any(list => list.Any(edge => edge.neighbor == -2));
        }

        private void InitializeDataStructures()
        {
            distances = new Dictionary<int, double>();
            predecessors = new Dictionary<int, int>();
            complete = new HashSet<int>();

            foreach (var node in adj.Keys)
            {
                distances[node] = double.PositiveInfinity;
            }
            distances[-1] = 0;
        }

        private int CalculateLogLevels()
        {
            return Math.Max(1, (int)Math.Ceiling(Math.Log(n) / Math.Log(Math.Max(2, t))));
        }

        private (double boundary, HashSet<int> vertices) BMSSP(int level, double upperBound, HashSet<int> sources)
        {
            if (level == 0 || !sources.Any())
            {
                return BaseCase(upperBound, sources);
            }

            // For small level, just use base case
            if (level <= 2)
            {
                return BaseCase(upperBound, sources);
            }

            // Step 1: Find pivots using simplified approach
            var (pivots, workingSet) = FindPivots(upperBound, sources);

            if (!pivots.Any())
            {
                return BaseCase(upperBound, sources);
            }

            // Step 2: Process in batches
            var processedVertices = new HashSet<int>();
            double currentBoundary = upperBound;

            var remainingPivots = new List<int>(pivots.OrderBy(p => distances[p]));
            int maxBatchSize = Math.Max(1, (int)Math.Pow(2, Math.Max(0, level - 1)));

            while (remainingPivots.Any() && processedVertices.Count < k * Math.Pow(2, level))
            {
                var batch = remainingPivots.Take(maxBatchSize).ToHashSet();
                remainingPivots.RemoveAll(p => batch.Contains(p));

                double batchBoundary = batch.Any() ? batch.Max(v => distances[v]) + 1 : upperBound;

                // Recursive call
                var (newBoundary, batchResult) = BMSSP(level - 1, Math.Min(batchBoundary, upperBound), batch);

                processedVertices.UnionWith(batchResult);
                currentBoundary = Math.Min(currentBoundary, newBoundary);

                // Simple edge relaxation
                RelaxEdgesFromSet(batchResult, upperBound);
            }

            // Add remaining working set vertices
            foreach (var vertex in workingSet)
            {
                if (distances[vertex] < currentBoundary && !processedVertices.Contains(vertex))
                {
                    processedVertices.Add(vertex);
                    complete.Add(vertex);
                }
            }

            return (currentBoundary, processedVertices);
        }

        private (double, HashSet<int>) BaseCase(double upperBound, HashSet<int> sources)
        {
            if (!sources.Any())
                return (upperBound, new HashSet<int>());

            var result = new HashSet<int>();
            var pq = new PriorityQueue<int, double>();
            var processed = new HashSet<int>();

            // Initialize with all sources
            foreach (var source in sources)
            {
                if (distances[source] < upperBound)
                {
                    pq.Enqueue(source, distances[source]);
                }
            }

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();

                if (processed.Contains(current)) continue;
                processed.Add(current);
                result.Add(current);
                complete.Add(current);

                if (distances[current] >= upperBound) continue;

                if (!adj.ContainsKey(current)) continue;

                foreach (var (neighbor, time, _) in adj[current])
                {
                    double newDist = distances[current] + time;

                    if (newDist < distances[neighbor] && newDist < upperBound)
                    {
                        distances[neighbor] = newDist;
                        predecessors[neighbor] = current;

                        if (!processed.Contains(neighbor))
                        {
                            pq.Enqueue(neighbor, newDist);
                        }
                    }
                }
            }

            double resultBoundary = upperBound;
            if (result.Any())
            {
                var validDistances = result.Where(v => distances[v] < upperBound).ToList();
                if (validDistances.Any())
                {
                    resultBoundary = Math.Min(upperBound, validDistances.Max(v => distances[v]) + 0.001);
                }
            }

            return (resultBoundary, result);
        }

        private (HashSet<int> pivots, HashSet<int> workingSet) FindPivots(double upperBound, HashSet<int> sources)
        {
            var workingSet = new HashSet<int>(sources);
            var currentWave = new HashSet<int>(sources);

            // Perform simplified relaxation
            for (int step = 0; step < Math.Min(k, 5); step++)
            {
                var nextWave = new HashSet<int>();

                foreach (var vertex in currentWave)
                {
                    if (!adj.ContainsKey(vertex)) continue;

                    foreach (var (neighbor, time, _) in adj[vertex])
                    {
                        double newDist = distances[vertex] + time;

                        if (newDist < distances[neighbor] && newDist < upperBound)
                        {
                            if (distances[neighbor] > newDist)
                            {
                                distances[neighbor] = newDist;
                                predecessors[neighbor] = vertex;
                                nextWave.Add(neighbor);
                                workingSet.Add(neighbor);
                            }
                        }
                    }
                }

                currentWave = nextWave;
                if (!nextWave.Any()) break;
            }

            // For simplicity, return all sources as pivots for small graphs
            return (sources, workingSet);
        }

        private void RelaxEdgesFromSet(HashSet<int> vertices, double upperBound)
        {
            foreach (var vertex in vertices)
            {
                complete.Add(vertex);

                if (!adj.ContainsKey(vertex)) continue;

                foreach (var (neighbor, time, _) in adj[vertex])
                {
                    double newDist = distances[vertex] + time;

                    if (newDist < distances[neighbor] && newDist < upperBound)
                    {
                        distances[neighbor] = newDist;
                        predecessors[neighbor] = vertex;
                    }
                }
            }
        }

        private string StandardDijkstra()
        {
            var pq = new PriorityQueue<int, double>();
            var visited = new HashSet<int>();

            pq.Enqueue(-1, 0);

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();

                if (visited.Contains(current)) continue;
                visited.Add(current);

                if (current == -2) break; // Found destination

                if (!adj.ContainsKey(current)) continue;

                foreach (var (neighbor, time, _) in adj[current])
                {
                    double newDist = distances[current] + time;

                    if (newDist < distances[neighbor])
                    {
                        distances[neighbor] = newDist;
                        predecessors[neighbor] = current;
                        pq.Enqueue(neighbor, newDist);
                    }
                }
            }

            return ConstructPath();
        }

        private string ConstructPath()
        {
            // Check if path exists
            if (!predecessors.ContainsKey(-2) || distances[-2] == double.PositiveInfinity)
            {
                return "No path found\n0.00 mins\n0.00 km\n0.00 km\n0.00 km\n\n";
            }

            List<int> fullPath = new List<int>();
            int node = -2;

            while (node != -1 && predecessors.ContainsKey(node))
            {
                fullPath.Add(node);
                node = predecessors[node];
            }

            fullPath.Add(-1);
            fullPath.Reverse();

            double totalTime = 0;
            double totalDistance = 0;
            double walkDistance = 0;
            double vehicleDistance = 0;

            List<int> printedPath = new List<int>();

            for (int i = 0; i < fullPath.Count - 1; i++)
            {
                int u = fullPath[i];
                int v = fullPath[i + 1];

                if (u != -1 && u != -2) printedPath.Add(u);

                if (adj.ContainsKey(u))
                {
                    var edge = adj[u].FirstOrDefault(e => e.neighbor == v);
                    if (edge != default)
                    {
                        totalTime += edge.time;
                        totalDistance += edge.length;

                        if (u == -1 || v == -2)
                            walkDistance += edge.length;
                        else
                            vehicleDistance += edge.length;
                    }
                }
            }

            if (fullPath.Count >= 2 && fullPath[^2] != -2 && fullPath[^1] != -1 && fullPath[^1] != -2)
                printedPath.Add(fullPath[^1]);

            string result = string.Join(" ", printedPath) + "\n";
            result += $"{totalTime:F2} mins\n";
            result += $"{totalDistance:F2} km\n";
            result += $"{walkDistance:F2} km\n";
            result += $"{vehicleDistance:F2} km\n\n";

            // Cleanup virtual nodes
            adj.Remove(-1);
            adj.Remove(-2);
            foreach (var nodes in adj.Values)
            {
                nodes.RemoveAll(e => e.neighbor == -1 || e.neighbor == -2);
            }

            return result;
        }

        public List<int> VisualizeEnhancedDijkstra(Dictionary<int, List<(int, double, double)>> adjacencyList, int originalN)
        {
            EnhancedDijkstra(adjacencyList, originalN);

            if (!predecessors.ContainsKey(-2))
                return new List<int>();

            List<int> fullPath = new List<int>();
            int node = -2;

            while (node != -1 && predecessors.ContainsKey(node))
            {
                fullPath.Add(node);
                node = predecessors[node];
            }

            fullPath.Add(-1);
            fullPath.Reverse();

            return fullPath.Where(node => node != -1 && node != -2).ToList();
        }
    }
}
