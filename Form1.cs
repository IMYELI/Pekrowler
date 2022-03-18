namespace Pekrowler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            if (radioButton1.Checked)
            {
                Crawler.BFS.searchBFS(fileName, rootFolder, findAll);
            }
            
            linkLabel1.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = (radioButton1.Checked || radioButton2.Checked);
        }
    }
}