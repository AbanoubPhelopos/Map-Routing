namespace MapRouting
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            checkBoxEnhanced = new CheckBox();
            button6 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(214, 364);
            button1.Name = "button1";
            button1.Size = new Size(141, 49);
            button1.TabIndex = 0;
            button1.Text = "Run Test";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(458, 364);
            button2.Name = "button2";
            button2.Size = new Size(141, 49);
            button2.TabIndex = 1;
            button2.Text = "Visualize Graph";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(329, 106);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(141, 43);
            button3.TabIndex = 4;
            button3.Text = "Select Map File";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(329, 183);
            button4.Margin = new Padding(2);
            button4.Name = "button4";
            button4.Size = new Size(141, 43);
            button4.TabIndex = 5;
            button4.Text = "Select Query File ";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(329, 262);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.Size = new Size(141, 43);
            button5.TabIndex = 6;
            button5.Text = "Select Output Path ";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe Script", 26F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(187, 23);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(412, 57);
            label1.TabIndex = 7;
            label1.Text = "Welcome to our App";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(69, 113);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(167, 24);
            label2.TabIndex = 8;
            label2.Text = "Select Map Path:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(69, 190);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(184, 24);
            label3.TabIndex = 9;
            label3.Text = "Select Query Path:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(69, 269);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(184, 24);
            label4.TabIndex = 10;
            label4.Text = "Select Output Path:";
            // 
            // checkBoxEnhanced
            // 
            checkBoxEnhanced.AutoSize = true;
            checkBoxEnhanced.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBoxEnhanced.ForeColor = Color.DarkGreen;
            checkBoxEnhanced.Location = new Point(69, 320);
            checkBoxEnhanced.Name = "checkBoxEnhanced";
            checkBoxEnhanced.Size = new Size(401, 24);
            checkBoxEnhanced.TabIndex = 11;
            checkBoxEnhanced.Text = "Use Enhanced Algorithm O(m log^(2/3) n)";
            checkBoxEnhanced.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(336, 364);
            button6.Name = "button6";
            button6.Size = new Size(141, 49);
            button6.TabIndex = 12;
            button6.Text = "Run Comparison";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button6);
            Controls.Add(checkBoxEnhanced);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Map Routing Algorithm Comparison";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private CheckBox checkBoxEnhanced;
        private Button button6;
    }
}
