namespace VirtualTerminal.Command
{
    public class PwdCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Console.WriteLine(VT.PWD);
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "pwd - 현제 작업중인 디렉터리의 경로 출력\n";
            }

            return "pwd - 현제 작업중인 디렉터리의 경로 출력";
        }
    }
}