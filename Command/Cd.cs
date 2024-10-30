using VirtualTerminal.Tree.General;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class CdCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 2)
            {
                VT.PwdNode = VT.HomeNode;
                VT.PWD = VT.HOME;
                return;
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

                absolutePath = FileSystem.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                file = VT.FileSystem.FindFile(absolutePath, VT.Root);

                if (file == null)
                {
                    Console.Write("bash: ");
                    Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                permission = FileSystem.FileSystem.CheckPermission(VT.USER, file, VT.Root);

                if (!permission[0] || !permission[2])
                {
                    Console.Write("bash: ");
                    Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (file.Data.FileType != FileType.D)
                {
                    Console.Write("bash: ");
                    Console.WriteLine(ErrorMessage.NotD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                VT.PwdNode = file;
                VT.PWD = absolutePath;
            }
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   cd - 현제 디렉터리 위치 변경\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   cd [옵션] 폴더명\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 디렉터리(폴더) 안에 들어갈 수 있습니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   cd ..\n" +
                       "   cd /home/user\n" +
                       "   cd ~\n" +
                       "   cd Item\n";
            }

            return "cd - 현제 디렉터리 위치 변경";
        }
    }
}