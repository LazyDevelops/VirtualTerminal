namespace VirtualTerminal.Command
{
    public class PwdCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            return VT.PWD + "\n";
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   pwd - 현제 작업중인 디렉터리의 경로 출력\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   pwd\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 내가 현제 들어와 있는 폴더(디렉터리)" +
                       "   위치를 출력할 수 있습니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   pwd\n";
            }

            return "pwd - 현제 작업중인 디렉터리의 경로 출력";
        }
    }
}