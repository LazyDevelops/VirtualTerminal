using Tree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class RmCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 2)
            {
                Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                return;
            }

            Tree<FileNode>? file;
            string? absolutePath;
            bool[] permission;

            Dictionary<string, bool> options = new() { { "r", false } };

            VT.OptionCheck(ref options, in argv);

            foreach (string arg in argv)
            {
                if (arg == argv[0] || arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                absolutePath = VT.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                absolutePath.Split('/');

                file = VT.FileSystem.FindFile(absolutePath, VT.Root);

                if (file == null)
                {
                    Console.WriteLine(ErrorMessage.NoSuchForD(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                permission = VT.FileSystem.CheckFilePermission(VT.USER, file, VT.Root);

                if (!permission[0] || !permission[1] || !permission[2])
                {
                    Console.WriteLine(ErrorMessage.PermissionDenied(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (!options["r"] && file.Data.FileType == FileType.D)
                {
                    Console.WriteLine(ErrorMessage.NotF(argv[0], ErrorMessage.DefaultErrorComment(arg)));
                    return;
                }

                if (options["r"])
                {
                    VT.FileSystem.RemoveFile(absolutePath, VT.Root, 'r');
                    return;
                }

                VT.FileSystem.RemoveFile(absolutePath, VT.Root, null);
            }
        }

        public string Description(bool detail)
        {
            return "rm - 파일이나 디렉터리 삭제";
        }
    }
}