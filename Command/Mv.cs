using Tree;
using VirtualTerminal.Error;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Command
{
    public class MvCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if(argc < 3){
                ErrorMessage.ArgLack(argv[0]);
                return;
            }

            foreach (string arg in argv)
            {
                if (arg != argv[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    
                }
            }
        }

        public string Description(bool detail)
        {
            return "mv - 파일이나 폴더 위치를 옮기거나 이름 제지정";
        }
    }
}