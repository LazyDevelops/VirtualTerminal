using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using Tree;

namespace VirtualTerminal {
    class VirtualTerminal
    {
        public struct FileNode{
            public string name { get; }
            public byte permission { get; }
            public string UID { get; }
            public enum FileType { 
                F, D 
            } public FileType fileType { get; }
            public string? content { get; }

            public FileNode(string name, string UID, byte permission, FileType fileType, string? content = null){
                this.name = name;
                this.UID = UID;
                this.permission = permission;
                this.fileType = fileType;
                this.content = content;
            }

            public override string ToString() {
                return name;
            }
        }

        private Tree<FileNode> root;
        private Tree<FileNode> pwdNode;
        private Tree<FileNode> homeNode;
        
        private string PWD;
        private string HOME;
        private string USER;

        public VirtualTerminal()
        {
            USER = "user";
            PWD = $"/home/{USER}";
            HOME = $"/home/{USER}";

            root = new Tree<FileNode>(new FileNode("/", "root", 0b111101, FileNode.FileType.D));
            CreateFile("/", new FileNode("home", "root", 0b111101, FileNode.FileType.D));
            CreateFile("/", new FileNode("root", "root", 0b111000, FileNode.FileType.D));
            
            homeNode = CreateFile("/home", new FileNode(USER, USER, 0b111101, FileNode.FileType.D));

            CreateFile(HOME, new FileNode($"Hello_{USER}.txt", "root", 0b111111, FileNode.FileType.F, $"Hello, {USER}!"));
            
            pwdNode = FindFile(PWD);
        }

        public Tree<FileNode> CreateFile(string path, FileNode entry)
        {
            string[] directories = path.Split('/');
            Tree<FileNode> current = root;

            current = FindFile(path);

            Tree<FileNode> newFile = new Tree<FileNode>(entry);
            current.AppendChildNode(newFile);
            return newFile;
        }

        public int RemoveFile(string path)
        {
            string[] directories = path.Split('/');
            Tree<FileNode> current = root;
            Tree<FileNode> parents = root;

            current = FindFile(path);
            if(current.Parents != null){
                parents = current.Parents;
            }

            if (current.LeftChild == null)
            {
                parents.RemoveChildNode(current);
                return 0;
            }
            return 1;
        }

        public Tree<FileNode> FindFile(string path)
        {
            string[] directories = path.Split('/');
            string fileName = directories[directories.Length - 1];
            Tree<FileNode> current = root;

            for (int i = 1; i < directories.Length; i++){
                foreach (Tree<FileNode> tempNode in current.GetChildren()){
                    if (tempNode.Data.name == directories[i]){
                        current = tempNode;
                        break;
                    }
                }
            }

            if(current.Data.name == fileName){
                return null;
            }

            return current;
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
            // if(!currentDirectory.Equals(homeDirectory))
            //     WriteColoredText("\x1b[1m~\x1b[22m", ConsoleColor.Blue);
            // else
            //     WriteColoredText("\x1b[1m/\x1b[22m", ConsoleColor.Blue);
            WriteColoredText($"\x1b[1m{PWD}\x1b[22m", ConsoleColor.Blue);
            WriteColoredText("$ ", Console.ForegroundColor);
        }

        private void ProcessCommand(string command)
        {
            string[] args = command.Split(' ');
            // foreach(string temp in args){
            //     Console.WriteLine(temp);
            // }

            switch (args[0])
            {
                // case "cd":
                //     ExecuteCd(args);
                //     break;
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
                    Console.WriteLine($"Command not found: {command}");
                    break;
            }
        }

        private void ExecuteLs(string[] args)
        {
            Dictionary<string, bool> options = new Dictionary<string, bool>{
                { "l", false },
            };
            
            foreach (string temp in args){
                // -- 옵션을 위한 코드
                /*if(temp.Contains("--")){
                    options[temp.Replace("--", "")] = true;
                }else */if(temp.Contains("-")){
                    foreach(char c in temp){
                        if(c != '-'){
                            options[temp] = true;
                        }
                    }
                }
            }

            List<Tree<FileNode>> pwdChildren = pwdNode.GetChildren();

            foreach(Tree<FileNode> temp in pwdChildren){
                if(options["l"]){
                    char? type = null;

                    switch(temp.Data.fileType){
                        case FileNode.FileType.F:
                            type = '-';
                            break;
                        case FileNode.FileType.D:
                            type = 'd';
                            break;
                        default:
                            break;
                    }

                    string permissions = ConvertPermissionsToString(temp.Data.permission);

                    Console.WriteLine($"{type}{permissions} {temp.Data.UID} {temp.Data.name}");
                }else{
                    Console.WriteLine(temp.Data.name);
                }
            }
        }

