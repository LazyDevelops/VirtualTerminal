using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;
using VirtualTerminal.Tree.General;

// 수정 필요

namespace VirtualTerminal.Command
{
    public class LsCommand : VirtualTerminal.ICommand
    {
        public string? Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            Node<FileDataStruct>? file;
            List<Node<FileDataStruct>>? fileChildren;
            string? absolutePath;
            bool[] permission;

            Dictionary<string, bool> options = new() { { "l", false } };

            VirtualTerminal.OptionCheck(ref options, in argv);

            fileChildren = VT.PwdNode?.Children;

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = VT.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                file = VT.FileSystem.FileFind(absolutePath, VT.Root);

                if (file == null)
                {
                    return ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                permission = VT.FileSystem.CheckPermission(VT.USER, file, VT.Root);

                if (!permission[0])
                {
                    return ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg));
                }

                if (file.Data.FileType != FileType.D)
                {
                    return arg;
                }

                fileChildren = file.Children;
            }

            if (fileChildren == null)
            {
                return null;
            }

            string? result = null;

            foreach (Node<FileDataStruct> fileChild in fileChildren)
            {
                permission = VT.FileSystem.CheckPermission(VT.USER, fileChild, VT.Root);

                if (options["l"])
                {
                    string permissions = VT.FileSystem.PermissionsToString(fileChild.Data.Permission);
                    result += $"{Convert.ToChar(fileChild.Data.FileType)}{permissions} {fileChild.Data.UID} ";
                }


                if (fileChild.Data.FileType == FileType.D)
                {
                    result += $"\u001b[34m{fileChild.Data.Name}\u001b[0m";
                }
                else if (permission[2])
                {
                    result += $"\u001b[32m{fileChild.Data.Name}\u001b[0m";
                }
                else
                {
                    result += fileChild.Data.Name;
                }

                result += "\n";
            }

            return result;
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "ls - 현제 디렉터리에 있는 디렉터리 및 파일 목록 출력\n";
            }

            return "ls - 현제 디렉터리에 있는 디렉터리 및 파일 목록 출력";
        }
    }
}