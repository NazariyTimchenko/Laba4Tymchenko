using System;
using System.IO;

namespace LB4
{
    class Program
    {

        static void Main(string[] args)
        {
            bool Subdir = false;
            bool Hidden = false;
            bool Read = false;
            bool Archive = false;
            string Path = Directory.GetCurrentDirectory();
            string Mask = "*.*";
            long Size = 0;

            if (args.Length == 0 || args[0].ToLower() == "/?")
            {
                Console.WriteLine("Param: [/s] [/a] [/h] [/r] [/p:path] [/m:mask]");
                Console.WriteLine("/s - enable counting of files in subdirectories");
                Console.WriteLine("/a - include files with archive attribute");
                Console.WriteLine("/h - enable hidden files");
                Console.WriteLine("/r - include read-only files");
                Console.WriteLine("/p:path - specify directory for counting");
                Console.WriteLine("/m:mask - specify file mask (For example -> *.txt)");
                Console.WriteLine();

                var files = Directory.GetFiles(Path, Mask, Subdir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                Console.WriteLine("Content of directory " + Path + ":");
                Console.WriteLine("-------------------------------------");
                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
                Console.WriteLine("-------------------------------------");
                Console.WriteLine();
                Environment.Exit(0);
            }
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "/s") { Subdir = true; }
                else if (args[i].ToLower() == "/a") { Archive = true; }
                else if (args[i].ToLower() == "/h") { Hidden = true; }
                else if (args[i].ToLower() == "/r") { Read = true; }
                else if (args[i].ToLower().StartsWith("/p:")) { Path = args[i].Substring(3); }
                else if (args[i].ToLower().StartsWith("/m:")) { Mask = args[i].Substring(3); }
                else
                {
                    Console.WriteLine("Error! Unknown parameter: " + args[i]);
                    Environment.Exit(1);
                }
            }

            try
            {
                var files = Directory.GetFiles(Path, Mask, Subdir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    var attributes = File.GetAttributes(file);
                    if (Archive || !attributes.HasFlag(FileAttributes.Archive))
                    {
                        if (Hidden || !attributes.HasFlag(FileAttributes.Hidden)) { if (Read || !attributes.HasFlag(FileAttributes.ReadOnly)) { Size += new FileInfo(file).Length; } }
                    }
                }

                Console.WriteLine("Total size of files: " + Size + " bytes");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Try again! Message: " + ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
