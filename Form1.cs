using System;
using System.Diagnostics;

namespace Pekrowler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void gViewer1_Load(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = (radioButton1.Checked || radioButton2.Checked);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            label4.Show();
            string fileName = textBox2.Text;
            string rootFolder = textBox1.Text;
            bool findAll = checkBox1.Checked;
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            List<string> paths;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (radioButton1.Checked)
            {
                paths = Crawler.BFS.searchBFS(fileName, rootFolder, findAll, ref graph);
            }
            else
            {
                paths = Crawler.DFS.searchDFS(fileName, rootFolder, findAll, ref graph, ref gViewer1);
            }
            stopWatch.Stop();

            if (paths.Count > 0)
            {
                linkLabel1.Text = "";
                for (int i = 0; i < paths.Count; i++)
                {
                    linkLabel1.Text = linkLabel1.Text + (i+1).ToString() + ". " + paths[i] + "\n";
                }
            }
            else
            {
                linkLabel1.Text = "File not found.";
            }

            label5.Text = "Elapsed time: ";
            label5.Text = label5.Text + stopWatch.Elapsed.Milliseconds.ToString() + "ms";

            gViewer1.Graph = graph;


            label5.Show();
            linkLabel1.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = (radioButton1.Checked || radioButton2.Checked);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}