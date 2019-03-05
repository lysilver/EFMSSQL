using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class FileHelper
    {
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static void Delete(string path)
        {
            File.Delete(path);
        }
    }
}