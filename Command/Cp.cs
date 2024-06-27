namespace VirtualTerminal.Commands
{
    public class CpCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            
        }

        public string Description(bool detail)
        {
            return "cp - 파일이나 디렉터리 복사";
        }
    }
}