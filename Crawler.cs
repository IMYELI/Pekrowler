using System.Diagnostics;
namespace Crawler
{
    class utility
    {
        public static List<string> copyList(List<String> l)
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
        public static List<string> result = new List<string>();
        public static Boolean isRunning = false;
        public static void clean()
        {
            Global.isRunning = false;
            Global.pathQueue = new Queue<string>();
            Global.result = new List<string>();
        }
        public static void colorEdge(string path, Microsoft.Msagl.Drawing.Graph graph, Microsoft.Msagl.Drawing.Color color)
        {
            string[] edges = path.Split(Path.DirectorySeparatorChar);

            for (int i=0;i<edges.Length-1;i++){
                System.Diagnostics.Debug.WriteLine("! " + edges[i] + " PEKO " + edges[i + 1]);
                Microsoft.Msagl.Drawing.Edge currEdge = Global.edgeMap[edges[i] + " PEKO " + edges[i + 1]];
                if (currEdge == null)
                {
                    System.Diagnostics.Debug.WriteLine("NOT FOUND " + edges[i] + " PEKO " + edges[i + 1]);
                }
                if (currEdge != null && currEdge.Attr.Color != Microsoft.Msagl.Drawing.Color.Green)
                {
                    currEdge.Attr.Color = color;
                    //currEdge.Attr.Id = edges[i] + " PEKO " + edges[i + 1];
                    System.Diagnostics.Debug.WriteLine("? " + currEdge.Attr.Id);
                }
            }
        }

        public static Dictionary<String, Microsoft.Msagl.Drawing.Edge> edgeMap = new();

    }
    class DFS
    {
        public static List<string> searchDFS(string fileToSearch, string path, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph,ref Microsoft.Msagl.GraphViewerGdi.GViewer gViewer1)
        {
            Global.isRunning = true;
            List<string> res;
            recursivelySearchDFS(fileToSearch, path, searchAll, ref graph, ref gViewer1,path);
            res = utility.copyList(Global.result);
            Global.clean();
            return res;
        }

        private static void recursivelySearchDFS(string fileToSearch, string path, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph,ref Microsoft.Msagl.GraphViewerGdi.GViewer gViewer1, string pathBapak)
        {
            String[] files = Directory.GetFiles(Path.GetFullPath(path));
            foreach (string file in files)
            {
                string parent = path.Split(Path.DirectorySeparatorChar).Last();
                string child = file.Split(Path.DirectorySeparatorChar).Last();
                
                Microsoft.Msagl.Drawing.Edge test = graph.AddEdge(parent, child);
                test.Attr.Id = parent +" PEKO " + child;
                Global.edgeMap[parent + " PEKO " + child] = test;
                if (file == path + "\\" + fileToSearch && Global.isRunning)
                {
                    Global.result.Add(file);
                    string parentAsli = pathBapak.Split(Path.DirectorySeparatorChar).Last();
                    string pathBenar = path.Replace(pathBapak, parentAsli);
                    pathBenar += "\\" + child;
                    Global.colorEdge(pathBenar,graph,Microsoft.Msagl.Drawing.Color.Green);
                    if (!searchAll)
                    {
                        Global.isRunning = false;
                        return;
                    }
                }else
                {
                    test.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }
            }

            String[] dirs = Directory.GetDirectories(path);
            foreach (String dir in dirs)
            {
                Console.WriteLine();
                if (Global.isRunning)
                {
                    string parent = path.Split(Path.DirectorySeparatorChar).Last();
                    string folder = dir.Split(Path.DirectorySeparatorChar).Last();
                    Microsoft.Msagl.Drawing.Edge eg = graph.AddEdge(parent, folder);
                    eg.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    eg.Attr.Id = parent + " PEKO " + folder;
                    Global.edgeMap[parent + " PEKO " + folder] = eg;

                    recursivelySearchDFS(fileToSearch, dir, searchAll, ref graph, ref gViewer1,pathBapak);
                }
            }
        }
    }


    class BFS
    {
        public static List<string> searchBFS(string fileToSearch, string pathToSearch, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph)
        {
            List<string> res;
            Global.pathQueue.Enqueue(pathToSearch);
            while (Global.pathQueue.Count > 0)
            {
                string path = Global.pathQueue.Dequeue();
                String[] files = Directory.GetFiles(Path.GetFullPath(path));
                Console.WriteLine();
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    string parent = path.Split(Path.DirectorySeparatorChar).Last();
                    string child = file.Split(Path.DirectorySeparatorChar).Last();
                    Microsoft.Msagl.Drawing.Edge test = graph.AddEdge(parent, child);
                    test.Attr.Id = parent + " PEKO " + child;
                    Global.edgeMap[parent + " PEKO " + child] = test;
                    System.Diagnostics.Debug.WriteLine("> " + test.Attr.Id);
                    string parentAsli = pathToSearch.Split(Path.DirectorySeparatorChar).Last();
                    string pathBenar = path.Replace(pathToSearch, parentAsli);
                    pathBenar += "\\" + child;
                    if (file == path + "\\" + fileToSearch)
                    {
                        Global.result.Add(file);
                        Global.colorEdge(pathBenar,graph,Microsoft.Msagl.Drawing.Color.Green);
                        if (!searchAll)
                        {
                            res = utility.copyList(Global.result);
                            Global.clean();
                            return res;
                        }
                    }else{
                        Global.colorEdge(pathBenar,graph,Microsoft.Msagl.Drawing.Color.Red);
                    }
                }

                String[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    string parent = path.Split(Path.DirectorySeparatorChar).Last();
                    string folder = dir.Split(Path.DirectorySeparatorChar).Last();
                    Microsoft.Msagl.Drawing.Edge eg = graph.AddEdge(parent, folder);
                    eg.Attr.Id = parent + " PEKO " + folder;
                    Global.edgeMap[parent + " PEKO " + folder] = eg;
                    System.Diagnostics.Debug.WriteLine("> " + eg.Attr.Id);
                    Global.pathQueue.Enqueue(dir);
                }
            }
            
            res = utility.copyList(Global.result);
            Global.clean();
            return res;
        }
    }
}