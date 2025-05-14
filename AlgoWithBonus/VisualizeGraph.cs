using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AlgoWithBonus
{
    public partial class VisualizeGraph : Form
    {
        Dictionary<int, (double x, double y)> coords;
        Dictionary<int, List<(int neighbor, double time, double length)>> adj;
        Dictionary<int, Query> queries;
        int N;

        private float zoom = 1.0f;
        private PointF offset = new PointF(0, 0);
        private Point lastMousePos;
        private bool dragging = false;

        private List<int> highlightPath = new List<int>();

        public VisualizeGraph(Dictionary<int, (double x, double y)> coordinates,
                   Dictionary<int, List<(int neighbor, double time, double length)>> adjacencyList, Dictionary<int, Query> queries, int N)
        {
            InitializeComponent();
            coords = coordinates;
            adj = adjacencyList;
            this.queries = queries;
            this.N = N;

            this.DoubleBuffered = true;

            this.MouseWheel += VisualizeGraph_MouseWheel;
            this.MouseDown += VisualizeGraph_MouseDown;
            this.MouseMove += VisualizeGraph_MouseMove;
            this.MouseUp += VisualizeGraph_MouseUp;
        }

        private void VisualizeGraph_MouseWheel(object sender, MouseEventArgs e)
        {
            float oldZoom = zoom;
            if (e.Delta > 0)
                zoom *= 1.1f;
            else
                zoom /= 1.1f;

            offset.X = e.X - (e.X - offset.X) * (zoom / oldZoom);
            offset.Y = e.Y - (e.Y - offset.Y) * (zoom / oldZoom);

            Invalidate();
        }

        private void VisualizeGraph_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                lastMousePos = e.Location;
            }
        }

        private void VisualizeGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                offset.X += e.X - lastMousePos.X;
                offset.Y += e.Y - lastMousePos.Y;
                lastMousePos = e.Location;
                Invalidate();
            }
        }

        private void VisualizeGraph_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            if (coords.Count == 0) return;

            double minX = coords.Values.Min(p => p.x);
            double maxX = coords.Values.Max(p => p.x);
            double minY = coords.Values.Min(p => p.y);
            double maxY = coords.Values.Max(p => p.y);
            double rangeX = maxX - minX;
            double rangeY = maxY - minY;

            float drawWidth = this.ClientSize.Width;
            float drawHeight = this.ClientSize.Height;

            float scaleX = (float)(drawWidth / (rangeX == 0 ? 1 : rangeX));
            float scaleY = (float)(drawHeight / (rangeY == 0 ? 1 : rangeY));
            float scale = Math.Min(scaleX, scaleY) * zoom;

            Func<(double x, double y), PointF> worldToScreen = p =>
            {
                float sx = (float)((p.x - minX) * scale + offset.X);
                float sy = (float)((p.y - minY) * scale + offset.Y);
                return new PointF(sx, sy);
            };

            Pen edgePen = new Pen(Color.Black, 1);
            Pen pathPen = new Pen(Color.Red, 2);
            Font font = new Font(this.Font.FontFamily, 8 / zoom);
            Brush nodeBrush = Brushes.Blue;

            // Draw all edges
            foreach (var kvp in coords)
            {
                int id = kvp.Key;
                var pt1 = worldToScreen(kvp.Value);

                if (adj.TryGetValue(id, out var neighbors))
                {
                    foreach (var (neighbor, time, length) in neighbors)
                    {
                        if (!coords.ContainsKey(neighbor)) continue;
                        var pt2 = worldToScreen(coords[neighbor]);
                        g.DrawLine(edgePen, pt1, pt2);
                    }
                }
            }

            // Draw red path if available
            if (highlightPath != null && highlightPath.Count >= 2)
            {
                for (int i = 0; i < highlightPath.Count - 1; i++)
                {
                    int from = highlightPath[i];
                    int to = highlightPath[i + 1];

                    if (coords.ContainsKey(from) && coords.ContainsKey(to))
                    {
                        var p1 = worldToScreen(coords[from]);
                        var p2 = worldToScreen(coords[to]);
                        g.DrawLine(pathPen, p1, p2);
                    }
                }
            }

            // Optionally draw nodes (disabled for now)
            /*
            foreach (var kvp in coords)
            {
                var pt = worldToScreen(kvp.Value);
                float r = 3;
                g.FillEllipse(nodeBrush, pt.X - r, pt.Y - r, 2 * r, 2 * r);
            }
            */
        }

        List<int> nodes;
        int i = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            nodes = Program.queryVisualize(coords, adj, queries, i++, N);
            highlightPath = nodes;
            Invalidate(); // force redraw
        }
    }
}
