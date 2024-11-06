using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;
using VirtualTerminal.Tree.General;

namespace VirtualTerminal.Command
{
    public class MvCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 3)
            {
                return ErrorMessage.ArgLack(argv[0]);
            }

            Tree<FileDataStruct>?[] file = new Tree<FileDataStruct>?[2];
            byte fileCounter = 0;
            string?[] absolutePath = new string?[2];
            string? fileName;
            bool[] permission;

            Dictionary<string, bool> options = new() { { "r", false }, { "f", false } };

            VirtualTerminal.OptionCheck(ref options, in argv);

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                if (fileCounter + 1 > file.Length)
                {
                    return ErrorMessage.ArgLack(argv[0]);
                }

                absolutePath[fileCounter] = VT.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                if (fileCounter == 0)
                {
                }

                fileCounter++;
            }

            return null;
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "mv - 파일이나 폴더 위치를 옮기거나 이름 제지정\n";
            }

            return "mv - 파일이나 폴더 위치를 옮기거나 이름 제지정";
        }
    }
}