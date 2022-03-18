namespace Pekrowler
{
    internal static class Program
    {
        public static void searchDFS(string fileToSearch, string path, bool searchAll)
        {
            String[] files = Directory.GetFiles(Path.GetFullPath(path));
            Console.WriteLine(String.Format("Searching \"{0}\" in \"{1}\"", fileToSearch, path));
            foreach (String file in files)
            {
                Console.WriteLine(file);
                if (file == path + "\\" + fileToSearch)
                {
                    Console.WriteLine("eureka!");
                    if (!searchAll)
                        return;
                }
            }

            String[] dirs = Directory.GetDirectories(path);
            foreach (String dir in dirs)
            {
                Console.WriteLine();
                searchDFS(fileToSearch, dir, searchAll);
            }
        }

        public static void searchBFS(string fileToSearch, string pathToSearch, bool searchAll)
        {
            pathQueue.contents.Enqueue(pathToSearch);
            while (pathQueue.contents.Count > 0)
            {
                string path = pathQueue.contents.Dequeue();
                String[] files = Directory.GetFiles(Path.GetFullPath(path));
                Console.WriteLine();
                Console.WriteLine(String.Format("Searching \"{0}\" in \"{1}\"", fileToSearch, path));
                foreach (String file in files)
                {
                    Console.WriteLine(file);
                    if (file == path + "\\" + fileToSearch)
                    {
                        Console.WriteLine("eureka!");
                        if (!searchAll)
                            return;
                    }
                }

                String[] dirs = Directory.GetDirectories(path);
                foreach (String dir in dirs)
                {
                    pathQueue.contents.Enqueue(dir);
                }
            }
        }
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
    class pathQueue
    {
        public static Queue<string> contents = new Queue<string>();
    }
}