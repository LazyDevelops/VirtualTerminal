using System;

namespace VirtualTerminal.Command
{
    public class DateCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            DateTime currentTime = DateTime.Now;

            Console.WriteLine(currentTime.ToString("yyyy. MM. dd. (ddd) HH:mm:ss"));
        }

        public string Description(bool detail)
        {
            return "date - 현제 날짜 및 시간 출력";
        }
    }
}