using System;

namespace VirtualTerminal.Commands
{
    public class DateCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            DateTime currentTime = DateTime.Now;

            Console.WriteLine(currentTime.ToString("yyyy. MM. dd. (ddd) HH:mm:ss"));
        }

        public string Description()
        {
            return "test";
        }
    }
}