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
                    CopyTo(msi, gs);
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
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];
            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}
