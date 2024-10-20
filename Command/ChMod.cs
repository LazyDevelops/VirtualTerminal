using VirtualTerminal.LCRSTree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class ChModCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Node<FileDataStruct>? file;
            string? absolutePath;
            byte? inputPermission;

            if (argv[1].Length == 6 && (argv[1].Contains('1') || argv[1].Contains('0')))
            {
                inputPermission = Convert.ToByte(argv[1].PadLeft(8, '0'), 2);
            }
            else
            {
                Console.WriteLine(ErrorMessage.InvalidMode(argv[0], ErrorMessage.DefaultErrorComment(argv[1])));
                return;
            }

            foreach (string arg in argv.Skip(2))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = FileSystem.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                file = VT.FileSystem.FindFile(absolutePath, VT.Root);

                if (file == null)
                {
                    Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (file.Data.UID != VT.USER)
                {
                    Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                // Error : 프로퍼티 'Data' 액세스는 임시값을 반환합니다. 액세스된 구조체가 변수로 분류되지 않는 경우 구조체 멤버를 수정할 수 없습니다
                //file.Data.Permission = inputPermission.Value;

                // 임시 코드
                FileDataStruct temp = file.Data;
                temp.Permission = inputPermission.Value;
                file.Data = temp;
            }
        }

        public string Description(bool detail)
        {
            /*if (detail)
            {
                return "chmod - 소유하고 있는 파일의 권한을 변경\n";
            }*/
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   chmod - 디렉토리 혹은 파일 권한 설정\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   chmod (파일 소유자 rwx 권한)(그외 사용자 rwd 권한) 파일명\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 파일 혹은 폴더의 권한을 변경할 수 있습니다.\n" +
                       "   rwx는 순서대로 읽기 쓰기 실행 권한을 의미합니다.\n" +
                       "   rwx권한은 100 = 읽기만 허용, 110 = 읽기와 쓰기 허용, 111 = 읽기 쓰기 실행 허용\n" +
                       "   이런 식으로 표현 할 수 있습니다." +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   chmod 110100 ./only_edit_me.txt\n" +
                       "   chmod 110110 ./all_show_edit_file.txt\n" +
                       "   chmod 111111 ./all_show_edit_run_file.txt~\n" +
                       "   cd Item\n";
            }

            return "chmod - 소유하고 있는 파일의 권한을 변경";
        }
    }
}