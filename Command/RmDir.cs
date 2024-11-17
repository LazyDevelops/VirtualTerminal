using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;
using VirtualTerminal.Tree.General;

namespace VirtualTerminal.Command
{
    public class RmDirCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 2)
            {
                return ErrorMessage.ArgLack(argv[0]);
            }

            Node<FileDataStruct>? file;
            string? absolutePath;
            bool[] permission;

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = VT.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                file = VT.FileSystem.FileFind(arg, VT.Root);

                if (file?.Parent == null)
                {
                    return ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                permission = VT.FileSystem.CheckPermission(VT.USER, file.Parent, VT.Root);

                if (permission[0] || !permission[1] || !permission[2])
                {
                    return ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (file.Data.FileType != FileType.D)
                {
                    return ErrorMessage.NotD(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (VT.FileSystem.FileRemove(absolutePath, VT.Root, null) != 0)
                {
                    return ErrorMessage.DNotEmpty(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }
            }

            return null;
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