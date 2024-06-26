namespace VirtualTerminal.Commands
{
    public class CpCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            
        }

        public string Description()
        {
            return "cp - ";
        }
    }
}