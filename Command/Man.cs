using VirtualTerminal.Error;

namespace VirtualTerminal.Command
{
    public class ManCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 2)
            {
                return ErrorMessage.ArgLack(argv[0]);
            }

            if (VT.CommandMap.TryGetValue(argv[1], out VirtualTerminal.ICommand? action))
            {
                return action.Description(true);
            }

            return ErrorMessage.CmdNotFound(argv[0], ErrorMessage.DefaultErrorComment(argv[1]));
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   man - 명령어의 자세한 설명 출력\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   man [옵션] 명령어\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 명령어의 상세 설명 출력할 수 있습니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   man ls\n" +
                       "   man cd\n";
            }

            return "man - 명령어에 대한 자세한 설명 출력";
        }
    }
}