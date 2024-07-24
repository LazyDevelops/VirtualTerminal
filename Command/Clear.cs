namespace VirtualTerminal.Command
{
    public class ClearCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Console.Clear();
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "clear - 터미널 창 정리\n";
            }

            return "clear - 터미널 창 정리";
        }
    }
}