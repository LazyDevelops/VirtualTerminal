using Tree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class LsCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Tree<FileNode>? file;
            List<Tree<FileNode>>? fileChildren;
            string? absolutePath;
            bool[] permission;

            Dictionary<string, bool> options = new() { { "l", false } };

            VirtualTerminal.OptionCheck(ref options, in argv);

            fileChildren = VT.PwdNode?.GetChildren();

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = FileSystem.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                file = VT.FileSystem.FindFile(absolutePath, VT.Root);

                if (file == null)
                {
                    Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                permission = FileSystem.FileSystem.CheckFilePermission(VT.USER, file, VT.Root);

                if (!permission[0])
                {
                    Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (file.Data.FileType != FileType.D)
                {
                    Console.WriteLine(arg);
                    return;
                }

                fileChildren = file.GetChildren();
            }

            if (fileChildren == null)
            {
                return;
            }

            foreach (Tree<FileNode> fileChild in fileChildren)
            {
                if (options["l"])
                {
                    string permissions = FileSystem.FileSystem.PermissionsToString(fileChild.Data.Permission);
                    Console.Write($"{Convert.ToChar(fileChild.Data.FileType)}{permissions} {fileChild.Data.UID} ");
                }


                if (fileChild?.Data.FileType == FileType.D)
                {
                    VirtualTerminal.WriteColoredText($"{fileChild?.Data.Name}", ConsoleColor.Blue);
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