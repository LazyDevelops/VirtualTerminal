using System.Text.RegularExpressions;
using VirtualTerminal.Tree.General;
using VirtualTerminal.Command;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;
using System;

// path stack이나 list로 수정 고려

namespace VirtualTerminal
{
    public class VirtualTerminal
    {
        internal readonly Dictionary<string, ICommand> CommandMap;

        internal FileSystem.FileSystem FileSystem = new();
        internal Tree<FileDataStruct> FileTree;

        // private List<string> PWD;
        // private List<FileNode> PWD;
        internal string HOME;
        internal Node<FileDataStruct>? HomeNode;

        internal string PWD;
        internal Node<FileDataStruct>? PwdNode;
        internal Node<FileDataStruct> Root;
        internal string USER;

        public VirtualTerminal(string user)
        {
            USER = user;
            PWD = $"/home/{USER}";
            HOME = $"/home/{USER}";

            FileTree = new Tree<FileDataStruct>(new Node<FileDataStruct>(new FileDataStruct("/", "root", 0b111101, FileType.D)));
            Root = FileTree.Root;

            FileSystem.CreateFile("/", new FileDataStruct("home", "root", 0b111101, FileType.D), Root);
            FileSystem.CreateFile("/", new FileDataStruct("root", "root", 0b111000, FileType.D), Root);

            HomeNode = FileSystem.CreateFile("/home", new FileDataStruct(USER, USER, 0b111101, FileType.D), Root);

            FileSystem.CreateFile(HOME,
                new FileDataStruct($"Hello_{USER}.txt", "root", 0b111111, FileType.F, $"Hello, {USER}!"), Root);

            PwdNode = FileSystem.FindFile(PWD, Root);

            // 명령어와 메서드를 매핑하는 사전 초기화
            CommandMap = new Dictionary<string, ICommand>
            {
                { "cat", new CatCommand() },
                { "cd", new CdCommand() },
                { "chmod", new ChModCommand() },
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
                { "rm", new RmCommand() },
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
            Console.Write($"\u001b[32;1m{USER}\u001b[0m:\u001b[34;1m{PWD}\u001b[0m$ ");
        }

        private void ProcessCommand(string inputLine)
        {
            string[] commands = inputLine.Split(';');

            foreach (string command in commands)
            {
                string[] argv = command.Split(' ').Where(arg => !string.IsNullOrWhiteSpace(arg)).ToArray();

                if (argv.Length == 0)
                {
                    continue;
                }

                string output = string.Empty;

                if(CommandMap.TryGetValue(argv[0], out ICommand? action)){
                    if (argv.Skip(1).Any(arg => arg == "--help"))
                    {
                        output = CommandMap["man"].Execute(2, ["man", argv[0]], this);
                    }
                    else
                    {
                        output = action.Execute(argv.Length, argv, this);
                    }
                }
                else
                {
                    output = ErrorMessage.CmdNotFound(argv[0]);
                }

                Console.Write(output);
            }
        }

        internal static string RemoveAnsiCodes(string input)
        {
            // 정규식을 사용하여 ANSI 코드 제거
            const string ansiEscapePattern = @"\u001b\[[0-9;]*m";
            return Regex.Replace(input, ansiEscapePattern, string.Empty);
        }

        internal string ReadMultiLineInput()
        {
            string content = string.Empty;

            while (true)
            {
                string? input = Console.ReadLine();

                if (input == null)
                {
                    break;
                }

                content += input + "\n";
            }

            return content.TrimEnd('\n');
        }

        internal static void OptionCheck(ref Dictionary<string, bool> option, in string[] argv)
        {
            foreach (string arg in argv)
            {
                if (arg.Contains("--"))
                {
                    option[arg.Replace("--", "")] = true;
                }
                else if (arg.Contains('-'))
                {
                    foreach (char c in arg.Replace("-", ""))
                    {
                        option[c.ToString()] = true;
                    }
                }
            }
        }

        internal interface ICommand
        {
            string? Execute(int argc, string[] argv, VirtualTerminal VT);
            string Description(bool detail);
        }
    }
}