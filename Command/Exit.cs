namespace VirtualTerminal.Command
{
    public class ExitCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Environment.Exit(0);
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   exit - 터미널 종료\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   exit\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 터미널을 종료할 수 있습니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   exit\n";
            }

            return "exit - 터미널 종료";
        }
    }
}