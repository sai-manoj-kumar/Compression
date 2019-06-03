using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Compression
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("config.txt");
            var compressedString = Zip(inputString);
            if (!string.Equals(inputString, UnZip(compressedString)))
            {
                throw new Exception("Compression failed");
            }
        }

        public static string Zip(string compressed)
        {
            var bytes = Encoding.UTF8.GetBytes(compressed); 
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }

                return Convert.ToBase64String(mso.ToArray());
            }
        }

        static string UnZip(string compressed)
        {
            var bytes = Convert.FromBase64String(compressed);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}
