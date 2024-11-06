using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;
using VirtualTerminal.Tree.General;

// 문제 있음

namespace VirtualTerminal.Command
{
    public class CatCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
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

                file = VT.FileSystem.FindFile(absolutePath, VT.Root);

                if (file == null)
                {
                    return ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                permission = VT.FileSystem.CheckPermission(VT.USER, file, VT.Root);

                if (!permission[0])
                {
                    return ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (file.Data.FileType == FileType.D)
                {
                    return ErrorMessage.NotF(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                return file.Data.Content + "\n";
            }

            return VT.ReadMultiLineInput();
        }

        public string Description(bool detail)
        {
            // 설명 약간의 수정 필요
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   cat - 파일 내용 출력\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   cat [옵션] 파일명\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 파일 내용을 출력 할 수 있으며\n" +
                       "   입출력 재지정자를 이용해 파일 생성 및 내용 작성 및 수정이 가능합니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   -f\n" +
                       "       강제로 실행합니다.\n" +
                       "       (입출력 재지정자 전용 옵션)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   파일 내용 출력 예시\n" +
                       "       cat file.txt\n" +
                       "   입출력 지정자 예시\n" +
                       "       cat > file.txt\n" +
                       "       cat -f > file.txt\n" +
                       "       (-f를 이용해 파일 덮어쓰기 가능)\n";
            }

            return "cat - 파일 내용 출력";
        }
    }
}