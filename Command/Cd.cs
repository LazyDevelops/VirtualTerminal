using Tree;
using VirtualTerminal.Error;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Command
{
    public class CdCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            string? absolutePath;
            bool[] permission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    
                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.Write("bash: ");
                        Console.WriteLine(ErrorMessage.NoSuchForD(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);
                    
                    if(!permission[0] || !permission[2])
                    {
                        Console.Write("bash: ");
                        Console.WriteLine(ErrorMessage.PermissionDenied(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (file.Data.FileType != FileType.D)
                    {
                        Console.Write("bash: ");
                        Console.WriteLine(ErrorMessage.NotD(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    VT.pwdNode = file;
                    VT.PWD = absolutePath;
                }
            }
        }

        public string Description(bool detail)
        {
            return "cd - 현제 디렉터리 위치 변경";
        }
    }
}