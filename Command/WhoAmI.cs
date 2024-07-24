namespace VirtualTerminal.Command
{
    public class WhoAmICommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Console.WriteLine(VT.USER);
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "whoami - 접속중인 유저 이름 출력\n";
            }

            return "whoami - 접속중인 유저 이름 출력";
        }
    }
}