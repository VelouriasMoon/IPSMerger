using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IPSMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                string path = Directory.GetCurrentDirectory();
                // Check for existing merged ips and delete it so it doesn't cause issues
                if (File.Exists(path + Path.DirectorySeparatorChar + "merged.ips"))
                {
                    File.Delete(path + Path.DirectorySeparatorChar + "merged.ips");
                }

                // Create new file stream for the new merged ips patch 
                FileStream fs = File.Create(path + Path.DirectorySeparatorChar + "merged.ips");
                byte[] info = new UTF8Encoding(true).GetBytes("PATCH");
                fs.Write(info, 0, info.Length);
                int num = args.Length;

                // Load each one by one and copy the patch data to the new file
                for (int i = 0; i < num; i++)
                {
                    Console.WriteLine("IPS Patch {0}: {1}", i, Convert.ToString(args[i]));
                    byte[] file = File.ReadAllBytes(args[i]);
                    byte[] patch = new byte[file.Length - 8];
                    Buffer.BlockCopy(file, 5, patch, 0, patch.Length);
                    fs.Write(patch, 0, patch.Length);
                }

                // Write the EOF bytes to finish
                byte[] end = new UTF8Encoding(true).GetBytes("EOF");
                fs.Write(end, 0, end.Length);

            }
            else
            {
                // Catch for stupid people
                Console.WriteLine("Usage: IPSMerger [.ips Files]");
            }

            return;
        }
    }
}
