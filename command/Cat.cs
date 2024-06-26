using Tree;
using VirtualTerminal.Errors;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class CatCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            Tree<FileNode>? parentFile;

            string[]? splitPath;
            string? absolutePath;
            string? parentPath;

            string? fileName;
            
            bool[] permission;

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    splitPath = absolutePath.Split('/');
                    fileName = splitPath[^1]; // path.Length - 1
                    parentPath = absolutePath.Replace('/' + fileName, "");

                    file = VT.fileSystem.FindFile(absolutePath, VT.root);
                    parentFile = VT.fileSystem.FindFile(parentPath, VT.root);

                    if(parentFile == null){
                        Console.WriteLine(ErrorsMassage.NoSuchForD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (file == null)
                    {
                        permission = VT.fileSystem.CheckFilePermission(VT.USER, parentFile, VT.root);

                        if(parentFile.Data.FileType != FileType.D){
                            Console.WriteLine(ErrorsMassage.NotD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                            return;
                        }

                        if(!permission[0] || !permission[1] || !permission[2])
                        {
                            Console.WriteLine(ErrorsMassage.PermissionDenied(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                            return;
                        }

                        Console.WriteLine($"File not found: {fileName}. Creating new file. Enter content (end with a single dot on a line):");
                        string content = VT.ReadMultiLineInput();
                        VT.fileSystem.CreateFile(parentPath, new FileNode(fileName, VT.USER, 0b110100, FileType.F, content), VT.root);
                        return;
                    }

                    if (file.Data.FileType == FileType.D)
                    {
                        Console.WriteLine(ErrorsMassage.NotF(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);

                    if(!permission[0])
                    {
                        Console.WriteLine(ErrorsMassage.PermissionDenied(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    Console.WriteLine(file.Data.Content);
                }
            }
        }
    }
}