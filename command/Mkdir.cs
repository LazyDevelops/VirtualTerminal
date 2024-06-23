using static FileSystem.FileSystem;

namespace VirtualTerminal
{
    public partial class VirtualTerminal
    {
        private void ExecuteMkdir(string[] args)
        {
            string[]? path;
            string? absolutePath;
            string? parentsPath;
            string? fileName;

            foreach (string temp in args)
            {
                if (temp != args[0] && !temp.Contains('-') && !temp.Contains("--"))
                {
                    absolutePath = fileSystem.GetAbsolutePath(temp, HOME, PWD);
                    path = absolutePath.Split('/');
                    fileName = path[^1]; // path.Length - 1
                    parentsPath = absolutePath.Replace('/' + fileName, "");

                    if (fileSystem.FindFile(absolutePath, root) != null)
                    {
                        Console.WriteLine($"{args[0]}: cannot create directory '{fileName}': File exists");
                        Console.WriteLine($"{path} {fileName}");
                        return;
                    }

                    if (fileSystem.FindFile(parentsPath, root) == null)
                    {
                        Console.WriteLine($"{args[0]}: cannot create directory '{fileName}': No such file or directory");
                        return;
                    }

                    fileSystem.CreateFile(parentsPath, new FileNode(fileName, USER, 0b111101, FileType.D), root);
                }
            }
        }
    }
}