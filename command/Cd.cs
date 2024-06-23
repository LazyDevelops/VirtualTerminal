using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal
{
    public partial class VirtualTerminal
    {
        private void ExecuteCd(string[] args)
        {
            Tree<FileNode>? file;

            foreach (string temp in args)
            {
                if (temp != args[0] && !temp.Contains('-') && !temp.Contains("--"))
                {
                    file = fileSystem.FindFile(fileSystem.GetAbsolutePath(temp, HOME, PWD), root);

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

                    pwdNode = file;
                    PWD = fileSystem.GetAbsolutePath(temp, HOME, PWD);
                }
            }
        }
    }
}