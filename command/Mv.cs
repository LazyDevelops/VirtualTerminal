using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class MvCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {

        }

        public string Description(bool detail)
        {
            return "mv - 파일이나 폴더 위치를 옮기거나 이름 제지정";
        }
    }
}