using VirtualTerminal.Error;

namespace VirtualTerminal.Command
{
    public class ManCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            if (VT.commandMap.TryGetValue(args[1], out var action))
            {
                Console.Write(action.Description(true));
            }
            else
            {
                Console.WriteLine(ErrorMessage.CmdNotFound(args[0], ErrorMessage.DefaultErrorComment(args[1])));
            }
        }

        public string Description(bool detail)
        {
            return "man - 명령어에 대한 자세한 설명 출력";
        }
    }
}