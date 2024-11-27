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
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   mkdir - 디렉터리 삭제\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   mkdir [옵션] 폴더명\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 빈 디렉터리를 삭제할 수 있습니다.\n" +
                       "   -f 없이는 빈 디렉터리가 아닐 경우 삭제가 불가능합니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   -f\n" +
                       "       안에 파일이 있어서 삭제합니다.\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   mkdir test\n" +
                       "   mkdir test/helloworld\n";
            }

            return "rmdir - 디렉터리 삭제";
        }
    }
}