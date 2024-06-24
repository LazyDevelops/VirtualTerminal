using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class LsCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Dictionary<string, bool> options = new(){
                { "l", false }
            };

            Tree<FileNode>? file;
            List<Tree<FileNode>>? fileChildren;
            string[]? path;
            string? absolutePath;

            foreach (string temp in args)
            {
                // -- 옵션을 위한 코드
                /*if(temp.Contains("--")) {
                    options[temp.Replace("--", "")] = true;
                }else */
                if (temp.Contains('-'))
                {
                    foreach (char c in temp)
                    {
                        if (c != '-')
                        {
                            options[c.ToString()] = true;
                        }
                    }
                }
            }

            fileChildren = VT.pwdNode?.GetChildren();

            foreach (string temp in args)
            {
                if (temp != args[0] && !temp.Contains('-') && !temp.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(temp, VT.HOME, VT.PWD);
                    path = absolutePath.Split('/');

                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine($"ls: cannot access '{temp}': No such file or directory");
                        return;
                    }

                    if (file.Data.FileType != FileType.D)
                    {
                        Console.WriteLine($"{temp}: Not a directory");
                        return;
                    }

                    fileChildren = file.GetChildren();
                }
            }

            if (fileChildren == null)
            {
                return;
            }

            foreach (Tree<FileNode> temp in fileChildren)
            {
                if (options["l"])
                {
                    string permissions = VT.fileSystem.ConvertPermissionsToString(temp.Data.Permission);
                    Console.WriteLine($"{Convert.ToChar(temp.Data.FileType)}{permissions} {temp.Data.UID} {temp.Data.Name}");
                }
                else
                {
                    Console.WriteLine(temp.Data.Name);
                }
            }
        }
    }
}