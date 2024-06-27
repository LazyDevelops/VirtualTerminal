using Tree;
using VirtualTerminal.Errors;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class RmDirCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            string? absolutePath;
            bool[] permission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

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

                    if (file.Data.FileType != FileType.D)
                    {
                        Console.WriteLine(ErrorsMassage.NotD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (VT.fileSystem.RemoveFile(absolutePath, VT.root, null) != 0)
                    {
                        Console.WriteLine(ErrorsMassage.DNotEmpty(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }
                }
            }
        }

        public string Description(bool detail)
        {
            return "rmdir - 비어있는 디렉터리를 삭제";
        }
    }
}