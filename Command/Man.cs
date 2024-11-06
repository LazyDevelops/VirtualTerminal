using VirtualTerminal.Error;

namespace VirtualTerminal.Command
{
    public class ManCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 2)
            {
                return ErrorMessage.ArgLack(argv[0]);
            }

            if (VT.CommandMap.TryGetValue(argv[1], out VirtualTerminal.ICommand? action))
            {
                return action.Description(true);
            }

            return ErrorMessage.CmdNotFound(argv[0], ErrorMessage.DefaultErrorComment(argv[1]));
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "man - 명령어에 대한 자세한 설명 출력\n";
            }

            return "man - 명령어에 대한 자세한 설명 출력";
        }
    }
}