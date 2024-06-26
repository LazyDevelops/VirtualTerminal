namespace VirtualTerminal.Commands
{
    public class WhoamiCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.WriteLine(VT.USER);
        }

        public string Description()
        {
            return "whoami - 접속중인 유저 이름 출력";
        }
    }
}