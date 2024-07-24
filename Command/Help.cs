namespace VirtualTerminal.Command
{
    public class HelpCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Console.WriteLine("\"man 명령어\"를 이용해 더 자세한 내용을 볼 수 있습니다.\n");
            Console.WriteLine("명령어 목록:");

            foreach (VirtualTerminal.ICommand action in VT.CommandMap.Values)
            {
                Console.WriteLine(action.Description(false));
            }
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   help - 모든 명령어의 간단한 사용방법 출력\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   help\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 모든 명령어의 간단한 사용방법 출력할 수 있습니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   help\n";
            }

            return "help - 모든 명령어의 간단한 사용방법 출력";
        }
    }
}