using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pekrowler
{
    public class DFS
    {
        public static void depth(string root)
        {
            string[] content = Directory.GetFiles(root);
            foreach (string item in content)
            {
                Console.Write(item);
            }
        }

    }
}
