using Tree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

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

            foreach (string arg in argv.Skip(2))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = FileSystem.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                file = VT.FileSystem.FindFile(absolutePath, VT.Root);

                if (file == null)
                {
                    Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (file.Data.UID != VT.USER)
                {
                    Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                file.Data.Permission = inputPermission.Value;
            }
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "chmod - 소유하고 있는 파일의 권한을 변경\n";
            }

            return "chmod - 소유하고 있는 파일의 권한을 변경";
        }
    }
}