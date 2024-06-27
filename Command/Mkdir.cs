using Tree;
using VirtualTerminal.Error;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Command
{
    public class MkDirCommand : VirtualTerminal.ICommand
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
                        Console.WriteLine(ErrorMessage.NoSuchForD(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, parentFile, VT.root);

                    if(!permission[0] || !permission[1] || !permission[2]){
                        Console.WriteLine(ErrorMessage.PermissionDenied(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    if(parentFile.Data.FileType != FileType.D){
                        Console.WriteLine(ErrorMessage.NotD(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (VT.fileSystem.FindFile(absolutePath, VT.root) != null)
                    {
                        Console.WriteLine(ErrorMessage.FExists(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    VT.fileSystem.CreateFile(parentPath, new FileNode(fileName, VT.USER, 0b111101, FileType.D), VT.root);
                }
            }
        }

        public string Description(bool detail)
        {
            return "mkdir - 디렉터리 생성";
        }
    }
}