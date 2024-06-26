using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class RmdirCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            string[]? path;
            string? absolutePath;
            string? fileName;
            bool[] parentPermission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    path = absolutePath.Split('/');
                    fileName = path[^1]; // path.Length - 1

                    file = VT.fileSystem.FindFile(arg, VT.root);

                    if (file == null || file.Parents == null)
                    {
                        Console.WriteLine($"{args[0]}: '{arg}' 디렉터리를 삭제할 수 없습니다: 파일이나 디렉터리를 찾을 수 없습니다.");
                        return;
                    }

                    if (file.Data.FileType == FileType.F)
                    {
                        Console.WriteLine($"{args[0]}: '{arg}' 디렉터리를 삭제할 수 없습니다: 디렉터리가 아닙니다.");
                        return;
                    }

                    parentPermission = VT.fileSystem.CheckFilePermission(VT.USER, file.Parents, VT.root);

                    if (parentPermission[0] || !parentPermission[1] || !parentPermission[2])
                    {
                        Console.WriteLine($"{args[0]}: '{arg}' 디렉터리를 삭제할 수 없습니다: 권한이 부족합니다.");
                        return;
                    }

                    if (VT.fileSystem.RemoveFile(absolutePath, VT.root) != 0)
                    {
                        Console.WriteLine($"{args[0]}: '{arg}' 디렉터리를 삭제할 수 없습니다: 디렉터리가 비어어있지 않습니다.");
                        return;
                    }
                }
            }
        }
    }
}