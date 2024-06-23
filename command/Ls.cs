using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal
{
    public partial class VirtualTerminal
    {
        private void ExecuteLs(string[] args)
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
                            options[temp] = true;
                        }
                    }
                }
            }

            List<Tree<FileNode>>? pwdChildren = pwdNode?.GetChildren();

            if (pwdChildren == null)
            {
                return;
            }

            foreach (Tree<FileNode> temp in pwdChildren)
            {
                if (options["l"])
                {
                    string permissions = fileSystem.ConvertPermissionsToString(temp.Data.Permission);
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