namespace VirtualTerminal.Commands
{
    public class ClearCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.Clear();
        }

        public string Description(bool detail)
        {
            return "clear - 터미널 창 정리";
        }
    }
}