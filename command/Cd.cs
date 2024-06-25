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

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine($"test: No such file or directory");
                        return;
                    }

                    if (file.Data.FileType != FileType.D)
                    {
                        Console.WriteLine($"{file.Data.Name}: Not a directory");
                        return;
                    }

                    VT.pwdNode = file;
                    VT.PWD = absolutePath;
                }
            }
        }
    }
}