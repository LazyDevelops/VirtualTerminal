namespace VirtualTerminal.Commands
{
    public class WhoamiCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.WriteLine(VT.USER);
        }
    }
}