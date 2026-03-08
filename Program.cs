using System.Text;
using System.IO.Compression;

namespace ArcConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("usage:<infile> <outfile> <delete bool>");
                Console.ReadKey();
                return;
            }

            string inPath = args[0];
            string outPath = args[1];

            bool deleteFile = false;

            if (args.Length > 2)
            {
                deleteFile = bool.TryParse(args[2], out deleteFile) ? deleteFile : false;
            }

            switch (Path.GetExtension(inPath))
            {
                case ".arc":
                    ArcToZip(inPath, Path.GetFileNameWithoutExtension(outPath) + ".zip", deleteFile);
                    break;
                case ".zip":
                    ZipToArc(inPath, Path.GetFileNameWithoutExtension(outPath) + ".arc", deleteFile);
                    break;
                default:
                    Console.WriteLine("Unknown file extension.");
                    Console.ReadKey();
                    break;
            }
        }

        static void ArcToZip(string inFilename, string outFilename, bool delete)
        {
            byte[] allBytes = File.ReadAllBytes(inFilename);

            using MemoryStream ms = new(allBytes);
            using BinaryReader reader = new(ms);

            using ZipArchive zipFile = ZipFile.Open(outFilename, ZipArchiveMode.Create);

            int entryCount = (int)Converters.Read(reader);

            for (int i = 0; i < entryCount; i++)
            {
                short nameLen = (short)Converters.Read(reader, true);
                byte[] nameBytes = reader.ReadBytes(nameLen);
                string filename = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0').Replace("\\", "/");

                int offset = (int)Converters.Read(reader);
                int size = (int)Converters.Read(reader);

                ZipArchiveEntry entry = zipFile.CreateEntry(filename);
                using Stream entryStream = entry.Open();
                entryStream.Write(allBytes, offset, size);
            }

            if (delete)
            {
                File.Delete(inFilename);
            }
        }

        static void ZipToArc(string inFilename, string outFilename, bool delete)
        {
            List<(string Name, byte[] Content)> files = [];

            using (ZipArchive zip = ZipFile.OpenRead(inFilename))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    using Stream stream = entry.Open();
                    using MemoryStream ms = new();

                    stream.CopyTo(ms);
                    files.Add((entry.FullName, ms.ToArray()));
                }
            }

            int headerLength = 4;
            foreach ((string Name, byte[] Content) in files)
            {
                headerLength += 10 + Encoding.UTF8.GetByteCount(Name);
            }

            using FileStream fs = new(outFilename, FileMode.Create);
            using BinaryWriter writer = new(fs);
            using MemoryStream dataBlock = new();

            Converters.Write(writer, files.Count);

            int currentOffset = headerLength;
            foreach ((string Name, byte[] Content) in files)
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(Name.Replace("/", "\\"));

                Converters.Write(writer, (short)nameBytes.Length);
                writer.Write(nameBytes);
                Converters.Write(writer, currentOffset);
                Converters.Write(writer, Content.Length);

                dataBlock.Write(Content, 0, Content.Length);
                currentOffset += Content.Length;
            }

            writer.Write(dataBlock.ToArray());

            if (delete)
            {
                File.Delete(inFilename);
            }
        }
    }
}