namespace VirtualTerminal.Commands
{
    public class ChmodCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            
        }

        public string Description(bool detail)
        {
            return "chmod - 파일의 권한 변경";
        }
    }
}