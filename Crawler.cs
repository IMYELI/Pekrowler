using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
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
        public static Queue<string> nodePathQueue = new Queue<string>();
        public static List<string> result = new List<string>();
        public static Boolean isRunning = false;
        public static void clean()
        {
            Global.isRunning = false;
            Global.pathQueue = new Queue<string>();
            Global.result = new List<string>();
            Global.nodeMap = new Dictionary<string, int>();
            Global.nodePathQueue = new Queue<string>();
        }
        public static void colorEdge(string path, Microsoft.Msagl.Drawing.Graph graph, Microsoft.Msagl.Drawing.Color color)
        {
            string[] edges = path.Split(Path.DirectorySeparatorChar);

            for (int i=0;i<edges.Length-1;i++){

                Microsoft.Msagl.Drawing.Edge currEdge = Global.edgeMap[edges[i] + " PEKO " + edges[i + 1]];
                if (currEdge == null)
                {

                }
                if (currEdge != null && currEdge.Attr.Color != Microsoft.Msagl.Drawing.Color.Green)
                {
                    currEdge.Attr.Color = color;
                    //currEdge.Attr.Id = edges[i] + " PEKO " + edges[i + 1];
                }
            }
        }


        public static Dictionary<String, Microsoft.Msagl.Drawing.Edge> edgeMap = new();

        public static void updateGraph(BackgroundWorker worker, int timeDelay)
        {
            Mutex mutex = new Mutex();
            mutex.WaitOne();
            worker.ReportProgress(1);
            Thread.Sleep(timeDelay);
            mutex.ReleaseMutex();
        }

        public static Dictionary<String,int> nodeMap = new();

        public static void addNode(string node)
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
        {
            Global.isRunning = true;
            List<string> res;
            recursivelySearchDFS(fileToSearch, path, searchAll, ref graph, timeDelay, path,worker, e, path);
            res = utility.copyList(Global.result);
            Global.clean();
            return res;
        }

        private static void recursivelySearchDFS(string fileToSearch, string path, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph, int timeDelay, string pathBapak, BackgroundWorker worker, DoWorkEventArgs e, string pathNode)
        {
            String[] files = Directory.GetFiles(Path.GetFullPath(path));
            foreach (string file in files)
            {
                string parent = pathNode.Split(Path.DirectorySeparatorChar).Last();
                string child = file.Split(Path.DirectorySeparatorChar).Last();
                Global.addNode(child);
                string newChildNode = Global.getNode(child);
                Microsoft.Msagl.Drawing.Node parentNode = null;
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
                
                graph.AddNode(childNode);
                eg.Attr.Id = parent +" PEKO " + newChildNode;
                Global.edgeMap[parent + " PEKO " + newChildNode] = eg;
                if (file == path + "\\" + fileToSearch && Global.isRunning)
                {
                    Global.result.Add(file);
                    string parentAsli = pathBapak.Split(Path.DirectorySeparatorChar).Last();
                    string pathBenar = pathNode.Replace(pathBapak, parentAsli);
                    pathBenar += "\\" + newChildNode;

                    Global.colorEdge(pathBenar,graph,Microsoft.Msagl.Drawing.Color.Green);
                    if (!searchAll)
                    {
                        Global.isRunning = false;
                        return;
                    }
                }else
                {
                    eg.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                }

                Global.updateGraph(worker, timeDelay);
            }

            String[] dirs = Directory.GetDirectories(path);
            foreach (String dir in dirs)
            {
                Console.WriteLine();
                if (Global.isRunning)
                {
                    string parent = pathNode.Split(Path.DirectorySeparatorChar).Last();
                    string folder = dir.Split(Path.DirectorySeparatorChar).Last();
                    Global.addNode(folder);
                    string newFolder = Global.getNode(folder);
                    Microsoft.Msagl.Drawing.Node parentNode = null;
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

                    eg.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    eg.Attr.Id = parent + " PEKO " + newFolder;
                    Global.edgeMap[parent + " PEKO " + newFolder] = eg;
                    string newPathNode = pathNode + "\\" + newFolder;
                    Global.updateGraph(worker, timeDelay);
                    recursivelySearchDFS(fileToSearch, dir, searchAll, ref graph, timeDelay, pathBapak, worker, e, newPathNode);
                }
            }
        }
    }


    class BFS
    {
        public static List<string> searchBFS(string fileToSearch, string pathToSearch, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph, int timeDelay, BackgroundWorker worker, DoWorkEventArgs e)
        {
            List<string> res;
            Global.pathQueue.Enqueue(pathToSearch);
            Global.nodePathQueue.Enqueue(pathToSearch);
            while (Global.pathQueue.Count > 0)
            {
                string path = Global.pathQueue.Dequeue();
                string pathNode = Global.nodePathQueue.Dequeue();

                String[] files = Directory.GetFiles(Path.GetFullPath(path));
                Microsoft.Msagl.Drawing.Edge eg = null;
                Microsoft.Msagl.Drawing.Node parentNode = null;
                Microsoft.Msagl.Drawing.Node childNode = null;
                foreach (string file in files)
                {
                    string parent = pathNode.Split(Path.DirectorySeparatorChar).Last();
                    string child = file.Split(Path.DirectorySeparatorChar).Last();
                    Global.addNode(child);
                    string newChild = Global.getNode(child);
                    
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
                    graph.AddNode(childNode);
                    eg.Attr.Id = parent + " PEKO " + newChild;
                    Global.edgeMap[parent + " PEKO " + newChild] = eg;

                    string parentAsli = pathToSearch.Split(Path.DirectorySeparatorChar).Last();
                    string pathBenar = pathNode.Replace(pathToSearch, parentAsli);
                    pathBenar += "\\" + newChild;
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

                    Global.updateGraph(worker, timeDelay);

                }
                Microsoft.Msagl.Drawing.Node folderNode = null;
                String[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    string parent = pathNode.Split(Path.DirectorySeparatorChar).Last();
                    string folder = dir.Split(Path.DirectorySeparatorChar).Last();
                    Global.addNode(folder);
                    string newFolder = Global.getNode(folder);
                    parentNode = null;
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

                    eg.Attr.Id = parent + " PEKO " + newFolder;
                    Global.edgeMap[parent + " PEKO " + newFolder] = eg;
                    
                    string newPathNode  = pathNode+ "\\" + newFolder;
                    Global.pathQueue.Enqueue(dir);
                    Global.nodePathQueue.Enqueue(newPathNode);
                    Global.updateGraph(worker, timeDelay);
                }

            }
            
            res = utility.copyList(Global.result);
            Global.clean();
            return res;
        }
    }
}