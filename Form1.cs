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
            button2.Enabled = (radioButton1.Checked || radioButton2.Checked) && textBox2.Text != "" && textBox1.Text != "" && textBox3.Text != "";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = (radioButton1.Checked || radioButton2.Checked) && textBox2.Text != "" && textBox1.Text != "" && textBox3.Text != "";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i > -1; i++)
            {
                if (this.Controls.Contains(this.Controls[i.ToString()]))
                {
                    this.Controls.Remove(this.Controls[i.ToString()]);
                }
                else
                {
                    break;
                }
            }
            label10.Hide();
            label4.Show();
            string fileName = textBox2.Text;
            string rootFolder = textBox1.Text;
            bool findAll = checkBox1.Checked;
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            button2.Enabled = false;
            button1.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            hScrollBar1.Enabled = false;
            checkBox1.Enabled = false;


            argPass.setArg(fileName, rootFolder, findAll, graph);
            backgroundWorker1.RunWorkerAsync();
           
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
            button2.Enabled = (radioButton1.Checked || radioButton2.Checked) && textBox2.Text != "" && textBox1.Text != "" && textBox3.Text != "";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            argPass.sw.Start();
            if (radioButton1.Checked)
            {

                e.Result = Crawler.BFS.searchBFS(argPass.fileName, argPass.rootFolder, argPass.findAll, ref argPass.graph, argPass.timeDelay, worker, e);
            }
            else
            {
                e.Result = Crawler.DFS.searchDFS(argPass.fileName, argPass.rootFolder, argPass.findAll, ref argPass.graph, argPass.timeDelay, worker, e);
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
                for (int i = 0; i < argPass.paths.Count; i++)
                {
                    int anchorX = this.Controls["label4"].Location.X;
                    int anchorY = this.Controls["label4"].Location.Y;
                    var linkLabel = new LinkLabel { Text = (((i + 1).ToString() + ". " + argPass.paths[i])), Location = new Point(anchorX, anchorY + ((i + 1) * 18)), AutoSize = true, Name = i.ToString() };
                    linkLabel.Click += (sender, e) => onLinkLabelClickOpenFile(sender, e);
                    this.Controls.Add(linkLabel);
                    linkLabel.Show();
                }
            }
            else
            {
                label10.Show();
            }
            label5.Show();
            button2.Enabled = true;
            button1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            hScrollBar1.Enabled = true;
            checkBox1.Enabled = true;
        }
        private void backgroundWorker1_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            gViewer1.Graph = null;
            while (argPass.graph is null) { Thread.Sleep(1); };
            gViewer1.Graph = argPass.graph;    
    }


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int animationSpeed = int.Parse(textBox3.Text);
                button2.Enabled = (radioButton1.Checked || radioButton2.Checked) && textBox2.Text != "" && textBox1.Text != "" && textBox3.Text != "" && animationSpeed > 30;
                argPass.timeDelay = animationSpeed;
                if (animationSpeed <= 30)
                {
                    label11.Show();
                    label11.ForeColor = Color.Red;
                    label12.Show();
                    label12.ForeColor = Color.Red;
                }
                else
                {
                    label11.Hide();
                    label12.Hide();
                }
            }
            catch(Exception ex)
            {
                textBox3.Text = "";
            }
            
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            textBox3.Text = hScrollBar1.Value.ToString();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = (radioButton1.Checked || radioButton2.Checked) && textBox2.Text != "" && textBox1.Text != "" && textBox3.Text != "";
        }

        private void onLinkLabelClickOpenFile(object sender, EventArgs e)
        {
            LinkLabel link = (LinkLabel)sender;
            string id = link.Name;
            int i = Int32.Parse(id);
            Debug.WriteLine(i);
            Process.Start("explorer.exe", (argPass.paths[i].Replace(argPass.paths[i].Split(Path.DirectorySeparatorChar).Last(), "")));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

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
        public static int timeDelay = 0;

        public static void setArg(string fileName, string rootFolder, bool findAll, Microsoft.Msagl.Drawing.Graph graph)
        {
            argPass.fileName = fileName;
            argPass.rootFolder = rootFolder;
            argPass.findAll = findAll;
            argPass.graph = graph;
        }
    }
}