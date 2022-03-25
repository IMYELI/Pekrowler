using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
namespace Crawler
{
    class utility
    {
        public static List<string> copyList(List<String> l)
        /* Digunakan untuk mengkopi list
           Masukan : l = list of string
           Keluaran : hasil copy dari list l } */ 
        {       
            List<string> result = new List<String>();
            foreach (string s in l)
            {
                string temp = s;
                result.Add(temp);
            }
            return result;
        }
    }


    class Global
    {
        public static Queue<string> pathQueue = new Queue<string>();
        public static Queue<string> nodePathQueue = new Queue<string>();
        /* Digunakan dalam pencarian BFS untuk menyimpan antrian path yang akan dicari. 
           Antrian path merupakan subfolder-subfolder dari folder utama yang ditemukan selama pencarian */
        
        public static List<string> result = new List<string>();
        /* Digunakan dalam pencarian BFS dan DFS untuk menyimpan path file yang ditemukan  */
        public static Boolean isRunning = false;
        /* Digunakan dalam pencarian DFS. Bernilai true di awal. 
           Jika sudah menemukan solusi, bernilai false untuk memberhentikan pencarian rekursif */
        public static void clean()
        /*  Digunakan untuk membersihkan variabel-variabel pada kelas Global */
        {
            Global.isRunning = false;
            Global.pathQueue = new Queue<string>();
            Global.result = new List<string>();
            Global.nodeMap = new Dictionary<string, int>();
            Global.nodePathQueue = new Queue<string>();
        }
        public static void colorEdge(string path, Microsoft.Msagl.Drawing.Graph graph, Microsoft.Msagl.Drawing.Color color)
        /*  Digunakan untuk mengubah warna edge berhubungan dengan path sesuai color 
            Initial State : edge yang berhubungan dengan path terdefinisi
            Final State : edge yang berhubungan dengan path berwarna sesuai color */
        {
            string[] edges = path.Split(Path.DirectorySeparatorChar);

            for (int i=0;i<edges.Length-1;i++){

                Microsoft.Msagl.Drawing.Edge currEdge = Global.edgeMap[edges[i] + " PEKO " + edges[i + 1]];
                if (currEdge == null)
                {

                }
                if (currEdge != null && currEdge.Attr.Color != Microsoft.Msagl.Drawing.Color.SkyBlue)
                {
                    currEdge.Attr.Color = color;
                    //currEdge.Attr.Id = edges[i] + " PEKO " + edges[i + 1];
                }

                //MEWARNAI NODE
                Microsoft.Msagl.Drawing.Node parentNode = graph.FindNode(currEdge.Source);
                Microsoft.Msagl.Drawing.Node childNode = graph.FindNode(currEdge.Target);
                if(parentNode.Attr.Color != Microsoft.Msagl.Drawing.Color.SkyBlue)
                {
                    if (i == edges.Length - 2)
                    {
                        childNode.Attr.Color = color;
                    }
                    parentNode.Attr.Color = color;
                }
                
            }
        }


        public static Dictionary<String, Microsoft.Msagl.Drawing.Edge> edgeMap = new();
        /* Digunakan untuk memetakan id edge dengan edge */

        public static void updateGraph(BackgroundWorker worker, int timeDelay)
        /* Digunakan untuk memperbarui tampilan graf dengan jeda waktu
         * yang telah ditentukan */
        {
            Mutex mutex = new Mutex();
            mutex.WaitOne();
            worker.ReportProgress(1);
            Thread.Sleep(timeDelay);
            mutex.ReleaseMutex();
        }

        public static Dictionary<String,int> nodeMap = new();

