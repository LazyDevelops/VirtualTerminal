using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal
{
    public partial class VirtualTerminal
    {
        private void ExecuteCat(string[] args)
        {
            Tree<FileNode>? file;
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

                    file = fileSystem.FindFile(absolutePath, root);

                    if (file == null)
                    {
                        Console.WriteLine($"File not found: {fileName}. Creating new file. Enter content (end with a single dot on a line):");
                        string content = ReadMultiLineInput();
                        fileSystem.CreateFile(parentsPath, new FileNode(fileName, USER, 0b110100, FileType.F, content), root);
                        return;
                    }

                    if (file.Data.FileType == FileType.D)
                    {
                        Console.WriteLine($"Not a file: {file.Data.Name}");
                        return;
                    }

                    Console.WriteLine(file.Data.Content);
                }
            }
        }
    }
}