using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class MkdirCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? parentFile;
            string[]? path;
            string? absolutePath;
            string? parentPath;
            string? fileName;
            bool[] parentPermission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    path = absolutePath.Split('/');
                    fileName = path[^1]; // path.Length - 1
                    parentPath = absolutePath.Replace('/' + fileName, "");
                    parentFile = VT.fileSystem.FindFile(parentPath, VT.root);

                    if (parentFile == null)
                    {
                        Console.WriteLine($"{args[0]}: cannot create directory '{arg}': No such file or directory");
                        return;
                    }

                    parentPermission = VT.fileSystem.CheckFilePermission(VT.USER, parentFile.Data);

                    if(!parentPermission[1]){
                        Console.WriteLine($"{args[0]}: cannot create directory '{arg}': Permission denied");
                        return;
                    }

                    if (VT.fileSystem.FindFile(absolutePath, VT.root) != null)
                    {
                        Console.WriteLine($"{args[0]}: cannot create directory '{arg}': File exists");
                        return;
                    }

                    VT.fileSystem.CreateFile(parentPath, new FileNode(fileName, VT.USER, 0b111101, FileType.D), VT.root);
                }
            }
        }
    }
}