        public static void addNode(string node)
        /* Digunakan menambah node dengan melakukan pengecekan terlebih
           dahulu apakah node tersebut sudah ada */
        {
            if (nodeMap.ContainsKey(node))
            {
                nodeMap[node]++;
                node += " (" + nodeMap[node] + ")";
                nodeMap.Add(node, 1);
            }
            else
            {
                nodeMap.Add(node, 1);
            }
        }
        public static string getNode(string node)
        /* Digunakan mendapatkan sebuah node.
           Menangani node dengan nama yang sama */
        {
            if (nodeMap[node] != 1)
            {
                return node + " (" + nodeMap[node] + ")";
            }
            else
            {
                return node;
            }
        }
    }
    class DFS
    {
        public static List<string> searchDFS(string fileToSearch, string path, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph, int timeDelay,BackgroundWorker worker, DoWorkEventArgs e)
        /* Melakukan inisiasi pencarian DFS. Global.result adalah larik string yang merupakan variabel Global
           (bisa diakses di mana saja).
  
           Masukan : fileToSearch = nama file yang akan dicari
                     path = lokasi pencarian
                     searchAll = boolean yang bernilai true jika ingin mencari lebih dari 1 file,
                                 false jika hanya mencari 1 file
                     graph = Graf yang digunakan untuk visualisasi
                     timeDelay = jeda waktu antara progres pembuatan graf

           Keluaran : Seluruh lokasi file yang dicari dalam bentuk larik string */
        {
            Global.isRunning = true;
            List<string> res;
            recursivelySearchDFS(fileToSearch, path, searchAll, ref graph, timeDelay, path,worker, e, path);
            res = utility.copyList(Global.result);
            Global.clean();
            return res;
        }

        private static void recursivelySearchDFS(string fileToSearch, string path, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph, int timeDelay, string pathBapak, BackgroundWorker worker, DoWorkEventArgs e, string pathNode)
        /* Melakukan proses pencarian DFS secara rekursif.
  
           Masukan : fileToSearch = nama file yang akan dicari
                     path = lokasi pencarian
                     searchAll = boolean yang bernilai true jika ingin mencari lebih dari 1 file,
                                 false jika hanya mencari 1 file
                     graph = Graf yang digunakan untuk visualisasi
                     timeDelay = jeda waktu antara progres pembuatan graf
                     pathBapak = lokasi bapak dari node
                     pathNode = lokasi node

           Initial State : Seluruh masukan terdefinisi
           Final State : Menambahkan path hasil ke Global.result kemudian memanggil
                         kembali fungsi ini dengan path baru merupakan folder dari path sekarang
                         jika masih ada */
        {
            /* Mengambil semua nama file dalam path  */
            String[] files = Directory.GetFiles(Path.GetFullPath(path));

            /* Melakukan pengecekan terhadap setiap file */
            foreach (string file in files)
            {
                string parent = pathNode.Split(Path.DirectorySeparatorChar).Last();
                string child = file.Split(Path.DirectorySeparatorChar).Last();

                /* Menambahkan node child  */
                Global.addNode(child);
                string newChildNode = Global.getNode(child);
                Microsoft.Msagl.Drawing.Node parentNode = null;

                /* Menambahkan node parent jika belum ada */
                if(graph.FindNode(parent) == null)
                {
                    parentNode = graph.AddNode(parent);
                    Global.addNode(parent);
                }
                else
                {
                    parentNode = graph.FindNode(parent);
                }

                Microsoft.Msagl.Drawing.Node childNode = new Microsoft.Msagl.Drawing.Node(child);
                childNode.Id = newChildNode;
                
                Microsoft.Msagl.Drawing.Edge eg = new Microsoft.Msagl.Drawing.Edge(parentNode,childNode,Microsoft.Msagl.Drawing.ConnectionToGraph.Connected);
                
                /* Menambahkan node child */
                graph.AddNode(childNode);

                /* Memberikan node id */
                eg.Attr.Id = parent +" PEKO " + newChildNode;
                Global.edgeMap[parent + " PEKO " + newChildNode] = eg;


                if (file == path + "\\" + fileToSearch && Global.isRunning) // Jika file ditemukan
                {
                    /* Menambah path ke list global */
                    Global.result.Add(file);
                    string parentAsli = pathBapak.Split(Path.DirectorySeparatorChar).Last();
                    string pathBenar = pathNode.Replace(pathBapak, parentAsli);
                    pathBenar += "\\" + newChildNode;

                    /* Mengubah warna edge yang benar menjadi SkyBlue */
                    Global.colorEdge(pathBenar,graph,Microsoft.Msagl.Drawing.Color.SkyBlue);
                    
                    if (!searchAll) // Jila hanya mencari satu file
                    {
                        Global.isRunning = false;
                        return;
                    }
                }else
                {
                    /* Mengubah warna edge yang salah menjadi Orange */
                    eg.Attr.Color = Microsoft.Msagl.Drawing.Color.Orange;
                    childNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Orange;
                }
                /* Mengubah tampilan graf sesuai time delay */
                Global.updateGraph(worker, timeDelay);
            }

            /* Mengambil semua nama folder dalam path */
            String[] dirs = Directory.GetDirectories(path);
            foreach (String dir in dirs)
            {
                Console.WriteLine();
                if (Global.isRunning)
                {
                    string parent = pathNode.Split(Path.DirectorySeparatorChar).Last();
                    string folder = dir.Split(Path.DirectorySeparatorChar).Last();
                    
                    /* Menambahkan node folder */
                    Global.addNode(folder);
                    string newFolder = Global.getNode(folder);
                    Microsoft.Msagl.Drawing.Node parentNode = null;

                    /* Menambahkan node parent jika belum ada */
                    if(graph.FindNode(parent) == null)
                    {
                        parentNode = graph.AddNode(parent);
                        Global.addNode(parent);
                    }
                    else
                    {
                        parentNode = graph.FindNode(parent);
                    }

                    Microsoft.Msagl.Drawing.Node folderNode = new Microsoft.Msagl.Drawing.Node(folder);
                    folderNode.Id = newFolder;

                    Microsoft.Msagl.Drawing.Edge eg = new Microsoft.Msagl.Drawing.Edge(parentNode,folderNode,Microsoft.Msagl.Drawing.ConnectionToGraph.Connected);;
                    graph.AddNode(folderNode);

                    /* Memberikan warna orange pada node */
                    eg.Attr.Color = Microsoft.Msagl.Drawing.Color.Orange;
                    folderNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Orange;

                    /* Memberikan id pada node */
                    eg.Attr.Id = parent + " PEKO " + newFolder;
                    Global.edgeMap[parent + " PEKO " + newFolder] = eg;
                    string newPathNode = pathNode + "\\" + newFolder;
                    Global.updateGraph(worker, timeDelay);

                    /* Memanggil fungsi ini lagi untuk mencari file dalam folder anak */
                    recursivelySearchDFS(fileToSearch, dir, searchAll, ref graph, timeDelay, pathBapak, worker, e, newPathNode);
                }
            }
        }
    }


