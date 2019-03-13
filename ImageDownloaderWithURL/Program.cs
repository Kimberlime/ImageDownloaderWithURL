using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageDownloaderWithURL
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "urls.txt";

            var filePath = $"{Directory.GetCurrentDirectory()}\\{fileName}";
            ImageDownloader.SaveImage(fileName, ImageFormat.Jpeg, false);
        }
    }
}
