using Tree;
using VirtualTerminal.Errors;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class LsCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            List<Tree<FileNode>>? fileChildren;
            string? absolutePath;
            bool[] permission;
            
            Dictionary<string, bool> options = new(){
                { "l", false }
            };

            foreach (string arg in args)
            {
                if (arg.Contains('-'))
                {
                    foreach (char c in arg)
                    {
                        if (c != '-')
                        {
                            options[c.ToString()] = true;
                        }
                    }
                }
            }

            fileChildren = VT.pwdNode?.GetChildren();

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine(ErrorsMassage.NoSuchForD(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);

                    if (!permission[0]){
                        Console.WriteLine(ErrorsMassage.PermissionDenied(args[0], ErrorsMassage.DefaultErrorComment(arg)));
                        return;
                    }

                    if (file.Data.FileType != FileType.D)
                    {
                        Console.WriteLine(arg);
                        return;
                    }

                    fileChildren = file.GetChildren();
                }
            }

            if (fileChildren == null)
            {
                return;
            }

            foreach (Tree<FileNode> fileChild in fileChildren)
            {
                if (options["l"])
                {
                    string permissions = VT.fileSystem.ConvertPermissionsToString(fileChild.Data.Permission);
                    Console.Write($"{Convert.ToChar(fileChild.Data.FileType)}{permissions} {fileChild.Data.UID} ");
                }
                
                
                if(fileChild?.Data.FileType == FileType.D)
                {
                    VT.WriteColoredText($"{fileChild?.Data.Name}", ConsoleColor.Blue);
                }
                else
                {
                    Console.Write(fileChild?.Data.Name);
                }

                Console.WriteLine();
            }
        }

        public string Description(bool detail)
        {
            return "ls - 현제 디렉터리에 있는 디렉터리 및 파일 목록 출력";
        }
    }
}