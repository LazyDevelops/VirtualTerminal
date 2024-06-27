namespace VirtualTerminal.Command
{
    public class WhoAmICommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.WriteLine(VT.USER);
        }

        public string Description(bool detail)
        {
            return "whoami - �������� ���� �̸� ���";
        }
    }
}