using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class MkdirCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? parentFile;
            string[]? splitPath;
            string? absolutePath;
            string? parentPath;
            string? fileName;
            bool[] permission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    splitPath = absolutePath.Split('/');
                    fileName = splitPath[^1]; // path.Length - 1
                    parentPath = absolutePath.Replace('/' + fileName, "");
                    parentFile = VT.fileSystem.FindFile(parentPath, VT.root);

                    if (parentFile == null)
                    {
                        Console.WriteLine($"{args[0]}: '{arg}' 디렉터리를 만들 수 없습니다: No such file or directory");
                        return;
                    }

                    if(parentFile.Data.FileType != FileType.D){
                        Console.WriteLine($"{args[0]}: {arg}: Not a directory");
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, parentFile, VT.root);

                    if(!permission[0] || !permission[1] || !permission[2]){
                        Console.WriteLine($"{args[0]}: '{arg}' 디렉터리를 만들 수 없습니다: Permission denied");
                        return;
                    }

                    if (VT.fileSystem.FindFile(absolutePath, VT.root) != null)
                    {
                        Console.WriteLine($"{args[0]}: '{arg}' 디렉터리를 만들 수 없습니다: File exists");
                        return;
                    }

                    VT.fileSystem.CreateFile(parentPath, new FileNode(fileName, VT.USER, 0b111101, FileType.D), VT.root);
                }
            }
        }
    }
}