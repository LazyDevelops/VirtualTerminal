namespace VirtualTerminal.Command
{
    public class DateCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.ToString("yyyy. MM. dd. (ddd) HH:mm:ss\n");
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "\u001b[1m간략한 설명\x1b[22m\n" +
                       "   date - 현제 날짜 및 시간 출력\n\n" +
                       "\u001b[1m사용법\u001b[22m\n" +
                       "   date\n\n" +
                       "\u001b[1m설명\u001b[22m\n" +
                       "   위에 사용법을 이용하여 현제 날짜 및 시간 출력할 수 있습니다.\n" +
                       "   (자세한 사용법은 예시 참조)\n\n" +
                       "\u001b[1m옵션\u001b[22m\n" +
                       "   (없음)\n\n" +
                       "\u001b[1m예시\u001b[22m\n" +
                       "   date\n";
            }

            return "date - 현제 날짜 및 시간 출력";
        }
    }
}