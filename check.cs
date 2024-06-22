using System;
using System.Collections.Generic;
using Tree;
using FileSystem;
using static FileSystem.FileSystem;

namespace VirtualTerminal
{
    class VirtualTerminal
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
            // homeDirectory 일시 ~ 표시 코드 짜기
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
                // case "mkdir":
                //     ExecuteMkdir(args);
                //     break;
                // case "rm":
                //     ExecuteRm(args);
                //     break;
                // case "rmdir":
                //     ExecuteRmdir(args);
                //     break;
                default:
                    Console.WriteLine($"Command not found: {command}");
                    break;
            }
        }

        private void ExecuteLs(string[] args)
        {
            Dictionary<string, bool> options = new(){
                { "l", false }
            };

            foreach (string temp in args)
            {
                // -- 옵션을 위한 코드
                /*if(temp.Contains("--")) {
                    options[temp.Replace("--", "")] = true;
                }else */
                if (temp.Contains('-'))
                {
                    foreach (char c in temp)
                    {
                        if (c != '-')
                        {
                            options[temp] = true;
                        }
                    }
                }
            }

            List<Tree<FileNode>>? pwdChildren = pwdNode?.GetChildren();

            if (pwdChildren == null)
            {
                return;
            }

            foreach (Tree<FileNode> temp in pwdChildren)
            {
                if (options["l"])
                {
                    /*char? type;

                    switch (temp.Data.fileType)
                    {
                        case FileNode.FileType.F:
                            type = '-';
                            break;
                        case FileNode.FileType.D:
                            type = 'd';
                            break;
                        default:
                            break;
                    }*/

                    string permissions = ConvertPermissionsToString(temp.Data.permission);

                    Console.WriteLine($"{temp.Data.fileType}{permissions} {temp.Data.UID} {temp.Data.name}");
                }
                else
                {
                    Console.WriteLine(temp.Data.name);
                }
            }
        }

        private void ExecuteCd(string[] args)
        {
            Tree<FileNode>? file;

            foreach (string temp in args)
            {
                if (temp.Contains('-'))
                {
                    file = fileSystem.FindFile(temp, root);

                    if (file == null)
                    {
                        Console.WriteLine($"test: No such file or directory");
                        return;
                    }

                    if (file.Data.fileType != FileType.D)
                    {
                        Console.WriteLine($"{file.Data.name}: Not a directory");
                        return;
                    }

                    pwdNode = file;
                    PWD = temp;
                }
            }
        }

        private void ExecuteCat(string[] args)
        {
            Tree<FileNode>? file;
            string[]? path;
            string? fileName;

            foreach (string temp in args)
            {
                if (temp.Contains('-'))
                {
                    file = fileSystem.FindFile(temp, root);
                    path = temp.Split('-');
                    fileName = path[path.Length - 1];

                    if (file == null)
                    {
                        Console.WriteLine($"File not found: {file}. Creating new file. Enter content (end with a single dot on a line):");
                        string content = ReadMultiLineInput();
                        fileSystem.CreateFile(temp, new FileNode(fileName, USER, 0b110100, FileType.F, content), root);
                        return;
                    }

                    if (file.Data.fileType == FileType.D)
                    {
                        Console.WriteLine($"Not a file: {file}");
                        return;
                    }

                    Console.WriteLine(file.Data.content);
                }
            }
        }

        private void ExecuteClear()
        {
            Console.Clear();
        }

        private void ExecuteMkdir(string[] args)
        {
            string[]? parts;
            string? fileName;

            foreach (string temp in args)
            {
                if (temp.Contains('/') && fileSystem.FindFile(temp, root) != null)
                {
                    parts = temp.Split('/');
                    fileName = parts[parts.Length - 1];
                    fileSystem.CreateFile(temp, new FileNode(fileName, USER, 0b111101, FileType.D), root);
                }
            }
        }

        // 에러 메세지 수정 필요
        private void ExecuteRmdir(string[] args)
        {
            Tree<FileNode>? file;
            string[]? path;
            string? fileName;

            foreach (string temp in args)
            {
                if (temp.Contains('/'))
                {
                    path = temp.Split('/');
                    fileName = path[^1]; // path[path.Length - 1];

                    file = fileSystem.FindFile(temp, root);

                    if (file == null)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{fileName}': No such file or directory");
                        return;
                    }

                    if (file.Data.fileType == FileType.D)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{file.Data.name}': Not a directory");
                        return;
                    }

                    if (fileSystem.RemoveFile(temp, root) != 0)
                    {
                        Console.WriteLine($"{args[0]}: failed to remove '{file.Data.name}': Directory not empty");
                        return;
                    }
                }
            }
        }

        // private void ExecuteRm(string[] args)
        // {
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
        // }

        private void ExecuteExit()
        {
            Environment.Exit(0);
        }

        private void ExecuteHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("ls - List directory contents");
            Console.WriteLine("cd - Change the current directory");
            Console.WriteLine("cat - Display file content");
            Console.WriteLine("clear - Clear the screen");
            Console.WriteLine("mkdir - Create a new directory");
            Console.WriteLine("rmdir - Remove a directory");
            Console.WriteLine("rm - Remove a file");
            Console.WriteLine("exit - Exit the terminal");
        }

        private string ConvertPermissionsToString(short permissions)
        {
            string result = string.Empty;
            result += (permissions & 040) != 0 ? "r" : "-";
            result += (permissions & 020) != 0 ? "w" : "-";
            result += (permissions & 010) != 0 ? "x" : "-";
            result += (permissions & 004) != 0 ? "r" : "-";
            result += (permissions & 002) != 0 ? "w" : "-";
            result += (permissions & 001) != 0 ? "x" : "-";
            return result;
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

class Check
{
    static void Main()
    {
        VirtualTerminal.VirtualTerminal terminal = new();
        terminal.Run();
    }
}
