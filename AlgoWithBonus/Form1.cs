namespace AlgoWithBonus
{
    public partial class Form1 : Form
    {
        string path = "D:\\college\\third year\\6th term\\algorism\\project map tests\\[1] MAP ROUTING\\TEST CASES\\";
        string map, query, output;
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Sample");
            comboBox1.Items.Add("Medium");
            comboBox1.Items.Add("Large");
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    comboBox2.Items.Add($"Test {i + 1}");
                }
                path += "[1] Sample Cases\\Input";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                comboBox2.Items.Add("OL Test");

                path += "[2] Medium Cases\\Input";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                comboBox2.Items.Add("SF Test");

                path += "[3] Large Cases\\Input";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            map = path;
            query = path;

            if (comboBox1.SelectedIndex == 0)
            {
                map += $"\\map{comboBox2.SelectedIndex + 1}.txt";
                query += $"\\queries{comboBox2.SelectedIndex + 1}.txt";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                map += "\\OLMap.txt";
                query += "\\OLQueries.txt";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                map += "\\SFMap.txt";
                query += "\\SFQueries.txt";
            }

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
                // Example: use the path
                // map = selectedFilePath;
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
                // Example: use the path
                // map = selectedFilePath;
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
                // Example: use the path
                // map = selectedFilePath;
            }
        }

        
    }
}
