namespace AlgoWithBonus
{
    public partial class Form1 : Form
    {
        string map, query, output;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.runTest(map, query, output);
            map = "";
            query = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var (coords, adj, M, N, R, queries) = Program.visualize(map, query);
            VisualizeGraph visualizeGraph = new VisualizeGraph(coords, adj, queries, N);
            visualizeGraph.Show();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                MessageBox.Show($"You selected: {selectedFilePath}");
                map = selectedFilePath;
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                MessageBox.Show($"You selected: {selectedFilePath}");
                query = selectedFilePath;
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                MessageBox.Show($"You selected: {selectedFilePath}");
                output = selectedFilePath;
               
            }
        }

        
    }
}
