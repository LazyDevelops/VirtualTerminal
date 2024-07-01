using Tree;
using VirtualTerminal.Error;
using static VirtualTerminal.FileSystem.FileSystem;

namespace VirtualTerminal.Command
{
    public class ChModCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            string? absolutePath;
            byte? inputPermission;

            if (argv[1].Length == 6 && (argv[1].Contains('1') || argv[1].Contains('0')))
            {
                inputPermission = Convert.ToByte(argv[1].PadLeft(8, '0'), 2);
            }
            else
            {
                Console.WriteLine(ErrorMessage.InvalidMode(argv[0], ErrorMessage.DefaultErrorComment(argv[1])));
                return;
            }

            foreach (string arg in argv)
            {
                if (arg != argv[0] && arg != argv[1] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    if(file.Data.UID != VT.USER){
                        Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    file.Data.Permission = inputPermission.Value;
                }
            }
        }

        public string Description(bool detail)
        {
            return "chmod - 소유하고 있는 파일의 권한을 변경";
        }
    }
}