using System.IO;

namespace Lab1_Tracer
{
    public class FileWritter : IWritter
    {
        public void Write(string serializedResult)
        {
            File.WriteAllText(PathToSave, serializedResult);
        }

        public string PathToSave { get; private set; }

        public FileWritter(string path)
        {
            PathToSave = path;
        }
    }
}