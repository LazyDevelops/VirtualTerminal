using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class CdCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;

            foreach (string temp in args)
            {
                if (temp != args[0] && !temp.Contains('-') && !temp.Contains("--"))
                {
                    file = VT.fileSystem.FindFile(VT.fileSystem.GetAbsolutePath(temp, VT.HOME, VT.PWD), VT.root);

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
                    VT.PWD = VT.fileSystem.GetAbsolutePath(temp, VT.HOME, VT.PWD);
                }
            }
        }
    }
}