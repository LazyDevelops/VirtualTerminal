using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class MvCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {

        }

        public string Description()
        {
            return "test";
        }
    }
}