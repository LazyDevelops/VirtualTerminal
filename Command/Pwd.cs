namespace VirtualTerminal.Commands
{
    public class PwdCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Console.WriteLine(VT.PWD);
        }

        public string Description(bool detail)
        {
            return "pwd - 현제 작업중인 디렉터리의 경로 출력";
        }
    }
}