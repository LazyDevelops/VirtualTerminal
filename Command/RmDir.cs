using Tree;
using VirtualTerminal.Error;
using static VirtualTerminal.FileSystem.FileSystem;

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

            foreach (string arg in argv)
            {
                if (arg == argv[0] || arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                file = VT.fileSystem.FindFile(arg, VT.root);

                if (file?.Parents == null)
                {
                    Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                permission = VT.fileSystem.CheckFilePermission(VT.USER, file.Parents, VT.root);

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

                if (VT.fileSystem.RemoveFile(absolutePath, VT.root, null) != 0)
                {
                    Console.WriteLine(ErrorMessage.DNotEmpty(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }
            }
        }

        public string Description(bool detail)
        {
            return "rmdir - 비어있는 디렉터리를 삭제";
        }
    }
}