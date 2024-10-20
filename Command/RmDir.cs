using VirtualTerminal.LCRSTree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class RmDirCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 2)
            {
                Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                return;
            }

            Tree<FileNode>? file;
            string? absolutePath;
            bool[] permission;

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = FileSystem.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                file = VT.FileSystem.FindFile(arg, VT.Root);

                if (file?.Parent == null)
                {
                    Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                permission = FileSystem.FileSystem.CheckPermission(VT.USER, file.Parent, VT.Root);

                if (permission[0] || !permission[1] || !permission[2])
                {
                    Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (file.Data.FileType != FileType.D)
                {
                    Console.WriteLine(ErrorMessage.NotD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (VT.FileSystem.RemoveFile(absolutePath, VT.Root, null) != 0)
                {
                    Console.WriteLine(ErrorMessage.DNotEmpty(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }
            }
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "rmdir - 비어있는 디렉터리를 삭제\n";
            }

            return "rmdir - 비어있는 디렉터리를 삭제";
        }
    }
}