using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal
{
    public partial class VirtualTerminal
    {
        // 에러 메세지 수정 필요
        private void ExecuteRmdir(string[] args)
        {
            Tree<FileNode>? file;
            string[]? path;
            string? absolutePath;
            string? fileName;

            foreach (string temp in args)
            {
                if (temp != args[0] && !temp.Contains('-') && !temp.Contains("--"))
                {
                    absolutePath = fileSystem.GetAbsolutePath(temp, HOME, PWD);
                    path = absolutePath.Split('/');
                    fileName = path[^1]; // path.Length - 1

                    file = fileSystem.FindFile(temp, root);

                    if (file == null)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{fileName}': No such file or directory");
                        return;
                    }

                    if (file.Data.FileType == FileType.D)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{file.Data.Name}': Not a directory");
                        return;
                    }

                    if (fileSystem.RemoveFile(absolutePath, root) != 0)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{file.Data.Name}': Directory not empty");
                        return;
                    }
                }
            }
        }
    }
}