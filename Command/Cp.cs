namespace VirtualTerminal.Command
{
    public class CpCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            
        }

        public string Description(bool detail)
        {
            return "cp - 파일과 디렉터리 복사";
        }
    }
}