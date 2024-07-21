using Tree;
using VirtualTerminal.Command;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal
{
    public class VirtualTerminal
    {
        internal readonly Dictionary<string, ICommand> commandMap;

        internal FileSystem.FileSystem fileSystem = new();

        // private List<string> PWD;
        // private List<FileNode> PWD;
        internal string HOME;
        internal Tree<FileNode>? homeNode;

        internal string PWD;
        internal Tree<FileNode>? pwdNode;
        internal Tree<FileNode> root;
        internal string USER;

        public VirtualTerminal()
        {
            USER = "user";
            PWD = $"/home/{USER}";
            HOME = $"/home/{USER}";

            root = new Tree<FileNode>(new FileNode("/", "root", 0b111101, FileType.D));
            fileSystem.CreateFile("/", new FileNode("home", "root", 0b111101, FileType.D), root);
            fileSystem.CreateFile("/", new FileNode("root", "root", 0b111000, FileType.D), root);

            homeNode = fileSystem.CreateFile("/home", new FileNode(USER, USER, 0b111101, FileType.D), root);

            fileSystem.CreateFile(HOME, new FileNode("Item", "root", 0b111101, FileType.D), root);
            fileSystem.CreateFile(HOME,
                new FileNode($"Hello_{USER}.txt", "root", 0b111111, FileType.F, $"Hello, {USER}!"), root);

            pwdNode = fileSystem.FindFile(PWD, root);

            // 명령어와 메서드를 매핑하는 사전 초기화
            commandMap = new Dictionary<string, ICommand>
            {
                { "cat", new CatCommand() },
                { "cd", new CdCommand() },
                { "chmod", new ChModCommand() }, // 제작 전
                { "clear", new ClearCommand() },
                { "cp", new CpCommand() }, // 제작 전
                { "date", new DateCommand() },
                { "echo", new EchoCommand() },
                { "exit", new ExitCommand() },
                { "help", new HelpCommand() },
                { "ls", new LsCommand() },
                { "man", new ManCommand() },
                { "mkdir", new MkDirCommand() },
                { "mv", new MvCommand() }, // 제작 전
                { "pwd", new PwdCommand() },
                { "rm", new RmCommand() }, // 제작 전
                { "rmdir", new RmDirCommand() },
                { "whoami", new WhoAmICommand() }
            };
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
            string[] argv = command.Split(' ').Where(arg => !string.IsNullOrWhiteSpace(arg)).ToArray();

            if (argv.Any(arg => arg == "--help"))
            {
                string[] manArgs = ["man", argv[0]];
                commandMap.TryGetValue(manArgs[0], out ICommand? man);
                man?.Execute(manArgs.Length, manArgs, this);
                return;
            }

            if (commandMap.TryGetValue(argv[0], out ICommand? action))
            {
                action.Execute(argv.Length, argv, this);
            }
            else
            {
                Console.WriteLine(ErrorMessage.CmdNotFound(argv[0]));
            }
        }

        internal string ReadMultiLineInput()
        {
            string content = string.Empty;
            string? line;
            while ((line = Console.ReadLine()) != ".")
            {
                content += line + Environment.NewLine;
            }

            return content.TrimEnd('\n');
        }

        internal void OptionCheck(ref Dictionary<string, bool> option, in string[] argv)
        {
            foreach (string arg in argv)
            {
                if (arg.Contains("--"))
                {
                    option[arg.Replace("--", "")] = true;
                }
                else if (arg.Contains('-'))
                {
                    option[arg.Replace("-", "")] = true;
                }
            }
        }

        internal void WriteColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        internal interface ICommand
        {
            void Execute(int argc, string[] argv, VirtualTerminal VT);
            string Description(bool detail);
        }
    }
}