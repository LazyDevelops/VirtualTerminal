using VirtualTerminal.Error;

namespace VirtualTerminal.Command
{
    public class CpCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if(argc < 3){
                Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                return;
            }
        }

        public string Description(bool detail)
        {
            return "cp - 파일과 디렉터리 복사";
        }
    }
}