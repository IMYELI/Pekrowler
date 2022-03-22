using System;
using System.Diagnostics;
using System.ComponentModel;

namespace Pekrowler
{
    public partial class Form1 : Form
    {
        private bool working = true;
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
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

            argPass.setArg(fileName, rootFolder, findAll, graph);
            backgroundWorker1.RunWorkerAsync();




            label5.Show();
            linkLabel1.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

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
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = (radioButton1.Checked || radioButton2.Checked);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            argPass.sw.Start();
            if (radioButton1.Checked)
            {

                e.Result = Crawler.BFS.searchBFS(argPass.fileName, argPass.rootFolder, argPass.findAll, ref argPass.graph, worker, e, argPass.rootFolder);
            }
            else
            {
                e.Result = Crawler.DFS.searchDFS(argPass.fileName, argPass.rootFolder, argPass.findAll, ref argPass.graph, worker, e);
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            argPass.sw.Stop();
            label5.Text = "Elapsed time: ";
            label5.Text = label5.Text + argPass.sw.Elapsed.Milliseconds.ToString() + "ms";
            argPass.sw.Reset();
            argPass.paths = (List<String>)e.Result;
            argPass.finished = true;
            working = false;
            gViewer1.Graph = null;
            gViewer1.Graph = argPass.graph;

            if (argPass.paths.Count > 0)
            {
                linkLabel1.Text = "";
                for (int i = 0; i < argPass.paths.Count; i++)
                {
                    linkLabel1.Text = linkLabel1.Text + (i + 1).ToString() + ". " + argPass.paths[i] + "\n";
                }
            }
            else
            {
                linkLabel1.Text = "File not found.";
            }
            label5.Show();
            linkLabel1.Show();
        }
        private void backgroundWorker1_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            gViewer1.Graph = null;
            gViewer1.Graph = argPass.graph;
        }
    }
    public class argPass
    {
        public static string fileName = "";
        public static string rootFolder = "";
        public static bool findAll = false;
        public static Microsoft.Msagl.Drawing.Graph graph = new();
        public static List<string> paths = new();
        public static bool finished = false;
        public static Stopwatch sw = new Stopwatch();

        public static void setArg(string fileName, string rootFolder, bool findAll, Microsoft.Msagl.Drawing.Graph graph)
        {
            argPass.fileName = fileName;
            argPass.rootFolder = rootFolder;
            argPass.findAll = findAll;
            argPass.graph = graph;
        }
    }
}