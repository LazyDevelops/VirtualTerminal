using VirtualTerminal.Tree.General;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class RmCommand : VirtualTerminal.ICommand
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

            Dictionary<string, bool> options = new() { { "r", false } };

            VirtualTerminal.OptionCheck(ref options, in argv);

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = FileSystem.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                absolutePath.Split('/');

                file = VT.FileSystem.FindFile(absolutePath, VT.Root);

                if (file == null)
                {
                    return ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                permission = FileSystem.FileSystem.CheckPermission(VT.USER, file, VT.Root);

                if (!permission[0])
                {
                    return ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (!options["r"] && file.Data.FileType == FileType.D)
                {
                    return ErrorMessage.NotF(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (options["r"])
                {
                    VT.FileSystem.RemoveFile(absolutePath, VT.Root, 'r');
                }

                VT.FileSystem.RemoveFile(absolutePath, VT.Root, null);
            }

            return null;
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "rm - 파일이나 디렉터리 삭제\n";
            }

            return "rm - 파일이나 디렉터리 삭제";
        }
    }
}