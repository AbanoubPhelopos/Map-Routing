namespace MapRouting
{
    public class solveProblem
    {

        public static void getStartAndEndNodes(Dictionary<int, (double x, double y)> coordinates,
        Dictionary<int, List<(int neighbor, double time, double length)>> adj, double R,
        double sourceX, double sourceY, double destinationX, double destinationY, int N)
        {

            adj[-1] = new List<(int, double, double)>();
            adj[-2] = new List<(int, double, double)>();

            foreach (var item in coordinates)
            {
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

            /*foreach (var item in adj)
            {
                Console.WriteLine($"ID: {item.Key}, {item.Value.First()} ");
            }*/

        }

        public static string dijkstra(Dictionary<int, List<(int neighbor, double time, double length)>> adj)
        {
            Dictionary<int, double> times = new Dictionary<int, double>();
            Dictionary<int, int> prev = new Dictionary<int, int>();
            HashSet<int> visited = new HashSet<int>();

            foreach (var node in adj.Keys)
            {
                times[node] = double.PositiveInfinity;
            }

            times[-1] = 0;

            PriorityQueue<int, double> pq = new PriorityQueue<int, double>();
            pq.Enqueue(-1, 0);

            while (pq.Count > 0)
            {
                int current = pq.Dequeue();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                if (!adj.ContainsKey(current))
                    continue;

                foreach (var (neighbor, time, _) in adj[current])
                {
                    double newDist = times[current] + time;

                    if (newDist < times[neighbor])
                    {
                        times[neighbor] = newDist;
                        prev[neighbor] = current;
                        pq.Enqueue(neighbor, newDist);
                    }
                }
            }


            return constructPath(prev, times, adj);
        }

        static string constructPath(
        Dictionary<int, int> prev,
        Dictionary<int, double> dist,
        Dictionary<int, List<(int neighbor, double time, double length)>> adj)
        {
            List<int> fullPath = new List<int>();
            int node = -2;

            while (node != -1)
            {
                fullPath.Add(node);
                if (!prev.ContainsKey(node))
                    break;
                node = prev[node];
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

                if (u != -1 && u != -2)
                    printedPath.Add(u);

                var edge = adj[u].FirstOrDefault(e => e.neighbor == v);

                totalTime += edge.time;
                totalDistance += edge.length;

                if (u == -1 || v == -2)
                {
                    walkDistance += edge.length;
                }
                else
                {
                    vehicleDistance += edge.length;
                }
            }

            int lastIndex = fullPath.Count - 1;
            int secondLastIndex = fullPath.Count - 2;

            int last = fullPath[lastIndex];
            int secondLast = fullPath[secondLastIndex];

            if (secondLast != -2 && last != -1 && last != -2)
                printedPath.Add(last);

            string result = string.Join(" ", printedPath) + "\n";
            result += $"{totalTime:F2} mins\n";
            result += $"{totalDistance:F2} km\n";
            result += $"{walkDistance:F2} km\n";
            result += $"{vehicleDistance:F2} km\n\n";

            //Console.WriteLine(result);

            adj.Remove(-1);
            adj.Remove(-2);
            foreach (var nodes in adj)
            {
                nodes.Value.RemoveAll(e => e.neighbor == -1 || e.neighbor == -2);
            }


            return result;
        }





        public static List<int> visDijkstra(Dictionary<int, List<(int neighbor, double time, double length)>> adj)
        {

            Dictionary<int, double> times = new Dictionary<int, double>();
            Dictionary<int, int> prev = new Dictionary<int, int>();
            HashSet<int> visited = new HashSet<int>();

            foreach (var node in adj.Keys)
            {
                times[node] = double.PositiveInfinity;
            }

            times[-1] = 0;

            PriorityQueue<int, double> pq = new PriorityQueue<int, double>();
            pq.Enqueue(-1, 0);

            while (pq.Count > 0)
            {
                int current = pq.Dequeue();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                if (!adj.ContainsKey(current)) continue;

                foreach (var (neighbor, time, _) in adj[current])
                {
                    double newDist = times[current] + time;

                    if (newDist < times[neighbor])
                    {
                        times[neighbor] = newDist;
                        prev[neighbor] = current;
                        pq.Enqueue(neighbor, newDist);
                    }
                }
            }


            return visConstructPath(prev, times, adj);
        }

        static List<int> visConstructPath(
        Dictionary<int, int> prev,
        Dictionary<int, double> dist,
        Dictionary<int, List<(int neighbor, double time, double length)>> adj)
        {
            List<int> fullPath = new List<int>();
            int node = -2;

            while (node != -1)
            {
                fullPath.Add(node);
                if (!prev.ContainsKey(node))
                    break;
                node = prev[node];
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

                if (u != -1 && u != -2)
                    printedPath.Add(u);

                var edge = adj[u].FirstOrDefault(e => e.neighbor == v);

                totalTime += edge.time;
                totalDistance += edge.length;

                if (u == -1 || v == -2)
                {
                    walkDistance += edge.length;
                }
                else
                {
                    vehicleDistance += edge.length;
                }
            }

            int lastIndex = fullPath.Count - 1;
            int secondLastIndex = fullPath.Count - 2;

            int last = fullPath[lastIndex];
            int secondLast = fullPath[secondLastIndex];

            if (secondLast != -2 && last != -1 && last != -2)
                printedPath.Add(last);


            adj.Remove(-1);
            adj.Remove(-2);
            foreach (var nodes in adj)
            {
                nodes.Value.RemoveAll(e => e.neighbor == -1 || e.neighbor == -2);
            }

            return printedPath;
        }
    }
}
