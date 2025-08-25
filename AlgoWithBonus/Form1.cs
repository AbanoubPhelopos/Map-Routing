namespace MapRouting
{
    public partial class Form1 : Form
    {
        private string? mapPath;
        private string? queryPath;
        private string? outputPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set default algorithm info
            checkBoxEnhanced.CheckedChanged += CheckBoxEnhanced_CheckedChanged;
            UpdateAlgorithmInfo();
        }

        private void CheckBoxEnhanced_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAlgorithmInfo();
        }

        private void UpdateAlgorithmInfo()
        {
            if (checkBoxEnhanced.Checked)
            {
                checkBoxEnhanced.Text = "? Enhanced Algorithm O(m log^(2/3) n) - ACTIVE";
                checkBoxEnhanced.ForeColor = Color.DarkGreen;
            }
            else
            {
                checkBoxEnhanced.Text = "Use Enhanced Algorithm O(m log^(2/3) n)";
                checkBoxEnhanced.ForeColor = Color.Black;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mapPath) || string.IsNullOrEmpty(queryPath))
            {
                MessageBox.Show("Please select both map file and query file first!");
                return;
            }

            // Use checkbox to determine algorithm
            bool useEnhanced = checkBoxEnhanced.Checked;

            string algorithmName = useEnhanced ? "Enhanced Algorithm O(m log^(2/3) n)" : "Original Dijkstra O(m + n log n)";

            var result = MessageBox.Show($"Ready to run test using: {algorithmName}\n\nProceed?",
                "Confirm Algorithm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Program.runTest(mapPath, queryPath, outputPath, useEnhanced);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Graph visualization feature has been removed.", "Feature Not Available",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Select Map File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    mapPath = openFileDialog.FileName;
                    MessageBox.Show($"Map file selected: {Path.GetFileName(mapPath)}");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Select Query File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    queryPath = openFileDialog.FileName;
                    MessageBox.Show($"Query file selected: {Path.GetFileName(queryPath)}");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.Title = "Select Output Path";
                saveFileDialog.DefaultExt = "txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    outputPath = saveFileDialog.FileName;
                    MessageBox.Show($"Output path selected: {Path.GetFileName(outputPath)}");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mapPath) || string.IsNullOrEmpty(queryPath))
            {
                MessageBox.Show("Please select both map file and query file first!");
                return;
            }

            var result = MessageBox.Show("This will run both algorithms and compare their performance.\n\nProceed?",
                "Run Algorithm Comparison", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Program.runComparison(mapPath, queryPath, outputPath);
            }
        }
    }
}
