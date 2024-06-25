using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class MkdirCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            string[]? path;
            string? absolutePath;
            string? parentsPath;
            string? fileName;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    path = absolutePath.Split('/');
                    fileName = path[^1]; // path.Length - 1
                    parentsPath = absolutePath.Replace('/' + fileName, "");

                    if (VT.fileSystem.FindFile(absolutePath, VT.root) != null)
                    {
                        Console.WriteLine($"{args[0]}: cannot create directory '{fileName}': File exists");
                        Console.WriteLine($"{path} {fileName}");
                        return;
                    }

                    if (VT.fileSystem.FindFile(parentsPath, VT.root) == null)
                    {
                        Console.WriteLine($"{args[0]}: cannot create directory '{fileName}': No such file or directory");
                        return;
                    }

                    VT.fileSystem.CreateFile(parentsPath, new FileNode(fileName, VT.USER, 0b111101, FileType.D), VT.root);
                }
            }
        }
    }
}