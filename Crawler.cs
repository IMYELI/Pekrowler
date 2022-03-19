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
    }
    class DFS
    {
        public static List<string> searchDFS(string fileToSearch, string path, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph)
        {
            Global.isRunning = true;
            List<string> res;
            recursivelySearchDFS(fileToSearch, path, searchAll, ref graph);
            res = utility.copyList(Global.result);
            Global.clean();
            return res;
        }

        private static void recursivelySearchDFS(string fileToSearch, string path, bool searchAll, ref Microsoft.Msagl.Drawing.Graph graph)
        {
            String[] files = Directory.GetFiles(Path.GetFullPath(path));
            Console.WriteLine(String.Format("Searching \"{0}\" in \"{1}\"", fileToSearch, path));
            foreach (string file in files)
            {
                string parent = path.Split(Path.DirectorySeparatorChar).Last();
                string child = file.Split(Path.DirectorySeparatorChar).Last();
                graph.AddEdge(parent, child);
                Console.WriteLine(file);
                if (file == path + "\\" + fileToSearch && Global.isRunning)
                {
                    Global.result.Add(file);
                    Console.WriteLine("eureka!");
                    if (!searchAll)
                    {
                        Global.isRunning = false;
                        return;
                    }
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
                    graph.AddEdge(parent, folder);
                    recursivelySearchDFS(fileToSearch, dir, searchAll, ref graph);
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
                Console.WriteLine(String.Format("Searching \"{0}\" in \"{1}\"", fileToSearch, path));
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    string parent = path.Split(Path.DirectorySeparatorChar).Last();
                    string child = file.Split(Path.DirectorySeparatorChar).Last();
                    graph.AddEdge(parent, child);
                    if (file == path + "\\" + fileToSearch)
                    {
                        Global.result.Add(file);
                        Console.WriteLine("eureka!");
                        if (!searchAll)
                        {
                            res = utility.copyList(Global.result);
                            Global.clean();
                            return res;
                        }
                    }
                }

                String[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    string parent = path.Split(Path.DirectorySeparatorChar).Last();
                    string folder = dir.Split(Path.DirectorySeparatorChar).Last();
                    graph.AddEdge(parent, folder);
                    Global.pathQueue.Enqueue(dir);
                }
            }
            
            res = utility.copyList(Global.result);
            Global.clean();
            return res;
        }
    }
}