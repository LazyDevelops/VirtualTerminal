namespace VirtualTerminal.Commands
{
    public class PwdCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.WriteLine(VT.PWD);
        }

        public string Description()
        {
            return "test";
        }
    }
}