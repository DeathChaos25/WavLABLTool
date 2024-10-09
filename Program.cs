using System.Text;

namespace WavLABLTool
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("WavLABLTool\nUsage: WavLABLTool <path-to-wav-file>\n\nPress Any Key to exit...");
                Console.ReadKey();
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found!");
                return;
            }

            List<string> NewLabels = new List<string>();

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

            string outputFilePath = Path.Combine(Path.GetDirectoryName(filePath), "Output", fileNameWithoutExtension + ".wav");
            Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(filePath), "Output"));

            if (File.Exists(Path.ChangeExtension(filePath, ".txt")))
            {
                NewLabels = File.ReadAllLines(Path.ChangeExtension(filePath, ".txt")).ToList();
            }
            else NewLabels.Add(fileNameWithoutExtension);

            using (var inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            using (var reader = new BinaryReader(inputFileStream))
            using (var writer = new BinaryWriter(outputFileStream))
            {
                writer.Write(reader.ReadBytes((int)reader.BaseStream.Length));

                writer.Write(Encoding.ASCII.GetBytes("LIST"));

                int targetChunkSize = 0;
                int currPos = (int)writer.BaseStream.Position;

                writer.Write(targetChunkSize);
                writer.Write(Encoding.ASCII.GetBytes("adtl"));

                int i = 0;

                foreach (var label in NewLabels)
                {
                    byte[] labelBytes = Encoding.ASCII.GetBytes(label);
                    int labelSize = 4 + labelBytes.Length + 1;

                    writer.Write(Encoding.ASCII.GetBytes("labl"));
                    writer.Write(labelSize);
                    writer.Write(i); // Cue ID
                    writer.Write(labelBytes);
                    writer.Write((byte)0);
                    if (i != NewLabels.Count - 1) writer.Write((byte)0);
                    i++;
                }

                targetChunkSize = ((int)writer.BaseStream.Position - currPos) - 4;

                writer.Seek(currPos, SeekOrigin.Begin);
                writer.Write(targetChunkSize);

                long finalFileSize = writer.BaseStream.Length;
                writer.Seek(4, SeekOrigin.Begin);
                writer.Write((int)(finalFileSize - 8));
            }

            Console.WriteLine($"New WAV file with label added: {outputFilePath}\nPress Any Key to exit...");
            Console.ReadKey();
        }
    }
}
