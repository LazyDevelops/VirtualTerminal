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

            List<Tree<FileNode>>? pwdChildren = VT.pwdNode?.GetChildren();

            if (pwdChildren == null)
            {
                return;
            }

            foreach (Tree<FileNode> temp in pwdChildren)
            {
                if (options["l"])
                {
                    string permissions = VT.fileSystem.ConvertPermissionsToString(temp.Data.Permission);
                    // file 타입이 -가 아닌 F로 출력됨
                    Console.WriteLine($"{temp.Data.FileType}{permissions} {temp.Data.UID} {temp.Data.Name}");
                }
                else
                {
                    Console.WriteLine(temp.Data.Name);
                }
            }
        }
    }
}