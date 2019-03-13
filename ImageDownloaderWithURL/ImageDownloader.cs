using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace ImageDownloaderWithURL
{
    public static class ImageDownloader
    {
        public static void SaveImage(string filePath, ImageFormat format, bool makeSquare, int squareSize = 256)
        {
            var folderName = "Images";

            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            var urls = File.ReadAllLines(filePath, System.Text.Encoding.Unicode).ToList();

            WebClient client = new WebClient();
            Stream stream = null;
            Bitmap bitmap;
            var extensionString = FileExtensionFromEncoder(format);
            
            for (int i = 0; i < urls.Count(); i++)
            {
                try
                {
                    stream = client.OpenRead(urls[i]);
                    bitmap = new Bitmap(stream);
                    if (bitmap != null)
                    {
                        if(makeSquare)
                            bitmap = MakeSquarePhoto(bitmap, squareSize);
                        bitmap.Save(folderName + "\\" + i.ToString() + extensionString, format);
                    }

                    stream.Flush();
                    stream.Close();
                    client.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"index: {i}\n{ex}");
                }

            }
        }
        
        public static Bitmap MakeSquarePhoto(Bitmap bmp, int size)
        {
            try
            {
                Bitmap resize = new Bitmap(size, size);
                Graphics g = Graphics.FromImage(resize);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size);
                int t = 0, l = 0;
                if (bmp.Height > bmp.Width)
                    t = (bmp.Height - bmp.Width) / 2;
                else
                    l = (bmp.Width - bmp.Height) / 2;
                g.DrawImage(bmp, new Rectangle(0, 0, size, size), new Rectangle(l, t, bmp.Width - l * 2, bmp.Height - t * 2), GraphicsUnit.Pixel);
                return resize;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static string FileExtensionFromEncoder(this ImageFormat format)
        {
            try
            {
                return ImageCodecInfo.GetImageEncoders()
                        .First(x => x.FormatID == format.Guid)
                        .FilenameExtension
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .First()
                        .Trim('*')
                        .ToLower();
            }
            catch (Exception)
            {
                return "." + format.ToString().ToLower();
            }
        }
    }
}
