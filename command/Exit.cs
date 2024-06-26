namespace VirtualTerminal.Commands
{
    public class ExitCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Environment.Exit(0);
        }

        public string Description()
        {
            return "test";
        }
    }
}