    class BFS
    {
        public static List<string> searchBFS(string fileToSearch, string pathToSearch, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph, int timeDelay, BackgroundWorker worker, DoWorkEventArgs e)
        /* Melakukan inisiasi pencarian DFS. Global.result adalah larik string yang merupakan variabel Global
           (bisa diakses di mana saja).
   
           Masukan : fileToSearch = nama file yang akan dicari
                     path = lokasi pencarian
                     searchAll = boolean yang bernilai true jika ingin mencari lebih dari 1 file,
                                 false jika hanya mencari 1 file
                     graph = Graf yang digunakan untuk visualisasi
                     timeDelay = jeda waktu antara progres pembuatan graf

           Keluaran : Seluruh lokasi file yang dicari dalam bentuk larik string */
        {
            List<string> res;
            /* Menambahkan path awal ke antrian */
            Global.pathQueue.Enqueue(pathToSearch);
            Global.nodePathQueue.Enqueue(pathToSearch);

            /* Lakukan eksekusi selagi masih ada path di antrian */
            while (Global.pathQueue.Count > 0)
            {
                /* Mengambil path dari antrian pertama */
                string path = Global.pathQueue.Dequeue();
                string pathNode = Global.nodePathQueue.Dequeue();

                /* Mengambil semua file yang ada di path */
                String[] files = Directory.GetFiles(Path.GetFullPath(path));
                Microsoft.Msagl.Drawing.Edge eg = null;
                Microsoft.Msagl.Drawing.Node parentNode = null;
                Microsoft.Msagl.Drawing.Node childNode = null;
                string parentAsli = pathToSearch.Split(Path.DirectorySeparatorChar).Last();
                string pathBenar = pathNode.Replace(pathToSearch, parentAsli);
                
                /* Melakukan pengecekan terhadap setiap file */
                foreach (string file in files)
                {
                    string parent = pathNode.Split(Path.DirectorySeparatorChar).Last();
                    string child = file.Split(Path.DirectorySeparatorChar).Last();
                    
                    /* Menambahkan node child  */
                    Global.addNode(child);
                    string newChild = Global.getNode(child);
                    
                    /* Menambahkan node parent jika belum ada */
                    if(graph.FindNode(parent) == null)
                    {
                        parentNode = graph.AddNode(parent);
                        Global.addNode(parent);
                    }
                    else
                    {
                        parentNode = graph.FindNode(parent);
                    }

                    childNode = new Microsoft.Msagl.Drawing.Node(child);
                    childNode.Id = newChild;


                    eg = new Microsoft.Msagl.Drawing.Edge(parentNode, childNode, Microsoft.Msagl.Drawing.ConnectionToGraph.Connected);
                   
                    /* Menambahkan node child */
                    graph.AddNode(childNode);

                    /* Memberikan node id */
                    eg.Attr.Id = parent + " PEKO " + newChild;
                    Global.edgeMap[parent + " PEKO " + newChild] = eg;

                    
                    String pathBenar1 = pathBenar + "\\" + newChild;
                    if (file == path + "\\" + fileToSearch) // Jika file ditemukan
                    {
                        /* Menambah path ke list global */
                        Global.result.Add(file);
                        
                        /* Mengubah warna edge yang benar menjadi SkyBlue */
                        Global.colorEdge(pathBenar1,graph,Microsoft.Msagl.Drawing.Color.SkyBlue);

                        if (!searchAll) // Jila hanya mencari satu file
                        {
                            res = utility.copyList(Global.result);
                            Global.clean();
                            return res;
                        }
                    }else{
                        /* Mengubah warna edge yang salah menjadi Orange */
                        Global.colorEdge(pathBenar1,graph,Microsoft.Msagl.Drawing.Color.Orange);
                        childNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Orange;
                    }
                    /* Mengubah tampilan graf sesuai time delay */
                    Global.updateGraph(worker, timeDelay);

                }
                Microsoft.Msagl.Drawing.Node folderNode = null;

                /* Mengambil semua nama folder dalam path */
                String[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    string parent = pathNode.Split(Path.DirectorySeparatorChar).Last();
                    string folder = dir.Split(Path.DirectorySeparatorChar).Last();

                    /* Menambahkan node folder */
                    Global.addNode(folder);
                    string newFolder = Global.getNode(folder);
                    parentNode = null;

                    /* Menambahkan node parent jika belum ada */
                    if(graph.FindNode(parent) == null)
                    {
                        parentNode = graph.AddNode(parent);
                        Global.addNode(parent);
                    }
                    else
                    {
                        parentNode = graph.FindNode(parent);
                    }

                    folderNode = new Microsoft.Msagl.Drawing.Node(folder);
                    folderNode.Id = newFolder;

                    eg = new Microsoft.Msagl.Drawing.Edge(parentNode,folderNode,Microsoft.Msagl.Drawing.ConnectionToGraph.Connected);;
                    graph.AddNode(folderNode);

                    /* Memberikan id pada node */
                    eg.Attr.Id = parent + " PEKO " + newFolder;
                    Global.edgeMap[parent + " PEKO " + newFolder] = eg;

                    if (Directory.GetDirectories(path+"\\"+folder).Length == 0 && Directory.GetFiles(path + "\\" + folder).Length == 0)
                    {
                        string pathBenar2 = pathBenar + "\\" + newFolder;
                        Global.colorEdge(pathBenar2, graph, Microsoft.Msagl.Drawing.Color.Orange);
                    }

                    string newPathNode  = pathNode+ "\\" + newFolder;

                    /* Memasukkan dir ke dalam antrian */
                    Global.pathQueue.Enqueue(dir);
                    Global.nodePathQueue.Enqueue(newPathNode);

                    /* Mengubah tampilan graf sesuai time delay */
                    Global.updateGraph(worker, timeDelay);
                }

            }
            
            res = utility.copyList(Global.result);
            Global.clean();
            return res;
        }
    }
}