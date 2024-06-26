using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class RmCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
        //     string path = PWD == "/" ? $"/{file}" : $"{PWD}/{file}";
        //     var entry = fileSystem.Find(entry => entry.Path == path);

        //     if (entry.Path == null)
        //     {
        //         Console.WriteLine($"File not found: {file}");
        //     }
        //     else if (entry.IsDirectory)
        //     {
        //         Console.WriteLine($"Not a file: {file}");
        //     }
        //     else
        //     {
        //         fileSystem.RemoveAll(entry => entry.Path == path);
        //         Console.WriteLine($"File removed: {file}");
        //     }
        }

        public string Description(bool detail)
        {
            return "rm - 파일이나 디렉터리 삭제";
        }
    }
}