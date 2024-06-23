using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class CatCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
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
                    absolutePath = VT.fileSystem.GetAbsolutePath(temp, VT.HOME, VT.PWD);
                    path = absolutePath.Split('/');
                    fileName = path[^1]; // path.Length - 1
                    parentsPath = absolutePath.Replace('/' + fileName, "");

                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine($"File not found: {fileName}. Creating new file. Enter content (end with a single dot on a line):");
                        string content = VT.ReadMultiLineInput();
                        VT.fileSystem.CreateFile(parentsPath, new FileNode(fileName, VT.USER, 0b110100, FileType.F, content), VT.root);
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