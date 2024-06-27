using Tree;
using VirtualTerminal.Errors;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class ChModCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file = null;
            string? absolutePath;
            byte? inputPermission;

            if (args[1].Length == 6 && (args[1].Contains('1') || args[1].Contains('0')))
            {
                inputPermission = Convert.ToByte(args[1].PadLeft(8, '0'), 2);
            }
            else
            {
                Console.WriteLine(ErrorsMassage.InvalidMode(args[0], ErrorsMassage.DefaultErrorComment(args[1])));
                return;
            }

            foreach (string arg in args)
            {
                if (arg != args[0] && arg != args[1] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine(ErrorsMassage.NoSuchForD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    if(file.Data.UID != VT.USER){
                        Console.WriteLine(ErrorsMassage.PermissionDenied(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }
                }
            }

            if (file == null)
            {
                Console.WriteLine(ErrorsMassage.MissingOperandAfter(args[0], args[1]));
                return;
            }

            file.Data.Permission = inputPermission.Value;
        }

        public string Description(bool detail)
        {
            return "chmod - ������ ���� ����";
        }
    }
}