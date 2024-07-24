using Tree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class CpCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 3)
            {
                Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                return;
            }

            Tree<FileNode>?[] file = new Tree<FileNode>?[2];
            List<Tree<FileNode>?> tempFile = [];
            byte fileCounter = 0;
            string?[] absolutePath = new string?[2];
            string? fileName;
            bool[] permission;

            Dictionary<string, bool> options = new() { { "r", false }, { "f", false } };

            VirtualTerminal.OptionCheck(ref options, in argv);

            foreach (string arg in argv.Skip(1))
            {
                if (arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                if (fileCounter + 1 > file.Length)
                {
                    Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                    return;
                }

                absolutePath[fileCounter] = FileSystem.FileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                if (fileCounter == 0)
                {
                    file[fileCounter] = VT.FileSystem.FindFile(absolutePath[fileCounter], VT.Root);
                }
                else
                {
                    fileName = absolutePath[fileCounter]?.Split('/')[^1];
                    absolutePath[fileCounter] = absolutePath[fileCounter]?.Replace('/' + fileName, "");
                    file[fileCounter] = VT.FileSystem.FindFile(absolutePath[fileCounter], VT.Root);
                }

                fileCounter++;
            }
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "cp - 파일과 디렉터리 복사\n";
            }

            return "cp - 파일과 디렉터리 복사";
        }
    }
}