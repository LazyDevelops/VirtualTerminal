namespace VirtualTerminal.Commands
{
    public class ExitCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Environment.Exit(0);
        }

        public string Description(bool detail)
        {
            return "exit - 터미널 종료";
        }
    }
}