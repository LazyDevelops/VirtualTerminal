namespace VirtualTerminal.Commands
{
    public class ClearCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.Clear();
        }

        public string Description()
        {
            return "test";
        }
    }
}