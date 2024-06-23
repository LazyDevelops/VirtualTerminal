using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal
{
    public partial class VirtualTerminal
    {
        private FileSystem.FileSystem fileSystem = new();
        private Tree<FileNode> root;
        private Tree<FileNode>? pwdNode;
        private Tree<FileNode>? homeNode;

        private string PWD;
        // private List<string> PWD;
        // private List<FileNode> PWD;
        private string HOME;
        private string USER;

        public VirtualTerminal()
        {
            USER = "user";
            PWD = $"/home/{USER}";
            HOME = $"/home/{USER}";

            root = new Tree<FileNode>(new FileNode("/", "root", 0b111101, FileType.D));
            fileSystem.CreateFile("/", new FileNode("home", "root", 0b111101, FileType.D), root);
            fileSystem.CreateFile("/", new FileNode("root", "root", 0b111000, FileType.D), root);

            homeNode = fileSystem.CreateFile("/home", new FileNode(USER, USER, 0b111101, FileType.D), root);

            fileSystem.CreateFile(HOME, new FileNode($"Hello_{USER}.txt", "root", 0b111111, FileType.F, $"Hello, {USER}!"), root);

            pwdNode = fileSystem.FindFile(PWD, root);

            if (homeNode == null)
            {
                Console.WriteLine($"homeNode err");
                Environment.Exit(0);
            }

            if (pwdNode == null)
            {
                Console.WriteLine("pwdNode err");
                Environment.Exit(0);
            }
        }
        
        public void Run()
        {
            while (true)
            {
                DisplayPrompt();
                string? command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                ProcessCommand(command);
            }
        }

        private void DisplayPrompt()
        {
            WriteColoredText("\x1b[1muser\x1b[22m", ConsoleColor.Green);
            WriteColoredText(":", Console.ForegroundColor);
            WriteColoredText($"\x1b[1m{PWD}\x1b[22m", ConsoleColor.Blue);
            WriteColoredText("$ ", Console.ForegroundColor);
        }

        private void ProcessCommand(string command)
        {
            string[] args = command.Split(' ');

            switch (args[0])
            {
                case "cd":
                    ExecuteCd(args);
                    break;
                case "cat":
                    ExecuteCat(args);
                    break;
                case "clear":
                    ExecuteClear();
                    break;
                case "exit":
                    ExecuteExit();
                    break;
                case "help":
                    ExecuteHelp();
                    break;
                case "ls":
                    ExecuteLs(args);
                    break;
                // case "mv":
                //     ExecuteMv(args);
                //     break;
                case "mkdir":
                    ExecuteMkdir(args);
                    break;
                // case "rm":
                //     ExecuteRm(args);
                //     break;
                case "rmdir":
                    ExecuteRmdir(args);
                    break;
                default:
                    Console.WriteLine($"Command not found: {args[0]}");
                    break;
            }
        }

        private string ReadMultiLineInput()
        {
            string content = string.Empty;
            string? line;
            while ((line = Console.ReadLine()) != ".")
            {
                content += line + Environment.NewLine;
            }
            return content.TrimEnd('\n');
        }

        private static void WriteColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
