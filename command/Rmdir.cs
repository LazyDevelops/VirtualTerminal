using Tree;
using static FileSystem.FileSystem;
using VirtualTerminal.Errors;

namespace VirtualTerminal.Commands
{
    public class RmdirCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            string[]? splitPath;
            string? absolutePath;
            string? fileName;
            bool[] permission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    splitPath = absolutePath.Split('/');
                    fileName = splitPath[^1]; // path.Length - 1

                    file = VT.fileSystem.FindFile(arg, VT.root);

                    if (file == null || file.Parents == null)
                    {
                        Console.WriteLine(ErrorsMassage.NoSuchForD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, file.Parents, VT.root);

                    if (permission[0] || !permission[1] || !permission[2])
                    {
                        Console.WriteLine(ErrorsMassage.PermissionDenied(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (file.Data.FileType == FileType.F)
                    {
                        Console.WriteLine(ErrorsMassage.NotD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (VT.fileSystem.RemoveFile(absolutePath, VT.root) != 0)
                    {
                        Console.WriteLine(ErrorsMassage.DNotEmpty(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }
                }
            }
        }
    }
}