        private void ExecuteCd(string[] args)
        {
            
            // if (dir == "." || dir == "./")
            // {
            //     return;
            // }
            // else if (dir == ".." || dir == "../")
            // {
            //     if (PWD != "/")
            //     {
            //         int lastSlashIndex = PWD.LastIndexOf('/');
            //         if (lastSlashIndex == 0)
            //         {
            //             PWD = "/";
            //         }
            //         else
            //         {
            //             PWD = PWD.Substring(0, lastSlashIndex);
            //         }
            //     }
            // }
            // else
            // {
            //     string newDir = PWD == "/" ? $"/{dir}" : $"{PWD}/{dir}";
            //     if (fileSystem.Exists(entry => entry.Path == newDir && entry.IsDirectory))
            //     {
            //         PWD = newDir;
            //     }
            //     else
            //     {
            //         Console.WriteLine($"Directory not found: {dir}");
            //     }
            // }
        }

        private void ExecuteCat(string[] args)
        {
            Tree<FileNode> file = new Tree<FileNode>();

            if(file.Data.fileType == FileNode.FileType.D){
                Console.WriteLine($"Not a file: {file}");
                return;
            }
            
            foreach(string temp in args){
                if(temp.Contains("/")){
                    file = FindFile(temp);

                    if(file != null){
                        Console.WriteLine(file.Data.content);
                    }else{
                        Console.WriteLine($"File not found: {file}. Creating new file. Enter content (end with a single dot on a line):");
                        string content = ReadMultiLineInput();
                        CreateFile(temp, new FileNode(temp, USER, 0b110100, FileNode.FileType.F, content));
                    }
                }
            }
            
            // string path = PWD == "/" ? $"/{file}" : $"{PWD}/{file}";
            // var entry = fileSystem.Find(entry => entry.Path == path);

            // if(entry.IsDirectory){
            //     Console.WriteLine($"Not a file: {file}");
            // }
            // else if (entry.Path != null)
            // {
            //     Console.WriteLine(entry.Content);
            // }
            // else
            // {
            //     Console.WriteLine($"File not found: {file}. Creating new file. Enter content (end with a single dot on a line):");
            //     string content = ReadMultiLineInput();
            //     fileSystem.Add(new FileSystemEntry(path, USER, 0b110100, 0, content));
            //     Console.WriteLine("File created.");
            // }
        }

        private void ExecuteClear()
        {
            Console.Clear();
        }

        private void ExecuteMkdir(string[] args){
            string[]? parts = null;
            string? fileName = null;
            
            foreach(string temp in args){
                if(temp.Contains("/") && FindFile(temp) != null){
                    parts = temp.Split('/');
                    fileName = parts[parts.Length - 1];
                    CreateFile(temp, new FileNode(fileName, USER, 0b111101, FileNode.FileType.D));
                }
            }
        }

        // 에러 메세지 수정 필요
        private void ExecuteRmdir(string[] args){
            Tree<FileNode> file = new Tree<FileNode>();

            foreach(string temp in args){
                if(temp.Contains("/")){
                    file = FindFile(temp);
                    
                    if(file.Data.fileType == FileNode.FileType.D){
                        Console.WriteLine($"failed to remove '{file.Data.name}': Not a directory");
                        return;
                    }

                    if(RemoveFile(temp) != 0){
                        Console.WriteLine($"failed to remove '{file.Data.name}': Directory not empty");
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
            string content = "";
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

class Program
{
    static void Main()
    {
        VirtualTerminal.VirtualTerminal terminal = new VirtualTerminal.VirtualTerminal();
        terminal.Run();
    }
}
