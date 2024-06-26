using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class CatCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            Tree<FileNode>? parentFile;
            string[]? path;
            string? absolutePath;
            string? parentPath;
            string? fileName;
            bool[] permissions;
            bool[] parentPermission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    path = absolutePath.Split('/');
                    fileName = path[^1]; // path.Length - 1
                    parentPath = absolutePath.Replace('/' + fileName, "");

                    file = VT.fileSystem.FindFile(absolutePath, VT.root);
                    parentFile = VT.fileSystem.FindFile(parentPath, VT.root);

                    if(parentFile == null){
                        Console.WriteLine($"{args[0]}: {arg}: No such file or directory");
                        return;
                    }

                    if (file == null)
                    {
                        parentPermission = VT.fileSystem.CheckFilePermission(VT.USER, parentFile, VT.root);

                        if(parentFile.Data.FileType != FileType.D){
                            Console.WriteLine($"bash: {args[0]}: {arg}: Not a directory");
                            return;
                        }

                        if(!parentPermission[0] || !parentPermission[1] || !parentPermission[2])
                        {
                            Console.WriteLine($"{args[0]}: {arg}: Permission denied");
                            return;
                        }

                        Console.WriteLine($"File not found: {fileName}. Creating new file. Enter content (end with a single dot on a line):");
                        string content = VT.ReadMultiLineInput();
                        VT.fileSystem.CreateFile(parentPath, new FileNode(fileName, VT.USER, 0b110100, FileType.F, content), VT.root);
                        return;
                    }

                    if (file.Data.FileType == FileType.D)
                    {
                        Console.WriteLine($"{args[0]}: {arg}: Not a file");
                        return;
                    }

                    permissions = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);

                    if(!permissions[0])
                    {
                        Console.WriteLine($"{args[0]}: {arg}: Permission denied");
                        return;
                    }

                    Console.WriteLine(file.Data.Content);
                }
            }
        }
    }
}