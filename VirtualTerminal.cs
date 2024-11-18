using System.Text.RegularExpressions;
using VirtualTerminal.Command;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;
using VirtualTerminal.Tree.General;

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

            FileSystem.FileCreate("/", new FileDataStruct("home", "root", 0b111101, FileType.D), Root);
            FileSystem.FileCreate("/", new FileDataStruct("root", "root", 0b111000, FileType.D), Root);

            HomeNode = FileSystem.FileCreate("/home", new FileDataStruct(USER, USER, 0b111101, FileType.D), Root);

            FileSystem.FileCreate(HOME,
                new FileDataStruct($"Hello_{USER}.txt", "root", 0b111111, FileType.F, $"Hello, {USER}!"), Root);

            PwdNode = FileSystem.FileFind(PWD, Root);

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
            foreach (string command in inputLine.Split(';'))
            {
                string? output = ExecuteCommand(command);
                HandleOutput(output, command.Split(' '));
            }
        }

        private string? ExecuteCommand(string command)
        {
            string[] argv = command.Split(' ').Where(arg => !string.IsNullOrWhiteSpace(arg)).ToArray();

            if (argv.Length == 0)
            {
                return null;
            }

            if (!CommandMap.TryGetValue(argv[0], out ICommand? action))
            {
                return ErrorMessage.CmdNotFound(argv[0]);
            }

            if (argv.Skip(1).Any(arg => arg == "--help"))
            {
                return CommandMap["man"].Execute(2, ["man", argv[0]], this);
            }

            string[] temp = argv.TakeWhile(x => x != ">>").ToArray();
            return action.Execute(temp.Length, temp, this);
        }

        private void HandleOutput(string? output, string[] argv)
        {
            if (argv.Skip(1).Any(arg => arg == ">>"))
            {
                int index = Array.IndexOf(argv, ">>");
                if (index == argv.Length - 1)
                {
                    Console.Write("bash: syntax error near unexpected token `newline'\n");
                    return;
                }
                AppendToFile(output, argv[index + 1]);
            }
            else
            {
                Console.Write(output);
            }
        }

        private void AppendToFile(string? output, string filePath)
        {
            string absolutePath = FileSystem.GetAbsolutePath(filePath, HOME, PWD);
            string fileName = absolutePath.Split('/')[^1];
            string parentPath = absolutePath.Replace('/' + fileName, "");
            bool[] permission;

            Node<FileDataStruct>? file = FileSystem.FileFind(absolutePath, Root);
            Node<FileDataStruct>? parentFile = FileSystem.FileFind(parentPath, Root);

            if (parentFile == null)
            {
                Console.Write(ErrorMessage.NoSuchForD(filePath, ErrorMessage.DefaultErrorComment(filePath)));
                return;
            }

            permission = FileSystem.CheckPermission(USER, parentFile, Root);

            if (!permission[0] || !permission[1] || !permission[2])
            {
                Console.Write(ErrorMessage.PermissionDenied(filePath, ErrorMessage.DefaultErrorComment(filePath)));
                return;
            }

            if (file != null)
            {
                permission = FileSystem.CheckPermission(USER, file, Root);

                if (!permission[0] || !permission[1])
                {
                    Console.Write(ErrorMessage.PermissionDenied(filePath, ErrorMessage.DefaultErrorComment(filePath)));
                    return;
                }

                output = "\n" + RemoveAnsiCodes(output)?.TrimEnd('\n');
                file.Data.Content += output;
            }
            else
            {
                if (parentFile.Data.FileType != FileType.D)
                {
                    Console.Write(ErrorMessage.NotD(filePath, ErrorMessage.DefaultErrorComment(filePath)));
                    return;
                }

                output = RemoveAnsiCodes(output)?.TrimEnd('\n');
                FileSystem.FileCreate(parentPath, new FileDataStruct(fileName, USER, 0b110100, FileType.F, output), Root);
            }
        }

        internal static string? RemoveAnsiCodes(string? input)
        {
            if (input == null)
            {
                return null;
            }

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