using VirtualTerminal.Error;

namespace VirtualTerminal.Command
{
    public class ManCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if(argc < 2){
                ErrorMessage.ArgLack(argv[0]);
                return;
            }

            if (VT.commandMap.TryGetValue(argv[1], out var action))
            {
                Console.Write(action.Description(true));
            }
            else
            {
                Console.WriteLine(ErrorMessage.CmdNotFound(argv[0], ErrorMessage.DefaultErrorComment(argv[1])));
            }
        }

        public string Description(bool detail)
        {
            return "man - 명령어에 대한 자세한 설명 출력";
        }
    }
}