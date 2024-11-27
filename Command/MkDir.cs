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
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   mkdir - 디렉터리 생성\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   mkdir 폴더명\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 디렉터리를 생성할 수 있습니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   mkdir test\n" +
                       "   mkdir test/helloworld\n";
            }

            return "mkdir - 디렉터리 생성";
        }
    }
}