using Tree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class CdCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, in string[] argv, VirtualTerminal VT)
        {
            if (argc < 2)
            {
                VT.PwdNode = VT.HomeNode;
                VT.PWD = VT.HOME;
                return;
            }

            Tree<FileNode>? file;
            string? absolutePath;
            bool[] permission;

            foreach (string arg in argv)
            {
                if (arg == argv[0] || arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = VT.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                file = VT.FileSystem.FindFile(absolutePath, VT.Root);

                if (file == null)
                {
                    Console.Write("bash: ");
                    Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                permission = VT.FileSystem.CheckFilePermission(VT.USER, file, VT.Root);

                if (!permission[0] || !permission[2])
                {
                    Console.Write("bash: ");
                    Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (file.Data.FileType != FileType.D)
                {
                    Console.Write("bash: ");
                    Console.WriteLine(ErrorMessage.NotD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                VT.PwdNode = file;
                VT.PWD = absolutePath;
            }
        }

        public string Description(bool detail)
        {
            return "cd - 현제 디렉터리 위치 변경";
        }
    }
}