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
            
            for (int i = 0; i < 20; i++) writer.Write((byte)0);
            writer.Write((byte)1);
            writer.Write(reader.VersionMinor);

            for (int i = 0; i < 78; i++) writer.Write((byte)0);
            writer.Write(point.format);
        }

        public void Dispose() { writer.Dispose(); }
    }
}
