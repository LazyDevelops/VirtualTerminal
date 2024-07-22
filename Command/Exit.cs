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
            return "exit - 터미널 종료";
        }
    }
}