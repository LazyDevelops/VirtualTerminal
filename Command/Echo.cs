namespace VirtualTerminal.Command
{
    public class EchoCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            string? result = null;

            foreach (string arg in argv.Skip(1))
            {
                result += arg + " ";
            }

            return result + "\n";
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   echo - 입력한 텍스트 출력\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   echo 문자열\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 터미널에 문자를 출력할 수 있습니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   echo Hello\n" +
                       "   echo Hello World!\n";
            }

            return "echo - 입력한 텍스트 출력";
        }
    }
}