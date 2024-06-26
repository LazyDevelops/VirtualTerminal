using VirtualTerminal.Errors;

namespace VirtualTerminal.Commands
{
    public class ManCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            if (VT.commandMap.TryGetValue(args[1], out var action))
            {
                Console.WriteLine(action.Description(true));
            }
            else
            {
                Console.WriteLine(ErrorsMassage.CmdNotFound(args[0], ErrorsMassage.DefaultErrorComment(args[1])));
            }
        }

        public string Description(bool detail)
        {
            return "man - 명령어에 대한 자세한 설명 출력";
        }
    }
}