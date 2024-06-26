using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class CdCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            string? absolutePath;
            bool[] permissions;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine($"bash: {args[0]}: {arg}: 그런 파일이나 디렉터리가 없습니다.");
                        return;
                    }

                    if (file.Data.FileType != FileType.D)
                    {
                        Console.WriteLine($"bash: {args[0]}: {arg}: Not a directory");
                        return;
                    }

                    permissions = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);
                    
                    if(!permissions[0] || !permissions[2])
                    {
                        Console.WriteLine($"bash: {args[0]}: {arg}: Permission denied");
                        return;
                    }

                    VT.pwdNode = file;
                    VT.PWD = absolutePath;
                }
            }
        }
    }
}