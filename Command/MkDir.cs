using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;
using VirtualTerminal.Tree.General;

namespace VirtualTerminal.Command
{
    public class MkDirCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 2)
            {
                return ErrorMessage.ArgLack(argv[0]);
            }

            Node<FileDataStruct>? parentFile;
            string? absolutePath;
            string? parentPath;
            string? fileName;
            bool[] permission;

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = VT.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                fileName = absolutePath.Split('/')[^1];
                parentPath = absolutePath.Replace('/' + fileName, "");

                parentFile = VT.FileSystem.FileFind(parentPath, VT.Root);

                if (parentFile == null)
                {
                    return ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                permission = VT.FileSystem.CheckPermission(VT.USER, parentFile, VT.Root);

                if (!permission[0] || !permission[1] || !permission[2])
                {
                    return ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (parentFile.Data.FileType != FileType.D)
                {
                    return ErrorMessage.NotD(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (VT.FileSystem.FileFind(absolutePath, VT.Root) != null)
                {
                    return ErrorMessage.FExists(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                VT.FileSystem.FileCreate(parentPath, new FileDataStruct(fileName, VT.USER, 0b111101, FileType.D), VT.Root);
            }

            return null;
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "mkdir - 디렉터리 생성\n";
            }

            return "mkdir - 디렉터리 생성";
        }
    }
}