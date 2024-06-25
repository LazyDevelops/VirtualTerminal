using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class RmdirCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            string[]? path;
            string? absolutePath;
            string? fileName;
            bool[] parentPermission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    path = absolutePath.Split('/');
                    fileName = path[^1]; // path.Length - 1

                    file = VT.fileSystem.FindFile(arg, VT.root);

                    if (file == null || file.Parents == null)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{arg}': No such file or directory");
                        return;
                    }

                    parentPermission = VT.fileSystem.CheckFilePermission(VT.USER, file.Parents.Data);

                    if (parentPermission[1])
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{arg}': No such file or directory");
                        return;
                    }

                    if (file.Data.FileType == FileType.F)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{arg}': Not a directory");
                        return;
                    }

                    if (VT.fileSystem.RemoveFile(absolutePath, VT.root) != 0)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{arg}': Directory not empty");
                        return;
                    }
                }
            }
        }
    }
}