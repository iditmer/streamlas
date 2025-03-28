using System;
using System.IO;
using System.Text;

namespace streamlas
{
    public class lasStreamWriter : IDisposable
    {
        internal BinaryWriter writer;

        public lasStreamWriter(lasStreamReader reader, lasPointRecord point, string path)
        {
            writer = new BinaryWriter(File.Create(path));
            writer.Write(Encoding.ASCII.GetBytes("LASF"));
        }

        public void Dispose() { writer.Dispose(); }
    }
}
